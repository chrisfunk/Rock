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
    /// CommunicationResponse Service class
    /// </summary>
    public partial class CommunicationResponseService : Service<CommunicationResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationResponseService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public CommunicationResponseService(RockContext context) : base(context)
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
        public bool CanDelete( CommunicationResponse item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// CommunicationResponse View Model Helper
    /// </summary>
    public partial class CommunicationResponseViewModelHelper : ViewModelHelper<CommunicationResponse, Rock.ViewModel.CommunicationResponseViewModel>
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.CommunicationResponseViewModel CreateViewModel( CommunicationResponse model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.CommunicationResponseViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                FromPersonAliasId = model.FromPersonAliasId,
                IsRead = model.IsRead,
                MessageKey = model.MessageKey,
                RelatedCommunicationId = model.RelatedCommunicationId,
                RelatedMediumEntityTypeId = model.RelatedMediumEntityTypeId,
                RelatedSmsFromDefinedValueId = model.RelatedSmsFromDefinedValueId,
                RelatedTransportEntityTypeId = model.RelatedTransportEntityTypeId,
                Response = model.Response,
                ToPersonAliasId = model.ToPersonAliasId,
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
    public static partial class CommunicationResponseExtensionMethods
    {
        /// <summary>
        /// Clones this CommunicationResponse object to a new CommunicationResponse object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static CommunicationResponse Clone( this CommunicationResponse source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as CommunicationResponse;
            }
            else
            {
                var target = new CommunicationResponse();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another CommunicationResponse object to this CommunicationResponse object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this CommunicationResponse target, CommunicationResponse source )
        {
            target.Id = source.Id;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.FromPersonAliasId = source.FromPersonAliasId;
            target.IsRead = source.IsRead;
            target.MessageKey = source.MessageKey;
            target.RelatedCommunicationId = source.RelatedCommunicationId;
            target.RelatedMediumEntityTypeId = source.RelatedMediumEntityTypeId;
            target.RelatedSmsFromDefinedValueId = source.RelatedSmsFromDefinedValueId;
            target.RelatedTransportEntityTypeId = source.RelatedTransportEntityTypeId;
            target.Response = source.Response;
            target.ToPersonAliasId = source.ToPersonAliasId;
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
        public static Rock.ViewModel.CommunicationResponseViewModel ToViewModel( this CommunicationResponse model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new CommunicationResponseViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
