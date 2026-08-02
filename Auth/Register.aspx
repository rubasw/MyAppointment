<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="ruba10828tl.Auth.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register User</h2>

    <!-- رسالة الأخطاء / النجاح -->
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
    <br /><br />

    <!-- File Number -->
    <asp:Label runat="server" Text="File Number:" AssociatedControlID="txtFileNo"></asp:Label><br />
    <asp:TextBox ID="txtFileNo" runat="server"></asp:TextBox><br /><br />

    <!-- Full Name -->
    <asp:Label runat="server" Text="Full Name:" AssociatedControlID="txtFullName"></asp:Label><br />
    <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox><br /><br />

    <!-- Age -->
    <asp:Label runat="server" Text="Age:" AssociatedControlID="txtAge"></asp:Label><br />
    <asp:TextBox ID="txtAge" runat="server"></asp:TextBox><br /><br />

    <!-- Country -->
    <asp:Label runat="server" Text="Country:" AssociatedControlID="ddlCountry"></asp:Label><br />
    <asp:DropDownList ID="ddlCountry" runat="server">
        <asp:ListItem Text="--Select Country--" Value="" />
        <asp:ListItem>Saudi Arabia</asp:ListItem>
        <asp:ListItem>Kuwait</asp:ListItem>
        <asp:ListItem>Qatar</asp:ListItem>
        <asp:ListItem>United Arab Emirates</asp:ListItem>
        <asp:ListItem>Bahrain</asp:ListItem>
        <asp:ListItem>Oman</asp:ListItem>
    </asp:DropDownList><br /><br />

    <!-- Email -->
    <asp:Label runat="server" Text="Email:" AssociatedControlID="txtEmail"></asp:Label><br />
    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox><br /><br />

    <!-- Gender -->
    <asp:Label runat="server" Text="Gender:" AssociatedControlID="rblGender"></asp:Label><br />
    <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem>Male</asp:ListItem>
        <asp:ListItem>Female</asp:ListItem>
    </asp:RadioButtonList><br /><br />

    <!-- Password -->
    <asp:Label runat="server" Text="Password:" AssociatedControlID="txtPassword"></asp:Label><br />
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br /><br />

    <!-- Confirm Password -->
    <asp:Label runat="server" Text="Confirm Password:" AssociatedControlID="txtConfirmPassword"></asp:Label><br />
    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox><br /><br />

    <!-- أزرار العمليات -->
    <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
    <asp:Button ID="btnExport" runat="server" Text="Export to Excel" OnClick="btnExport_Click" />
    <br /><br />

    <!-- GridView لعرض البيانات -->
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" GridLines="Both"
                  OnSelectedIndexChanged="gvUsers_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="FileNumber" HeaderText="File Number" />
            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Age" HeaderText="Age" />
            <asp:BoundField DataField="Country" HeaderText="Country" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" />
            <asp:CommandField ShowSelectButton="True" SelectText="Select" />
        </Columns>
    </asp:GridView>

    <br /><br />

    <!-- Dropdown لاختيار مستخدم -->
    <asp:DropDownList ID="ddlUsers" runat="server"></asp:DropDownList>
    <asp:Button ID="btnProcess" runat="server" Text="Show Selected" OnClick="btnProcess_Click" />
    <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click" />
    <br /><br />

    <!-- Repeater لعرض المستخدمين كبطاقات -->
    <asp:Repeater ID="rpUsers" runat="server">
        <ItemTemplate>
            <div style="border:1px solid #ccc; padding:10px; margin:5px; width:300px;">
                <strong>File #:</strong> <%# Eval("FileNumber") %><br />
                <strong>Name:</strong> <%# Eval("FullName") %><br />
                <strong>Age:</strong> <%# Eval("Age") %><br />
                <strong>Country:</strong> <%# Eval("Country") %><br />
                <strong>Email:</strong> <%# Eval("Email") %><br />
                <strong>Gender:</strong> <%# Eval("Gender") %><br />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>