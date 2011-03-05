<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        function menu_toggle_off() {
            window.parent.document.all.frame.cols = '0,*';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%@ Import Namespace="cpsc594_cdl.Models" %>
<body>
    <div id="content">
    <div id="header">
        <a href="<%= Url.Action("Index")  %>"><img src="../../Content/logo.jpg" alt="Home" /></a>
    </div>
    <div id="info">
        <table>
        <% using (Html.BeginForm("Index", "Menu"))
           { %>
            <tr>
                <td id="menuItem">Project:</td>
                <td><%= Html.DropDownListFor(m => m.ProjectID, new MultiSelectList(Model.Projects, "ProjectID", "ProjectName"), new { @onchange = "submit();" })%></td>
            </tr>
        <% } %>
        <% if (Model.Components != null) { %>
            <% using (Html.BeginForm("Index", "Report", FormMethod.Post, new { @target = "report" }))
               { %>
                <%= Html.HiddenFor(m => m.ProjectID)%>
                <tr>
                    <td id="menuItem">Components:</td>
                    <td><%= Html.ListBoxFor(m => m.ComponentIDs, new MultiSelectList(Model.Components, "ComponentID", "ComponentName"), new { @size = "5", @onclick = "if (options[0].selected) {for(i=0; i<options.length; i++) options[i].selected = true; options[0].selected=false;}" })%></td>
                </tr>
                <tr>
                    <td id="menuItem">Metrics:</td>
                    <td><%= Html.ListBoxFor(m => m.MetricIDs, new MultiSelectList(new[] {
                        new { Id = (int)MetricType.Coverage , Name = "Code Coverage" },
                        new { Id = (int)MetricType.DefectInjectionRate , Name = "Defect Injection Rate" },
                        new { Id = (int)MetricType.DefectRepairRate , Name = "Defect Repair Rate" },
                        new { Id = (int)MetricType.TestEffectiveness , Name = "Test Effectiveness" },
                    }, "Id", "Name"), new { @size = "7" })%></td>
                </tr>
                <tr>
                    <td id="menuItem"></td>
                    <td><input type="submit" value="Search" onclick="menu_toggle_off();" /></td>
                </tr>
            <% } %>
        <% } %>
        </table>
    </div>
    </div>
</body>
</asp:Content>
