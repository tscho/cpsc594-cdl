<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <link rel="Stylesheet" href="/content/jquery-ui-1.8.7.custom.css" />
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript" language="javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.7.custom.min.js" type="text/javascript" language="javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%@ Import Namespace="cpsc594_cdl.Models" %>
<body>
    <div id="content">
    <div id="info">
        <% if (Html.ValidationSummary()!=null) { %>
            <%: Html.ValidationSummary(true, "Chart creation was unsuccessful. Please try again.") %>
        <% } else { %> 
            <h1><%= Model.ProjectName%></h1>
            <div id="tabs" style="overflow: visible; width: 100%;">
                <ul>
                    <li><a href="#overview">Overview</a></li>
                    <% foreach (var comp in Model.Components)
                       { %>
                        <li><a href="#<%= Html.Encode(comp.ComponentID) %>"><%= Html.Encode(comp.Name)%></a></li>
                    <% } %>
                </ul>
                <div id="overview">
                    <div>
                        <img src="data:image/png;base64,<%= CoverageMetric.GenerateHistogram(Model.Components, "Project Coverage Overview") %>" alt="Overview" /><br />
                        <img src="data:image/png;base64,<%= DefectInjectionMetric.GenerateHistogram(Model.Components, "Project Defect Injection Rate Overview") %>" alt="Overview" /><br />
                    </div>
                </div>
                <% foreach (var comp in Model.Components)
                   { %>
                       <div id="<%= Html.Encode(comp.ComponentID) %>">
                            <img src="data:image/png;base64,<%= CoverageMetric.GenerateHistogram(new Component[] {comp}, comp.Name+" Coverage History") %>" alt="<%= Html.Encode(comp.Name) %>" /><br />
                            <img src="data:image/png;base64,<%= DefectInjectionMetric.GenerateHistogram(new Component[] {comp}, comp.Name+" Defect Injection Rate History") %>" alt="<%= Html.Encode(comp.Name) %>" /><br />
                       </div>
                   <% } %>
            </div>
        <% } %>
    </div>
    <script type="text/javascript" language="javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    </div>
</body>
</asp:Content>
