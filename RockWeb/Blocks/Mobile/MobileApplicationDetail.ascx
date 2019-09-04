﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MobileApplicationDetail.ascx.cs" Inherits="RockWeb.Blocks.Mobile.MobileApplicationDetail" %>

<style>
    .mobile-app-preview {
        padding: 20px;
        border: 1px solid #bbb;
        border-radius: 18px;
    }
    .mobile-app-preview img {
        width: 100%;
        height: auto;
        display: block;
        min-height: 50px;
        background-color: #ddd;
    }
    .mobile-app-icon {
        width: 100%;
        height: auto;
        display: block;
    }
</style>

<asp:UpdatePanel ID="upContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbError" runat="server" NotificationBoxType="Danger" />

        <asp:Panel ID="pnlOverview" runat="server" CssClass="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">
                    <i class="fa fa-mobile"></i>
                    <asp:Literal ID="ltAppName" runat="server" />

                    <div class="panel-labels">
                        <span class="label label-default">Site Id: <asp:Literal ID="lSiteId" runat="server" /></span>
                        <asp:Literal ID="lLastDeployDate" runat="server" />
                    </div>
                </h3>
            </div>

            <div class="panel-body">
                <asp:Panel ID="pnlContent" runat="server">
                    <asp:HiddenField ID="hfSiteId" runat="server" />
                    <asp:HiddenField ID="hfCurrentTab" runat="server" />

                    <div class="row">
                        <div class="col-md-12 col-lg-12">
                            <ul class="nav nav-tabs margin-b-lg">
                                <li id="liTabApplication" runat="server">
                                    <asp:LinkButton ID="lbTabApplication" runat="server" OnClick="lbTabApplication_Click">Application</asp:LinkButton>
                                </li>
                                <li id="liTabStyles" runat="server">
                                    <asp:LinkButton ID="lbTabStyles" runat="server" OnClick="lbTabStyles_Click">Styles</asp:LinkButton>
                                </li>
                                <li id="liTabLayouts" runat="server">
                                    <asp:LinkButton ID="lbTabLayouts" runat="server" OnClick="lbTabLayouts_Click">Layouts</asp:LinkButton>
                                </li>
                                <li id="liTabPages" runat="server">
                                    <asp:LinkButton ID="lbTabPages" runat="server" OnClick="lbTabPages_Click">Pages</asp:LinkButton>
                                </li>
                            </ul>

                            <asp:Panel ID="pnlApplication" runat="server">
                                <div class="row">
                                    <div class="col-md-8 col-lg-8">
                                        <fieldset>
                                            <p>
                                                <asp:Literal ID="ltDescription" runat="server" />
                                            </p>

                                            <div>
                                                <asp:Literal ID="ltAppDetails" runat="server" />
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-md-4 hidden-sm hidden-xs">
                                        <div class="row">
                                            <div class="col-sm-4">
                                                <asp:Image ID="imgAppIcon" runat="server" CssClass="mobile-app-icon" />
                                            </div>

                                            <div class="col-sm-8">
                                                <asp:Panel ID="pnlPreviewImage" runat="server" CssClass="mobile-app-preview">
                                                    <asp:Image ID="imgAppPreview" runat="server" />
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="actions margin-t-md">
                                    <asp:LinkButton ID="lbEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="lbEdit_Click" AccessKey="m" ToolTip="Alt+m" />
                                    <asp:LinkButton ID="lbCancel" runat="server" CssClass="btn btn-link" Text="Cancel" OnClick="lbCancel_Click" CausesValidation="false" AccessKey="c" ToolTip="Alt+c" />
                                    <div class="pull-right">
                                        <asp:LinkButton ID="lbDeploy" runat="server" CssClass="btn btn-default" OnClick="lbDeploy_Click" OnClientClick="Rock.dialogs.confirmPreventOnCancel( event, 'Are you sure you wish to replace the current package and deploy a new one?');"><i class="fa fa-upload"></i> Deploy</asp:LinkButton>
                                    </div>
                                </div>

                            </asp:Panel>

                            <asp:Panel ID="pnlStyles" runat="server">
                                <asp:HiddenField ID="hfShowAdvancedStylesFields" runat="server" />
                                <div class="row">
                                    <div class="col-md-4">
                                        <Rock:ColorPicker ID="cpEditBarBackgroundColor" runat="server" Label="Bar Background Color" Help="Override the default title bar background color provided by the mobile OS." />
                                    </div>
                                    <div class="col-md-4">
                                        <Rock:ColorPicker ID="cpEditMenuButtonColor" runat="server" Label="Menu Button Color" Help="The color of the menu button in the title bar."/>
                                    </div>
                                    <div class="col-md-4">
                                        <Rock:ColorPicker ID="cpEditActivityIndicatorColor" runat="server" Label="Activity Indicator Color" Help="Defines the color that will be used when displaying an activity indicator, these alert the user that something is happening in the background." Enabled="false" />
                                    </div>
                                </div>

                                <hr />

                                <Rock:RockControlWrapper ID="rcwAdditionalColors" runat="server" Label="Application Colors">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpPrimary" runat="server" Label="Primary" Help="Override the default color provided by the mobile OS." />
                                        </div>
                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpSecondary" runat="server" Label="Secondary" Help="Override the default color provided by the mobile OS." />
                                        </div>
                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpSuccess" runat="server" Label="Success" Help="Override the default color provided by the mobile OS." />
                                        </div>

                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpDanger" runat="server" Label="Danger" Help="Override the default color provided by the mobile OS." />
                                        </div>
                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpWarning" runat="server" Label="Warning" Help="Override the default color provided by the mobile OS." />
                                        </div>
                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpLight" runat="server" Label="Light" Help="Override the default color provided by the mobile OS." />
                                        </div>

                                        <div class="col-md-4 col-sm-6">
                                            <Rock:ColorPicker ID="cpDark" runat="server" Label="Dark" Help="Override the default color provided by the mobile OS." />
                                        </div>
                                    </div>
                                </Rock:RockControlWrapper>

                                <div class="row">
                                    <div class="col-md-4">
                                        <Rock:NumberBox ID="nbRadiusBase" runat="server" NumberType="Integer" Label="Radius Base" Help="" ></Rock:NumberBox>
                                    </div>
                                    <div class="col-md-4">
                                        <Rock:ImageUploader ID="imgEditHeaderImage" runat="server" Label="Header Image" Help="The image that appears on the top header. While the size is dependent on design we recommend a height of 120px and minimum width of 560px." />
                                    </div>
                                </div>

                                <div class="clearfix">
                                    <div class="pull-right">
                                        <a href="#" class="btn btn-xs btn-link js-show-advanced-style-fields" >Show Advanced Fields</a>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlStylesAdvancedFields" runat="server" CssClass="js-advanced-style-fields" style="display:none">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <Rock:NumberBox ID="nbSpacingBase" runat="server" NumberType="Integer" Label="Spacing Base"></Rock:NumberBox>
                                        </div>
                                        <div class="col-md-4">
                                            <Rock:NumberBox ID="nbFontSizeDefault" runat="server" NumberType="Integer" Label="Font Size Default"></Rock:NumberBox>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <div class="actions margin-t-md">
                                    <Rock:BootstrapButton ID="lbStylesEditSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="lbStylesEditSave_Click"
                                        DataLoadingText="&lt;i class='fa fa-refresh fa-spin'&gt;&lt;/i&gt; Saving"
                                        CompletedText="Saved"
                                        CompletedMessage="<div class='margin-t-md alert alert-success'>Changes have been saved.</div>"
                                        CompletedDuration="5"></Rock:BootstrapButton>
                                </div>

                            </asp:Panel>

                            <asp:Panel ID="pnlLayouts" runat="server">
                                <Rock:Grid ID="gLayouts" runat="server" RowItemText="Layout" DisplayType="Light" OnGridRebind="gLayouts_GridRebind" OnRowSelected="gLayouts_RowSelected">
                                    <Columns>
                                        <Rock:RockBoundField DataField="Name" SortExpression="Name" HeaderText="Name" />
                                        <Rock:RockBoundField DataField="Description" SortExpression="Description" HeaderText="Description" />
                                        <Rock:DeleteField OnClick="gLayouts_DeleteClick" />
                                    </Columns>
                                </Rock:Grid>
                            </asp:Panel>

                            <asp:Panel ID="pnlPages" runat="server">
                                <Rock:Grid ID="gPages" runat="server" RowItemText="Page" DisplayType="Light" OnGridRebind="gPages_GridRebind" OnRowSelected="gPages_RowSelected" OnGridReorder="gPages_GridReorder">
                                    <Columns>
                                        <Rock:ReorderField />
                                        <Rock:RockBoundField DataField="InternalName" SortExpression="Name" HeaderText="Name" />
                                        <Rock:RockBoundField DataField="LayoutName" SortExpression="LayoutName" HeaderText="Layout" />
                                        <Rock:BoolField DataField="DisplayInNav" SortExpression="DisplayInNav" HeaderText="Display In Nav" />
                                        <Rock:DeleteField OnClick="gPages_DeleteClick" />
                                    </Columns>
                                </Rock:Grid>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlEdit" runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockTextBox ID="tbEditName" runat="server" Label="Application Name" Required="true" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockCheckBox ID="cbEditActive" runat="server" Label="Active" />
                        </div>
                    </div>

                    <Rock:RockTextBox ID="tbEditDescription" runat="server" Label="Description" TextMode="MultiLine" />

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockRadioButtonList ID="rblEditApplicationType" runat="server" Label="Application Type" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblEditApplicationType_SelectedIndexChanged" AutoPostBack="true" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockRadioButtonList ID="rblEditAndroidTabLocation" runat="server" Label="Android Tab Location" RepeatDirection="Horizontal" Help="Determines where the Tab bar should be displayed on Android. iOS will always display at the bottom." />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockDropDownList ID="ddlEditLockPhoneOrientation" runat="server" Label="Lock Phone Orientation" Help="Setting this value will lock the orientation on phones to the specified orientation." />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockDropDownList ID="ddlEditLockTabletOrientation" runat="server" Label="Lock Tablet Orientation" Help="Setting this value will lock the orientation on tablets to the specified orientation." />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:PagePicker ID="ppEditLoginPage" runat="server" Label="Login Page" />
                        </div>

                        <div class="col-md-6">
                            <Rock:PagePicker ID="ppEditProfilePage" runat="server" Label="Profile Page" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockTextBox ID="tbEditApiKey" runat="server" Label="API Key" Required="true" />
                        </div>

                        <div class="col-md-6">
                            <Rock:CategoryPicker ID="cpEditPersonAttributeCategories" runat="server" Label="Person Attribute Categories" Help="All attributes in selected categories will be sent to the client and made available remotely."  AllowMultiSelect="true" />
                        </div>
                    </div>

                    <Rock:CodeEditor ID="ceEditCssStyles" runat="server" Label="CSS Styles" Help="CSS Styles to apply to UI elements." EditorMode="Css" />

                    <Rock:CodeEditor ID="ceEditFlyoutXaml" runat="server" Label="Flyout Xaml" Help="The XAML template to use for the menu in the Flyout Shell." EditorMode="Xml" Required="true" />

                    <div class="row">
                        <%--<div class="col-md-4">
                            <Rock:ImageUploader ID="imgEditHeaderImage" runat="server" Label="Header Image" Help="The image that appears on the top header. While the size is dependent on design we recommend a height of 120px and minimum width of 560px." />
                        </div>--%>

                        <div class="col-md-4">
                            <Rock:ImageUploader ID="imgEditPreviewThumbnail" runat="server" Label="Preview Thumbnail" Help="Preview thumbnail to be used by Rock to distinguish application." />
                        </div>
                    </div>

                    <div class="actions margin-t-md">
                        <asp:LinkButton ID="lbEditSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="lbEditSave_Click" AccessKey="s" ToolTip="Alt+s" />
                        <asp:LinkButton ID="lbEditCancel" runat="server" CssClass="btn btn-link" Text="Cancel" OnClick="lbEditCancel_Click" CausesValidation="false" AccessKey="c" ToolTip="Alt+c" />
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>

        <script type="text/javascript">
            Sys.Application.add_load(function () {
                $('.js-show-advanced-style-fields').off('click').on('click', function () {
                    var isVisible = !$('.js-advanced-style-fields').is(':visible');
                    $('#<%=hfShowAdvancedStylesFields.ClientID %>').val(isVisible);
                    $('.js-show-advanced-style-fields').text(isVisible ? 'Hide Advanced Fields' : 'Show Advanced Fields');
                    $('.js-advanced-style-fields').slideToggle();
                    return false;
                });

                if ($('#<%=hfShowAdvancedStylesFields.ClientID %>').val() == "true") {
                    $('.js-advanced-style-fields').show();
                    $('.js-show-advanced-style-fields').text('Hide Additional Fields');
                }
            });

        </script>
    </ContentTemplate>
</asp:UpdatePanel>