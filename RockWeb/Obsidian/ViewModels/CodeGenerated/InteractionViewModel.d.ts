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

export default interface Interaction extends Entity {
    Campaign: string | null;
    ChannelCustom1: string | null;
    ChannelCustom2: string | null;
    ChannelCustomIndexed1: string | null;
    Content: string | null;
    EntityId: number | null;
    InteractionComponentId: number;
    InteractionData: string | null;
    InteractionDateTime: RockDateType;
    InteractionEndDateTime: RockDateType | null;
    InteractionLength: number | null;
    InteractionSessionId: number | null;
    InteractionSummary: string | null;
    InteractionTimeToServe: number | null;
    Medium: string | null;
    Operation: string | null;
    PersonalDeviceId: number | null;
    PersonAliasId: number | null;
    RelatedEntityId: number | null;
    RelatedEntityTypeId: number | null;
    Source: string | null;
    Term: string | null;
    CreatedDateTime: RockDateType | null;
    ModifiedDateTime: RockDateType | null;
    CreatedByPersonAliasId: number | null;
    ModifiedByPersonAliasId: number | null;
}
