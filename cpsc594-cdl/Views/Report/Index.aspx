<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        Project: 
        <%= Model.ProjectID %>
        <br />
        Components:
        <%= String.Join(",", Model.ComponentIDs) %>
        <br />
        Metrics:
        <%= String.Join(",", Model.MetricIDs) %>
        <br />
        Start From:
        <br />
        <img src="<%= Url.Action("GetChart1", "Report", new { pid = Model.ProjectID, components = Model.ComponentIDs, metrics = Model.MetricIDs }) %>" border="0" /><br />
        <img src="<%= Url.Action("GetChart2", "Report", new { pid = Model.ProjectID, components = Model.ComponentIDs, metrics = Model.MetricIDs }) %>" border="0" /><br />
        <img src="<%= Url.Action("GetChart3", "Report", new { pid = Model.ProjectID, components = Model.ComponentIDs, metrics = Model.MetricIDs }) %>" border="0" /><br />
    </div>
</asp:Content>
