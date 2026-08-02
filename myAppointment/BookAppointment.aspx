<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookAppointment.aspx.cs" Inherits="BookAppointment" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>حجز موعد</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>حجز موعد جديد</h2>

        <asp:Label Text="اختر الطبيب:" runat="server" AssociatedControlID="ddlDoctors" />
        <asp:DropDownList ID="ddlDoctors" runat="server"></asp:DropDownList><br /><br />

        <asp:Label Text="اختر المستشفى:" runat="server" AssociatedControlID="ddlHospitals" />
        <asp:DropDownList ID="ddlHospitals" runat="server"></asp:DropDownList><br /><br />

        <asp:Label Text="تاريخ ووقت الموعد:" runat="server" AssociatedControlID="txtAppointmentDate" />
        <asp:TextBox ID="txtAppointmentDate" runat="server" TextMode="DateTimeLocal" /><br /><br />

        <asp:CheckBox ID="chkNotifications" runat="server" Text="أرغب بتلقي الإشعارات / الإعلانات" /><br /><br />

        <asp:Button ID="btnBook" runat="server" Text="حجز" OnClick="btnBook_Click" /><br /><br />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>