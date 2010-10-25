<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<cpsc594_cdl.Models.IndexModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        var r_comp = new Array();
        var m_total = 10;
        var c_total = 10;
        function is_Enable(mid) {
            for (var id in r_comp) {
                if (mid == id)
                    return r_comp[id];
            }
            return false;
        }
        function c2_onclick(mid, type) {
            var is_all = (mid == "c_all" || mid == "m_all");
            var prefix = mid.substr(0, 2);
            var total = (prefix == "m_") ? m_total : c_total;
            var menu = document.getElementById(mid);
            if (is_Enable(mid)) {
                r_comp[mid] = false;
                menu.style.background = "#2a8ace";
                if (is_all) {
                    for (var id = 1; id <= total; id++) {
                        r_comp[prefix+id.toString()] = false;
                        menu = document.getElementById(prefix + id.toString());
                        menu.style.background = "#2a8ace";
                    }
                }
            } else {
                r_comp[mid] = true;
                menu.style.background = "#000000";
                if (is_all) {
                    for (var id = 1; id <= total; id++) {
                        r_comp[prefix + id.toString()] = true;
                        menu = document.getElementById(prefix + id.toString());
                        menu.style.background = "#000000";
                    }
                }
            }
            CM_Array.value = "";
            for (var id in r_comp) {
                if (r_comp[id])
                    CM_Array.value += id.toString() + ",";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm()) { %>
    <div id="frame">
        <div id="header">
            Header (<%= Html.ActionLink("Home", "Index")%>)
        </div>
        <div id="menu1">
            Select Project<br />
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
                <div id="menu2_info">
                    Select Components
                    <a class="a_m2" id="c_all" onclick="c2_onclick(this.id)">All</a>
                    <a class="a_m2" id="c_1" onclick="c2_onclick(this.id)">Component 1</a>
                    <a class="a_m2" id="c_2" onclick="c2_onclick(this.id)">Component 2</a>
                    <a class="a_m2" id="c_3" onclick="c2_onclick(this.id)">Component 3</a>
                    <a class="a_m2" id="c_4" onclick="c2_onclick(this.id)">Component 4</a>
                    <a class="a_m2" id="c_5" onclick="c2_onclick(this.id)">Component 5</a>
                    <a class="a_m2" id="c_6" onclick="c2_onclick(this.id)">Component 6</a>
                    <a class="a_m2" id="c_7" onclick="c2_onclick(this.id)">Component 7</a>
                    <a class="a_m2" id="c_8" onclick="c2_onclick(this.id)">Component 8</a>
                    <a class="a_m2" id="c_9" onclick="c2_onclick(this.id)">Component 9</a>
                    <a class="a_m2" id="c_10" onclick="c2_onclick(this.id)">Component 10</a>
                </div>
            </div>
            <div id="menu3">
                <div id="menu3_info">
                    Select Metrics
                    <!--<br /><%: ViewData["Message"] %>-->
                    <a class="a_m2" id="m_all" onclick="c2_onclick(this.id)">All</a>
                    <a class="a_m2" id="m_1" onclick="c2_onclick(this.id)">Metric 1</a>
                    <a class="a_m2" id="m_2" onclick="c2_onclick(this.id)">Metric 2</a>
                    <a class="a_m2" id="m_3" onclick="c2_onclick(this.id)">Metric 3</a>
                    <a class="a_m2" id="m_4" onclick="c2_onclick(this.id)">Metric 4</a>
                    <a class="a_m2" id="m_5" onclick="c2_onclick(this.id)">Metric 5</a>
                    <a class="a_m2" id="m_6" onclick="c2_onclick(this.id)">Metric 6</a>
                    <a class="a_m2" id="m_7" onclick="c2_onclick(this.id)">Metric 7</a>
                    <a class="a_m2" id="m_8" onclick="c2_onclick(this.id)">Metric 8</a>
                    <a class="a_m2" id="m_9" onclick="c2_onclick(this.id)">Metric 9</a>
                    <a class="a_m2" id="m_10" onclick="c2_onclick(this.id)">Metric 10</a>
                </div>
            </div>
        <div id="search">
            <%= Html.HiddenFor(m => m.CM_Array)%>
            <input type="submit" value="Search" onclick="IsSelectProject.value='FALSE'" />
        </div>
        <% } %>
        <div id="footer">
	        Footer
        </div>
    </div>
    <% } %>
</asp:Content>
