<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                <td><%= Html.DropDownListFor(m => m.ProjectID, new MultiSelectList(Model.Projects, "ProjectID", "Name"), new { @onchange = "submit();" })%></td>
            </tr>
        <% } %>
        <% if (Model.Components != null) { %>
            <% using (Html.BeginForm("Index", "Report", FormMethod.Post, new { @target = "report" }))
               { %>
                <%= Html.HiddenFor(m => m.ProjectID)%>
                <tr>
                    <td id="menuItem">Components:</td>
                    <td><%= Html.ListBoxFor(m => m.ComponentIDs, new MultiSelectList(Model.Components, "ComponentID", "Name"), new { @size = "5", @onclick = "if (options[0].selected) {for(i=0; i<options.length; i++) options[i].selected = true; options[0].selected=false;}" })%></td>
                </tr>
                <tr>
                    <td id="menuItem">Metrics:</td>
                    <td><%= Html.ListBoxFor(m => m.MetricIDs, new MultiSelectList(new[] {
                        new { Id = 1, Name = "Code Coverage" },
                    }, "Id", "Name"), new { @size = "7" })%></td>
                </tr>
                <tr>
                    <td id="menuItem"></td>
                    <td><input type="submit" value="Search" /></td>
                </tr>
            <% } %>
        <% } %>
        </table>
    </div>
    </div>
</body>
</asp:Content>
