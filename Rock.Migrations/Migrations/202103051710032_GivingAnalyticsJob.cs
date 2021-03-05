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
    /// <summary>
    ///
    /// </summary>
    public partial class GivingAnalyticsJob : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            Sql( $@"
                INSERT INTO ServiceJob (
                    IsSystem,
                    IsActive,
                    Name,
                    Description,
                    Class,
                    Guid,
                    CreatedDateTime,
                    NotificationStatus,
                    CronExpression
                ) VALUES (
                    1, -- IsSystem
                    1, -- IsActive
                    'Giving Analytics', -- Name
                    'Job that updates giving classification attributes as well as creating giving alerts.', -- Description
                    'Rock.Jobs.GivingAnalytics', -- Class
                    '{Rock.SystemGuid.ServiceJob.GIVING_ANALYTICS}', -- Guid
                    GETDATE(), -- Created
                    1, -- All notifications
                    '0 0 22 * * ?' -- Cron: 10pm everyday
                );" );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( $"DELETE FROM ServiceJob WHERE Guid = '{Rock.SystemGuid.ServiceJob.GIVING_ANALYTICS}';" );
        }
    }
}
