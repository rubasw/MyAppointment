<%@ Page Title="Contact Us" Language="C#"
    MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Contact.aspx.cs"
    Inherits="ruba10828tl.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .contact-wrap {max-width:720px; background:#fff; padding:24px; border:1px solid #eee; border-radius:10px}
        .row {display:flex; gap:12px; margin-bottom:14px}
        .row label {width:160px; font-weight:600; padding-top:7px}
        .row .fld {flex:1}
        .row .fld input[type=text],
        .row .fld input[type=email],
        .row .fld textarea,
        .row .fld input[type=file] {width:100%}
        .btns {margin-top:8px}
        .btn {background:#198754; color:#fff; border:none; padding:8px 16px; border-radius:6px; cursor:pointer}
        .btn:hover {opacity:.9}
        .msg-ok {color:#198754; font-weight:600}
        .msg-err {color:#dc3545; font-weight:600}
        .hint {color:#666; font-size:.9em}
    </style>

    <h2>Contact Us</h2>

    <div class="contact-wrap">
        <asp:ValidationSummary ID="vs" runat="server" CssClass="msg-err" />

        <div class="row">
            <label>From - Email</label>
            <div class="fld">
                <asp:TextBox ID="txtFrom" runat="server" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvFrom" runat="server"
                    ControlToValidate="txtFrom" ErrorMessage="Email is required"
                    Display="Dynamic" CssClass="msg-err" />
                <asp:RegularExpressionValidator ID="revFrom" runat="server"
                    ControlToValidate="txtFrom" Display="Dynamic" CssClass="msg-err"
                    ErrorMessage="Invalid email format"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
            </div>
        </div>

        <div class="row">
            <label>Subject</label>
            <div class="fld">
                <asp:TextBox ID="txtSubject" runat="server" />
                <asp:RequiredFieldValidator ID="rfvSub" runat="server"
                    ControlToValidate="txtSubject" ErrorMessage="Subject is required"
                    Display="Dynamic" CssClass="msg-err" />
            </div>
        </div>

        <div class="row">
            <label>File Attachments</label>
            <div class="fld">
                <asp:FileUpload ID="fuFiles" runat="server" AllowMultiple="true" />
                <div class="hint">You can attach multiple files (size limit depends on server settings).</div>
            </div>
        </div>

        <div class="row">
            <label>Message</label>
            <div class="fld">
                <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Rows="8" />
                <asp:RequiredFieldValidator ID="rfvBody" runat="server"
                    ControlToValidate="txtBody" ErrorMessage="Message is required"
                    Display="Dynamic" CssClass="msg-err" />
            </div>
        </div>

        <div class="btns">
            <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="btn" OnClick="btnSend_OnClick" />
        </div>

        <div style="margin-top:10px">
            <asp:Label ID="lblStatus" runat="server" />
        </div>
    </div>
</asp:Content>