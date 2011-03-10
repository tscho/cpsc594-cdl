<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%@ Import Namespace="cpsc594_cdl.Models" %>
<body>
    <div id="content">
    <input id="toggle" type="submit" value="Show Menu" onclick="menu_toggle();return false;" />
    <div id="info">
        <% if (Html.ValidationSummary()!=null) { %>
            <%: Html.ValidationSummary(true, "Chart creation was unsuccessful. Please try again.") %>
        <% } else { %> 
            <h1><%= Model.ProjectName%></h1>
            <div id="tabs" class="tabs" style="overflow: visible; width: 100%;">
                <ul>
                    <li><a href="#overview">Overview</a></li>
                    <% foreach (var comp in Model.Components)
                       { %>
                        <li><a href="#<%= Html.Encode(comp.ComponentID) %>"><%= Html.Encode(comp.ComponentName)%></a></li>
                    <% } %>
                </ul>
                <div id="overview">
                    <div class="tabs" id="overview-metrics">
                        <ul>
                            <% foreach (var metric in Model.Metrics)
                               { %>
                                <li><a href="#overview-<%= Html.Encode(metric.ID) %>"><%= Html.Encode(metric.Name) %></a> </li>
                               <% } %>
                        </ul>
                        <% foreach (var metric in Model.Metrics)
                           { %>
                            <div id="overview-<%= Html.Encode(metric.ID) %>">
                                <img src="<%= metric.GenerateOverviewGraph(metric.Name + " Overview", Model.Components) %>" alt="Overview" /><br />
                            </div>
                           <% } %>
                    </div>
                </div>
                <% foreach (var comp in Model.Components)
                   { %>
                       <div id="<%= Html.Encode(comp.ComponentID) %>">
                            <div class="tabs" id="<%= Html.Encode(comp.ComponentID) %>-metrics">
                                <ul>
                                    <% foreach (var metric in Model.Metrics)
                                       { %>
                                           <% if(!metric.OverviewOnly) 
                                              { %>
                                                <li><a href="#<%= Html.Encode(comp.ComponentID) %>-<%= Html.Encode(metric.ID) %>"><%= Html.Encode(metric.Name) %></a> </li>
                                           <% } %>
                                       <% } %>
                                </ul>
                                <% foreach (var metric in Model.Metrics)
                                   { %>
                                   <% if(!metric.OverviewOnly) 
                                      { %>
                                        <div id="<%= Html.Encode(comp.ComponentID) %>-<%= Html.Encode(metric.ID) %>">
                                            <img src="<%= metric.GenerateComponentGraph(comp.ComponentName + " " + metric.Name, comp) %>" 
                                            alt="<%= Html.Encode(comp.ComponentName + " " + metric.Name) %>" /><br />
                                        </div>
                                   <% } %>
                               <% } %>
                            </div>
                       </div>
                   <% } %>
            </div>
        <% } %>
    </div>
    <script type="text/javascript" language="javascript">
        $(function () {
            $(".tabs").tabs();
        });
    </script>
    </div>
</body>
</asp:Content>
