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
using System.Linq;

using Rock.Attribute;
using Rock.Data;
using Rock.ViewModel;
using Rock.Web.Cache;

namespace Rock.Model
{
    /// <summary>
    /// ContentChannelItem Service class
    /// </summary>
    public partial class ContentChannelItemService : Service<ContentChannelItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentChannelItemService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public ContentChannelItemService(RockContext context) : base(context)
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
        public bool CanDelete( ContentChannelItem item, out string errorMessage )
        {
            errorMessage = string.Empty;

            // ignoring ContentChannelItemAssociation,ChildContentChannelItemId

            // ignoring ContentChannelItemAssociation,ContentChannelItemId

            // ignoring ContentChannelItemSlug,ContentChannelItemId
            return true;
        }
    }

    /// <summary>
    /// ContentChannelItem View Model Helper
    /// </summary>
    public partial class ContentChannelItemViewModelHelper : ViewModelHelper<ContentChannelItem, Rock.ViewModel.ContentChannelItemViewModel>
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.ContentChannelItemViewModel CreateViewModel( ContentChannelItem model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.ContentChannelItemViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                ApprovedByPersonAliasId = model.ApprovedByPersonAliasId,
                ApprovedDateTime = model.ApprovedDateTime,
                Content = model.Content,
                ContentChannelId = model.ContentChannelId,
                ContentChannelTypeId = model.ContentChannelTypeId,
                ExpireDateTime = model.ExpireDateTime,
                ItemGlobalKey = model.ItemGlobalKey,
                Order = model.Order,
                Permalink = model.Permalink,
                Priority = model.Priority,
                StartDateTime = model.StartDateTime,
                Status = ( int ) model.Status,
                StructuredContent = model.StructuredContent,
                Title = model.Title,
                CreatedDateTime = model.CreatedDateTime,
                ModifiedDateTime = model.ModifiedDateTime,
                CreatedByPersonAliasId = model.CreatedByPersonAliasId,
                ModifiedByPersonAliasId = model.ModifiedByPersonAliasId,
            };

            AddAttributesToViewModel( model, viewModel, currentPerson, loadAttributes );
            ApplyAdditionalPropertiesAndSecurityToViewModel( viewModel, currentPerson, loadAttributes );
            return viewModel;
        }
    }


    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class ContentChannelItemExtensionMethods
    {
        /// <summary>
        /// Clones this ContentChannelItem object to a new ContentChannelItem object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static ContentChannelItem Clone( this ContentChannelItem source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as ContentChannelItem;
            }
            else
            {
                var target = new ContentChannelItem();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another ContentChannelItem object to this ContentChannelItem object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this ContentChannelItem target, ContentChannelItem source )
        {
            target.Id = source.Id;
            target.ApprovedByPersonAliasId = source.ApprovedByPersonAliasId;
            target.ApprovedDateTime = source.ApprovedDateTime;
            target.Content = source.Content;
            target.ContentChannelId = source.ContentChannelId;
            target.ContentChannelTypeId = source.ContentChannelTypeId;
            target.ExpireDateTime = source.ExpireDateTime;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.ItemGlobalKey = source.ItemGlobalKey;
            target.Order = source.Order;
            target.Permalink = source.Permalink;
            target.Priority = source.Priority;
            target.StartDateTime = source.StartDateTime;
            target.Status = source.Status;
            target.StructuredContent = source.StructuredContent;
            target.Title = source.Title;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }

        /// <summary>
        /// Creates a view model from this entity
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson" >The currentPerson.</param>
        /// <param name="loadAttributes" >Load attributes?</param>
        public static Rock.ViewModel.ContentChannelItemViewModel ToViewModel( this ContentChannelItem model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new ContentChannelItemViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
