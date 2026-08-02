<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>تسجيل الدخول</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>تسجيل الدخول</h2>

        <asp:Label Text="البريد الإلكتروني:" runat="server" />
        <asp:TextBox ID="txtEmail" runat="server" /><br /><br />

        <asp:Label Text="كلمة المرور:" runat="server" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" /><br /><br />

        <asp:Button ID="btnLogin" runat="server" Text="دخول" OnClick="btnLogin_Click" /><br /><br />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br />

        <asp:HyperLink NavigateUrl="Register.aspx" runat="server">مريض جديد؟ سجل هنا</asp:HyperLink>
    </form>
</body>
</html>