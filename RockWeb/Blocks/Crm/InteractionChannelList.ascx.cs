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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotLiquid;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Crm
{
    /// <summary>
    /// List all the Interaction Channel.
    /// </summary>
    [DisplayName( "Interaction Channel List" )]
    [Category( "CRM" )]
    [Description( "List all the Interaction Channel" )]

    [LinkedPage( "Session List Page", "Page reference to the session list page. This will be included as a variable in the Lava.", false, order: 0 )]
    [LinkedPage( "Component List Page", "Page reference to the component list page. This will be included as a variable in the Lava.", false, order: 1 )]
    [CodeEditorField( "Default Template", "The Lava template to use as default.", Rock.Web.UI.Controls.CodeEditorMode.Lava, Rock.Web.UI.Controls.CodeEditorTheme.Rock, 300, order: 2, defaultValue: @"
{% if InteractionChannel != null and InteractionChannel != '' %}

                    {% if InteractionChannel.UsesSession == false and ComponentListPage != ''  %}
                        <a href = '{{ ComponentListPage }}?channelId={{ InteractionChannel.Id }}' >
                    {% elseif InteractionChannel.UsesSession == true and SessionListPage != ''  %}
                        <a href = '{{ SessionListPage }}?channelId={{ InteractionChannel.Id }}' >
                    {% endif %}
                    <div class='row'>
                        <div class='col-md-6'>
                            {% if InteractionChannel.Name != '' %}
                                <dl>
                               <dt>Name</dt>
                               <dd>{{ InteractionChannel.Name }}<dd/>
                               </dl>
                            {% endif %}
                            {% if InteractionChannel.RetentionDuration != '' %}
                                <dl>
                               <dt>Retention Duration</dt
                               <dd>{{ InteractionChannel.RetentionDuration }}<dd/>
                            </dl>
                            {% endif %}
                        </div>
                        <div class='col-md-6'>
                            {% if InteractionChannel.ChannelTypeMediumValue != null and InteractionChannel.ChannelTypeMediumValue != '' %}
                            <dl>
                               <dt>Name</dt
                               <dd>{{ InteractionChannel.ChannelTypeMediumValue.Value }}<dd/>
                            </dl>
                            {% endif %}
                        </div>
                    </div>
                    {% if (InteractionChannel.UsesSession == false and ComponentListPage != '') or (InteractionChannel.UsesSession == true and SessionListPage != '') %}
                        </a>
                    {% endif %}
{% endif %}" )]
    public partial class InteractionChannelList : Rock.Web.UI.RockBlock
    {
        #region Fields

        private const string MEDIUM_TYPE_FILTER = "Medium Type";

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                BindFilter();
                ShowList();
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            ShowList();
        }

        /// <summary>
        /// Handles the Apply Filter event for the GridFilter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gfFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            gfFilter.SaveUserPreference( MEDIUM_TYPE_FILTER, ddlMediumValue.SelectedValue );
            ShowList();
        }

        /// <summary>
        /// Handles displaying the stored filter values.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e as DisplayFilterValueArgs (hint: e.Key and e.Value).</param>
        protected void gfFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {

            switch ( e.Key )
            {
                case "Medium Type":
                    var mediumTypeValueId = e.Value.AsIntegerOrNull();
                    if ( mediumTypeValueId.HasValue )
                    {
                        var mediumTypeValue = DefinedValueCache.Read( mediumTypeValueId.Value );
                        e.Value = mediumTypeValue.Value;
                    }
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the filter.
        /// </summary>
        private void BindFilter()
        {
            var definedType = DefinedTypeCache.Read( Rock.SystemGuid.DefinedType.INTERACTION_CHANNEL_MEDIUM.AsGuid() );
            ddlMediumValue.BindToDefinedType( definedType, true );

            var channelMediumValueId = gfFilter.GetUserPreference( MEDIUM_TYPE_FILTER ).AsIntegerOrNull();
            ddlMediumValue.SetValue( channelMediumValueId );
        }

        /// <summary>
        /// Shows the list.
        /// </summary>
        public void ShowList()
        {
            using ( var rockContext = new RockContext() )
            {
                InteractionChannelService interactionChannelService = new InteractionChannelService( rockContext );
                var interactionChannels = interactionChannelService.Queryable().AsNoTracking();

                var channelMediumValueId = gfFilter.GetUserPreference( MEDIUM_TYPE_FILTER ).AsIntegerOrNull();
                if ( channelMediumValueId.HasValue )
                {
                    interactionChannels = interactionChannels.Where( a => a.ChannelTypeMediumValueId == channelMediumValueId.Value );
                }

                // Parse the default template so that it does not need to be parsed multiple times
                var defaultTemplate = Template.Parse( GetAttributeValue( "DefaultTemplate" ) );
                var channelItems = new List<ChannelItem>();

                foreach ( var interacationChannel in interactionChannels )
                {
                    if ( !interacationChannel.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
                    {
                        continue;
                    }

                    var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( this.RockPage, this.CurrentPerson );
                    mergeFields.AddOrIgnore( "Person", CurrentPerson );
                    mergeFields.Add( "ComponentListPage", LinkedPageRoute( "ComponentListPage" ) );
                    mergeFields.Add( "SessionListPage", LinkedPageRoute( "SessionListPage" ) );
                    mergeFields.Add( "InteractionChannel", interacationChannel );

                    string html = interacationChannel.ChannelListTemplate.IsNotNullOrWhitespace() ?
                        interacationChannel.ChannelListTemplate.ResolveMergeFields( mergeFields ) :
                        defaultTemplate.Render( Hash.FromDictionary( mergeFields ) );

                    channelItems.Add( new ChannelItem
                    {
                        Id = interacationChannel.Id,
                        ChannelHtml = html
                    } );
                }

                rptChannel.DataSource = channelItems;
                rptChannel.DataBind();
            }
        }

        #endregion

    }

    public class ChannelItem
    {
        public int Id { get; set; }
        public string ChannelHtml { get; set; }
    }
}