<%@ Page Title="Book & View Appointment" Language="C#"
    MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BookAndview.aspx.cs"
    Inherits="ruba10828tl.Appointment.BookAndview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .form { width:720px; max-width:100%; }
        .form label { display:inline-block; width:140px; margin:6px 0; vertical-align:top; }
        .form .fld { display:inline-block; width:320px; }
        .btns { margin-top:10px }
        h2 { margin:10px 0 16px }
    </style>

    <h2>Book an Appointment</h2>

    <div class="form">
        <asp:HiddenField ID="hfAppointmentID" runat="server" />

        <div>
            <label>File Number:</label>
            <div class="fld"><asp:TextBox ID="txtFileNumber" runat="server" /></div>
        </div>

        <div>
            <label>Department:</label>
            <div class="fld">
                <asp:DropDownList ID="ddlDepartment" runat="server">
                    <asp:ListItem Text="--Select Department--" Value=""></asp:ListItem>
                    <asp:ListItem Text="Cardiology"  Value="Cardiology"></asp:ListItem>
                    <asp:ListItem Text="Orthopedics" Value="Orthopedics"></asp:ListItem>
                    <asp:ListItem Text="Dermatology" Value="Dermatology"></asp:ListItem>
                    <asp:ListItem Text="Neurology"   Value="Neurology"></asp:ListItem>
                    <asp:ListItem Text="Pediatrics"  Value="Pediatrics"></asp:ListItem>
                    <asp:ListItem Text="ENT"         Value="ENT"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div>
            <label>Doctor:</label>
            <div class="fld">
                <asp:DropDownList ID="ddlDoctor" runat="server">
                    <asp:ListItem Text="--Select Doctor--" Value=""></asp:ListItem>
                    <asp:ListItem Text="Dr. Smith"   Value="Dr. Smith"></asp:ListItem>
                    <asp:ListItem Text="Dr. Johnson" Value="Dr. Johnson"></asp:ListItem>
                    <asp:ListItem Text="Dr. Brown"   Value="Dr. Brown"></asp:ListItem>
                    <asp:ListItem Text="Dr. Taylor"  Value="Dr. Taylor"></asp:ListItem>
                    <asp:ListItem Text="Dr. Wilson"  Value="Dr. Wilson"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div>
            <label>Date:</label>
            <div class="fld"><asp:TextBox ID="txtDate" runat="server" TextMode="Date" /></div>
        </div>

        <div>
            <label>Time:</label>
            <div class="fld"><asp:TextBox ID="txtTime" runat="server" TextMode="Time" /></div>
        </div>

        <div style="margin:12px 0 6px"><b>History</b></div>
        <asp:CheckBoxList ID="chkHistory" runat="server" RepeatDirection="Vertical">
            <asp:ListItem>Diabetes</asp:ListItem>
            <asp:ListItem>Hypertension</asp:ListItem>
            <asp:ListItem>Heart Disease</asp:ListItem>
            <asp:ListItem>Asthma</asp:ListItem>
            <asp:ListItem>Previous Surgery</asp:ListItem>
            <asp:ListItem>Allergy</asp:ListItem>
        </asp:CheckBoxList>

        <div class="btns">
            <asp:Button ID="btnInsert"    runat="server" Text="Insert"         OnClick="btnInsert_Click" />
            <asp:Button ID="btnUpdate"    runat="server" Text="Update"         OnClick="btnUpdate_Click" />
            <asp:Button ID="btnDelete"    runat="server" Text="Delete"         OnClick="btnDelete_Click" />
            <asp:Button ID="btnExportPdf" runat="server" Text="Export to PDF" OnClick="btnExportPdf_Click" />
        </div>

        <div style="margin-top:10px">
            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" />
        </div>
    </div>

    <hr />
    <h3>Appointments</h3>

    <asp:GridView ID="gvAppointments" runat="server" AutoGenerateColumns="False"
                  DataKeyNames="AppointmentID"
                  OnSelectedIndexChanged="gvAppointments_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="True" SelectText="Select" />
            <asp:BoundField DataField="AppointmentID" HeaderText="ID" />
            <asp:BoundField DataField="FileNumber"   HeaderText="File Number" />
            <asp:BoundField DataField="Department"   HeaderText="Department" />
            <asp:BoundField DataField="Doctor"       HeaderText="Doctor" />
            <asp:BoundField DataField="Date"         HeaderText="Date" />
            <asp:BoundField DataField="Time"         HeaderText="Time" />
            <asp:BoundField DataField="History"      HeaderText="History" />
        </Columns>
    </asp:GridView>

</asp:Content>