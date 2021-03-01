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
    /// FinancialPersonBankAccount Service class
    /// </summary>
    public partial class FinancialPersonBankAccountService : Service<FinancialPersonBankAccount>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialPersonBankAccountService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public FinancialPersonBankAccountService(RockContext context) : base(context)
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
        public bool CanDelete( FinancialPersonBankAccount item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// FinancialPersonBankAccount View Model Helper
    /// </summary>
    public partial class FinancialPersonBankAccountViewModelHelper : ViewModelHelper<FinancialPersonBankAccount, Rock.ViewModel.FinancialPersonBankAccountViewModel>
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.FinancialPersonBankAccountViewModel CreateViewModel( FinancialPersonBankAccount model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.FinancialPersonBankAccountViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                AccountNumberMasked = model.AccountNumberMasked,
                AccountNumberSecured = model.AccountNumberSecured,
                PersonAliasId = model.PersonAliasId,
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
    public static partial class FinancialPersonBankAccountExtensionMethods
    {
        /// <summary>
        /// Clones this FinancialPersonBankAccount object to a new FinancialPersonBankAccount object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static FinancialPersonBankAccount Clone( this FinancialPersonBankAccount source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as FinancialPersonBankAccount;
            }
            else
            {
                var target = new FinancialPersonBankAccount();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another FinancialPersonBankAccount object to this FinancialPersonBankAccount object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this FinancialPersonBankAccount target, FinancialPersonBankAccount source )
        {
            target.Id = source.Id;
            target.AccountNumberMasked = source.AccountNumberMasked;
            target.AccountNumberSecured = source.AccountNumberSecured;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.PersonAliasId = source.PersonAliasId;
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
        public static Rock.ViewModel.FinancialPersonBankAccountViewModel ToViewModel( this FinancialPersonBankAccount model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new FinancialPersonBankAccountViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
