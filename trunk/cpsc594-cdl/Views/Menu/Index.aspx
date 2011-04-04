<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        function menu_toggle_off() {
            parent.document.getElementById('frame').cols = '0,*';
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
                <td id="Td2">Metrics:</td>
                <td><%= Html.ListBoxFor(m => m.MetricIDs, new MultiSelectList(
                    Enum.GetNames(typeof(MetricType)).Join<String, int, int, object>(
                    (int[])Enum.GetValues(typeof(MetricType)), 
                    x => (int)Enum.Parse(typeof(MetricType), x),
                    y => y, (x, y) => new { Name = x, Id = y }),
                "Id", "Name"), new { @size = "8", @onclick = "submit();"})%></td>
            </tr>
            <tr><td></td><td><input type="submit" value="Next" /></td></tr>
        <% } %>
        <% if (Model.MetricIDs != null) { %>
            <% using (Html.BeginForm("Index", "Report", FormMethod.Post, new { @id = "reportForm", @target = "report" }))
               { %>
               <% for (int i = 0; i < Model.MetricIDs.Count(); i++)
                  { %>
                       <%= Html.HiddenFor(m => m.MetricIDs[i])%>
               <% } %>
               <% if (Model.MetricIDs.Intersect(PerComponentMetric.IDs).Count() > 0)
                  { %>
                    <tr>
                        <td class="menuItem">Product:</td>
                        <td><%= Html.DropDownListFor(m => m.ProductID, new SelectList(Model.Products, "ProductID", "ProductName"), new { @onchange = "document.getElementById('reportForm').action='/Menu'; document.getElementById('reportForm').target='menu'; submit();" })%></td>
                    </tr>
                    <tr>
                        <td class="menuItem">Components:</td>
                        <td><%= Html.ListBoxFor(m => m.ComponentIDs, new MultiSelectList(Model.Components, "ComponentID", "ComponentName"), new { @size = "5", @onclick = "if (options[0].selected) {for(i=0; i<options.length; i++) options[i].selected = true; options[0].selected=false;}" })%></td>
                    </tr>
                <% } %>
                <% if (Model.MetricIDs.Intersect(PerProductMetric.IDs).Count() > 0)
                  { %>
                    <tr>
                        <td class="menuItem">Products:</td>
                        <% Model.Products.First().ProductName = "Select All"; %>
                        <td><%= Html.ListBoxFor(m => m.ProductIDs, new MultiSelectList(Model.Products, "ProductID", "ProductName"), new { @size = "5", @onclick = "if (options[0].selected) {for(i=0; i<options.length; i++) options[i].selected = true; options[0].selected=false;}" }) %></td>
                    </tr>
                    <tr>
                        <td class="menuItem">Start Iteration:</td>
                        <td><%= Html.DropDownListFor(m => m.StartIteration, new SelectList(Model.Iterations, "IterationID", "IterationLabel")) %></td>
                    </tr>
                    <tr>
                        <td id = "Td1">End Iteration:</td>
                        <td><%= Html.DropDownListFor(m => m.EndIteration, new SelectList(Model.Iterations, "IterationID", "IterationLabel")) %></td>
                    </tr>
                <% } %>
                <tr>
                    <td class="menuItem"></td>
                    <td><input type="submit" value="Search" onclick="menu_toggle_off();" /></td>
                </tr>
            <% } %>
        <% } %>
        </table>
    </div>
    </div>
</body>
</asp:Content>
