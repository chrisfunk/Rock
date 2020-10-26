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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Quartz;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;

namespace Rock.Jobs
{
    /// <summary>
    /// This job runs through all items in a channel, and using the configured Lava attribute 'Template Key' (for each item),
    /// the job will update another attribute specified by the 'Target Key' (on that item) with the results.
    /// </summary>
    /// <seealso cref="Quartz.IJob" />
    [DisplayName( "Content Channel Item Self Update" )]
    [Description( "This job runs through all items in a channel, and using the configured Lava attribute 'Template Key' (for each item), the job will update another attribute specified by the 'Target Key' (on that item) with the results." )]

    #region Job Attributes

    [ContentChannelField( "Content Channel",
        Description = "Lists all content channels on the system.",
        IsRequired = true,
        Order = 0,
        Key = AttributeKey.ContentChannel )]

    [KeyValueListField( "Attribute Links",
        Description = "Provide the attribute keys for the Source Lava Template and the Target Attribute. This will take the Lava from the source template, run it and put the results into the target attribute.",
        IsRequired = true,
        KeyPrompt = "Template Key",
        ValuePrompt = "Target Key",
        Order = 1,
        Key = AttributeKey.AttributeLinks )]

    #endregion Job Attributes

    [DisallowConcurrentExecution]
    public class ContentChannelItemSelfUpdate : IJob
    {
        private static class AttributeKey
        {
            public const string ContentChannel = "ContentChannel";
            public const string AttributeLinks = "AttributeLinks";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentChannelItemSelfUpdate"/> class.
        /// </summary>
        public ContentChannelItemSelfUpdate()
        {
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute( IJobExecutionContext context )
        {
            var jobDataMap = context.JobDetail.JobDataMap;
            var contentChannelGuid = jobDataMap.GetString( AttributeKey.ContentChannel ).AsGuidOrNull();

            if ( !contentChannelGuid.HasValue )
            {
                context.Result = $"The Service Job {jobDataMap.GetString( "Name" )} did not specify a valid Content Channel";
                ExceptionLogService.LogException( context.Result.ToString() );
                return;
            }

            var rockContext = new RockContext();
            var contentChannelId = new ContentChannelService( rockContext ).GetId( contentChannelGuid.Value );
            var contentChannelItems = new ContentChannelItemService( rockContext ).Queryable().Where( i => i.ContentChannelId == contentChannelId ).ToList();

            var attributeLinks = new Field.Types.KeyValueListFieldType().GetValuesFromString( null, jobDataMap.GetString( AttributeKey.AttributeLinks ), null, false );
            var itemMergeFields = new Dictionary<string, object>( Lava.LavaHelper.GetCommonMergeFields( null ) );
            var jobResultStringBuilder = new StringBuilder();
            var totalItems = contentChannelItems.Count();
            var updatedItems = 0;
            var updatedAttributes = 0;
            var itemId = 0;

            foreach ( var contentChannelItem in contentChannelItems )
            {
                itemMergeFields.AddOrReplace( "ContentChannelItem", contentChannelItem );

                foreach ( var attributeLink in attributeLinks )
                {
                    try
                    {
                        // merge the template lava and put the rendered text into the target key
                        contentChannelItem.LoadAttributes();
                        var lavaTemplate = contentChannelItem.GetAttributeValue( attributeLink.Key );
                        var lavaOutput = contentChannelItem.GetAttributeValue( attributeLink.Value.ToString() );
                        var mergedContent = lavaTemplate.ResolveMergeFields( itemMergeFields );

                        if ( lavaOutput.Equals( mergedContent ) )
                        {
                            continue;
                        }

                        contentChannelItem.SetAttributeValue( attributeLink.Value.ToString(), mergedContent );
                        updatedAttributes++;

                        if ( contentChannelItem.Id != itemId )
                        {
                            itemId = contentChannelItem.Id;
                            updatedItems++;
                        }
                    }
                    catch ( Exception ex )
                    {
                        jobResultStringBuilder.AppendLine( ex.Message );
                        ExceptionLogService.LogException( ex );
                        continue;
                    }

                    contentChannelItem.SaveAttributeValues( rockContext );
                }
            }

            jobResultStringBuilder.AppendLine( $"Updated {updatedAttributes} ContentChannelItem {"attribute".PluralizeIf( updatedAttributes != 1 )} in {updatedItems} of {totalItems} ContentChannelItems" );
            context.Result = jobResultStringBuilder.ToString();

            rockContext.SaveChanges();
        }
    }
}
