<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.Project>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        Project: 
        <%=Model.ProjectID %>
        <br />
        Components:
        Start From:
        <br />
        <img  src="/Report/GetChart1?pid=<%= Model.ProjectID %>&str_components=<%=String.Join(",", Model.ComponentIDs)%>&str_metrics=<%=String.Join(",", Model.MetricIDs)%>" border="0" /><br />
        <img  src="/Report/GetChart2?pid=<%= Model.ProjectID %>&str_components=<%=String.Join(",", Model.ComponentIDs)%>&str_metrics=<%=String.Join(",", Model.MetricIDs)%>" border="0" /><br />
        <img  src="/Report/GetChart3?pid=<%= Model.ProjectID %>&str_components=<%=String.Join(",", Model.ComponentIDs)%>&str_metrics=<%=String.Join(",", Model.MetricIDs)%>" border="0" /><br />
    </div>
</asp:Content>
