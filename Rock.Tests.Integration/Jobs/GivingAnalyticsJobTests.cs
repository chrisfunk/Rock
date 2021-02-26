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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Jobs;
using Rock.Model;
using Rock.SystemKey;
using Rock.Utility.Settings.GivingAnalytics;
using Rock.Web.Cache;

namespace Rock.Tests.Integration.Jobs
{
    [TestClass]
    public class GivingAnalyticsJobTests
    {
        /// <summary>
        /// Tests an example a giving family
        /// </summary>
        [TestMethod]
        public void UpdateGivingUnitClassifications_ClassifiesCorrectly()
        {
            var settings = new GivingAnalyticsSetting();
            settings.GivingAnalytics.GiverBins = new List<GiverBin> {
                new GiverBin { LowerLimit = 20000 },
                new GiverBin { LowerLimit = 10000 },
                new GiverBin { LowerLimit = 1000 },
                new GiverBin { LowerLimit = 0 }
            };

            Rock.Web.SystemSettings.SetValue( SystemSetting.GIVING_ANALYTICS_CONFIGURATION, settings.ToJson() );

            var givingGroupId = 800;
            var givingId = $"G{givingGroupId}";

            var firstCurrencyTypeValueId = 22222;
            var secondCurrencyTypeValueId = 44444;

            var firstSourceTypeValueId = 33333;
            var secondSourceTypeValueId = 55555;

            var mostRecentOldTransactionDate = new DateTime( 2019, 12, 29 );
            var minDate = new DateTime( 2020, 1, 1 );

            var jobExecutionContext = new TestJobContext();
            var context = new GivingAnalyticsContext( jobExecutionContext )
            {
                PercentileLowerRange = new List<decimal>()
            };

            for ( var i = 0; i < 100; i++ )
            {
                context.PercentileLowerRange.Add( 200 * i );
            }

            var people = new List<Person>
            {
                new Person
                {
                    GivingGroupId = givingGroupId
                },
                new Person
                {
                    GivingGroupId = givingGroupId
                }
            };

            people.ForEach( p => p.LoadAttributes() );

            var transactions = new List<TransactionView>();

            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 28 ),
                CurrencyTypeValueId = firstCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 750.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 11 ),
                CurrencyTypeValueId = firstCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 1150.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 26 ),
                CurrencyTypeValueId = firstCurrencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 11 ),
                CurrencyTypeValueId = firstCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 11 ),
                CurrencyTypeValueId = firstCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 11 ),
                CurrencyTypeValueId = firstCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 11 ),
                CurrencyTypeValueId = secondCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 11 ),
                CurrencyTypeValueId = secondCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = firstSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 11 ),
                CurrencyTypeValueId = secondCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = secondSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 11 ),
                CurrencyTypeValueId = secondCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = secondSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 11 ),
                CurrencyTypeValueId = secondCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = secondSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 11 ),
                CurrencyTypeValueId = secondCurrencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = secondSourceTypeValueId,
                TotalAmount = 1200.0000000000m,
            } );

            Rock.Jobs.GivingAnalytics.UpdateGivingUnitClassifications( givingId, people, transactions, mostRecentOldTransactionDate, context, minDate );

            Assert.AreEqual( 0, context.Errors.Count );

            // Preferred Currency - Defined Type
            var firstPerson = people.First();
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY );
            var preferredCurrency = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY ).AsIntegerOrNull();
            Assert.AreEqual( secondCurrencyTypeValueId, preferredCurrency );

            // Preferred Source - Defined Type
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE );
            var preferredSource = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE ).AsIntegerOrNull();
            Assert.AreEqual( firstSourceTypeValueId, preferredSource );

            // Frequency Label - Single Select (1^Weekly, 2^Bi-Weekly, 3^Monthly, 4^Quarterly, 5^Erratic, 6^Undetermined)
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL );
            var frequencyLabel = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL ).AsIntegerOrNull();
            Assert.AreEqual( 3, frequencyLabel );

            // Percent of Gifts Scheduled - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED );
            var percentScheduled = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED ).AsIntegerOrNull();
            Assert.AreEqual( 92, percentScheduled );

            // Gift Amount: Median - Currency
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN );
            var medianGivingAmount = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN ).AsDecimalOrNull();
            Assert.AreEqual( 1200.00m, decimal.Round( medianGivingAmount ?? 0, 2 ) );

            // Gift Amount: IQR - Currency
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR );
            var iqr = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR ).AsDecimalOrNull();
            Assert.AreEqual( 50.00m, decimal.Round( iqr ?? 0, 2 ) );

            // Gift Frequency Days: Mean - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS );
            var meanFrequencyDays = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS ).AsDecimalOrNull();
            Assert.AreEqual( 26.50m, decimal.Round( meanFrequencyDays ?? 0, 2 ) );

            // Gift Frequency Days: Standard Deviation - Number 
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS );
            var stdDevFrequencyDays = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS ).AsDecimalOrNull();
            Assert.AreEqual( 7.04m, decimal.Round( stdDevFrequencyDays ?? 0, 2 ) );

            // Giving Bin - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_BIN );
            var bin = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_BIN ).AsIntegerOrNull();
            Assert.AreEqual( 2, bin );

            // Giving Percentile - Number - This will be rounded to the nearest percent and stored as a whole number (15 vs .15)
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE );
            var percentile = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE ).AsIntegerOrNull();
            Assert.AreEqual( 66, percentile );

            // Last Gift Date - Exists, but link to the ‘Giving Analytics’ category
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE );
            var lastGave = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE ).AsDateTime();
            Assert.AreEqual( new DateTime( 2020, 11, 11 ), lastGave );

            // Next Expected Gift Date - Date
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE );
            var nextExpected = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE ).AsDateTime();
            Assert.AreEqual( lastGave.Value.AddDays( ( double ) meanFrequencyDays.Value ), nextExpected );

            // Next Expected Gift Date - Date
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE );
            var lastClassified = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE ).AsDateTime();
            Assert.AreEqual( context.Now, lastClassified );
        }

        /// <summary>
        /// Gets the attribute key.
        /// </summary>
        /// <param name="guidString">The unique identifier string.</param>
        /// <returns></returns>
        private static string GetAttributeKey( string guidString )
        {
            var key = AttributeCache.Get( guidString )?.Key;

            if ( key.IsNullOrWhiteSpace() )
            {
                return "%$$$ KEY DOES NOT EXIST $$$%";
            }

            return key;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="guidString">The unique identifier string.</param>
        /// <returns></returns>
        private static string GetAttributeValue( Person person, string guidString )
        {
            var key = GetAttributeKey( guidString );
            return person.GetAttributeValue( key );
        }

        /// <summary>
        /// Asserts the people have same attribute value.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="guidString">The unique identifier string.</param>
        /// <returns></returns>
        private static void AssertPeopleHaveSameAttributeValue( List<Person> people, string guidString )
        {
            var value = GetAttributeValue( people[0], guidString );

            for ( var i = 1; i < people.Count; i++ )
            {
                var otherValue = GetAttributeValue( people[i], guidString );
                Assert.AreEqual( value, otherValue );
            }
        }
    }
}
