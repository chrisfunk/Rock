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
    /// AnalyticsSourceFinancialTransaction Service class
    /// </summary>
    public partial class AnalyticsSourceFinancialTransactionService : Service<AnalyticsSourceFinancialTransaction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsSourceFinancialTransactionService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public AnalyticsSourceFinancialTransactionService(RockContext context) : base(context)
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
        public bool CanDelete( AnalyticsSourceFinancialTransaction item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class AnalyticsSourceFinancialTransactionExtensionMethods
    {
        /// <summary>
        /// Clones this AnalyticsSourceFinancialTransaction object to a new AnalyticsSourceFinancialTransaction object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static AnalyticsSourceFinancialTransaction Clone( this AnalyticsSourceFinancialTransaction source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as AnalyticsSourceFinancialTransaction;
            }
            else
            {
                var target = new AnalyticsSourceFinancialTransaction();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another AnalyticsSourceFinancialTransaction object to this AnalyticsSourceFinancialTransaction object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this AnalyticsSourceFinancialTransaction target, AnalyticsSourceFinancialTransaction source )
        {
            target.Id = source.Id;
            target.AccountId = source.AccountId;
            target.Amount = source.Amount;
            target.AuthorizedCurrentPersonKey = source.AuthorizedCurrentPersonKey;
            target.AuthorizedFamilyId = source.AuthorizedFamilyId;
            target.AuthorizedPersonAliasId = source.AuthorizedPersonAliasId;
            target.AuthorizedPersonKey = source.AuthorizedPersonKey;
            target.BatchId = source.BatchId;
            target.Count = source.Count;
            target.CreditCardTypeValueId = source.CreditCardTypeValueId;
            target.CurrencyTypeValueId = source.CurrencyTypeValueId;
            target.DaysSinceLastTransactionOfType = source.DaysSinceLastTransactionOfType;
            target.EntityId = source.EntityId;
            target.EntityTypeId = source.EntityTypeId;
            target.FinancialGatewayId = source.FinancialGatewayId;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.GivingGroupId = source.GivingGroupId;
            target.GivingId = source.GivingId;
            target.IsFirstTransactionOfType = source.IsFirstTransactionOfType;
            target.IsScheduled = source.IsScheduled;
            target.ProcessedByPersonAliasId = source.ProcessedByPersonAliasId;
            target.ProcessedDateTime = source.ProcessedDateTime;
            target.SourceTypeValueId = source.SourceTypeValueId;
            target.Summary = source.Summary;
            target.TransactionCode = source.TransactionCode;
            target.TransactionDateKey = source.TransactionDateKey;
            target.TransactionDateTime = source.TransactionDateTime;
            target.TransactionDetailId = source.TransactionDetailId;
            target.TransactionFrequency = source.TransactionFrequency;
            target.TransactionId = source.TransactionId;
            target.TransactionKey = source.TransactionKey;
            target.TransactionTypeValueId = source.TransactionTypeValueId;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }

    }

}
