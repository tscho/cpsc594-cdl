<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MetricAnalyzer.Portal.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <meta http-equiv="cache-control" content="no-cache" />
    <link rel="Stylesheet" href="/content/jquery-ui-1.8.7.custom.css" />
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript" language="javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.7.custom.min.js" type="text/javascript" language="javascript"></script>
    <script type="text/javascript">
        var isHidden = true;
        function menu_toggle() {
            if (isHidden) {
               parent.document.getElementById('frame').cols = '400,*';
               document.getElementById('toggle').value = 'Hide Menu';
            } else {
               parent.document.getElementById('frame').cols = '0,*';
               document.getElementById('toggle').value = 'Show Menu';
            }
            isHidden = !isHidden;
        }
    </script>
    <script type="text/javascript" src="/Scripts/js/highcharts.src.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%@ Import Namespace="MetricAnalyzer.Portal.Models" %>
<%@ Import Namespace="MetricAnalyzer.Common.Models" %>
<body>
    <div id="content">
    <input id="toggle" type="submit" value="Show Menu" onclick="menu_toggle();return false;" />
    <div id="info">
        <% if (Html.ValidationSummary()!=null) { %>
            <%= Html.ValidationSummary(false, "Chart creation was unsuccessful. Please try again.") %>
        <% } else { %> 
            <div id="tabs" class="tabs" style="overflow: visible; width: 100%;">
                <ul>
                    <li><a href="#overview">Overview</a></li>
                    <% if (Model.ComponentMetrics.Count > 0)
                       { %>
                        <% foreach (var comp in Model.Components)
                           { %>
                            <li><a href="#<%= Html.Encode(comp.ComponentID) %>"><%= Html.Encode(comp.ComponentName)%></a></li>
                        <% } %>
                    <% } %>
                </ul>
                <div id="overview">
                    <div class="tabs" id="overview-metrics">
                        <ul>
                            <% foreach (var metric in Model.ProductMetrics)
                               { %>
                                <li><a href="#overview-<%= Html.Encode(metric.ID) %>"><%= Html.Encode(metric.Name) %></a> </li>
                               <% } %>
                            <% foreach (var metric in Model.ComponentMetrics)
                               { %>
                                <li><a href="#overview-<%= Html.Encode(metric.ID) %>"><%= Html.Encode(metric.Name) %></a> </li>
                               <% } %>
                        </ul>
                        <% foreach (var metric in Model.ProductMetrics)
                           { %>
                            <div id="overview-<%= Html.Encode(metric.ID) %>">
                                <!--<img src="<%= metric.GenerateOverviewGraph(metric.Name + " Overview", Model.Products) %>" alt="Overview" /><br />-->
                                <div id="<%= metric.StringEncode(Model.ProductIDs.ToArray()) %>" class="highchart" title="<%= metric.Name + " Overview" %>"></div>
                            </div>
                           <% } %>
                        <% foreach (var metric in Model.ComponentMetrics)
                           { %>
                            <div id="overview-<%= Html.Encode(metric.ID) %>">
                                <!--<img src="<%= metric.GenerateOverviewGraph(metric.Name + " Overview", Model.Components) %>" alt="Overview" /><br />-->
                                <div id="<%= metric.StringEncode(Model.Components.Select<Component, int>(x => x.ComponentID).ToArray()) %>" class="highchart"></div>
                            </div>
                           <% } %>
                    </div>
                </div>
                <% if (Model.ComponentMetrics.Count > 0)
                   { %>
                    <% foreach (var comp in Model.Components)
                       { %>
                           <div id="<%= Html.Encode(comp.ComponentID) %>">
                                <div class="tabs" id="<%= Html.Encode(comp.ComponentID) %>-metrics">
                                    <ul>
                                        <% foreach (var metric in Model.ComponentMetrics)
                                           { %>
                                                <li><a href="#<%= Html.Encode(comp.ComponentID) %>-<%= Html.Encode(metric.ID) %>"><%= Html.Encode(metric.Name)%></a> </li>
                                           <% } %>
                                    </ul>
                                    <% foreach (var metric in Model.ComponentMetrics)
                                       { %>
                                        <div id="<%= Html.Encode(comp.ComponentID) %>-<%= Html.Encode(metric.ID) %>">
                                            <!--<img src="<%= metric.GenerateComponentGraph(comp.ComponentName + " " + metric.Name, comp) %>" 
                                            alt="<%= Html.Encode(comp.ComponentName + " " + metric.Name) %>" /><br />-->
                                            <div id="<%= metric.StringEncode(new int[] { comp.ComponentID }) %>" class="highchart" specificComponent="true"></div>
                                        </div>
                                   <% } %>
                                </div>
                           </div>
                       <% } %>
                   <% } %>
            </div>
        <% } %>
    </div>
    <script type="text/javascript" language="javascript">
        var highcharts = new Object;
        function tryRender(ui) {
            target = $(ui.panel).find(".highchart");
            if (highcharts[target.attr("id")] == undefined) {
                renderChart(target);
            }
        }

        function renderChart(target) {
            $.ajax({
                type: "POST",
                url: "/Report/HighChart",
                data: {
                    title: target.attr("title"),
                    target: target.attr("id"),
                    encodedString: target.attr("id"),
                    specificComponent: target.attr("specificComponent")
                },
                success: function (data) {
                    highcharts[target.attr("id")] = new Highcharts.Chart(data);
                    return true;
                }
            });
        }

        $(function () {
            $(".tabs").tabs();
            /*
            $(".highchart").each(function () {
            $.ajax({
            type: "POST",
            url: "/Report/HighChart",
            data: {
            title: $(this).attr("title"),
            target: $(this).attr("id"),
            encodedString: $(this).attr("id"),
            specificComponent: $(this).attr("specificComponent")
            },
            success: function (data) {
            highcharts.push(new Highcharts.Chart(data));
            }
            });
            });
            */
            renderChart($(".highchart").first(), 0);
            $(".tabs").bind('tabsshow', function (event, ui) {
                event.stopPropagation();
                tryRender(ui);
                $(window).resize();
                $(".tabs").resize();
            });
        });
    </script>
    </div>
</body>
</asp:Content>
