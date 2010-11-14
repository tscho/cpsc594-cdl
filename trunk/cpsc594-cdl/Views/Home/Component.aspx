<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        Project: 
        <%=ViewData["PID"] %>
        <br />
        Components:
        <%
            IEnumerator list;
            list = ((IEnumerable<int>)ViewData["Components"]).GetEnumerator();
            while (list.MoveNext())
            {
                Response.Write(list.Current + " ");
            }
        %>
        <br />
        Metrics:
        <%
            list = ((IEnumerable<int>)ViewData["Metrics"]).GetEnumerator();
            while (list.MoveNext())
            {
                Response.Write(list.Current + " ");
            }
        %>
        <br />
        <img src="/Report/GetChart1" alt="Chart" /><br />
        <img src="/Report/GetChart2" alt="Chart" /><br />
        <img src="/Report/GetChart3" alt="Chart" /><br />
    </div>
</asp:Content>
