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

export default interface GroupType extends Entity {
    Id: number;
    AdministratorTerm: string | null;
    AllowAnyChildGroupType: boolean;
    AllowedScheduleTypes: number;
    AllowGroupSync: boolean;
    AllowMultipleLocations: boolean;
    AllowSpecificGroupMemberAttributes: boolean;
    AllowSpecificGroupMemberWorkflows: boolean;
    AttendanceCountsAsWeekendService: boolean;
    AttendancePrintTo: number;
    AttendanceRule: number;
    Attributes: Record<string, unknown>;
    DefaultGroupRoleId: number | null;
    Description: string | null;
    EnableGroupHistory: boolean;
    EnableGroupTag: boolean;
    EnableInactiveReason: boolean;
    EnableLocationSchedules: boolean | null;
    EnableRSVP: boolean;
    EnableSpecificGroupRequirements: boolean;
    GroupAttendanceRequiresLocation: boolean;
    GroupAttendanceRequiresSchedule: boolean;
    GroupCapacityRule: number;
    GroupMemberTerm: string | null;
    GroupsRequireCampus: boolean;
    GroupStatusDefinedTypeId: number | null;
    GroupTerm: string | null;
    GroupTypeColor: string | null;
    GroupTypePurposeValueId: number | null;
    GroupViewLavaTemplate: string | null;
    IconCssClass: string | null;
    IgnorePersonInactivated: boolean;
    InheritedGroupTypeId: number | null;
    IsIndexEnabled: boolean;
    IsSchedulingEnabled: boolean;
    IsSystem: boolean;
    LocationSelectionMode: number;
    Name: string | null;
    Order: number;
    RequiresInactiveReason: boolean;
    RequiresReasonIfDeclineSchedule: boolean;
    RSVPReminderOffsetDays: number | null;
    RSVPReminderSystemCommunicationId: number | null;
    ScheduleCancellationWorkflowTypeId: number | null;
    ScheduleConfirmationEmailOffsetDays: number | null;
    ScheduleConfirmationSystemCommunicationId: number | null;
    ScheduleReminderEmailOffsetDays: number | null;
    ScheduleReminderSystemCommunicationId: number | null;
    SendAttendanceReminder: boolean;
    ShowAdministrator: boolean;
    ShowConnectionStatus: boolean;
    ShowInGroupList: boolean;
    ShowInNavigation: boolean;
    ShowMaritalStatus: boolean;
    TakesAttendance: boolean;
    CreatedDateTime: RockDateType | null;
    ModifiedDateTime: RockDateType | null;
    CreatedByPersonAliasId: number | null;
    ModifiedByPersonAliasId: number | null;
    Guid: Guid;
}
