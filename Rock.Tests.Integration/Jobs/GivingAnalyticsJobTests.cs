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
        /// Tests an example giving family
        /// </summary>
        [TestMethod]
        public void UpdateGivingUnitClassifications_ClassifiesMonthlyCorrectly()
        {
            SetGivingAnalyticsSetting( 20000, 10000, 1000, 0 );

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

            // Last Classified - Date
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE );
            var lastClassified = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE ).AsDateTime();
            Assert.AreEqual( context.Now, lastClassified );
        }

        /// <summary>
        /// Tests an example a giving family
        /// </summary>
        [TestMethod]
        public void UpdateGivingUnitClassifications_ClassifiesWeeklyCorrectly()
        {
            SetGivingAnalyticsSetting( 20000, 10000, 1000, 0 );

            var givingGroupId = 900;
            var givingId = $"G{givingGroupId}";

            var currencyTypeValueId = 808080;
            var sourceTypeValueId = 909090;

            var mostRecentOldTransactionDate = new DateTime( 2019, 12, 28 );
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
                },
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
                TransactionDateTime = new DateTime( 2020, 1, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 11 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 18 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 25 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 1 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 15 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 22 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 29 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 7 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 14 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 21 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 11 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 18 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 25 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 2 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 9 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 23 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 30 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 6 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 20 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 27 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 11 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 18 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 25 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 1 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 15 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 22 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 29 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 5 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 12 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 100000.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 19 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 26 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 10 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 17 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 31 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 7 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 14 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = true,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 500.0000000000m,
            } );

            Rock.Jobs.GivingAnalytics.UpdateGivingUnitClassifications( givingId, people, transactions, mostRecentOldTransactionDate, context, minDate );

            Assert.AreEqual( 0, context.Errors.Count );

            // Preferred Currency - Defined Type
            var firstPerson = people.First();
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY );
            var preferredCurrency = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY ).AsIntegerOrNull();
            Assert.AreEqual( currencyTypeValueId, preferredCurrency );

            // Preferred Source - Defined Type
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE );
            var preferredSource = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE ).AsIntegerOrNull();
            Assert.AreEqual( sourceTypeValueId, preferredSource );

            // Frequency Label - Single Select (1^Weekly, 2^Bi-Weekly, 3^Monthly, 4^Quarterly, 5^Erratic, 6^Undetermined)
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL );
            var frequencyLabel = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL ).AsIntegerOrNull();
            Assert.AreEqual( 1, frequencyLabel );

            // Percent of Gifts Scheduled - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED );
            var percentScheduled = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED ).AsIntegerOrNull();
            Assert.AreEqual( 98, percentScheduled );

            // Gift Amount: Median - Currency
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN );
            var medianGivingAmount = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN ).AsDecimalOrNull();
            Assert.AreEqual( 500.00m, decimal.Round( medianGivingAmount ?? 0, 2 ) );

            // Gift Amount: IQR - Currency
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR );
            var iqr = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR ).AsDecimalOrNull();
            Assert.AreEqual( 0.00m, decimal.Round( iqr ?? 0, 2 ) );

            // Gift Frequency Days: Mean - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS );
            var meanFrequencyDays = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS ).AsDecimalOrNull();
            Assert.AreEqual( 6.85m, decimal.Round( meanFrequencyDays ?? 0, 2 ) );

            // Gift Frequency Days: Standard Deviation - Number 
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS );
            var stdDevFrequencyDays = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS ).AsDecimalOrNull();
            Assert.AreEqual( 0.87m, decimal.Round( stdDevFrequencyDays ?? 0, 2 ) );

            // Giving Bin - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_BIN );
            var bin = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_BIN ).AsIntegerOrNull();
            Assert.AreEqual( 1, bin );

            // Giving Percentile - Number - This will be rounded to the nearest percent and stored as a whole number (15 vs .15)
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE );
            var percentile = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE ).AsIntegerOrNull();
            Assert.AreEqual( 99, percentile );

            // Last Gift Date - Exists, but link to the ‘Giving Analytics’ category
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE );
            var lastGave = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE ).AsDateTime();
            Assert.AreEqual( new DateTime( 2020, 11, 14 ), lastGave );

            // Next Expected Gift Date - Date
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE );
            var nextExpected = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE ).AsDateTime();
            Assert.AreEqual( lastGave.Value.AddDays( ( double ) meanFrequencyDays.Value ), nextExpected );

            // Last Classified - Date
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE );
            var lastClassified = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE ).AsDateTime();
            Assert.AreEqual( context.Now, lastClassified );
        }

        /// <summary>
        /// Tests an example a giving family
        /// </summary>
        [TestMethod]
        public void UpdateGivingUnitClassifications_ClassifiesErraticCorrectly()
        {
            SetGivingAnalyticsSetting( 20000, 10000, 1000, 0 );

            var personId = 1111;
            var givingId = $"P{personId}";

            var currencyTypeValueId = 112233;
            var sourceTypeValueId = 445566;

            var mostRecentOldTransactionDate = new DateTime( 2019, 12, 27 );
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
                    Id = personId
                },
            };

            people.ForEach( p => p.LoadAttributes() );

            var transactions = new List<TransactionView>();
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 50.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 180.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 18 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 82.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 45.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 1, 31 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 155.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 85.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 15 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 140.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 15 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 30.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 22 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 115.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 29 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 150.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 2, 29 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 82.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 6 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 130.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 66.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 150.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 20 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 10.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 20 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 97.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 90.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 140.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 3, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 10.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 63.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 17.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 10 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 81.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 10 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 19.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 15 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 120.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 15 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 120.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 17 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 12.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 17 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 98.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 112.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 18.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 150.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 4, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 200.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 1 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 10.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 1 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 110.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 7 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 130.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 7 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 25.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 130.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 17.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 14 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 25.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 120.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 20.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 19 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 35.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 21 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 130.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 21 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 135.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 22 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 12.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 22 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 108.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 29 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 10.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 5, 29 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 110.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 75.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 140.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 22.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 98.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 116.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 14.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 20.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 18 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 140.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 19 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 18.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 19 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 132.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 25 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 80.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 26 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 6, 26 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 105.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 100.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 107.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 13.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 5 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 35.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 10 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 85.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 12 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 100.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 12 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 20.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 17 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 135.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 20 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 110.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 70.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 25 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 50.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 25 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 80.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 7, 30 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 150.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 1 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 1 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 75.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 10.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 8 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 80.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 120.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 75.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 13.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 77.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 14 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 130.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 73.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 8, 28 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 17.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 18.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 4 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 72.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 10 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 130.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 96.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 14.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 18 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 66.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 24 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 175.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 26 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 20.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 9, 26 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 110.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 3 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 95.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 9 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 125.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 9 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 136.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 9 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 14.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 95.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 82.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 16 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 18.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 22 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 125.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 23 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 12.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 23 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 70.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 30 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 150.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 30 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 110.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 30 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 70.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 10, 30 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 6 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 190.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 6 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 10.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 9 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 175.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 85.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 13 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 15.0000000000m,
            } );
            transactions.Add( new TransactionView
            {
                TransactionDateTime = new DateTime( 2020, 11, 14 ),
                CurrencyTypeValueId = currencyTypeValueId,
                IsScheduled = false,
                SourceTypeValueId = sourceTypeValueId,
                TotalAmount = 140.0000000000m,
            } );

            Rock.Jobs.GivingAnalytics.UpdateGivingUnitClassifications( givingId, people, transactions, mostRecentOldTransactionDate, context, minDate );

            Assert.AreEqual( 0, context.Errors.Count );

            // Preferred Currency - Defined Type
            var firstPerson = people.First();
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY );
            var preferredCurrency = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY ).AsIntegerOrNull();
            Assert.AreEqual( currencyTypeValueId, preferredCurrency );

            // Preferred Source - Defined Type
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE );
            var preferredSource = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE ).AsIntegerOrNull();
            Assert.AreEqual( sourceTypeValueId, preferredSource );

            // Frequency Label - Single Select (1^Weekly, 2^Bi-Weekly, 3^Monthly, 4^Quarterly, 5^Erratic, 6^Undetermined)
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL );
            var frequencyLabel = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL ).AsIntegerOrNull();
            Assert.AreEqual( 5, frequencyLabel );

            // Percent of Gifts Scheduled - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED );
            var percentScheduled = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED ).AsIntegerOrNull();
            Assert.AreEqual( 0, percentScheduled );

            // Gift Amount: Median - Currency
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN );
            var medianGivingAmount = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN ).AsDecimalOrNull();
            Assert.AreEqual( 81.00m, decimal.Round( medianGivingAmount ?? 0, 2 ) );

            // Gift Amount: IQR - Currency
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR );
            var iqr = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR ).AsDecimalOrNull();
            Assert.AreEqual( 101.50m, decimal.Round( iqr ?? 0, 2 ) );

            // Gift Frequency Days: Mean - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS );
            var meanFrequencyDays = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS ).AsDecimalOrNull();
            Assert.AreEqual( 2.76m, decimal.Round( meanFrequencyDays ?? 0, 2 ) );

            // Gift Frequency Days: Standard Deviation - Number 
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS );
            var stdDevFrequencyDays = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS ).AsDecimalOrNull();
            Assert.AreEqual( 3.17m, decimal.Round( stdDevFrequencyDays ?? 0, 2 ) );

            // Giving Bin - Number
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_BIN );
            var bin = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_BIN ).AsIntegerOrNull();
            Assert.AreEqual( 3, bin );

            // Giving Percentile - Number - This will be rounded to the nearest percent and stored as a whole number (15 vs .15)
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE );
            var percentile = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_PERCENTILE ).AsIntegerOrNull();
            Assert.AreEqual( 45, percentile );

            // Last Gift Date - Exists, but link to the ‘Giving Analytics’ category
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE );
            var lastGave = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_ERA_LAST_GAVE ).AsDateTime();
            Assert.AreEqual( new DateTime( 2020, 11, 14 ), lastGave );

            // Next Expected Gift Date - Date
            AssertPeopleHaveSameAttributeValue( people, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE );
            var nextExpected = GetAttributeValue( firstPerson, SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE ).AsDateTime();
            Assert.AreEqual( lastGave.Value.AddDays( ( double ) meanFrequencyDays.Value ), nextExpected );

            // Last Classified - Date
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

        /// <summary>
        /// Creates the giving analytics setting.
        /// </summary>
        /// <param name="bin1">The bin1.</param>
        /// <param name="bin2">The bin2.</param>
        /// <param name="bin3">The bin3.</param>
        /// <param name="bin4">The bin4.</param>
        /// <returns></returns>
        private static void SetGivingAnalyticsSetting( decimal bin1, decimal bin2, decimal bin3, decimal bin4 )
        {
            var settings = new GivingAnalyticsSetting();
            settings.GivingAnalytics.GiverBins = new List<GiverBin> {
                new GiverBin { LowerLimit = bin1 },
                new GiverBin { LowerLimit = bin2 },
                new GiverBin { LowerLimit = bin3 },
                new GiverBin { LowerLimit = bin4 }
            };

            Rock.Web.SystemSettings.SetValue( SystemSetting.GIVING_ANALYTICS_CONFIGURATION, settings.ToJson() );
        }
    }
}
