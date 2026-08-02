<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Home" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>الصفحة الرئيسية</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>مرحبًا <asp:Label ID="lblWelcome" runat="server" /></h2>

        <asp:Button ID="btnBookAppointment" runat="server" Text="احجز موعد" OnClick="btnBookAppointment_Click" /><br /><br />
        <asp:Button ID="btnMyAppointments" runat="server" Text="مواعيدي القادمة" OnClick="btnMyAppointments_Click" /><br /><br />
        <asp:Button ID="btnLogout" runat="server" Text="تسجيل الخروج" OnClick="btnLogout_Click" />
    </form>
</body>
</html>