<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurviewLst.ascx.cs" Inherits="JtgTMS.AdminControl.PurviewLst" %>

 <script language="javascript" type="text/javascript">

        function postBackByObject() {

            var o = window.event.srcElement;

            if (o.tagName == "INPUT" && o.type == "checkbox") {


                __doPostBack("<%= tvCategory.ClientID%>", "");

            }

        }
        </script>
        
        <div class="po1pover popo1er-static">
                  <h3 class="popover-title">操作权限</h3>
                  <div class="popover-content" >
                         <asp:TreeView ID="tvCategory" runat="server" ShowCheckBoxes="All" Width="100%" Height="100%"
                            CssClass="treelist-condensed table-hover" onclick="javascript:postBackByObject()" 
                            ontreenodecheckchanged="tvCategory_TreeNodeCheckChanged">
                            <Nodes>
                            </Nodes>
                        </asp:TreeView>        
                    </div>
               </div>
