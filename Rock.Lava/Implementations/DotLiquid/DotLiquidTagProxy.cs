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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using DotLiquid;
using Rock.Lava.Blocks;

namespace Rock.Lava.DotLiquid
{
    /// <summary>
    /// A wrapper for a Lava Tag that is compatible with the DotLiquid templating engine.
    /// </summary>
    /// <remarks>
    /// The DotLiquid framework processes a custom tag by creating a new instance of the Tag object from a registered Type and initializing the new instance
    /// by calling the Initialize() method.
    /// We need to intercept this process and generate a proxy Tag from a source class that does not inherit from the DotLiquid.Tag base class.
    /// This proxy class is instantiated by the DotLiquid framework, and we generate an internal instance of a Lava tag that performs the processing.
    /// </remarks>
    internal class DotLiquidTagProxy : Tag, ILiquidFrameworkElementRenderer
    {
        private static Dictionary<string, Func<string, IRockLavaTag>> _factoryMethods = new Dictionary<string, Func<string, IRockLavaTag>>( StringComparer.OrdinalIgnoreCase );

        public static void RegisterFactory( string name, Func<string, IRockLavaTag> factoryMethod )
        {
            if ( string.IsNullOrWhiteSpace( name ) )
            {
                throw new ArgumentException( "Name must be specified." );
            }

            name = name.Trim().ToLower();

            _factoryMethods[name] = factoryMethod;
        }

        private IRockLavaTag _lavaElement = null;

        public string SourceElementName
        {
            get
            {
                return _lavaElement.SourceElementName;
            }
        }

        #region DotLiquid Tag Overrides

        public override void Initialize( string tagName, string markup, List<string> tokens )
        {
            if ( !_factoryMethods.ContainsKey( tagName ) )
            {
                throw new Exception( "Block factory could not be found." );
            }

            var factoryMethod = _factoryMethods[tagName];

            _lavaElement = factoryMethod( tagName );

            if ( _lavaElement == null )
            {
                throw new Exception( "Block factory could not provide a compatible block instance." );
            }

            // Initialize the Lava block first, because it may be called during the DotLiquid.Block initialization process.
            _lavaElement.OnInitialize( tagName, markup, tokens );

            // Initialize the DotLiquid block.
            base.Initialize( tagName, markup, tokens );
        }

        public override void Render( Context context, TextWriter result )
        {
            var lavaContext = new DotLiquidLavaContext( context );

            var tag = _lavaElement as ILiquidFrameworkElementRenderer;

            if ( tag == null )
            {
                throw new Exception( "Tag proxy cannot be rendered." );
            }

            // Call the renderer implemented by the wrapped Lava block.
            tag.Render( this, lavaContext, result, null );
        }

        #endregion

        #region ILiquidFrameworkElementRenderer implementation

        void ILiquidFrameworkElementRenderer.Render( ILiquidFrameworkElementRenderer baseRenderer, ILavaContext context, TextWriter result, TextEncoder encoder )
        {
            var dotLiquidContext = ( (DotLiquidLavaContext)context ).DotLiquidContext;

            base.Render( dotLiquidContext, result );
        }

        public void OnStartup()
        {
            throw new NotImplementedException( "The OnStartup method is not a valid operation for the DotLiquidTagProxy." );
        }

        void ILiquidFrameworkElementRenderer.Parse( ILiquidFrameworkElementRenderer baseRenderer, List<string> tokens, out List<object> nodes )
        {
            base.Parse( tokens );

            nodes = base.NodeList;
        }

        #endregion

    }
}
