﻿// <copyright>
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

using Rock.Model;
using Rock.Web.Cache;

namespace Rock.ViewModel
{
    /// <summary>
    /// AttributeValueViewModel
    /// </summary>
    /// <seealso cref="Rock.ViewModel.IViewModel" />
    public partial class AttributeValueViewModel
    {
        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        [TypeScriptType( "Attribute | null", "import Attribute from './AttributeViewModel.js';" )]
        public AttributeViewModel Attribute { get; set; }

        /// <summary>
        /// Sets the properties from entity.
        /// </summary>
        /// <param name="entity">The entity, cache item, or some object.</param>
        /// <param name="currentPerson">The current person.</param>
        /// <param name="loadAttributes">if set to <c>true</c> [load attributes].</param>
        public override void SetPropertiesFrom( object entity, Person currentPerson = null, bool loadAttributes = true )
        {
            if ( entity == null )
            {
                return;
            }

            base.SetPropertiesFrom( entity, currentPerson, loadAttributes );

            if ( entity is AttributeValueCache attributeValueCache )
            {
                Attribute = AttributeCache.Get( attributeValueCache.AttributeId ).ToViewModel<AttributeViewModel>();
            }
            else if ( entity is AttributeValue attributeValue )
            {
                Attribute = AttributeCache.Get( attributeValue.AttributeId ).ToViewModel<AttributeViewModel>();
            }
        }
    }
}
