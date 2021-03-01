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

export default interface DataView extends Entity {
    CategoryId: number | null;
    DataViewFilterId: number | null;
    Description: string | null;
    EntityTypeId: number | null;
    IncludeDeceased: boolean;
    IsSystem: boolean;
    LastRunDateTime: RockDateType | null;
    Name: string;
    PersistedLastRefreshDateTime: RockDateType | null;
    PersistedLastRunDurationMilliseconds: number | null;
    PersistedScheduleIntervalMinutes: number | null;
    RunCount: number | null;
    RunCountLastRefreshDateTime: RockDateType | null;
    TimeToRunDurationMilliseconds: number | null;
    TransformEntityTypeId: number | null;
    CreatedDateTime: RockDateType | null;
    ModifiedDateTime: RockDateType | null;
    CreatedByPersonAliasId: number | null;
    ModifiedByPersonAliasId: number | null;
}
