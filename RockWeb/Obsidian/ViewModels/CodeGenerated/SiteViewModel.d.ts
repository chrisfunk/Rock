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

import Entity from '../Entity.js';
import { RockDateType } from '../../Util/RockDate.js';
import { Guid } from '../../Util/Guid.js';

export default interface Site extends Entity {
    Id: number;
    AdditionalSettings: string | null;
    AllowedFrameDomains: string | null;
    AllowIndexing: boolean;
    Attributes: Record<string, unknown>;
    ChangePasswordPageId: number | null;
    ChangePasswordPageRouteId: number | null;
    CommunicationPageId: number | null;
    CommunicationPageRouteId: number | null;
    ConfigurationMobilePhoneBinaryFileId: number | null;
    ConfigurationMobileTabletBinaryFileId: number | null;
    DefaultPageId: number | null;
    DefaultPageRouteId: number | null;
    Description: string | null;
    EnabledForShortening: boolean;
    EnableExclusiveRoutes: boolean;
    EnableMobileRedirect: boolean;
    EnablePageViews: boolean;
    ErrorPage: string | null;
    ExternalUrl: string | null;
    FavIconBinaryFileId: number | null;
    GoogleAnalyticsCode: string | null;
    IndexStartingLocation: string | null;
    IsActive: boolean;
    IsIndexEnabled: boolean;
    IsSystem: boolean;
    LatestVersionDateTime: RockDateType | null;
    LoginPageId: number | null;
    LoginPageRouteId: number | null;
    MobilePageId: number | null;
    Name: string | null;
    PageHeaderContent: string | null;
    PageNotFoundPageId: number | null;
    PageNotFoundPageRouteId: number | null;
    RedirectTablets: boolean;
    RegistrationPageId: number | null;
    RegistrationPageRouteId: number | null;
    RequiresEncryption: boolean;
    SiteLogoBinaryFileId: number | null;
    SiteType: number;
    Theme: string | null;
    ThumbnailBinaryFileId: number | null;
    CreatedDateTime: RockDateType | null;
    ModifiedDateTime: RockDateType | null;
    CreatedByPersonAliasId: number | null;
    ModifiedByPersonAliasId: number | null;
    Guid: Guid;
}
