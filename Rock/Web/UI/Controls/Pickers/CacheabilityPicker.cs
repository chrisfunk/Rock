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

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Utility;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// This control yields a Cacheability picker control.
    /// </summary>
    /// <seealso cref="System.Web.UI.WebControls.CompositeControl" />
    /// <seealso cref="Rock.Web.UI.Controls.IRockControl" />
    public class CacheabilityPicker : CompositeControl, IRockControl
    {
        private const string CACHEABILITY_PICKER_NAME = "cacheabilityPicker";
        private const string MAX_AGE_UNIT_NAME = "maxAgeUnit";
        private const string MAX_AGE_VALUE_NAME = "maxAgeValue";
        private const string MAX_SHARED_AGE_UNIT_NAME = "maxSharedAgeUnit";
        private const string MAX_SHARED_AGE_VALUE_NAME = "maxSharedAgeValue";

        #region Internal Controls
        private RockRadioButtonList _cacheabilityType;
        private NumberBox _maxAgeValue;
        private RockDropDownList _maxAgeUnit;
        private Literal _maxAgeLabel;

        private NumberBox _maxSharedAgeValue;
        private RockDropDownList _maxSharedAgeUnit;
        private Literal _maxSharedAgeLabel;
        #endregion

        private RockCacheability _rockCacheability = new RockCacheability();
        /// <summary>
        /// Gets or sets the current cacheablity.
        /// </summary>
        /// <value>
        /// The current cacheablity.
        /// </value>
        public RockCacheability CurrentCacheablity
        {
            get => _rockCacheability;
            set => _rockCacheability = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheabilityPicker"/> class.
        /// </summary>
        public CacheabilityPicker() : base()
        {
            EnsureChildControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( Page.IsPostBack )
            {

                if ( _maxAgeValue.Text.IsNullOrWhiteSpace() )
                {
                    _rockCacheability.MaxAge = null;
                }
                else
                {
                    _rockCacheability.MaxAge = new TimeInterval
                    {
                        Unit = _maxAgeUnit.SelectedValue.ConvertToEnum<TimeIntervalUnit>(),
                        Value = _maxAgeValue.Text.AsInteger()
                    };
                }

                if ( _maxSharedAgeValue.Text.IsNullOrWhiteSpace() )
                {
                    _rockCacheability.SharedMaxAge = null;
                }
                else
                {
                    _rockCacheability.SharedMaxAge = new TimeInterval
                    {
                        Unit = _maxSharedAgeUnit.SelectedValue.ConvertToEnum<TimeIntervalUnit>(),
                        Value = _maxSharedAgeValue.Text.AsInteger()
                    };
                }

                _rockCacheability.RockCacheablityType = _cacheabilityType.SelectedValue.ConvertToEnum<RockCacheablityType>();
            }
        }

        #region IRockControl Implementation
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label text
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        public string Help { get; set; }
        /// <summary>
        /// Gets or sets the warning text.
        /// </summary>
        /// <value>
        /// The warning text.
        /// </value>
        public string Warning { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IRockControl" /> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }
        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage { get; set; }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsValid
        {
            get
            {
                return _cacheabilityType.IsValid;
            }
        }

        /// <summary>
        /// Gets or sets the form group class.
        /// </summary>
        /// <value>
        /// The form group class.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public string FormGroupCssClass { get; set; }
        /// <summary>
        /// Gets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public HelpBlock HelpBlock { get; set; }
        /// <summary>
        /// Gets the warning block.
        /// </summary>
        /// <value>
        /// The warning block.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public WarningBlock WarningBlock { get; set; }
        /// <summary>
        /// Gets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }
        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// This is where you implement the simple aspects of rendering your control.  The rest
        /// will be handled by calling RenderControlHelper's RenderControl() method.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
            _cacheabilityType.SelectedValue = _rockCacheability.RockCacheablityType.ConvertToInt().ToString();
            _cacheabilityType.Enabled = Enabled;

            if ( _rockCacheability.MaxAge != null )
            {
                _maxAgeUnit.SelectedValue = _rockCacheability.MaxAge.Unit.ConvertToInt().ToStringSafe();
            }
            _maxAgeUnit.Enabled = Enabled;

            _maxAgeValue.Text = _rockCacheability.MaxAge?.Value.ToStringSafe();
            _maxAgeValue.Enabled = Enabled;

            if ( _rockCacheability.SharedMaxAge != null )
            {
                _maxSharedAgeUnit.SelectedValue = _rockCacheability.SharedMaxAge?.Unit.ConvertToInt().ToStringSafe();
            }
            _maxSharedAgeUnit.Enabled = Enabled;

            _maxSharedAgeValue.Text = _rockCacheability.SharedMaxAge?.Value.ToStringSafe();
            _maxSharedAgeValue.Enabled = Enabled;

            writer.AddAttribute( HtmlTextWriterAttribute.Style, Style.Value );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            _cacheabilityType.RenderControl( writer );

            if ( _rockCacheability.OptionSupportsAge( _rockCacheability.RockCacheablityType ) )
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6 pl-0" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                _maxAgeLabel.RenderControl( writer );
                _maxAgeValue.RenderControl( writer );
                _maxAgeUnit.RenderControl( writer );
                writer.RenderEndTag();

                writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6" );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                _maxSharedAgeLabel.RenderControl( writer );
                _maxSharedAgeValue.RenderControl( writer );
                _maxSharedAgeUnit.RenderControl( writer );
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
        }
        #endregion

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                RockControlHelper.RenderControl( this, writer );
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Controls.Clear();

            _cacheabilityType = new RockRadioButtonList
            {
                ID = $"{ID}_{CACHEABILITY_PICKER_NAME}",
                Label = "Cacheability Type",
                Help = @"This determines how the item will be treated in cache.<br />
                                        Public - This item can be cached on the browser or any other shared network cache like a CDN.<br />
                                        Private - This item can only be cached in the browser.<br />
                                        No-Cache - The item will be checked on every load, but if it is deemed to not have changed since the last load it will use a local copy.<br />
                                        No-Store - This item will never be stored by the local browser.This is used for sensitive files like check images."
            };
            _cacheabilityType.Items.AddRange( new ListItem[] {
                new ListItem{ Text = "Public", Value=RockCacheablityType.Public.ConvertToInt().ToString()},
                new ListItem{ Text = "Private", Value=RockCacheablityType.Private.ConvertToInt().ToString()},
                new ListItem{ Text = "No-Cache", Value=RockCacheablityType.NoCache.ConvertToInt().ToString()},
                new ListItem{ Text = "No-Store", Value=RockCacheablityType.NoStore.ConvertToInt().ToString()}
            } );
            _cacheabilityType.RepeatDirection = RepeatDirection.Horizontal;
            _cacheabilityType.AutoPostBack = true;

            _cacheabilityType.SelectedIndexChanged += CacheabilityType_SelectedIndexChanged;

            CreateMaxAgeControls();
            CreateMaxSharedAgeControls();

            Controls.Add( _cacheabilityType );

            Controls.Add( _maxAgeLabel );
            Controls.Add( _maxAgeValue );
            Controls.Add( _maxAgeUnit );

            Controls.Add( _maxSharedAgeLabel );
            Controls.Add( _maxSharedAgeValue );
            Controls.Add( _maxSharedAgeUnit );
        }

        private void CreateMaxAgeControls()
        {
            _maxAgeUnit = new RockDropDownList
            {
                ID = $"{ID}_{MAX_AGE_UNIT_NAME}",
                CssClass = "w-auto pull-left"
            };

            _maxAgeUnit.Items.AddRange( new ListItem[]
            {
                new ListItem("Seconds", TimeIntervalUnit.Seconds.ConvertToInt().ToString()),
                new ListItem("Minutes", TimeIntervalUnit.Minutes.ConvertToInt().ToString()),
                new ListItem("Hours", TimeIntervalUnit.Hours.ConvertToInt().ToString()),
                new ListItem("Days", TimeIntervalUnit.Days.ConvertToInt().ToString())
            } );

            _maxAgeValue = new NumberBox
            {
                ID = $"{ID}_{MAX_AGE_VALUE_NAME}",
                CssClass = "w-auto pull-left"
            };

            _maxAgeLabel = new Literal
            {
                Text = @"<div><label class=""control-label"">Max Age<a class=""help""
                href=""#"" tabindex=""-1"" data-toggle=""tooltip"" data-placement=""auto""
                data-container=""body"" data-html=""true"" title=""The maximum amount of time that the item will be cached."">
                <i class=""fa fa-info-circle""></i></a></label></div>"
            };
        }

        private void CreateMaxSharedAgeControls()
        {
            _maxSharedAgeUnit = new RockDropDownList
            {
                ID = $"{ID}_{MAX_SHARED_AGE_UNIT_NAME}",
                CssClass = "w-auto pull-left"
            };
            _maxSharedAgeUnit.Items.AddRange( new ListItem[]
            {
                new ListItem("Seconds", TimeIntervalUnit.Seconds.ConvertToInt().ToString()),
                new ListItem("Minutes", TimeIntervalUnit.Minutes.ConvertToInt().ToString()),
                new ListItem("Hours", TimeIntervalUnit.Hours.ConvertToInt().ToString()),
                new ListItem("Days", TimeIntervalUnit.Days.ConvertToInt().ToString())
            } );

            _maxSharedAgeValue = new NumberBox
            {
                ID = $"{ID}_{MAX_SHARED_AGE_VALUE_NAME}",
                CssClass = "w-auto pull-left"
            };

            _maxSharedAgeLabel = new Literal
            {
                Text = @"<div><label class=""control-label"">Max Shared Age<a class=""help""
                href=""#"" tabindex=""-1"" data-toggle=""tooltip"" data-placement=""auto""
                data-container=""body"" data-html=""true"" title=""The maximum amount of time the item will be cached in a shared cache (e.g. CDN). If not provided then the Max Age is typically used."">
                <i class=""fa fa-info-circle""></i></a></label></div>"
            };
        }

        private void CacheabilityType_SelectedIndexChanged( object sender, EventArgs e )
        {
            _rockCacheability.RockCacheablityType = _cacheabilityType.SelectedValue.ConvertToEnum<RockCacheablityType>();
        }


    }
}