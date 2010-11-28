<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="info">
        <div id="col">
            Project:<br />
            <%= Html.ListBoxFor(m => m.ProjectID, new MultiSelectList(Model.Projects, "ProjectID", "Name"), new { @size = "7", @onchange = "submit();" }) %>
        </div>
        <% if (Model.Components != null) { %>
            <%= Html.BeginForm("Index", "Report") %>
                <%= Html.HiddenFor(m => m.ProjectID) %>
                <div id="col">
                    Components:<br />
                    <%= Html.ListBoxFor(m => m.ComponentIDs, new MultiSelectList(Model.Components, "ComponentID", "Name"), new { @size = "7" }) %>
                </div>
                <div id="col">
                    Metrics:<br />
                    <%= Html.ListBoxFor(m => m.MetricIDs, new MultiSelectList(new[] {
                        new { Id = 1, Name = "Code Coverage" },
                    }, "Id", "Name"), new { @size = "7" })%>
                </div>
                <div id="col">
                    Start From:<br />
                    <%= Html.DropDownListFor(m => m.StartDate, new SelectListItem[] {
                        new SelectListItem { Text = "Aug 31, 2009", Value = "timestamp" },
                        new SelectListItem { Text = "Sep 31, 2009", Value = "timestamp" },
                        new SelectListItem { Text = "Jan 31, 2010", Value = "timestamp" }
                    }) %>
                </div>
                <br />
                <input type="submit" value="Search" />
            <% Html.EndForm(); %>
        <% } %>
    </div>
</asp:Content>
