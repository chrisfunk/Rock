//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Site Service class
    /// </summary>
    public partial class SiteService : Service<Site>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public SiteService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( Site item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<Block>( Context ).Queryable().Any( a => a.SiteId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Site.FriendlyTypeName, Block.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<PersonalDevice>( Context ).Queryable().Any( a => a.SiteId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Site.FriendlyTypeName, PersonalDevice.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class SiteExtensionMethods
    {
        /// <summary>
        /// Clones this Site object to a new Site object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static Site Clone( this Site source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as Site;
            }
            else
            {
                var target = new Site();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another Site object to this Site object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this Site target, Site source )
        {
            target.Id = source.Id;
            target.AdditionalSettings = source.AdditionalSettings;
            target.AllowedFrameDomains = source.AllowedFrameDomains;
            target.AllowIndexing = source.AllowIndexing;
            target.ChangePasswordPageId = source.ChangePasswordPageId;
            target.ChangePasswordPageRouteId = source.ChangePasswordPageRouteId;
            target.CommunicationPageId = source.CommunicationPageId;
            target.CommunicationPageRouteId = source.CommunicationPageRouteId;
            target.ConfigurationMobilePhoneBinaryFileId = source.ConfigurationMobilePhoneBinaryFileId;
            target.ConfigurationMobileTabletBinaryFileId = source.ConfigurationMobileTabletBinaryFileId;
            target.DefaultPageId = source.DefaultPageId;
            target.DefaultPageRouteId = source.DefaultPageRouteId;
            target.Description = source.Description;
            target.EnabledForShortening = source.EnabledForShortening;
            target.EnableExclusiveRoutes = source.EnableExclusiveRoutes;
            target.EnableMobileRedirect = source.EnableMobileRedirect;
            target.EnablePageViews = source.EnablePageViews;
            target.ErrorPage = source.ErrorPage;
            target.ExternalUrl = source.ExternalUrl;
            target.FavIconBinaryFileId = source.FavIconBinaryFileId;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GoogleAnalyticsCode = source.GoogleAnalyticsCode;
            target.IndexStartingLocation = source.IndexStartingLocation;
            target.IsActive = source.IsActive;
            target.IsIndexEnabled = source.IsIndexEnabled;
            target.IsSystem = source.IsSystem;
            target.LatestVersionDateTime = source.LatestVersionDateTime;
            target.LoginPageId = source.LoginPageId;
            target.LoginPageRouteId = source.LoginPageRouteId;
            target.MobilePageId = source.MobilePageId;
            target.Name = source.Name;
            target.PageHeaderContent = source.PageHeaderContent;
            target.PageNotFoundPageId = source.PageNotFoundPageId;
            target.PageNotFoundPageRouteId = source.PageNotFoundPageRouteId;
            target.RedirectTablets = source.RedirectTablets;
            target.RegistrationPageId = source.RegistrationPageId;
            target.RegistrationPageRouteId = source.RegistrationPageRouteId;
            target.RequiresEncryption = source.RequiresEncryption;
            target.SiteLogoBinaryFileId = source.SiteLogoBinaryFileId;
            target.SiteType = source.SiteType;
            target.Theme = source.Theme;
            target.ThumbnailBinaryFileId = source.ThumbnailBinaryFileId;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
