﻿// <copyright>
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
namespace Rock.Plugin.HotFixes
{
    /// <summary>
    /// Plugin Migration. The migration number jumps to 83 because 75-82 were moved to EF migrations and deleted.
    /// </summary>
    [MigrationNumber( 91, "1.9.0" )]
    public class SecureGetImpersonationParameter : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            UpdateRestSecurity();
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            // Not yet used by hotfix migrations.
        }

        /// <summary>
        /// ED: Restrict GetImpersonationParameter API Endpoint to Rock Administrators
        /// </summary>
        private void UpdateRestSecurity()
        {
            Sql( @"
                IF NOT EXISTS (SELECT [Id] FROM [RestController] WHERE [ClassName] = 'Rock.Rest.Controllers.PeopleController') 

                IF NOT EXISTS (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/People/GetImpersonationParameter?personId={personId}&expireDateTime={expireDateTime}&usageLimit={usageLimit}&pageId={pageId}')

                DECLARE @RestActionEntityTypeId INT = (SELECT [Id] FROM [EntityType] WHERE [Guid] = 'D4F7F055-5351-4ADF-9F8D-4802CAD6CC9D')
                DECLARE @GetImpersonationParameterRestActionId INT = (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/People/GetImpersonationParameter?personId={personId}&expireDateTime={expireDateTime}&usageLimit={usageLimit}&pageId={pageId}')
                DECLARE @RockAdminSecurityGroupId INT = (SELECT [Id] FROM [Group] WHERE [Guid] = '628C51A8-4613-43ED-A18D-4A6FB999273E')

                -- There is already user defined security on this don't change it.
                IF NOT EXISTS(SELECT * FROM Auth WHERE EntityTypeId = @RestActionEntityTypeId AND EntityId = @GetImpersonationParameterRestActionId)
                BEGIN
                    INSERT INTO [Auth] ([EntityTypeId], [EntityId], [Order], [Action], [AllowOrDeny], [SpecialRole], [GroupId], [Guid])
                END" );
        }
    }
}