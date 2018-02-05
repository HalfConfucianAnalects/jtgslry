<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomExtEdit.ascx.cs" Inherits="JtgTMS.SalaryControl.CustomExtEdit" %>

<asp:TextBox ID="txtFieldValue" CssClass="dfinput2" onkeypress="if (event.keyCode!=46 && event.keyCode!=45 && (event.keyCode<48 || event.keyCode>57)  || /\.\d{3}$/.Service(value)) event.returnValue=false" runat="server"></asp:TextBox>
<asp:Label ID="lblFieldValue" Visible="false" runat="server"></asp:Label>
<asp:Label ID="lblFieldName" Visible="false" runat="server"></asp:Label>
<asp:Label ID="lblFieldType" Visible="false" runat="server"></asp:Label>
<asp:Label ID="lblIsReadOnly" Visible="false" Text="0" runat="server"></asp:Label>