// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Quartz;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.SystemKey;
using Rock.Utility.Settings.GivingAnalytics;
using Rock.Web.Cache;

namespace Rock.Jobs
{
    /// <summary>
    /// Job that serves two purposes:
    ///   1.) Update Classification Attributes. This will be done no more than once a day and only on the days of week
    ///       configured in the analytics settings.
    ///   2.) Send Alerts - Sends alerts for gifts since the last run date and determines ‘Follow-up Alerts’ (alerts
    ///       triggered from gifts expected but not given) once a day.
    /// </summary>
    [DisplayName( "Giving Analytics" )]
    [Description( "Job that updates giving classification attributes as well as creating giving alerts." )]
    [DisallowConcurrentExecution]

    [IntegerField( "Max Days Since Last Gift",
        Description = "The maximum number of days since a giving group last gave where alerts can be made. If the last gift was earlier than this maximum, then alerts are not relevant.",
        DefaultIntegerValue = AttributeDefaultValue.MaxDaysSinceLastGift,
        Key = AttributeKey.MaxDaysSinceLastGift,
        Order = 1 )]

    public class GivingAnalytics : IJob
    {
        #region Keys

        /// <summary>
        /// Attribute Keys
        /// </summary>
        private static class AttributeKey
        {
            public const string MaxDaysSinceLastGift = "MaxDaysSinceLastGift";
        }

        /// <summary>
        /// Default Values for Attributes
        /// </summary>
        private static class AttributeDefaultValue
        {
            public const int MaxDaysSinceLastGift = 548;
        }

        /// <summary>
        /// The lower percentile for the giver bin
        /// </summary>
        private static class GiverBinLowerPercentile
        {
            public const decimal First = 0.95m;
            public const decimal Second = 0.80m;
            public const decimal Third = 0.60m;
        }

        #endregion Keys

        #region Constructors

        /// <summary>
        /// Empty constructor for job initialization
        /// <para>
        /// Jobs require a public empty constructor so that the
        /// scheduler can instantiate the class whenever it needs.
        /// </para>
        /// </summary>
        public GivingAnalytics()
        {
        }

        #endregion Constructors

        #region Execute

        /// <summary>
        /// Job to get a National Change of Address (NCOA) report for all active people's addresses.
        ///
        /// Called by the <see cref="IScheduler" /> when a
        /// <see cref="ITrigger" /> fires that is associated with
        /// the <see cref="IJob" />.
        /// </summary>
        public virtual void Execute( IJobExecutionContext jobContext )
        {
            // Create a context object that will help transport state and helper information so as to not rely on the
            // job class itself being a single use instance
            var context = new GivingAnalyticsContext( jobContext );

            // First determine the ranges for each of the 4 giving bins by looking at all contribution transactions in the last 12 months.
            // These ranges will be updated in the Giving Analytics system settings.
            UpdateGiverBinRanges( context );

            // Get a list of all giving units (distinct giver ids) that have given since the last classification
            HydrateGivingIdsToClassify( context );

            // For each giving id, classify and run analysis
            foreach ( var givingId in context.GivingIdsToClassify )
            {
                ProcessGivingId( givingId, context );
            }

            // Store the last run date
            LastRunDateTime = context.Now;

            // Format the result message
            jobContext.Result = $"Classified {context.GivingIdsSuccessful} giving {"group".PluralizeIf( context.GivingIdsSuccessful != 1 )}. There were {context.GivingIdsFailed} {"failure".PluralizeIf( context.GivingIdsFailed != 1 )}";

            if ( context.Errors.Any() )
            {
                var sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine( "Errors: " );

                foreach ( var error in context.Errors )
                {
                    sb.AppendLine( error );
                }

                var errorMessage = sb.ToString();
                jobContext.Result += errorMessage;
            }
        }

        #endregion Execute

        #region Settings and Attribute Helpers

        /// <summary>
        /// Gets the attribute key.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <returns></returns>
        private static string GetAttributeKey( GivingAnalyticsContext context, string guidString )
        {
            var key = AttributeCache.Get( guidString )?.Key;

            if ( key.IsNullOrWhiteSpace() )
            {
                context.Errors.Add( $"An attribute was excepted using the guid '{guidString}', but failed to resolve" );
            }

            return key;
        }

        /// <summary>
        /// Gets the giving unit attribute value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The guid string.</param>
        /// <returns></returns>
        private static string GetGivingUnitAttributeValue( GivingAnalyticsContext context, List<Person> people, string guidString )
        {
            if ( !people.Any() )
            {
                return string.Empty;
            }

            var key = GetAttributeKey( context, guidString );

            if ( key.IsNullOrWhiteSpace() )
            {
                // GetAttributeKey logs an error in the context
                return string.Empty;
            }

            var unitValue = people.First().GetAttributeValue( key );

            for ( var i = 1; i < people.Count; i++ )
            {
                var person = people[i];
                var personValue = person.GetAttributeValue( key );

                if ( unitValue != personValue )
                {
                    // The people in this giving unit have different values for this. We don't know which is actually correct, so assume no value.
                    return string.Empty;
                }
            }

            return unitValue;
        }

        /// <summary>
        /// Sets the giving unit attribute value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        private static void SetGivingUnitAttributeValue( GivingAnalyticsContext context, List<Person> people, string guidString, double? value, RockContext rockContext = null )
        {
            SetGivingUnitAttributeValue( context, people, guidString, value.ToStringSafe(), rockContext );
        }

        /// <summary>
        /// Sets the giving unit attribute value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        private static void SetGivingUnitAttributeValue( GivingAnalyticsContext context, List<Person> people, string guidString, decimal? value, RockContext rockContext = null )
        {
            SetGivingUnitAttributeValue( context, people, guidString, value.ToStringSafe(), rockContext );
        }

        /// <summary>
        /// Sets the giving unit attribute value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        private static void SetGivingUnitAttributeValue( GivingAnalyticsContext context, List<Person> people, string guidString, int? value, RockContext rockContext = null )
        {
            SetGivingUnitAttributeValue( context, people, guidString, value.ToStringSafe(), rockContext );
        }

        /// <summary>
        /// Sets the giving unit attribute value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        private static void SetGivingUnitAttributeValue( GivingAnalyticsContext context, List<Person> people, string guidString, DateTime? value, RockContext rockContext = null )
        {
            SetGivingUnitAttributeValue( context, people, guidString, value.ToISO8601DateString(), rockContext );
        }

        /// <summary>
        /// Sets the giving unit attribute value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <param name="value">The value.</param>
        /// <param name="rockContext">The rock context.</param>
        private static void SetGivingUnitAttributeValue( GivingAnalyticsContext context, List<Person> people, string guidString, string value, RockContext rockContext = null )
        {
            var key = GetAttributeKey( context, guidString );

            if ( key.IsNullOrWhiteSpace() )
            {
                // GetAttributeKey logs an error in the context
                return;
            }

            foreach ( var person in people )
            {
                person.SetAttributeValue( key, value );
            }
        }

        /// <summary>
        /// Gets the percent int. Ex: 50 and 200 => 25. This is safeguarded from 0 as a
        /// denominator by returning 0 in that case.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns></returns>
        private static int GetPercentInt( int numerator, int denominator )
        {
            if ( denominator == 0 )
            {
                return 0;
            }

            var asDecimal = decimal.Divide( numerator, denominator );
            return GetPercentInt( asDecimal );
        }

        /// <summary>
        /// Gets the percent int. Ex: .976 => 98
        /// </summary>
        /// <param name="percentDecimal">The percent decimal.</param>
        /// <returns></returns>
        private static int GetPercentInt( decimal percentDecimal )
        {
            return ( int ) decimal.Round( percentDecimal * 100 );
        }

        /// <summary>
        /// Gets the giving analytics settings.
        /// </summary>
        /// <returns></returns>
        private static GivingAnalyticsSetting GetGivingAnalyticsSettings()
        {
            return Rock.Web.SystemSettings
                .GetValue( SystemSetting.GIVING_ANALYTICS_CONFIGURATION )
                .FromJsonOrNull<GivingAnalyticsSetting>() ?? new GivingAnalyticsSetting();
        }

        /// <summary>
        /// Saves the giving analytics settings.
        /// </summary>
        /// <param name="givingAnalyticsSetting">The giving analytics setting.</param>
        private static void SaveGivingAnalyticsSettings( GivingAnalyticsSetting givingAnalyticsSetting )
        {
            Rock.Web.SystemSettings.SetValue( SystemSetting.GIVING_ANALYTICS_CONFIGURATION, givingAnalyticsSetting.ToJson() );
        }

        /// <summary>
        /// Gets the last run date time.
        /// </summary>
        /// <returns></returns>
        private static DateTime? LastRunDateTime
        {
            get
            {
                var settings = GetGivingAnalyticsSettings();
                return settings.GivingAnalytics.GivingAnalyticsLastRunDateTime;
            }
            set
            {
                var settings = GetGivingAnalyticsSettings();
                settings.GivingAnalytics.GivingAnalyticsLastRunDateTime = value;
                SaveGivingAnalyticsSettings( settings );
            }
        }

        /// <summary>
        /// Gets the last run date time.
        /// </summary>
        /// <returns></returns>
        private static decimal? GetGivingBinLowerLimit( int binIndex )
        {
            var settings = GetGivingAnalyticsSettings();
            var giverBin = settings.GivingAnalytics.GiverBins.Count > binIndex ?
                settings.GivingAnalytics.GiverBins[binIndex] :
                null;

            return giverBin?.LowerLimit;
        }

        /// <summary>
        /// Gets the last run date time.
        /// </summary>
        /// <returns></returns>
        private static void SetGivingBinLowerLimit( int binIndex, decimal? lowerLimit )
        {
            var settings = GetGivingAnalyticsSettings();

            if ( settings.GivingAnalytics.GiverBins == null )
            {
                settings.GivingAnalytics.GiverBins = new List<GiverBin>();
            }

            while ( settings.GivingAnalytics.GiverBins.Count <= binIndex )
            {
                settings.GivingAnalytics.GiverBins.Add( new GiverBin() );
            }

            var giverBin = settings.GivingAnalytics.GiverBins[binIndex];
            giverBin.LowerLimit = lowerLimit;
            SaveGivingAnalyticsSettings( settings );
        }

        /// <summary>
        /// Gets the earliest last gift date time.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private static DateTime GetEarliestLastGiftDateTime( GivingAnalyticsContext context )
        {
            var days = context.GetAttributeValue( AttributeKey.MaxDaysSinceLastGift ).AsIntegerOrNull() ??
                AttributeDefaultValue.MaxDaysSinceLastGift;
            return context.Now.AddDays( 0 - days );
        }

        /// <summary>
        /// Splits Interquartile Ranges.
        /// Ex: 1,2,3,4,5,6 => (1,2), (3,4), (5,6)
        /// </summary>
        /// <param name="orderedValues"></param>
        /// <returns></returns>
        private static (List<decimal>, List<decimal>, List<decimal>) SplitQuartileRanges( List<decimal> orderedValues )
        {
            var count = orderedValues.Count;

            if ( count <= 2 )
            {
                return (new List<decimal>(), orderedValues, new List<decimal>());
            }

            var lastMidIndex = count / 2;
            var isSingleMidIndex = count % 2 != 0;
            var firstMidIndex = isSingleMidIndex ? lastMidIndex : lastMidIndex - 1;

            var medianValues = isSingleMidIndex ?
                orderedValues.GetRange( firstMidIndex, 1 ) :
                orderedValues.GetRange( firstMidIndex, 2 );

            var q1 = orderedValues.GetRange( 0, firstMidIndex );
            var q3 = orderedValues.GetRange( lastMidIndex + 1, count - lastMidIndex - 1 );

            return (q1, medianValues, q3);
        }

        /// <summary>
        /// Gets the median range.
        /// Ex: 1,2,3,4,5,6 => 3,4
        /// </summary>
        /// <param name="orderedValues">The ordered values.</param>
        /// <returns></returns>
        private static List<decimal> GetMedianRange( List<decimal> orderedValues )
        {
            var ranges = SplitQuartileRanges( orderedValues );
            return ranges.Item2;
        }

        /// <summary>
        /// Gets the median.
        /// Ex: 1,2,3,4,5,6 => 3.5
        /// </summary>
        /// <param name="orderedValues">The ordered values.</param>
        /// <returns></returns>
        private static decimal GetMedian( List<decimal> orderedValues )
        {
            if ( orderedValues.Count == 0 )
            {
                return 0;
            }

            var medianRange = GetMedianRange( orderedValues );
            return medianRange.Average();
        }

        #endregion Settings and Attribute Helpers

        #region Execute Logic

        /// <summary>
        /// Hydrates the giving ids to classify.
        /// </summary>
        /// <param name="context">The context.</param>
        private static void HydrateGivingIdsToClassify( GivingAnalyticsContext context )
        {
            // Classification attributes need to be written for all adults with the same giver id in Rock. So Ted &
            // Cindy should have the same attribute values if they are set to contribute as a family even if Cindy
            // is always the one giving the gift.

            // We will reclassify anyone who has given since the last run of this job. This covers all alerts except
            // the "late gift" alert, which needs to find people based on the absense of a gift.
            using ( var rockContext = new RockContext() )
            {
                var financialTransactionService = new FinancialTransactionService( rockContext );

                // This is the people that have given since the last run date or the configured old gift date point.
                var minTransactionDate = LastRunDateTime ?? GetEarliestLastGiftDateTime( context );
                var givingIds = financialTransactionService.Queryable()
                    .AsNoTracking()
                    .Where( t => t.TransactionDateTime >= minTransactionDate )
                    .Select( t => t.AuthorizedPersonAlias.Person.GivingId )
                    .Distinct()
                    .ToList();

                // This transforms the set of people to classify into distinct giving ids.
                context.GivingIdsToClassify = givingIds;
            }
        }

        /// <summary>
        /// Updates the giver bins ranges.
        /// </summary>
        /// <param name="context">The context.</param>
        private static void UpdateGiverBinRanges( GivingAnalyticsContext context )
        {
            // First determine the ranges for each of the 4 giving bins by looking at all contribution transactions in the last 12 months.
            // These ranges will be updated in the Giving Analytics system settings.
            using ( var rockContext = new RockContext() )
            {
                var minDate = context.Now.AddMonths( -12 );
                var contributionTypeGuid = SystemGuid.DefinedValue.TRANSACTION_TYPE_CONTRIBUTION.AsGuid();

                var financialTransactionService = new FinancialTransactionService( rockContext );
                var givingGroups = financialTransactionService.Queryable()
                    .AsNoTracking()
                    .Where( t =>
                        t.TransactionDateTime.HasValue &&
                        t.TransactionDateTime > minDate &&
                        t.AuthorizedPersonAliasId.HasValue &&
                        t.TransactionTypeValue.Guid == contributionTypeGuid &&
                        t.AuthorizedPersonAlias.Person.GivingId != null &&
                        t.AuthorizedPersonAlias.Person.GivingId.Length > 0 )
                    .GroupBy( t => t.AuthorizedPersonAlias.Person.GivingId )
                    .Select( g => new
                    {
                        GivingId = g.Key,
                        Last12MonthsTotalGift = g.Sum( t => t.TransactionDetails.Sum( d => d.Amount ) )
                    } )
                    .ToList();

                givingGroups = givingGroups.OrderBy( g => g.Last12MonthsTotalGift ).ToList();
                var givingGroupCount = givingGroups.Count;

                // Calculate the current giving percentile lower amounts. This means the lower range of the 48th percentile will
                // be at index 48 of the array.
                if ( givingGroupCount > 0 )
                {
                    context.PercentileLowerRange = new List<decimal>();

                    for ( var i = 0; i < 100; i++ )
                    {
                        var percentDecimal = decimal.Divide( i, 100 );
                        var firstIndex = ( int ) decimal.Round( givingGroupCount * percentDecimal );

                        if ( firstIndex >= givingGroupCount )
                        {
                            firstIndex = givingGroupCount - 1;
                        }

                        context.PercentileLowerRange.Add( givingGroups[firstIndex].Last12MonthsTotalGift );
                    }
                }

                // These should be static, but just in case the count changes for some reason
                var percentileCount = context.PercentileLowerRange.Count;
                var firstBinStartIndex = ( int ) decimal.Round( percentileCount * GiverBinLowerPercentile.First );
                var secondBinStartIndex = ( int ) decimal.Round( percentileCount * GiverBinLowerPercentile.Second );
                var thirdBinStartIndex = ( int ) decimal.Round( percentileCount * GiverBinLowerPercentile.Third );

                SetGivingBinLowerLimit( 0, context.PercentileLowerRange[firstBinStartIndex] );
                SetGivingBinLowerLimit( 1, context.PercentileLowerRange[secondBinStartIndex] );
                SetGivingBinLowerLimit( 2, context.PercentileLowerRange[thirdBinStartIndex] );
                SetGivingBinLowerLimit( 3, context.PercentileLowerRange[0] );
            }
        }

        /// <summary>
        /// Processes the giving identifier.
        /// </summary>
        /// <param name="givingId">The giving identifier.</param>
        /// <param name="context">The context.</param>
        private static void ProcessGivingId( string givingId, GivingAnalyticsContext context )
        {
            using ( var rockContext = new RockContext() )
            {
                // Load the people that are in this giving group so their attribute values can be set
                var personService = new PersonService( rockContext );
                var people = personService.Queryable().Where( p => p.GivingId == givingId ).ToList();
                people.LoadAttributes();

                // Get the gifts from the past 12 months for the giving group
                var financialTransactionService = new FinancialTransactionService( rockContext );

                // Classifications for: % Scheduled, Gives As ___, Preferred Source, Preferred Currency will be based
                // off of all giving in the last 12 months.In the case of a tie in values( e.g. 50% credit card, 50%
                // cash ) use the most recent value as the tie breaker. This could be calculated with only one gift.
                var minDate = context.Now.AddMonths( -12 );
                var transactions = financialTransactionService.Queryable()
                    .AsNoTracking()
                    .Where( t =>
                        t.AuthorizedPersonAlias.Person.GivingId == givingId &&
                        t.TransactionDateTime >= minDate )
                    .Select( t => new TransactionView
                    {
                        TransactionDateTime = t.TransactionDateTime.Value,
                        TotalAmount = t.TransactionDetails.Sum( d => d.Amount ),
                        CurrencyTypeValueId = t.FinancialPaymentDetail.CurrencyTypeValueId,
                        SourceTypeValueId = t.SourceTypeValueId,
                        IsScheduled = t.ScheduledTransactionId.HasValue
                    } )
                    .ToList()
                    .OrderBy( t => t.TransactionDateTime )
                    .ToList();

                // We need to know if this giving group has other transactions. If they do then we do not need to
                // extrapolate because we have the complete 12 month data picture.
                var mostRecentOldTransactionDate = financialTransactionService.Queryable()
                    .AsNoTracking()
                    .OrderByDescending( t => t.TransactionDateTime )
                    .Where( t =>
                         t.AuthorizedPersonAlias.Person.GivingId == givingId &&
                         t.TransactionDateTime < minDate )
                    .Select( t => t.TransactionDateTime )
                    .FirstOrDefault();

                // If the group doesn't have FirstGiftDate attribute, set it by querying for the value
                var firstGiftDate = GetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_ERA_FIRST_GAVE ).AsDateTime();

                if ( !firstGiftDate.HasValue )
                {
                    firstGiftDate = financialTransactionService.Queryable()
                        .AsNoTracking()
                        .Where( t =>
                            t.AuthorizedPersonAlias.Person.GivingId == givingId &&
                            t.TransactionDateTime.HasValue )
                        .OrderBy( t => t.TransactionDateTime )
                        .Select( t => t.TransactionDateTime )
                        .FirstOrDefault();

                    SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_ERA_FIRST_GAVE, firstGiftDate );
                }

                // Update the attributes using the logic function
                var success = UpdateGivingUnitClassifications( givingId, people, transactions, mostRecentOldTransactionDate, context, minDate );

                if ( success )
                {
                    // Save all the attribute value changes
                    people.ForEach( p => p.SaveAttributeValues( rockContext ) );
                    rockContext.SaveChanges();
                    context.GivingIdsSuccessful++;
                }
                else
                {
                    context.GivingIdsFailed++;
                }
            }
        }

        /// <summary>
        /// Processes the giving identifier. This logic was isolated for automated testing.
        /// </summary>
        /// <param name="givingId">The giving identifier.</param>
        /// <param name="people">The people.</param>
        /// <param name="transactions">The past year transactions.</param>
        /// <param name="mostRecentOldTransactionDate">The most recent old transaction date.</param>
        /// <param name="context">The context.</param>
        /// <param name="minDate">The minimum date that the transactions were queried with.</param>
        /// <returns>
        /// True if success
        /// </returns>
        public static bool UpdateGivingUnitClassifications(
            string givingId,
            List<Person> people,
            List<TransactionView> transactions,
            DateTime? mostRecentOldTransactionDate,
            GivingAnalyticsContext context,
            DateTime minDate )
        {
            if ( transactions == null )
            {
                context.Errors.Add( $"the list of transactions was null for giving id {givingId}" );
                return false;
            }

            if ( people?.Any() != true )
            {
                context.Errors.Add( $"There were no people passed in the giving group {givingId}" );
                return false;
            }

            if ( people.Any( p => p.GivingId != givingId ) )
            {
                context.Errors.Add( $"The people (IDs: {people.Select( p => p.Id.ToString() ).JoinStringsWithCommaAnd()}) are not within the same giving group {givingId}" );
                return false;
            }

            if ( people.Any( p => p.Attributes == null ) )
            {
                context.Errors.Add( $"The people (IDs: {people.Select( p => p.Id.ToString() ).JoinStringsWithCommaAnd()}) did not have attributes loaded for giving group {givingId}" );
                return false;
            }

            // Update the groups lastgiftdate attribute
            var lastGiftDate = transactions.LastOrDefault().TransactionDateTime;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE, lastGiftDate );

            // Store percent scheduled
            var transactionCount = transactions.Count;
            var scheduledTransactionsCount = transactions.Count( t => t.IsScheduled );
            var percentScheduled = GetPercentInt( scheduledTransactionsCount, transactionCount );
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED, percentScheduled );

            // Store preferred source
            var sourceGroups = transactions.GroupBy( t => t.SourceTypeValueId ).OrderByDescending( g => g.Count() );
            var maxSourceCount = sourceGroups.FirstOrDefault()?.Count() ?? 0;
            var preferredSourceTransactions = sourceGroups
                .Where( g => g.Count() == maxSourceCount )
                .SelectMany( g => g.ToList() )
                .OrderByDescending( t => t.TransactionDateTime );

            var preferredSourceId = preferredSourceTransactions.FirstOrDefault()?.SourceTypeValueId;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE, preferredSourceId );

            // Store preferred currency
            var currencyGroups = transactions.GroupBy( t => t.CurrencyTypeValueId ).OrderByDescending( g => g.Count() );
            var maxCurrencyCount = currencyGroups.FirstOrDefault()?.Count() ?? 0;
            var preferredCurrencyTransactions = currencyGroups
                .Where( g => g.Count() == maxCurrencyCount )
                .SelectMany( g => g.ToList() )
                .OrderByDescending( t => t.TransactionDateTime );

            var preferredCurrencyId = preferredCurrencyTransactions.FirstOrDefault()?.CurrencyTypeValueId;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY, preferredCurrencyId );

            // ii.) Classifications for: Bin, Percentile
            //      a.) If there is 12 months of giving use that.
            //      b.) If not then use the current number of days of gifts to extrapolate a full year. So if you have 60
            //          days of giving, multiply the giving amount by 6.08( 356 / 60 ). But there must be at least 3 gifts.
            var extrapolationFactor = 1m;
            var hasMoreTransactions = mostRecentOldTransactionDate.HasValue;

            if ( !hasMoreTransactions )
            {
                var oldestGiftDate = transactions.FirstOrDefault()?.TransactionDateTime;
                var daysSinceOldestGift = oldestGiftDate == null ? 0d : ( context.Now - oldestGiftDate.Value ).TotalDays;
                var daysSinceMinDate = ( context.Now - minDate ).TotalDays;
                extrapolationFactor = Convert.ToDecimal( daysSinceOldestGift > 0d ? ( daysSinceMinDate / daysSinceOldestGift ) : 0d );

                if ( extrapolationFactor > 1m )
                {
                    extrapolationFactor = 1m;
                }
            }

            // Store bin
            var yearGiftAmount = transactions.Sum( t => t.TotalAmount ) * extrapolationFactor;
            var binIndex = 3;

            while ( binIndex >= 1 )
            {
                var lowerLimitForNextBin = GetGivingBinLowerLimit( binIndex - 1 );

                if ( !lowerLimitForNextBin.HasValue || yearGiftAmount >= lowerLimitForNextBin )
                {
                    binIndex--;
                }
                else
                {
                    break;
                }
            }

            var bin = binIndex + 1;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_BIN, bin );

            // Store percentile
            var percentileInt = 0;

            while ( percentileInt < ( context.PercentileLowerRange.Count - 1 ) )
            {
                var nextPercentileInt = percentileInt + 1;
                var nextPercentileLowerRange = context.PercentileLowerRange[nextPercentileInt];

                if ( yearGiftAmount >= nextPercentileLowerRange )
                {
                    percentileInt++;
                }
                else
                {
                    break;
                }
            }

            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE, percentileInt );

            // Update the last classification run date to now
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE, context.Now );

            // iii.) Classification for: Median Amount, IQR Amount, Mean Frequency, Frequency Standard Deviation
            //      a.) If there is 12 months of giving use all of those
            //      b.) If not use the previous gifts that are within 12 months but there must be at least 5 gifts.
            //      c.) For Amount: we will calulate the median and interquartile range
            //      d.) For Frequency: we will calculate the trimmed mean and standard deviation. The trimmed mean will
            //          exlcude the top 10 % largest and smallest gifts with in the dataset. If the number of gifts
            //          available is < 10 then we’ll remove the top largest and smallest gift.

            if ( transactionCount < 5 )
            {
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN, string.Empty );
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR, string.Empty );
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS, string.Empty );
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS, string.Empty );
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, string.Empty );
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE, string.Empty );
                return true;
            }

            // Interquartile range deals with finding the median. Then we say the numbers before the median numbers
            // are q1 and the numbers after are q1.
            // Ex: 50, 100, 101, 103, 103, 5000
            // Q1, Median, and then Q3: (50, 100), (101, 103), (103, 5000)
            // IQR is the median(Q3) - median(Q1)

            // Store median amount
            var orderedAmounts = transactions.Select( t => t.TotalAmount ).OrderBy( a => a ).ToList();
            var quartileRanges = SplitQuartileRanges( orderedAmounts );
            var medianAmount = quartileRanges.Item2.Average();
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN, medianAmount );

            // Store IQR amount
            var q1Median = GetMedian( quartileRanges.Item1 );
            var q3Median = GetMedian( quartileRanges.Item3 );
            var iqrAmount = q3Median - q1Median;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR, iqrAmount );

            // Create a parallel array that stores the days since the last transaction for the transaction at that index
            var daysSinceLastTransaction = new List<double?>();
            var lastTransactionDate = mostRecentOldTransactionDate;

            foreach ( var transaction in transactions )
            {
                var currentTransactionDate = transaction.TransactionDateTime;

                if ( lastTransactionDate.HasValue )
                {
                    var daysSince = ( currentTransactionDate - lastTransactionDate.Value ).TotalDays;
                    daysSinceLastTransaction.Add( daysSince );
                }
                else
                {
                    daysSinceLastTransaction.Add( null );
                }

                lastTransactionDate = currentTransactionDate;
            }

            // Store Mean Frequency
            var daysSinceLastTransactionWithValue = daysSinceLastTransaction.Where( d => d.HasValue ).Select( d => d.Value ).ToList();
            var meanFrequencyDays = daysSinceLastTransactionWithValue.Count > 0 ? daysSinceLastTransactionWithValue.Average() : 0;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS, meanFrequencyDays );

            // Store Frequency Std Dev
            var frequencyStdDevDays = Math.Sqrt( daysSinceLastTransactionWithValue.Average( d => Math.Pow( d - meanFrequencyDays, 2 ) ) );
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS, frequencyStdDevDays );

            // Frequency Labels:  
            //      Weekly = Avg days between 4.5 - 8.5; Std Dev< 7;
            //      2 Weeks = Avg days between 9 - 17; Std Dev< 10;
            //      Monthly = Avg days between 25 - 35; Std Dev< 10;
            //      Quarterly = Avg days between 80 - 110; Std Dev< 15;
            //      Erratic = Freq Avg / 2 < Std Dev;
            //      Undetermined = Everything else

            // Attribute value values: 1^Weekly, 2^Bi-Weekly, 3^Monthly, 4^Quarterly, 5^Erratic, 6^Undetermined
            if ( meanFrequencyDays >= 4.5d && meanFrequencyDays <= 8.5d && frequencyStdDevDays < 7d )
            {
                // Weekly
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, 1 );
            }
            else if ( meanFrequencyDays >= 9d && meanFrequencyDays <= 17d && frequencyStdDevDays < 10d )
            {
                // BiWeekly
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, 2 );
            }
            else if ( meanFrequencyDays >= 25d && meanFrequencyDays <= 35d && frequencyStdDevDays < 10d )
            {
                // Monthly
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, 3 );
            }
            else if ( meanFrequencyDays >= 80d && meanFrequencyDays <= 110d && frequencyStdDevDays < 15d )
            {
                // Quarterly
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, 4 );
            }
            else if ( ( meanFrequencyDays / 2 ) < frequencyStdDevDays )
            {
                // Erratic
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, 5 );
            }
            else
            {
                // Undetermined
                SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, 6 );
            }

            // Update the next expected gift date
            var nextExpectedGiftDate = lastTransactionDate.HasValue ? lastTransactionDate.Value.AddDays( meanFrequencyDays ) : ( DateTime? ) null;
            SetGivingUnitAttributeValue( context, people, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE, nextExpectedGiftDate );

            return true;
        }

        #endregion Execute Logic
    }

    /// <summary>
    /// Giving Analytics Context
    /// </summary>
    public sealed class GivingAnalyticsContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GivingAnalyticsContext"/> class.
        /// </summary>
        /// <param name="jobExecutionContext">The job execution context.</param>
        public GivingAnalyticsContext( IJobExecutionContext jobExecutionContext )
        {
            JobExecutionContext = jobExecutionContext;
            JobDataMap = jobExecutionContext.JobDetail.JobDataMap;
        }

        /// <summary>
        /// The date time to consider as current time. The time when this processing instance began
        /// </summary>
        public readonly DateTime Now = RockDateTime.Now;

        /// <summary>
        /// The errors
        /// </summary>
        public readonly HashSet<string> Errors = new HashSet<string>();

        /// <summary>
        /// Gets the job execution context.
        /// </summary>
        /// <value>
        /// The job execution context.
        /// </value>
        public IJobExecutionContext JobExecutionContext { get; }

        /// <summary>
        /// Gets the job data map.
        /// </summary>
        /// <value>
        /// The job data map.
        /// </value>
        public JobDataMap JobDataMap { get; }

        /// <summary>
        /// Gets or sets the giving ids to classify.
        /// </summary>
        /// <value>
        /// The giving ids to classify.
        /// </value>
        public List<string> GivingIdsToClassify { get; set; }

        /// <summary>
        /// Gets or sets the giving ids classified.
        /// </summary>
        /// <value>
        /// The giving ids classified.
        /// </value>
        public int GivingIdsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the giving ids failed.
        /// </summary>
        /// <value>
        /// The giving ids failed.
        /// </value>
        public int GivingIdsFailed { get; set; }

        /// <summary>
        /// Gets or sets the percentile lower range.
        /// Ex. Index 50 holds the lower range for being in the 50th percentile of the givers within the church
        /// </summary>
        /// <value>
        /// The percentile lower range.
        /// </value>
        public List<decimal> PercentileLowerRange { get; set; }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetAttributeValue( string key )
        {
            return JobDataMap.GetString( key );
        }
    }

    /// <summary>
    /// Transaction View
    /// </summary>
    public sealed class TransactionView
    {
        /// <summary>
        /// Gets or sets the transaction date time.
        /// </summary>
        /// <value>
        /// The transaction date time.
        /// </value>
        public DateTime TransactionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the currency type value identifier.
        /// </summary>
        /// <value>
        /// The currency type value identifier.
        /// </value>
        public int? CurrencyTypeValueId { get; set; }

        /// <summary>
        /// Gets or sets the source type value identifier.
        /// </summary>
        /// <value>
        /// The source type value identifier.
        /// </value>
        public int? SourceTypeValueId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is scheduled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is scheduled; otherwise, <c>false</c>.
        /// </value>
        public bool IsScheduled { get; set; }
    }
}