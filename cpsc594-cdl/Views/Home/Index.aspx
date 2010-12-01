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
                    <%= Html.DropDownListFor(m => m.StartYear, new SelectListItem[] {
                        new SelectListItem { Text = "2010", Value = "2010" }
                    })%>
                    <%= Html.DropDownListFor(m => m.StartMonth, new SelectListItem[] {
                        new SelectListItem { Text = "Jan", Value = "1" },
                        new SelectListItem { Text = "Feb", Value = "2" },
                        new SelectListItem { Text = "Mar", Value = "3" },
                        new SelectListItem { Text = "Apr", Value = "4" },
                        new SelectListItem { Text = "May", Value = "5" },
                        new SelectListItem { Text = "Jun", Value = "6" },
                        new SelectListItem { Text = "Jul", Value = "7" },
                        new SelectListItem { Text = "Aug", Value = "8" },
                        new SelectListItem { Text = "Sep", Value = "9" },
                        new SelectListItem { Text = "Oct", Value = "10" },
                        new SelectListItem { Text = "Nov", Value = "11" },
                        new SelectListItem { Text = "Dec", Value = "12" }
                    })%>
                    <%= Html.DropDownListFor(m => m.StartDay, new SelectListItem[] {
                        new SelectListItem { Text = "1", Value = "1" }, new SelectListItem { Text = "2", Value = "2" }, new SelectListItem { Text = "3", Value = "3" },
                        new SelectListItem { Text = "4", Value = "4" }, new SelectListItem { Text = "5", Value = "5" }, new SelectListItem { Text = "6", Value = "6" },
                        new SelectListItem { Text = "7", Value = "7" }, new SelectListItem { Text = "8", Value = "8" }, new SelectListItem { Text = "9", Value = "9" },
                        new SelectListItem { Text = "10", Value = "10" }, new SelectListItem { Text = "11", Value = "11" }, new SelectListItem { Text = "12", Value = "12" },
                        new SelectListItem { Text = "13", Value = "13" }, new SelectListItem { Text = "14", Value = "14" }, new SelectListItem { Text = "15", Value = "15" },
                        new SelectListItem { Text = "16", Value = "16" }, new SelectListItem { Text = "17", Value = "17" }, new SelectListItem { Text = "18", Value = "18" },
                        new SelectListItem { Text = "19", Value = "19" }, new SelectListItem { Text = "20", Value = "20" }, new SelectListItem { Text = "21", Value = "21" },
                        new SelectListItem { Text = "22", Value = "22" }, new SelectListItem { Text = "23", Value = "23" }, new SelectListItem { Text = "24", Value = "24" },
                        new SelectListItem { Text = "25", Value = "25" }, new SelectListItem { Text = "26", Value = "26" }, new SelectListItem { Text = "27", Value = "27" },
                        new SelectListItem { Text = "28", Value = "28" }, new SelectListItem { Text = "29", Value = "29" }, new SelectListItem { Text = "30", Value = "30" },
                        new SelectListItem { Text = "31", Value = "31" }
                    })%>
                </div>
                <br />
                <input type="submit" value="Search" />
            <% } %>
        <% } %>
    </div>
</asp:Content>
