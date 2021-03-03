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

export default interface Location extends Entity {
    Id: number;
    AssessorParcelId: string | null;
    Attributes: Record<string, unknown>;
    Barcode: string | null;
    City: string | null;
    Country: string | null;
    County: string | null;
    FirmRoomThreshold: number | null;
    GeocodeAttemptedDateTime: RockDateType | null;
    GeocodeAttemptedResult: string | null;
    GeocodeAttemptedServiceType: string | null;
    GeocodedDateTime: RockDateType | null;
    GeoFence: Record<string, unknown>;
    GeoPoint: Record<string, unknown>;
    ImageId: number | null;
    IsActive: boolean;
    IsGeoPointLocked: boolean | null;
    LocationTypeValueId: number | null;
    Name: string | null;
    ParentLocationId: number | null;
    PostalCode: string | null;
    PrinterDeviceId: number | null;
    SoftRoomThreshold: number | null;
    StandardizeAttemptedDateTime: RockDateType | null;
    StandardizeAttemptedResult: string | null;
    StandardizeAttemptedServiceType: string | null;
    StandardizedDateTime: RockDateType | null;
    State: string | null;
    Street1: string | null;
    Street2: string | null;
    CreatedDateTime: RockDateType | null;
    ModifiedDateTime: RockDateType | null;
    CreatedByPersonAliasId: number | null;
    ModifiedByPersonAliasId: number | null;
    Guid: Guid;
}
