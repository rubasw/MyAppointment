<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ruba10828tl.Auth.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Login</h2>

    <!-- رقم الملف -->
    <asp:Label ID="lblFileNumber" runat="server" Text="File Number:" AssociatedControlID="txtFileNumber"></asp:Label><br />
    <asp:TextBox ID="txtFileNumber" runat="server"></asp:TextBox><br /><br />

    <!-- كلمة المرور -->
    <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword"></asp:Label><br />
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox><br /><br />

    <!-- زر تسجيل الدخول -->
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" /><br /><br />

    <!-- رابط للانتقال للتسجيل -->
    <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Auth/Register.aspx" 
                   ForeColor="Blue" Font-Underline="true">
        New user? Create an account
    </asp:HyperLink>
</asp:Content>