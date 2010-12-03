<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        <div id="col">
            <% using (Html.BeginForm("Index", "Home")) { %>
                Project:<br />
                <%= Html.ListBoxFor(m => m.ProjectID, new MultiSelectList(Model.Projects, "ProjectID", "Name"), new { @size = "7", @onchange = "submit();" }) %>
            <% } %>
        </div>
        <% if (Model.Components != null) { %>
            <% using (Html.BeginForm("Index", "Report"))
               { %>
                <%= Html.HiddenFor(m => m.ProjectID)%>
                <div id="col">
                    Components:<br />
                    <%= Html.ListBoxFor(m => m.ComponentIDs, new MultiSelectList(Model.Components, "ComponentID", "Name"), new { @size = "7" })%>
                </div>
                <div id="col">
                    Metrics:<br />
                    <%= Html.ListBoxFor(m => m.MetricIDs, new MultiSelectList(new[] {
                        new { Id = 1, Name = "Code Coverage" },
                    }, "Id", "Name"), new { @size = "7" })%>
                </div>
                <div id="col">
                    Start From:<br />
                    <%= Html.DropDownListFor(m => m.StartDate, new MultiSelectList(Model.Iterations, "StartDate", "StartDate"))%>
                </div>
                <br />
                <input type="submit" value="Search" />
            <% } %>
        <% } %>
    </div>
</asp:Content>
