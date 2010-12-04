<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
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
            <img src="data:image/png;base64,<%= Model.Chart1_Base64 %>" border="0" /><br />
            <img src="data:image/png;base64,<%= Model.Chart2_Base64 %>" border="0" /><br />
            <img src="data:image/png;base64,<%= Model.Chart3_Base64 %>" border="0" /><br />
            <img src="data:image/png;base64,<%= Model.Chart4_Base64 %>" border="0" /><br />
        <% } %>
    </div>
</asp:Content>
