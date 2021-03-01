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

export default interface History extends Entity {
    Caption: string | null;
    CategoryId: number;
    ChangeType: string | null;
    EntityId: number;
    EntityTypeId: number;
    IsSensitive: boolean | null;
    IsSystem: boolean;
    NewRawValue: string | null;
    NewValue: string | null;
    OldRawValue: string | null;
    OldValue: string | null;
    RelatedData: string | null;
    RelatedEntityId: number | null;
    RelatedEntityTypeId: number | null;
    SourceOfChange: string | null;
    ValueName: string | null;
    Verb: string | null;
    CreatedDateTime: RockDateType | null;
    ModifiedDateTime: RockDateType | null;
    CreatedByPersonAliasId: number | null;
    ModifiedByPersonAliasId: number | null;
}
