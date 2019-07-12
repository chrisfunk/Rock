﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

using Rock.Attribute;
using Rock.Data;
using Rock.Lava;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Blocks.Types.Mobile
{
    [DisplayName( "Content Channel Item List" )]
    [Category( "Mobile" )]
    [Description( "Lists content channel items for a given channel." )]
    [IconCssClass( "fa fa-th-list" )]

    #region Block Attributes
    
    [ContentChannelField(
        "Content Channel",
        Description = "The content channel to retrieve the items for.",
        Key = AttributeKeys.ContentChannel,
        Order = 1,
        Category = "CustomSetting" )]

    [TextField(
        "Page Size",
        Description = "The number of items to send per page.",
        Key = AttributeKeys.PageSize,
        DefaultValue = "50",
        Order = 2,
        Category = "CustomSetting" )]

    [BooleanField(
        "Include Following",
        Description = "Determines if following data should be sent along with the results.",
        Key = AttributeKeys.IncludeFollowing,
        Order = 3,
        Category = "CustomSetting" )]

    [TextField(
        "Field Settings",
        Description = "JSON object of the configured fields to show.",
        Key = AttributeKeys.FieldSettings,
        Order = 4,
        Category = "CustomSetting" )]

    [CodeEditorField(
        "List Data Template",
        Description = "The XAML for the lists data template.",
        Key = AttributeKeys.ListDataTemplate,
        Order = 0,
        EditorMode = Web.UI.Controls.CodeEditorMode.Xml,
        DefaultValue = defaultDataTemplate,
        Category = "custommobile")]

    [IntegerField( "Cache Duration",
        "The number of seconds the data should be cached on the client before it is requested from the server again. A value of 0 means always reload.",
        false,
        86400,
        category: "custommobile",
        order: 1 )]

    #endregion

    public class ContentChannelItemList : RockBlockType, IRockMobileBlockType
    {
        public static class AttributeKeys
        {
            public const string LavaTemplate = "LavaTemplate";

            public const string ContentChannel = "ContentChannel";

            public const string FieldSettings = "FieldSettings";

            public const string PageSize = "PageSize";

            public const string IncludeFollowing = "IncludeFollowing";

            public const string ListDataTemplate = "ListDataTemplate";

            public const string CacheDuration = "CacheDuration";
        }

        #region Constants
        private const string defaultDataTemplate = @"<StackLayout HeightRequest=""50"" WidthRequest=""200"" Orientation=""Horizontal"" Padding=""0,5,0,5"">
    <Label Text=""{Binding Content}"" />
</StackLayout>";
        #endregion

        #region IRockMobileBlockType Implementation

        /// <summary>
        /// Gets the class name of the mobile block to use during rendering on the device.
        /// </summary>
        /// <value>
        /// The class name of the mobile block to use during rendering on the device
        /// </value>
        public string MobileBlockType => "Rock.Mobile.Blocks.ItemList";

        public int RequiredMobileAbiVersion => 1;

        #endregion

        /// <summary>
        /// Gets the property values that will be sent to the device in the application bundle.
        /// </summary>
        /// <returns>
        /// A collection of string/object pairs.
        /// </returns>
        public object GetMobileConfigurationValues()
        {
            return new Dictionary<string, object>();
        }

        #region Actions

        [BlockAction]
        public object GetItems( int pageNumber = 0 )
        {
            var contentChannelId = GetAttributeValue( AttributeKeys.ContentChannel ).AsInteger();
            var pageSize = GetAttributeValue( AttributeKeys.PageSize ).AsInteger();
            var includeFollowing = GetAttributeValue( AttributeKeys.IncludeFollowing ).AsBoolean();

            var skipNumber = pageNumber * pageSize;

            var rockContext = new RockContext();

            var results = new ContentChannelItemService( rockContext ).Queryable().AsNoTracking()
                            .Where( i => i.ContentChannelId == contentChannelId )
                            .OrderBy( i => i.Id )  // TODO make this a setting
                            .Skip( skipNumber )
                            .Take( pageSize )
                            .ToList();

            List<int> followedItemIds = new List<int>();

            // Get the ids of items that are followed by the current person
            if ( includeFollowing )
            {
                var currentPerson = GetCurrentPerson();

                if ( currentPerson != null )
                {
                    var resultIds = results.Select( r => r.Id ).ToList();
                    var contentChannelItemEntityTypeId = EntityTypeCache.Get( SystemGuid.EntityType.CONTENT_CHANNEL_ITEM.AsGuid() ).Id;

                    followedItemIds = new FollowingService( rockContext ).Queryable().AsNoTracking()
                                        .Where( f =>
                                            f.EntityTypeId == contentChannelItemEntityTypeId &&
                                            resultIds.Contains( f.EntityId ) &&
                                            f.PersonAlias.PersonId == currentPerson.Id )
                                        .Select( f => f.EntityId )
                                        .ToList();
                }
            }

            var lavaTemplate = CreateLavaTemplate( followedItemIds );

            var commonMergeFields = new CommonMergeFieldsOptions
            {
                GetLegacyGlobalMergeFields = false
            };

            var mergeFields = RequestContext.GetCommonMergeFields( commonMergeFields );
            mergeFields.Add( "Items", results );
            mergeFields.Add( "FollowedItemIds", followedItemIds );

            var output = lavaTemplate.ResolveMergeFields( mergeFields );

            return ActionOk( new StringContent( output, Encoding.UTF8, "application/json" ) );
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the lava template from the list of fields.
        /// </summary>
        /// <returns></returns>
        private string CreateLavaTemplate( List<int> followedItemIds )
        {
            var fieldSettingJson = GetAttributeValue( AttributeKeys.FieldSettings );
            var fields = JsonConvert.DeserializeObject<List<FieldSetting>>( fieldSettingJson );

            var contentChannelItemEntityTypeId = EntityTypeCache.Get( SystemGuid.EntityType.CONTENT_CHANNEL_ITEM.AsGuid() ).Id;

            var template = new StringBuilder();
            template.AppendLine( "[" );
            template.AppendLine( "    {% for item in Items %}" );
            template.AppendLine( "    {" );

            for ( int i = 0; i < fields.Count; i++ )
            {
                var field = fields[i];

                template.AppendLine( string.Format( @"        {{% jsonproperty name:'{0}' format:'{1}' %}}{2}{{% endjsonproperty %}},", field.Key, field.FieldFormat, field.Value ) );
            }

            // Append the fields we'd need for the following button
            template.AppendLine(                "    \"EntityId\": {{ item.Id }}," );
            template.AppendLine( string.Format( "    \"EntityTypeId\": {0}, ", contentChannelItemEntityTypeId ) );
            template.AppendLine(                "    \"IsFollowing\": {{ FollowedItemIds | Contains:item.Id }} " );

            template.Append( "    }" );
            template.AppendLine( "{% if forloop.last != true %},{% endif %}" );
            template.AppendLine( "    {% endfor %}" );
            template.AppendLine( "]" );

            return template.ToString();
        }

        #endregion


        #region Custom Settings

        [TargetType( typeof( ContentChannelItemList ) )]
        public class MobileContentCustomSettingsProvider : RockCustomSettingsUserControlProvider
        {
            protected override string UserControlPath => "~/BlockConfig/ContentChannelListSettings.ascx";

            public override string CustomSettingsTitle => "Basic Settings";
        }

        #endregion
    }

    #region POCOs
    /// <summary>
    /// POCO to store the settings for the fields
    /// </summary>
    public class FieldSetting
    {
        /// <summary>
        /// Creates an identifier based off the key. This is used for grid operations.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id
        {
            get
            {
                return this.Key.GetHashCode();
            }
        }

        /// <summary>
        /// Gets or sets the field key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the field value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the field source.
        /// </summary>
        /// <value>
        /// The field source.
        /// </value>
        public FieldSource FieldSource { get; set; }

        /// <summary>
        /// Gets or sets the attribute format.
        /// </summary>
        /// <value>
        /// The attribute format.
        /// </value>
        public AttributeFormat AttributeFormat { get; set; }

        /// <summary>
        /// Gets or sets the field format.
        /// </summary>
        /// <value>
        /// The field format.
        /// </value>
        public FieldFormat FieldFormat { get; set; }
    }

    /// <summary>
    /// The source of the data for the field. The two types are properties on the item model and an attribute expression.
    /// </summary>
    public enum FieldSource
    {
        Property = 0,
        Attribute = 1,
        LavaExpression = 2
    }

    /// <summary>
    /// The format to use for the attribute.
    /// </summary>
    public enum AttributeFormat
    {
        FriendlyValue = 0,
        RawValue = 1
    }

    /// <summary>
    /// Determines the field's format. This will be used to properly format the Json sent to the client.
    /// </summary>
    public enum FieldFormat
    {
        String = 0,
        Number = 1,
        Date = 2,
        Boolean = 3
    }
    #endregion
}