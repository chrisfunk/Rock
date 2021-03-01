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
    /// CommunicationTemplate Service class
    /// </summary>
    public partial class CommunicationTemplateService : Service<CommunicationTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationTemplateService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public CommunicationTemplateService(RockContext context) : base(context)
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
        public bool CanDelete( CommunicationTemplate item, out string errorMessage )
        {
            errorMessage = string.Empty;

            // ignoring Communication,CommunicationTemplateId
            return true;
        }
    }

    /// <summary>
    /// CommunicationTemplate View Model Helper
    /// </summary>
    public partial class CommunicationTemplateViewModelHelper : ViewModelHelper<CommunicationTemplate, Rock.ViewModel.CommunicationTemplateViewModel>
    {
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        /// <returns></returns>
        public override Rock.ViewModel.CommunicationTemplateViewModel CreateViewModel( CommunicationTemplate model, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( model == null )
            {
                return default;
            }

            var viewModel = new Rock.ViewModel.CommunicationTemplateViewModel
            {
                Id = model.Id,
                Guid = model.Guid,
                BCCEmails = model.BCCEmails,
                CategoryId = model.CategoryId,
                CCEmails = model.CCEmails,
                CssInliningEnabled = model.CssInliningEnabled,
                Description = model.Description,
                FromEmail = model.FromEmail,
                FromName = model.FromName,
                ImageFileId = model.ImageFileId,
                IsActive = model.IsActive,
                IsSystem = model.IsSystem,
                LavaFieldsJson = model.LavaFieldsJson,
                LogoBinaryFileId = model.LogoBinaryFileId,
                Message = model.Message,
                MessageMetaData = model.MessageMetaData,
                Name = model.Name,
                PushData = model.PushData,
                PushImageBinaryFileId = model.PushImageBinaryFileId,
                PushMessage = model.PushMessage,
                PushOpenAction = ( int? ) model.PushOpenAction,
                PushOpenMessage = model.PushOpenMessage,
                PushSound = model.PushSound,
                PushTitle = model.PushTitle,
                ReplyToEmail = model.ReplyToEmail,
                SenderPersonAliasId = model.SenderPersonAliasId,
                SMSFromDefinedValueId = model.SMSFromDefinedValueId,
                SMSMessage = model.SMSMessage,
                Subject = model.Subject,
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
    public static partial class CommunicationTemplateExtensionMethods
    {
        /// <summary>
        /// Clones this CommunicationTemplate object to a new CommunicationTemplate object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static CommunicationTemplate Clone( this CommunicationTemplate source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as CommunicationTemplate;
            }
            else
            {
                var target = new CommunicationTemplate();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another CommunicationTemplate object to this CommunicationTemplate object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this CommunicationTemplate target, CommunicationTemplate source )
        {
            target.Id = source.Id;
            target.BCCEmails = source.BCCEmails;
            target.CategoryId = source.CategoryId;
            target.CCEmails = source.CCEmails;
            target.CssInliningEnabled = source.CssInliningEnabled;
            target.Description = source.Description;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.FromEmail = source.FromEmail;
            target.FromName = source.FromName;
            target.ImageFileId = source.ImageFileId;
            target.IsActive = source.IsActive;
            target.IsSystem = source.IsSystem;
            target.LavaFieldsJson = source.LavaFieldsJson;
            target.LogoBinaryFileId = source.LogoBinaryFileId;
            target.Message = source.Message;
            target.MessageMetaData = source.MessageMetaData;
            target.Name = source.Name;
            target.PushData = source.PushData;
            target.PushImageBinaryFileId = source.PushImageBinaryFileId;
            target.PushMessage = source.PushMessage;
            target.PushOpenAction = source.PushOpenAction;
            target.PushOpenMessage = source.PushOpenMessage;
            target.PushSound = source.PushSound;
            target.PushTitle = source.PushTitle;
            target.ReplyToEmail = source.ReplyToEmail;
            target.SenderPersonAliasId = source.SenderPersonAliasId;
            target.SMSFromDefinedValueId = source.SMSFromDefinedValueId;
            target.SMSMessage = source.SMSMessage;
            target.Subject = source.Subject;
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
        public static Rock.ViewModel.CommunicationTemplateViewModel ToViewModel( this CommunicationTemplate model, Person currentPerson = null, bool loadAttributes = false )
        {
            var helper = new CommunicationTemplateViewModelHelper();
            var viewModel = helper.CreateViewModel( model, currentPerson, loadAttributes );
            return viewModel;
        }

    }

}
