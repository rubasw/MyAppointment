<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppointmentList.aspx.cs" Inherits="AppointmentList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>قائمة المواعيد</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:GridView ID="gvAppointments" runat="server" AutoGenerateColumns="False"
            DataKeyNames="AppointmentID"
            OnRowEditing="gvAppointments_RowEditing"
            OnRowUpdating="gvAppointments_RowUpdating"
            OnRowDeleting="gvAppointments_RowDeleting"
            OnRowCancelingEdit="gvAppointments_RowCancelingEdit">
            
            <Columns>
                <asp:BoundField DataField="AppointmentID" HeaderText="رقم الموعد" ReadOnly="True" />
                <asp:BoundField DataField="PatientName" HeaderText="اسم المريض" ReadOnly="True" />
                <asp:BoundField DataField="DoctorName" HeaderText="اسم الطبيب" ReadOnly="True" />
                <asp:BoundField DataField="HospitalName" HeaderText="المستشفى" ReadOnly="True" />
                <asp:BoundField DataField="AppointmentDate" HeaderText="تاريخ الموعد" />
                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" 
                    EditText="تعديل" UpdateText="تحديث" CancelText="الغاء" DeleteText="حذف" ButtonType="Button"/>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>