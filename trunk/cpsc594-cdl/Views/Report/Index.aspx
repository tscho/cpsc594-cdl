<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <link rel="Stylesheet" href="/content/jquery-ui-1.8.7.custom.css" />
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript" language="javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.7.custom.min.js" type="text/javascript" language="javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        <% if (Html.ValidationSummary()!=null) { %>
            <%: Html.ValidationSummary(true, "Chart creation was unsuccessful. Please go back and try again.") %>
            <input type="submit" value="Back" onclick="history.go(-1)" /><br />
        <% } else { %>
            Project: 
            <%= Model.ProjectID%>
            <br />
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
                    <img src="data:image/png;base64,<%= Model.Chart1_Base64 %>" border="0" /><img src="data:image/png;base64,<%= Model.Chart2_Base64 %>" border="0" /><br />
                    <img src="data:image/png;base64,<%= Model.Chart3_Base64 %>" border="0" /><br />
                </div>
                <% foreach (var comp in Model.Components)
                   { %>
                       <div id="<%= Html.Encode(comp.ComponentID) %>">
                            <img src="data:image/png;base64,<%= Model.Chart4_Base64 %>" border="0" /><br />
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
</asp:Content>
