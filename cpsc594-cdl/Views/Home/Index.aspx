<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="menu1">
        Project: 
        <%= Html.DropDownListFor(m => m.ProjectID, new SelectListItem[] {
            new SelectListItem { Text = "Project 1", Value = "0" },
            new SelectListItem { Text = "Project 2", Value = "1" },
            new SelectListItem { Text = "Project 3", Value = "2" }
        }) %>
        <%= Html.HiddenFor(m => m.IsSelectProject)%>
        <input type="submit" value="Select" onclick="IsSelectProject.value='TRUE'" />
    </div>
    <% if (ViewData["PID"]!=null) { %>
        <div id="menu2">
            Selected Project: <%=Model.ProjectID %>
            <br /><br />
            <div id="menu2_info">
                Components:<br />
                <%= Html.ListBoxFor(m => m.Components, new MultiSelectList(new[] {
                    new { Id = 1, Name = "Component 1" },
                    new { Id = 2, Name = "Component 2" },
                    new { Id = 3, Name = "Component 3" },
                }, "Id", "Name"), new { @size = "7" })%>
                <br /><br />
                <input type="submit" value="Search" onclick="IsSelectProject.value='FALSE'" />
            </div>
            <div id="menu2_info">
                Metrics:<br />
                <%= Html.ListBoxFor(m => m.Metrics, new MultiSelectList(new[] {
                    new { Id = 1, Name = "Metric 1" },
                    new { Id = 2, Name = "Metric 2" },
                    new { Id = 3, Name = "Metric 3" },
                }, "Id", "Name"), new { @size = "7" })%>
            </div>
        </div>
    <% } %>
</asp:Content>
