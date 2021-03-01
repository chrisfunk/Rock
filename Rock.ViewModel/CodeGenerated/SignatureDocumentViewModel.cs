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

namespace Rock.ViewModel
{
    /// <summary>
    /// SignatureDocument View Model
    /// </summary>
    public partial class SignatureDocumentViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the AppliesToPersonAliasId.
        /// </summary>
        /// <value>
        /// The AppliesToPersonAliasId.
        /// </value>
        public int? AppliesToPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the AssignedToPersonAliasId.
        /// </summary>
        /// <value>
        /// The AssignedToPersonAliasId.
        /// </value>
        public int? AssignedToPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the BinaryFileId.
        /// </summary>
        /// <value>
        /// The BinaryFileId.
        /// </value>
        public int? BinaryFileId { get; set; }

        /// <summary>
        /// Gets or sets the DocumentKey.
        /// </summary>
        /// <value>
        /// The DocumentKey.
        /// </value>
        public string DocumentKey { get; set; }

        /// <summary>
        /// Gets or sets the InviteCount.
        /// </summary>
        /// <value>
        /// The InviteCount.
        /// </value>
        public int InviteCount { get; set; }

        /// <summary>
        /// Gets or sets the LastInviteDate.
        /// </summary>
        /// <value>
        /// The LastInviteDate.
        /// </value>
        public DateTime? LastInviteDate { get; set; }

        /// <summary>
        /// Gets or sets the LastStatusDate.
        /// </summary>
        /// <value>
        /// The LastStatusDate.
        /// </value>
        public DateTime? LastStatusDate { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The Name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SignatureDocumentTemplateId.
        /// </summary>
        /// <value>
        /// The SignatureDocumentTemplateId.
        /// </value>
        public int SignatureDocumentTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the SignedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The SignedByPersonAliasId.
        /// </value>
        public int? SignedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDateTime.
        /// </summary>
        /// <value>
        /// The CreatedDateTime.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDateTime.
        /// </summary>
        /// <value>
        /// The ModifiedDateTime.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the CreatedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The CreatedByPersonAliasId.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedByPersonAliasId.
        /// </summary>
        /// <value>
        /// The ModifiedByPersonAliasId.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

    }
}
