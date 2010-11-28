<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        Project: 
        <%=ViewData["PID"]%>
        <%=ViewData["ProjectName"] %>
        <br />
        Components:
        <%=ViewData["Components"]%>
        <br />
        Metrics:
        <%=ViewData["Metrics"]%>
        <br />
        Start From:
        <%=ViewData["StartDate"]%>
        <br />
        <img src="/Report/GetChart1?pid=<%=ViewData["PID"]%>&str_components=<%=ViewData["Components"]%>&str_metrics=<%=ViewData["Metrics"]%>" border="0" /><br />
        <img src="/Report/GetChart2?pid=<%=ViewData["PID"]%>&str_components=<%=ViewData["Components"]%>&str_metrics=<%=ViewData["Metrics"]%>" border="0" /><br />
        <img src="/Report/GetChart3?pid=<%=ViewData["PID"]%>&str_components=<%=ViewData["Components"]%>&str_metrics=<%=ViewData["Metrics"]%>" border="0" /><br />
    </div>
</asp:Content>
