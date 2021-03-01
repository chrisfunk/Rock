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
    /// GroupTypeRole Service class
    /// </summary>
    public partial class GroupTypeRoleService : Service<GroupTypeRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupTypeRoleService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public GroupTypeRoleService(RockContext context) : base(context)
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
        public bool CanDelete( GroupTypeRole item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new Service<ConnectionOpportunityGroupConfig>( Context ).Queryable().Any( a => a.GroupMemberRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupTypeRole.FriendlyTypeName, ConnectionOpportunityGroupConfig.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupMember>( Context ).Queryable().Any( a => a.GroupRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupTypeRole.FriendlyTypeName, GroupMember.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupMemberHistorical>( Context ).Queryable().Any( a => a.GroupRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupTypeRole.FriendlyTypeName, GroupMemberHistorical.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupSync>( Context ).Queryable().Any( a => a.GroupTypeRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupTypeRole.FriendlyTypeName, GroupSync.FriendlyTypeName );
                return false;
            }

            if ( new Service<GroupType>( Context ).Queryable().Any( a => a.DefaultGroupRoleId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", GroupTypeRole.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// GroupTypeRole View Model Helper
    /// </summary>
    public partial class GroupTypeRoleViewModelHelper : ViewModelHelper<GroupTypeRole, Rock.ViewModel.GroupTypeRoleViewModel>
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.GroupTypeRoleViewModel CreateViewModel( GroupTypeRole model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.GroupTypeRoleViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                CanEdit = model.CanEdit,
                CanManageMembers = model.CanManageMembers,
                CanView = model.CanView,
                Description = model.Description,
                GroupTypeId = model.GroupTypeId,
                IsLeader = model.IsLeader,
                IsSystem = model.IsSystem,
                MaxCount = model.MaxCount,
                MinCount = model.MinCount,
                Name = model.Name,
                Order = model.Order,
                ReceiveRequirementsNotifications = model.ReceiveRequirementsNotifications,
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
    public static partial class GroupTypeRoleExtensionMethods
    {
        /// <summary>
        /// Clones this GroupTypeRole object to a new GroupTypeRole object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static GroupTypeRole Clone( this GroupTypeRole source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as GroupTypeRole;
            }
            else
            {
                var target = new GroupTypeRole();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another GroupTypeRole object to this GroupTypeRole object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this GroupTypeRole target, GroupTypeRole source )
        {
            target.Id = source.Id;
            target.CanEdit = source.CanEdit;
            target.CanManageMembers = source.CanManageMembers;
            target.CanView = source.CanView;
            target.Description = source.Description;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GroupTypeId = source.GroupTypeId;
            target.IsLeader = source.IsLeader;
            target.IsSystem = source.IsSystem;
            target.MaxCount = source.MaxCount;
            target.MinCount = source.MinCount;
            target.Name = source.Name;
            target.Order = source.Order;
            target.ReceiveRequirementsNotifications = source.ReceiveRequirementsNotifications;
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
        public static Rock.ViewModel.GroupTypeRoleViewModel ToViewModel( this GroupTypeRole model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new GroupTypeRoleViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
