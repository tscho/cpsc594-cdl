<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <link rel="Stylesheet" href="/content/jquery-ui-1.8.7.custom.css" />
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript" language="javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.7.custom.min.js" type="text/javascript" language="javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<body>
    <div id="content">
    <div id="info">
        <% if (Html.ValidationSummary()!=null) { %>
            <%: Html.ValidationSummary(true, "Chart creation was unsuccessful. Please try again.") %>
        <% } else { %> 
            <h1><%= Model.ProjectName%></h1>
            Components:
            <%= String.Join(",", Model.ComponentIDs)%>
            <br />
            Metrics:
            <%= String.Join(",", Model.MetricIDs)%>
            <br />
            Start From:
            <%= Model.StartDate%>
            <br />
            <div id="tabs">
                <ul>
                    <li><a href="#overview">Overview</a></li>
                    <% foreach (var comp in Model.Components)
                       { %>
                        <li><a href="#<%= Html.Encode(comp.ComponentID) %>"><%= Html.Encode(comp.Name)%></a></li>
                    <% } %>
                </ul>
                <div id="overview">
                    <img src="data:image/png;base64,<%= Model.Chart1_Base64 %>" alt="Chart 1" /><img src="data:image/png;base64,<%= Model.Chart2_Base64 %>" alt="Chart 2" /><br />
                    <img src="data:image/png;base64,<%= Model.Chart3_Base64 %>" alt="Chart 3" /><br />
                </div>
                <% foreach (var comp in Model.Components)
                   { %>
                       <div id="<%= Html.Encode(comp.ComponentID) %>">
                            <img src="data:image/png;base64,<%= Model.Chart4_Base64 %>" alt="Chart 4" /><br />
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
