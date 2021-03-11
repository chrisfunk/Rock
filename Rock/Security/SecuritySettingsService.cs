﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Rock.Utility.Enums;
using Rock.Web;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Data;

namespace Rock.Security
{
    /// <summary>
    /// Class for handling rock security settings.
    /// </summary>
    public class SecuritySettingsService
    {
        private const string SYSTEM_SETTING_KEY = "core_RockSecuritySettings";
        private List<ValidationResult> _validationResults;
        /// <summary>
        /// Gets the validation results.
        /// </summary>
        /// <value>
        /// The validation results.
        /// </value>
        public virtual List<ValidationResult> ValidationResults
        {
            get { return _validationResults; }
        }

        /// <summary>
        /// Gets the security settings.
        /// </summary>
        /// <value>
        /// The security settings.
        /// </value>
        public SecuritySettings SecuritySettings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuritySettingsService"/> class.
        /// </summary>
        public SecuritySettingsService()
        {
            _validationResults = new List<ValidationResult>();
            SecuritySettings = SystemSettings.GetValue( SYSTEM_SETTING_KEY ).FromJsonOrNull<SecuritySettings>() ?? GetDefaultSecuritySettings();
        }

        /// <summary>
        /// Gets the default security settings.
        /// </summary>
        /// <returns></returns>
        private SecuritySettings GetDefaultSecuritySettings()
        {
            Group adminGroup = null;
            Group dataIntegrityGroup = null;

            using (var rockContext = new RockContext() )
            {
                var groupService = new GroupService( rockContext );
                adminGroup = groupService.GetByGuid( SystemGuid.Group.GROUP_ADMINISTRATORS.AsGuid() );
                dataIntegrityGroup = groupService.GetByGuid( SystemGuid.Group.GROUP_DATA_INTEGRITY_WORKER.AsGuid() );
            }

            var allRoles = RoleCache.AllRoles();
            var adminRole = allRoles
                .Where( r => r.Guid == SystemGuid.Group.GROUP_ADMINISTRATORS.AsGuid() )
                .FirstOrDefault();
            var dataIntegrityRole = allRoles
                .Where( r => r.Guid == SystemGuid.Group.GROUP_DATA_INTEGRITY_WORKER.AsGuid() )
                .FirstOrDefault();

            return new SecuritySettings
            {
                AccountProtectionProfilesForDuplicateDetectionToIgnore = new List<AccountProtectionProfile> {
                    AccountProtectionProfile.Extreme,
                    AccountProtectionProfile.High,
                    AccountProtectionProfile.Medium
                },
                AccountProtectionProfileSecurityGroup = new Dictionary<AccountProtectionProfile, RoleCache>
                {
                    { AccountProtectionProfile.Extreme, adminRole },
                    { AccountProtectionProfile.High, dataIntegrityRole }
                }
            };
        }

        /// <summary>
        /// Saves the SecuritySettings data.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            if ( Validate() )
            {
                SystemSettings.SetValue( SYSTEM_SETTING_KEY, this.SecuritySettings.ToJson() );
                return true;
            }
            return false;
        }

        /// <summary>
        /// Validates the SecuritySettings data.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            var valContext = new ValidationContext( this.SecuritySettings, serviceProvider: null, items: null );
            var isValid = Validator.TryValidateObject( this.SecuritySettings, valContext, _validationResults, true );

            if ( SecuritySettings?.AccountProtectionProfilesForDuplicateDetectionToIgnore == null )
            {
                ValidationResults.Add( new ValidationResult( "The account protection profile list is null." ) );
                isValid = false;
            }

            // Validate Groups are security groups.
            var securityGroupsToValidate = SecuritySettings?.AccountProtectionProfileSecurityGroup?.Values.Select( g => g );
            if ( securityGroupsToValidate == null )
            {
                // The only way invalidGroups would be null is if the SecuritySettings or property is null.
                ValidationResults.Add( new ValidationResult( "The account protection profile security group list is null." ) );
                isValid = false;
            }
            
            return isValid;
        }
    }
}
