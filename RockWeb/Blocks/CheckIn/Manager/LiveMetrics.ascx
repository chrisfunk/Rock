﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LiveMetrics.ascx.cs" Inherits="RockWeb.Blocks.CheckIn.Manager.LiveMetrics" %>

<script type="text/javascript">
    Sys.Application.add_load(function () {
        loadCharts();
        $('.js-threshold-btn-edit').off('click').on('click', function (e) {
            var $parentDiv = $(this).closest('div.js-threshold');
            $parentDiv.find('.js-threshold-nb').val($parentDiv.find('.js-threshold-hf').val());
            $parentDiv.find('.js-threshold-view').hide();
            $parentDiv.find('.js-threshold-edit').show();
        });

        $('a.js-threshold-edit').off('click').on('click', function (e) {
            var $parentDiv = $(this).closest('div.js-threshold');
            $parentDiv.find('.js-threshold-edit').hide();
            $parentDiv.find('.js-threshold-view').show();
            return true;
        });

        $('.js-threshold').on('click', function (e) {
            e.stopPropagation();
        });
    });

     <%-- Load the Analytics Charts --%>
    function loadCharts() {

        var chartData = eval($('#<%=hfChartData.ClientID%>').val());
        var chartLabel = eval($('#<%=hfChartLabel.ClientID%>').val());

        var options = {
            maintainAspectRatio: false,
            legend: {
                position: 'bottom',
                display: false
            },
            tooltips: {
                enabled: true,
                backgroundColor: '#000',
                bodyFontColor: '#fff',
                titleFontColor: '#fff'
            }
            , scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                        stepSize: 1
                    },
                }]
            }
        };

        var data = {
            labels: chartLabel,
            datasets: [{
                label: 'Room 1',
                fill: false,
                backgroundColor: '#059BFF',
                borderColor: '#059BFF',
                borderWidth: 0,
                pointRadius: 3,
                pointBackgroundColor: '#059BFF',
                pointBorderColor: '#059BFF',
                pointBorderWidth: 0,
                pointHoverBackgroundColor: 'rgba(5,155,255,.6)',
                pointHoverBorderColor: 'rgba(5,155,255,.6)',
                pointHoverRadius: '3',
                lineTension: 0,
                data: chartData
            }
            ],
            borderWidth: 0
        };

        Chart.defaults.global.defaultFontColor = '#777';
        Chart.defaults.global.defaultFontFamily = 'sans-serif';

        var ctx = document.getElementById( '<%=chartCanvas.ClientID%>').getContext('2d');
        var chart = new Chart(ctx, {
            type: 'line',
            data: data,
            options: options
        });

    }
</script>
<style>
    .metric {
        border: 1px solid #ccc;
        padding: 12px;
        margin-bottom: 12px;
    }
</style>

<Rock:RockUpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <Rock:NotificationBox ID="nbWarning" runat="server" NotificationBoxType="Warning" Dismissable="true" />

        <asp:Panel ID="pnlContent" runat="server" CssClass="checkin-manager">

            <div class="panel-heading hidden">
                <h1 class="panel-title"><i class="fa fa-sitemap"></i>&nbsp;<asp:Literal ID="lGroupTypeName" runat="server" /></h1>
            </div>

            <div class="panel">
                <asp:HiddenField ID="hfChartData" runat="server" />
                <asp:HiddenField ID="hfChartLabel" runat="server" />
                <asp:Literal ID="lChartCanvas" runat="server" />
                <div id="pnlChart" runat="server" class="chart-banner">
                    <canvas id="chartCanvas" runat="server" />
                </div>
            </div>

            <div class="row">
                <asp:Panel ID="pnlCheckedIn" runat="server" CssClass="col-lg-4">
                    <div class="metric">
                        <b>Checked-In People</b>
                        <h5>
                            <asp:Literal ID="lCheckedInPeople" runat="server" /></h5>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlPending" runat="server" CssClass="col-lg-4">
                    <div class="metric">
                        <b>Pending People</b>
                        <h5>
                            <asp:Literal ID="lPendingPeople" runat="server" /></h5>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlCheckedOut" runat="server" CssClass="col-lg-4">
                    <div class="metric">
                        <b>Checked-Out People</b>
                        <h5>
                            <asp:Literal ID="lCheckedOutPeople" runat="server" /></h5>
                    </div>
                </asp:Panel>
            </div>

            <div class="panel panel-default">

                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" />

                <asp:Panel ID="pnlNavHeading" runat="server" CssClass="panel-heading cursor-pointer clearfix">
                    <asp:PlaceHolder runat="server">
                        <div class="pull-left">
                            <i class="fa fa-chevron-left"></i>
                            <asp:Literal ID="lNavHeading" runat="server" />
                        </div>
                        <div class="pull-right">
                            <asp:Literal ID="lHeadingStatus" runat="server" />
                        </div>
                        <asp:Panel ID="pnlThreshold" runat="server" CssClass="pull-right margin-r-md margin-t-sm js-threshold paneleditor">
                            <span class="paneleditor-label">Threshold:</span>
                            <Rock:HiddenFieldWithClass ID="hfThreshold" runat="server" CssClass="js-threshold-hf" />
                            <asp:Label ID="lThreshold" runat="server" CssClass="js-threshold-view js-threshold-l" />
                            <a class="btn btn-default btn-xs js-threshold-view js-threshold-btn-edit"><i class="fa fa-edit"></i></a>
                            <Rock:NumberBox ID="nbThreshold" runat="server" CssClass="input-width-xs js-threshold-edit js-threshold-nb paneleditor-input" NumberType="Integer" Style="display: none"></Rock:NumberBox>
                            <asp:LinkButton ID="lbUpdateThreshold" runat="server" CssClass="btn btn-success btn-xs js-threshold-edit js-threshold-btn-save paneleditor-button" OnClick="lbUpdateThreshold_Click" Style="display: none"><i class="fa fa-check"></i></asp:LinkButton>
                            <a class="btn btn-warning btn-xs js-threshold-edit js-threshold-btn-cancel paneleditor-button" style="display: none"><i class="fa fa-ban"></i></a>
                        </asp:Panel>
                    </asp:PlaceHolder>
                </asp:Panel>

                <ul class="list-group">
                    <asp:Repeater ID="rptNavItems" runat="server">
                        <ItemTemplate>
                            <li id="liNavItem" runat="server" class="list-group-item cursor-pointer">
                                <div class="content"><%# Eval("Name") %></div>
                                <div class="pull-right d-flex align-items-center">
                                    <asp:Literal ID="lStatus" runat="server" />
                                    <asp:Label ID="lblCurrentCount" runat="server" CssClass="badge mr-2" />
                                    <i class='fa fa-fw fa-chevron-right'></i>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Repeater ID="rptPeople" runat="server">
                        <ItemTemplate>
                            <li id="liNavItem" runat="server" class="list-group-item d-flex align-items-center cursor-pointer clearfix">
                                <div class="d-flex align-items-center">
                                    <asp:Literal ID="imgPerson" runat="server" />
                                    <div>
                                        <span class="js-checkin-person-name"><%# Eval("Name") %></span><asp:Literal ID="lAge" runat="server" />
                                        <%# Eval("ScheduleGroupNames") %>
                                    </div>
                                </div>
                                <div class="ml-auto">
                                    <asp:Literal ID="lStatus" runat="server" />
                                    <asp:Literal ID="lMobileStatus" runat="server" />
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>

                </ul>

            </div>

        </asp:Panel>

    </ContentTemplate>
</Rock:RockUpdatePanel>
