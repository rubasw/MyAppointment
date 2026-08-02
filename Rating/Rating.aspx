<%@ Page Title="Rating" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Rating.aspx.cs" Inherits="ruba10828tl.Auth.Rating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Feedback</h2>

    <style>
        .rate-table { border-collapse: collapse; width: 100%; max-width: 720px; }
        .rate-table th, .rate-table td { border:1px solid #ddd; padding:10px; }
        .rate-table th { background:#f7f7f7; text-align:left; }
        .rbl-inline .aspNetDisabled { opacity:.6 }
        .rbl-inline input { margin-right:4px }
        .summary { background:#fafafa; border:1px solid #eee; padding:10px; max-width:720px; margin:12px 0 }
        .ok { color:green } .err{ color:#c00 }
    </style>

    <!-- نموذج التقييم -->
    <table class="rate-table">
        <tr>
            <th style="width:45%">Criterion</th>
            <th>Rating (1–5)</th>
        </tr>

        <tr>
            <td>Facilities</td>
            <td>
                <asp:RadioButtonList ID="rblFacilities" runat="server" CssClass="rbl-inline" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Text="1" /><asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="3" Text="3" /><asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="5" Text="5" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvF" runat="server" ControlToValidate="rblFacilities" ErrorMessage="Required" ForeColor="Red"/>
            </td>
        </tr>

        <tr>
            <td>Usability (ease of use)</td>
            <td>
                <asp:RadioButtonList ID="rblUsability" runat="server" CssClass="rbl-inline" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Text="1" /><asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="3" Text="3" /><asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="5" Text="5" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvU" runat="server" ControlToValidate="rblUsability" ErrorMessage="Required" ForeColor="Red"/>
            </td>
        </tr>

        <tr>
            <td>Staff</td>
            <td>
                <asp:RadioButtonList ID="rblStaff" runat="server" CssClass="rbl-inline" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Text="1" /><asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="3" Text="3" /><asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="5" Text="5" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvS" runat="server" ControlToValidate="rblStaff" ErrorMessage="Required" ForeColor="Red"/>
            </td>
        </tr>

        <tr>
            <td>Cleanliness</td>
            <td>
                <asp:RadioButtonList ID="rblCleanliness" runat="server" CssClass="rbl-inline" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Text="1" /><asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="3" Text="3" /><asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="5" Text="5" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvC" runat="server" ControlToValidate="rblCleanliness" ErrorMessage="Required" ForeColor="Red"/>
            </td>
        </tr>

        <tr>
            <td>Wait Time</td>
            <td>
                <asp:RadioButtonList ID="rblWait" runat="server" CssClass="rbl-inline" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" Text="1" /><asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="3" Text="3" /><asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="5" Text="5" />
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfvW" runat="server" ControlToValidate="rblWait" ErrorMessage="Required" ForeColor="Red"/>
            </td>
        </tr>

        <tr>
            <td>Comments (optional)</td>
            <td><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" Columns="50"/></td>
        </tr>
    </table>

    <br />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit Rating" OnClick="btnSubmit_Click" />
    <asp:Label ID="lblMsg" runat="server" />

    <!-- ملخص المتوسطات -->
    <div class="summary">
        <b>Average Ratings</b><br />
        Facilities: <asp:Label ID="avgF" runat="server" /> |
        Usability: <asp:Label ID="avgU" runat="server" /> |
        Staff: <asp:Label ID="avgS" runat="server" /> |
        Cleanliness: <asp:Label ID="avgC" runat="server" /> |
        Wait Time: <asp:Label ID="avgW" runat="server" />
    </div>

    <!-- عرض آخر التقييمات -->
    <asp:GridView ID="gvRatings" runat="server" AutoGenerateColumns="False" GridLines="Both">
        <Columns>
            <asp:BoundField DataField="CreatedAt" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
            <asp:BoundField DataField="Facilities" HeaderText="Facilities" />
            <asp:BoundField DataField="Usability" HeaderText="Usability" />
            <asp:BoundField DataField="Staff" HeaderText="Staff" />
            <asp:BoundField DataField="Cleanliness" HeaderText="Cleanliness" />
            <asp:BoundField DataField="WaitTime" HeaderText="Wait Time" />
            <asp:BoundField DataField="Comments" HeaderText="Comments" />
        </Columns>
    </asp:GridView>
</asp:Content>