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
namespace Rock.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    /// <summary>
    ///
    /// </summary>
    public partial class GivingAnalyticsPersonAttributes : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            var givingAnalyticsCategory = new List<string>() { SystemGuid.Category.PERSON_ATTRIBUTES_GIVING_ANALYTICS };
            var givingAnalyticsAndEraCategory = new List<string>() { SystemGuid.Category.PERSON_ATTRIBUTES_GIVING_ANALYTICS, SystemGuid.Category.PERSON_ATTRIBUTES_ERA };

            RockMigrationHelper.UpdatePersonAttributeCategory( "Giving Analytics", "fas fa-hand-holding-usd", "Attributes that describe the most recent classification of this person's giving habits", SystemGuid.Category.PERSON_ATTRIBUTES_GIVING_ANALYTICS );

            // Person Attribute "Last Gave"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"6B6AA175-4758-453F-8D83-FCD8044B5F36", givingAnalyticsAndEraCategory, @"Last Gave", @"", @"core_EraLastGave", @"", @"", 6, @"", @"02F64263-E290-399E-4487-FC236F4DE81F" );

            // Person Attribute "First Gave"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"6B6AA175-4758-453F-8D83-FCD8044B5F36", givingAnalyticsAndEraCategory, @"First Gave", @"", @"core_EraFirstGave", @"", @"", 6, @"", @"EE5EC76A-D4B9-56B5-4B48-29627D945F10" );

            // Person Attribute "Preferred Currency"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"59D5A94C-94A0-4630-B80A-BB25697D74C7", givingAnalyticsCategory, @"Preferred Currency", @"Preferred Currency", @"PreferredCurrency", @"", @"The most used means of giving that this person employed in the past 12 months.", 1041, @"", SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY, @"definedtype", @"10", @"73E158CE-81E8-44DB-B3DC-F8AA2CF1B4B1" );

            // Person Attribute "Preferred Source"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"59D5A94C-94A0-4630-B80A-BB25697D74C7", givingAnalyticsCategory, @"Preferred Source", @"Preferred Source", @"PreferredSource", @"", @"The most used giving source (kiosk, app, web) that this person employed in the past 12 months.", 1042, @"", SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE, @"definedtype", @"12", @"B51B7E3B-5D14-41E1-BA5C-9DEBDA2528A7" );

            // Person Attribute "Frequency Label"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"7525C4CB-EE6B-41D4-9B64-A08048D5A5C0", givingAnalyticsCategory, @"Frequency Label", @"Frequency Label", @"FrequencyLabel", @"", @"The frequency that this person typically has given in the past 12 months.", 1043, @"", SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, @"fieldtype", @"ddl", @"73637E4E-F872-4066-8E04-B4A8429F6FD6" );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL, @"values", @"1^Weekly, 2^Bi-Weekly, 3^Monthly, 4^Quarterly, 5^Erratic, 6^Undetermined", @"DEDE252F-E8FF-4858-A616-BBE6A6FB95FF" );

            // Person Attribute "Percent of Gifts Scheduled"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"A75DFC58-7A1B-4799-BF31-451B2BBE38FF", givingAnalyticsCategory, @"Percent of Gifts Scheduled", @"Percent of Gifts Scheduled", @"PercentofGiftsScheduled", @"", @"The percent of gifts in the past 12 months that have been part of a scheduled transaction. Note that this is stored as an integer. Ex: 15% is stored as 15.", 1044, @"", SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED );

            // Person Attribute "Gift Amount: IQR"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"3EE69CBC-35CE-4496-88CC-8327A447603F", givingAnalyticsCategory, @"Gift Amount: IQR", @"Gift Amount: IQR", @"GiftAmountIQR", @"", @"The gift amount interquartile range calculated from the past 12 months of giving.", 1046, @"", SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR );

            // Person Attribute "Gift Amount: Median"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"3EE69CBC-35CE-4496-88CC-8327A447603F", givingAnalyticsCategory, @"Gift Amount: Median", @"Gift Amount: Median", @"GiftAmountMedian", @"", @"The median gift amount given in the past 12 months.", 1047, @"", SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN );

            // Person Attribute "Gift Frequency Days: Mean"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"C757A554-3009-4214-B05D-CEA2B2EA6B8F", givingAnalyticsCategory, @"Gift Frequency Days: Mean", @"Gift Frequency Days: Mean", @"GiftFrequencyDaysMean", @"", @"The mean days between gifts given in the past 12 months.", 1048, @"", SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS );

            // Person Attribute "Gift Frequency Days: Standard Deviation"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"C757A554-3009-4214-B05D-CEA2B2EA6B8F", givingAnalyticsCategory, @"Gift Frequency Days: Standard Deviation", @"Gift Frequency Days: Standard Deviation", @"GiftFrequencyDaysStandardDeviation", @"", @"The standard deviation for the number of days between gifts given in the past 12 months.", 1049, @"", SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS );

            // Person Attribute "Giving Bin"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"A75DFC58-7A1B-4799-BF31-451B2BBE38FF", givingAnalyticsCategory, @"Giving Bin", @"Giving Bin", @"GivingBin", @"", @"The bin that this person's giving habits fall within.", 1050, @"", SystemGuid.Attribute.PERSON_GIVING_BIN );

            // Person Attribute "Giving Percentile"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"A75DFC58-7A1B-4799-BF31-451B2BBE38FF", givingAnalyticsCategory, @"Giving Percentile", @"Giving Percentile", @"GivingPercentile", @"", @"Within the context of all givers over the past twelve months, this is the percentile for this person.  Note that this is stored as an integer. Ex: 15% is stored as 15.", 1051, @"", SystemGuid.Attribute.PERSON_GIVING_PERCENTILE );

            // Person Attribute "Next Expected Gift Date"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"6B6AA175-4758-453F-8D83-FCD8044B5F36", givingAnalyticsCategory, @"Next Expected Gift Date", @"Next Expected Gift Date", @"NextExpectedGiftDate", @"", @"The date, based on giving habits, that this person is next anticipated to give.", 1052, @"", SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE, @"datePickerControlType", @"Date Picker", @"65E9B929-9A43-4CE6-AD8F-7055BEE35CF4" );

            // Person Attribute "Last Classification Run Date Time"
            RockMigrationHelper.AddOrUpdatePersonAttributeByGuid( @"6B6AA175-4758-453F-8D83-FCD8044B5F36", givingAnalyticsCategory, @"Last Classification Run Date Time", @"Last Classification Run Date Time", @"LastClassificationRunDateTime", @"", @"The date that this person's giving analytics were last classified.", 1053, @"", SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE );
            RockMigrationHelper.AddAttributeQualifier( SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE, @"datePickerControlType", @"Date Picker", @"8819FE04-D1A6-45DE-9BD4-7C1AD9AD9F63" );

            // Security on all new attributes: admins, financial roles, and deny everyone else
            var guidStrings = new string[] {
                SystemGuid.Attribute.PERSON_GIVING_PREFERRED_CURRENCY,
                SystemGuid.Attribute.PERSON_GIVING_PREFERRED_SOURCE,
                SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_LABEL,
                SystemGuid.Attribute.PERSON_GIVING_PERCENT_SCHEDULED,
                SystemGuid.Attribute.PERSON_GIVING_AMOUNT_MEDIAN,
                SystemGuid.Attribute.PERSON_GIVING_AMOUNT_IQR,
                SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_MEAN_DAYS,
                SystemGuid.Attribute.PERSON_GIVING_FREQUENCY_STD_DEV_DAYS,
                SystemGuid.Attribute.PERSON_GIVING_BIN,
                SystemGuid.Attribute.PERSON_GIVING_PERCENTILE,
                SystemGuid.Attribute.PERSON_GIVING_NEXT_EXPECTED_GIFT_DATE,
                SystemGuid.Attribute.PERSON_GIVING_LAST_CLASSIFICATION_DATE
            };

            foreach ( var guidString in guidStrings )
            {
                RockMigrationHelper.AddSecurityAuthForAttribute( guidString, 0, Security.Authorization.VIEW, true, SystemGuid.Group.GROUP_ADMINISTRATORS, 0, Guid.NewGuid().ToString() );
                RockMigrationHelper.AddSecurityAuthForAttribute( guidString, 1, Security.Authorization.VIEW, true, SystemGuid.Group.GROUP_FINANCE_ADMINISTRATORS, 0, Guid.NewGuid().ToString() );
                RockMigrationHelper.AddSecurityAuthForAttribute( guidString, 2, Security.Authorization.VIEW, true, SystemGuid.Group.GROUP_FINANCE_USERS, 0, Guid.NewGuid().ToString() );
                RockMigrationHelper.AddSecurityAuthForAttribute( guidString, 3, Security.Authorization.VIEW, false, null, 1, Guid.NewGuid().ToString() );
            }
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }
    }
}
