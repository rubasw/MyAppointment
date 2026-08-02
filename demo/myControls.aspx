<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="myControls.aspx.cs" Inherits="ruba10828tl.demo.myControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    select your gender : <br />
    <asp:RadioButtonList ID="RblPatients" runat="server" DataTextField="gender" DataValueField="gender"></asp:RadioButtonList>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ruba10828tlConStr %>" SelectCommand="SELECT * FROM [patients]"></asp:SqlDataSource>
</asp:Content>
