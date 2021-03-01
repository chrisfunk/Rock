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
    /// PrayerRequest Service class
    /// </summary>
    public partial class PrayerRequestService : Service<PrayerRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrayerRequestService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public PrayerRequestService(RockContext context) : base(context)
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
        public bool CanDelete( PrayerRequest item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// PrayerRequest View Model Helper
    /// </summary>
    public partial class PrayerRequestViewModelHelper : ViewModelHelper<PrayerRequest, Rock.ViewModel.PrayerRequestViewModel>
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.PrayerRequestViewModel CreateViewModel( PrayerRequest model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.PrayerRequestViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                AllowComments = model.AllowComments,
                Answer = model.Answer,
                ApprovedByPersonAliasId = model.ApprovedByPersonAliasId,
                ApprovedOnDateTime = model.ApprovedOnDateTime,
                CampusId = model.CampusId,
                CategoryId = model.CategoryId,
                Email = model.Email,
                EnteredDateTime = model.EnteredDateTime,
                ExpirationDate = model.ExpirationDate,
                FirstName = model.FirstName,
                FlagCount = model.FlagCount,
                GroupId = model.GroupId,
                IsActive = model.IsActive,
                IsApproved = model.IsApproved,
                IsPublic = model.IsPublic,
                IsUrgent = model.IsUrgent,
                LastName = model.LastName,
                PrayerCount = model.PrayerCount,
                RequestedByPersonAliasId = model.RequestedByPersonAliasId,
                Text = model.Text,
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
    public static partial class PrayerRequestExtensionMethods
    {
        /// <summary>
        /// Clones this PrayerRequest object to a new PrayerRequest object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static PrayerRequest Clone( this PrayerRequest source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as PrayerRequest;
            }
            else
            {
                var target = new PrayerRequest();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another PrayerRequest object to this PrayerRequest object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this PrayerRequest target, PrayerRequest source )
        {
            target.Id = source.Id;
            target.AllowComments = source.AllowComments;
            target.Answer = source.Answer;
            target.ApprovedByPersonAliasId = source.ApprovedByPersonAliasId;
            target.ApprovedOnDateTime = source.ApprovedOnDateTime;
            target.CampusId = source.CampusId;
            target.CategoryId = source.CategoryId;
            target.Email = source.Email;
            target.EnteredDateTime = source.EnteredDateTime;
            target.ExpirationDate = source.ExpirationDate;
            target.FirstName = source.FirstName;
            target.FlagCount = source.FlagCount;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GroupId = source.GroupId;
            target.IsActive = source.IsActive;
            target.IsApproved = source.IsApproved;
            target.IsPublic = source.IsPublic;
            target.IsUrgent = source.IsUrgent;
            target.LastName = source.LastName;
            target.PrayerCount = source.PrayerCount;
            target.RequestedByPersonAliasId = source.RequestedByPersonAliasId;
            target.Text = source.Text;
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
        public static Rock.ViewModel.PrayerRequestViewModel ToViewModel( this PrayerRequest model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new PrayerRequestViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
