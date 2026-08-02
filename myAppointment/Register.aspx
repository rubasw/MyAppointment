<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>تسجيل مريض جديد</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>تسجيل مريض جديد</h2>

        <asp:Label Text="الاسم الكامل:" runat="server" />
        <asp:TextBox ID="txtFullName" runat="server" /><br /><br />

        <asp:Label Text="الجنس:" runat="server" />
        <asp:RadioButtonList ID="rblGender" runat="server">
            <asp:ListItem Text="ذكر" Value="Male" />
            <asp:ListItem Text="أنثى" Value="Female" />
        </asp:RadioButtonList><br /><br />

        <asp:Label Text="تاريخ الميلاد:" runat="server" />
        <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" /><br /><br />

        <asp:Label Text="رقم الجوال:" runat="server" />
        <asp:TextBox ID="txtPhone" runat="server" /><br /><br />

        <asp:Label Text="البريد الإلكتروني:" runat="server" />
        <asp:TextBox ID="txtEmail" runat="server" /><br /><br />

        <asp:Label Text="كلمة المرور:" runat="server" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" /><br /><br />

        <asp:CheckBox ID="chkNotifications" runat="server" Text="أرغب بتلقي الإشعارات والإعلانات" /><br /><br />

        <asp:CheckBox ID="chkTerms" runat="server" Text="أوافق على شروط الاستخدام" /><br /><br />

        <asp:Button ID="btnRegister" runat="server" Text="تسجيل" OnClick="btnRegister_Click" /><br /><br />

        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
    </form>
</body>
</html>