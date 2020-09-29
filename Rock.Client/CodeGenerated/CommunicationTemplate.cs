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
using System.Collections.Generic;


namespace Rock.Client
{
    /// <summary>
    /// Base client model for CommunicationTemplate that only includes the non-virtual fields. Use this for PUT/POSTs
    /// </summary>
    public partial class CommunicationTemplateEntity
    {
        /// <summary />
        public int Id { get; set; }

        /// <summary />
        public string BCCEmails { get; set; }

        /// <summary />
        public int? CategoryId { get; set; }

        /// <summary />
        public string CCEmails { get; set; }

        /// <summary />
        public bool CssInliningEnabled { get; set; } = true;

        /// <summary />
        public string Description { get; set; }

        /// <summary />
        public Guid? ForeignGuid { get; set; }

        /// <summary />
        public string ForeignKey { get; set; }

        /// <summary />
        public string FromEmail { get; set; }

        /// <summary />
        public string FromName { get; set; }

        /// <summary />
        public int? ImageFileId { get; set; }

        /// <summary />
        public bool IsActive { get; set; } = true;

        /// <summary />
        public bool IsSystem { get; set; }

        /// <summary />
        public string LavaFieldsJson { get; set; } = @"{}";

        /// <summary />
        public int? LogoBinaryFileId { get; set; }

        /// <summary />
        // Made Obsolete in Rock "1.7"
        [Obsolete( "MediumDataJson is no longer used.", true )]
        public string MediumDataJson { get; set; }

        /// <summary />
        public string Message { get; set; }

        /// <summary />
        public string MessageMetaData { get; set; }

        /// <summary>
        /// If the ModifiedByPersonAliasId is being set manually and should not be overwritten with current user when saved, set this value to true
        /// </summary>
        public bool ModifiedAuditValuesAlreadyUpdated { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public string PushData { get; set; }

        /// <summary />
        public BinaryFile PushImageBinaryFile { get; set; }

        /// <summary />
        public int? PushImageBinaryFileId { get; set; }

        /// <summary />
        public string PushMessage { get; set; }

        /// <summary />
        public int /* PushOpenAction*/? PushOpenAction { get; set; }

        /// <summary />
        public string PushOpenMessage { get; set; }

        /// <summary />
        public string PushSound { get; set; }

        /// <summary />
        public string PushTitle { get; set; }

        /// <summary />
        public string ReplyToEmail { get; set; }

        /// <summary />
        public int? SenderPersonAliasId { get; set; }

        /// <summary />
        public int? SMSFromDefinedValueId { get; set; }

        /// <summary />
        public string SMSMessage { get; set; }

        /// <summary />
        public string Subject { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// This does not need to be set or changed. Rock will always set this to the current date/time when saved to the database.
        /// </summary>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Leave this as NULL to let Rock set this
        /// </summary>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// If you need to set this manually, set ModifiedAuditValuesAlreadyUpdated=True to prevent Rock from setting it
        /// </summary>
        public int? ModifiedByPersonAliasId { get; set; }

        /// <summary />
        public Guid Guid { get; set; }

        /// <summary />
        public int? ForeignId { get; set; }

        /// <summary>
        /// Copies the base properties from a source CommunicationTemplate object
        /// </summary>
        /// <param name="source">The source.</param>
        public void CopyPropertiesFrom( CommunicationTemplate source )
        {
            this.Id = source.Id;
            this.BCCEmails = source.BCCEmails;
            this.CategoryId = source.CategoryId;
            this.CCEmails = source.CCEmails;
            this.CssInliningEnabled = source.CssInliningEnabled;
            this.Description = source.Description;
            this.ForeignGuid = source.ForeignGuid;
            this.ForeignKey = source.ForeignKey;
            this.FromEmail = source.FromEmail;
            this.FromName = source.FromName;
            this.ImageFileId = source.ImageFileId;
            this.IsActive = source.IsActive;
            this.IsSystem = source.IsSystem;
            this.LavaFieldsJson = source.LavaFieldsJson;
            this.LogoBinaryFileId = source.LogoBinaryFileId;
            this.Message = source.Message;
            this.MessageMetaData = source.MessageMetaData;
            this.ModifiedAuditValuesAlreadyUpdated = source.ModifiedAuditValuesAlreadyUpdated;
            this.Name = source.Name;
            this.PushData = source.PushData;
            this.PushImageBinaryFile = source.PushImageBinaryFile;
            this.PushImageBinaryFileId = source.PushImageBinaryFileId;
            this.PushMessage = source.PushMessage;
            this.PushOpenAction = source.PushOpenAction;
            this.PushOpenMessage = source.PushOpenMessage;
            this.PushSound = source.PushSound;
            this.PushTitle = source.PushTitle;
            this.ReplyToEmail = source.ReplyToEmail;
            this.SenderPersonAliasId = source.SenderPersonAliasId;
            this.SMSFromDefinedValueId = source.SMSFromDefinedValueId;
            this.SMSMessage = source.SMSMessage;
            this.Subject = source.Subject;
            this.CreatedDateTime = source.CreatedDateTime;
            this.ModifiedDateTime = source.ModifiedDateTime;
            this.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            this.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            this.Guid = source.Guid;
            this.ForeignId = source.ForeignId;

        }
    }

    /// <summary>
    /// Client model for CommunicationTemplate that includes all the fields that are available for GETs. Use this for GETs (use CommunicationTemplateEntity for POST/PUTs)
    /// </summary>
    public partial class CommunicationTemplate : CommunicationTemplateEntity
    {
        /// <summary />
        public ICollection<CommunicationTemplateAttachment> Attachments { get; set; }

        /// <summary />
        public Category Category { get; set; }

        /// <summary />
        public BinaryFile ImageFile { get; set; }

        /// <summary />
        public bool IsPersonal { get; set; }

        /// <summary />
        public Dictionary<string, string> LavaFields { get; set; }

        /// <summary />
        public BinaryFile LogoBinaryFile { get; set; }

        /// <summary />
        public BinaryFile PushImageBinaryFile { get; set; }

        /// <summary />
        public PersonAlias SenderPersonAlias { get; set; }

        /// <summary />
        public DefinedValue SMSFromDefinedValue { get; set; }

        /// <summary>
        /// NOTE: Attributes are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.Attribute> Attributes { get; set; }

        /// <summary>
        /// NOTE: AttributeValues are only populated when ?loadAttributes is specified. Options for loadAttributes are true, false, 'simple', 'expanded' 
        /// </summary>
        public Dictionary<string, Rock.Client.AttributeValue> AttributeValues { get; set; }
    }
}
