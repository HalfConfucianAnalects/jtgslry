<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestHandler.aspx.cs" Inherits="JtgSalary.MobilePlatform.TestHandler" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn_GetName" runat="server" Text="GetName" OnClick="Button1_Click" Width="180px"/>
            <br />
            <asp:Button ID="btn_UpdateRegistrationID" runat="server" Text="UpdateRegistrationID" OnClick="Button2_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetPhoneCode" runat="server" Text="GetPhoneCode" OnClick="Button3_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_Login" runat="server" Text="Login" OnClick="Button4_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetHeadPortrait" runat="server" Text="GetHeadPortrait" OnClick="Button5_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_UpdateHeadPortrait" runat="server" Text="UpdateHeadPortrait" OnClick="Button6_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetSalary" runat="server" Text="GetSalary" OnClick="Button7_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetSalaryDetail" runat="server" Text="GetSalaryDetail" OnClick="Button8_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_UpdateSalary" runat="server" Text="UpdateSalary" OnClick="Button9_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetUserNotice" runat="server" Text="GetUserNotice" OnClick="Button10_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetUserNoticeDetail" runat="server" Text="GetUserNoticeDetail" OnClick="Button11_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_GetUserProfile" runat="server" Text="GetUserProfile" OnClick="Button12_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_UpdatePhoneNum" runat="server" Text="UpdatePhoneNum" OnClick="Button13_Click" Width="180px" />
            <br />
            <asp:Button ID="btn_DownLoadApp" runat="server" Text="DownLoadApp" OnClick="Button14_Click" Width="180px" />
    
        </div>
    </form>
</body>
</html>
