using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;
using CyxPack.UserCommonOperation;
using System.Net;

namespace JtgTMS.SysClass
{
    public class SysUser
    {
        public const string User_SearchText = "User_SearchText";
        public const string User_IsCanLogin = "User_IsCanLogin";
        public const string User_HasChildren = "User_HasChildren";

        public const string UserLst_PageNo = "UserLst_PageNo";

        public static SqlDataReader QueryOpNameLst(string SearchKey, int count)
        {
            int iRecCount = 20;
            if (count > 0)
            {
                iRecCount = count;
            }
            string sSQL = "Select distinct Top " + iRecCount + " OpCode, OpName from SysUser_Info"
                + " Where (OpCode Like '%" + SearchKey + "%' OR OpCode Like '%" + SearchKey + "%')"
                + " and Status=0";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader QuerySelfOpNameLst(string SearchKey, int count)
        {
            int iRecCount = 20;
            if (count > 0)
            {
                iRecCount = count; 
            }
            string sSQL = "Select distinct Top " + iRecCount + " OpCode, OpName from SysUser_Info"
                + " Where (OpCode Like '%" + SearchKey + "%' OR OpCode Like '%" + SearchKey + "%')"
                + " and Status=0 And OrganID in (select ID From dbo.GetOrganChildren(" + SysClass.SysGlobal.CurrentUserOrganID.ToString() + "))";
            return DataCommon.GetDataByReader(sSQL);
        }


        //获取用户角色到User_Edit.DropDownList控件
        public static void GetUserRole(DropDownList ddLoginRole, bool HasAll)
        {
            ddLoginRole.Items.Clear();
            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "";
                ddLoginRole.Items.Add(liItem);
            }

            string sSQL = "select * from SysRole_Info where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " order by SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["RoleName"].ToString();
                ddLoginRole.Items.Add(liItem);
            }
        }

        //获取用户信息
        public static SqlDataReader GetUserInfoByReader(int UserID)
        {
            string sSQL = "Select a.* FROM V_SysUser_Info a "
                + " Where a.Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And a.ID=" + UserID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户信息
        public static SqlDataReader GetUserInfoByReader(string OpCode)
        {
            string sSQL = "Select a.*, b.OrganName FROM SysUser_Info a "
                + " Left Join SysOrgan_Info b on b.Status=0 And a.OrganID = b.ID"
                + " Where a.Status=0 "
                //+ " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And a.OpCode='" + OpCode + "'";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户列表
        public static DataSet GetUserLstByWhere(int OrganID, string sWhereSQL)
        {
            string sSQL = "select a.*,b.RoleName,b.Purview,(Case IsNull(IsCanLogin,0) when 0 then '锁定' else '正常' end) as StatusName FROM SysUser_Info a"
                + " left join SysRole_Info b on a.RoleID=b.ID and b.status=0 "//And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " where a.Status=0 "
                //+ " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and a.OrganID=" + OrganID.ToString() + "" + sWhereSQL;

            

            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取用户列表
        public static DataSet GetUserLstByDataSet(int OrganID, string WhereSQL)
        {
            string sSQL = "select a.*,b.RoleName,b.Purview,(Case IsNull(IsCanLogin,0) when 0 then '锁定' else '正常' end) as StatusName FROM SysUser_Info a"
                + " left join SysRole_Info b on a.RoleID=b.ID and b.status=0 "//And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " where a.Status=0 "
                //+ " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and a.OrganID=" + OrganID.ToString() + "" + WhereSQL;

            //if (SearchText.Length > 0)
            //{
            //    sSQL = sSQL + " And (a.OpName Like '%" + SearchText + "%' OR a.OpCode Like '%" + SearchText + "%')";
            //}

            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取用户列表
        public static DataSet GetCanLoginUserLstByDataSet(int OrganID, string SearchText)
        {
            string sSQL = "select a.*,b.RoleName,b.Purview,(Case IsNull(IsCanLogin,0) when 0 then '锁定' else '正常' end) as StatusName FROM SysUser_Info a"
                + " left join SysRole_Info b on a.RoleID=b.ID and b.status=0 "//And IsNull(b.SystemId,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " where a.Status=0"// And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and a.OrganID=" + OrganID.ToString() + "";

            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpName Like '%" + SearchText + "%' OR a.OpCode Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " And IsNull(IsCanLogin,0)=1 Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //删除用户信息
        public static int DeleteUser(int UserID)
        {
            string sSqlText = "Delete From SysUser_Info Where "//IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " And 
                + " ID=" + UserID.ToString();

            return DataCommon.QueryData(sSqlText);
        }

        //获取组织机构列表至控件
        public static void FullUserToRadioButtonList(RadioButtonList rblList, int OrganID, bool HasAll)
        {
            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                rblList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysOrgan_Info Where Status=0 And POrganID=" + OrganID.ToString() 
                //+ "  And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                sSQL = "Select * from SysUser_Info Where OrganID=" + sdr["ID"].ToString() + " Order By SortID";
                SqlDataReader sdrUser = DataCommon.GetDataByReader(sSQL);
                while (sdrUser.Read())
                {
                    ListItem liItem = new ListItem();
                    liItem.Value = sdrUser["ID"].ToString();
                    liItem.Text = sdrUser["OpCode"].ToString() + "|" + sdrUser["OpName"].ToString();
                    rblList.Items.Add(liItem);
                }
                sdrUser.Close();

                FullUserToRadioButtonList(rblList, int.Parse(sdr["ID"].ToString()), false);
            }
            sdr.Close();
        }

        ////更新用户信息
        //public static int UpdateSingleUser(int UserID, string[] FieldValues)
        //{
        //    string sSqlText = "";
        //    if (UserID > 0)
        //    {
        //        sSqlText = "UPDATE SysUser_Info SET OpName='"
        //            + FieldValues.GetValue(0) + "',OpCode='"                     
        //            + FieldValues.GetValue(1) + "',RoleID="
        //            + FieldValues.GetValue(2) + ",Comment='"
        //            + FieldValues[3] + "' WHERE ID=" + UserID;
        //    }
        //    else
        //    {
        //        sSqlText = "Insert Into SysUser_Info (OpName,OpCode,RoleID,Comment,Status, Password) Values('"
        //        + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','"
        //        + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "',"
        //        + "0,'111111' )";

        //    }
        //    return DataCommon.QueryData(sSqlText);
        //}

       

        //更新用户信息
        public static int UpdateSingleUserPurview(int UserID, string UserPurview)
        {
            string sSqlText = "";       
     
                sSqlText = "begin ";

                sSqlText = sSqlText + " if not Exists(Select 1 From User_Purview Where Status=0 And UserID=" + UserID.ToString() + ")";
                sSqlText = sSqlText + " Insert into User_Purview (SystemID, UserID, Purview, SortID, Status) Values(" + SysParams.GetPurviewSystemID().ToString() + "," + UserID.ToString() + ",'" + UserPurview + "',0,0" + ");";
                sSqlText = sSqlText + " else ";
                sSqlText = sSqlText + " Update User_Purview  Set Purview='" + UserPurview + "' Where  Status=0 and UserID=" + UserID.ToString() + "; ";

                string sLogText = " 更新 系统管理>权限管理： " + UserID.ToString() + " 记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";           
            return DataCommon.QueryData(sSqlText);
        }

        //更新用户信息
        public static int UpdateCurrentUser(int UserID, string[] FieldValues)
        {
            string sSqlText = "";
            if (UserID > 0)
            {
                sSqlText = "UPDATE SysUser_Info SET OpName='"                    
                    + FieldValues.GetValue(0) + "',Comment='"
                    + FieldValues[1] + "' WHERE ID=" + UserID;
            }
            else
            {
                sSqlText = "Insert Into SysUser_Info (SystemID, OpName,Comment,Status, Password) Values(" + SysParams.GetPurviewSystemID().ToString() + ",'"
                + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "',"
                + "0,'111111' )";

            }
            return DataCommon.QueryData(sSqlText);
        }

        //更新用户密码
        public static Boolean CheckUserPassword(int UserID, string Password)
        {
            string sSqlText = "Select 1 From SysUser_Info Where "//IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " Password='" + Password + "' And ID=" + UserID;

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新用户密码
        public static Boolean CheckOpCodeExists(int UserID, string OpCode)
        {
            string sSqlText = "Select 1 From SysUser_Info Where "//IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " OpCode='" + OpCode + "' And ID<>" + UserID;

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新用户密码
        public static int UpdateUserPassword(int UserID, string Password)
        {
            string sSqlText = "begin UPDATE SysUser_Info SET Password='" + Password + "' WHERE ID=" + UserID + "; ";
            string sLogText = "修改登录密码。";
            sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
           
            return DataCommon.QueryData(sSqlText);
        }

        //开通用户登录
        public static int PassUserByUser(int UserID)
        {
            string sSqlText = "begin UPDATE SysUser_Info SET IsCanLogin=1 WHERE ID=" + UserID + "; ";
            string sLogText = "系统管理>权限管理> 开通用户: " + UserID.ToString() + " 登录。";
            sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSqlText);
        }

        //锁定用户登录
        public static int StopUserByUser(int UserID)
        {
            string sSqlText = "begin UPDATE SysUser_Info SET IsCanLogin=0 WHERE ID=" + UserID + "; ";
            string sLogText = "系统管理>权限管理> 锁定用户: " + UserID.ToString() + " 登录。";
            sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSqlText);
        }

        public static string GetCreateGUID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static SqlDataReader GetUserLogin(string sOpCode, string sPassword)
        {
            ///执行SQL
            string SqlText = "SELECT a.*,b.RoleName FROM SysUser_Info a "
                + " left join SysRole_Info b on a.RoleID=b.ID "//and IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.OpCode='" + sOpCode + "' AND a.Password='" + sPassword
                + "' AND a.Status=0 And IsNull(a.IsCanLogin,1)=1";
                //+ " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();
            return DataCommon.GetDataByReader(SqlText);
        }

        public static string GetPurviewByUserID(int UserID)
        {
            string sPurviews = "";

            for (int i = 0; i < 5000; i++)
            {
                sPurviews = sPurviews + "0";
            }

            SqlDataReader sdr = SysClass.SysRole.GetUserRolePurviewByReader(UserID);
            while (sdr.Read())
            {
                string stPurviews = sdr["Purview"].ToString();
                string tPurviews = "";
                for (int i = 0; i < 5000; i++)
                {
                    if ((sPurviews[i] == char.Parse("1")) | (stPurviews[i] == char.Parse("1")))
                    {
                        tPurviews = tPurviews + "1";
                    }
                    else
                    {
                        tPurviews = tPurviews + "0";
                    }
                }
                sPurviews = tPurviews;
            }
            sdr.Close();
                
            return sPurviews;
        }

        public static string GetPurviewByRoles(string Roles)
        {
            string sPurviews = "";

            for (int i = 0; i < 500; i++)
            {
                sPurviews = sPurviews + "0";
            }
            string sUserRoles = Roles + ",";
            char[] delimit = new char[] { ',' };
            foreach (string substr in sUserRoles.Split(delimit))
            {
                if (substr.Length > 0)
                {
                    SqlDataReader sdr = SysClass.SysRole.GetSingleRoleByReader(int.Parse(substr));
                    if (sdr.Read())
                    {
                        string stPurviews = sdr["Purview"].ToString();
                        string tPurviews = "";
                        for (int i = 0; i < 500; i++)
                        {
                            if ((sPurviews[i] == char.Parse("1")) | (stPurviews[i] == char.Parse("1")))
                            {
                                tPurviews = tPurviews + "1";
                            }
                            else
                            {
                                tPurviews = tPurviews + "0";
                            }
                        }
                        sPurviews = tPurviews;
                    }
                    sdr.Close();                    
                }
            }
            return sPurviews;
        }
       
        //系统登录
        public static bool UserLogin(string sOpCode, string sPassword, System.Web.SessionState.HttpSessionState hsSession)
        {
            bool bSuccess = false;
            SqlDataReader sda = GetUserLogin(sOpCode, sPassword);
            if (sda.Read())
            {
                UserInfo info = new UserInfo();
                info.UserID = int.Parse(sda["ID"].ToString());
                info.OpGuid = sda["Guid"].ToString();
                info.OpCode = sda["OpCode"].ToString();
                info.OpName = sda["OpName"].ToString();
                //权限移到选择系统界面
                //info.UserRolePurview = GetPurviewByUserID(info.UserID);
                info.Purview = info.UserRolePurview;
                info.UserRoles = sda["UserRoles"].ToString();
                info.IsAdmin = sda["IsAdmin"].ToString() == "1";
                if (sda["LastDate"].ToString().Length > 0)
                {
                    info.LastDate = DateTime.Parse(sda["LastDate"].ToString()).ToString("yyyy年MM月dd日 HH:mm:ss");
                }
                //info.PurviewSystemID = 0;
                info.LastIp = sda["LastIP"].ToString();
                info.LastComputerName = sda["LastComputerName"].ToString();
                if (sda["OrganID"].ToString().Length > 0)
                {
                    info.OrganID = SysClass.SysOrgan.GetWorkShopByOrganID(int.Parse(sda["OrganID"].ToString()));
                }
                else
                {
                    info.OrganID = SysClass.SysOrgan.GetTopOrganID(0);
                }

                CyxPack.UserCommonOperation.UserCommonOperation.StoreUserInfo(info);
               // string IP = HttpContext.Current.Request.UserHostAddress;//获取客户端电脑IP
                string IP = "";
                System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
                IP = addressList[0].ToString();
                string ComputerName;
                ComputerName = System.Net.Dns.GetHostName();  //获取本地计算机的主机名

                SysLogs.CreateUserLogin(info.UserID, info.OpCode, info.OpName, "用户登录系统。", IP, ComputerName);

                SysClass.SysGlobal.CurrentUserOrganID = SysGlobal.GetCurrentUserOrganID();
               
                bSuccess = true; 
            }
            sda.Close();
            return bSuccess;
        }

        //系统登录
        public static bool CheckUserLogin(string sOpCode, string sPassword, System.Web.SessionState.HttpSessionState hsSession)
        {
            bool bSuccess = false;
            SqlDataReader sda = GetUserLogin(sOpCode, sPassword);
            if (sda.Read())
            {
                UserInfo info = new UserInfo();
                info.UserID = int.Parse(sda["ID"].ToString());
                info.OpCode = sda["OpCode"].ToString();
                info.OpName = sda["OpName"].ToString();
                info.Purview = sda["Purview"].ToString();
                CyxPack.UserCommonOperation.UserCommonOperation.StoreUserInfo(info);
                bSuccess = true;
            }
            sda.Close();
            return bSuccess;
        }

        public static int DeleteUserByIsCanlogin(int _UserID)
        {
            string sSQL = "Update SysUser_Info Set IsCanLogin=0 where Status =0 and ID=" + _UserID.ToString() + ";";
            sSQL = sSQL + "";
            return DataCommon.QueryData(sSQL);
        }

        public static DataSet GetSysOpCodeLstByNoLogin(int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysUser_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            if (OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID = " + OrganID.ToString();
            }
            sSQL = sSQL + " And IsNull(a.IsCanLogin,0)=0 And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSysOpCodeLstByNoRole(int RoleID, int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysUser_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            if (OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID = " + OrganID.ToString();
            }
            sSQL = sSQL + " And charindex('," + RoleID.ToString() + ",',','+IsNull(UserRoles,'')+',')=0 And IsNull(a.SystemID,0)=" 
                + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSysOpCodeLstByRole(int RoleID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysUser_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And charindex('," + RoleID.ToString() + ",',','+IsNull(UserRoles,'')+',')>0 And IsNull(a.SystemID,0)="
                + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSysUserByDataSet(int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName, dbo.RoleNameLstByUserID(" + SysParams.GetPurviewSystemID().ToString() + ", a.ID) As RoleName FROM SysUser_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";           
            if (OrganID > 0)
            {

                sSQL = sSQL + " And IsNull(a.OrganID,0)=" + OrganID.ToString();
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetChildrenUserByDataSet(string CategoryNo, int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysUser_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (CategoryNo.Length > 0)
            {
                sSQL = sSQL + " And a.CategoryNo='" + CategoryNo.ToString() + "'";
            }

            if (OrganID > 0)
            {

                sSQL = sSQL + " And (IsNull(a.OrganID,0) in (Select ID From dbo.GetOrganChildren(" + OrganID.ToString() + ", 1)))";
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }     

        public static SqlDataReader GetSysUserByReader(int OrganID)
        {
            string sSQL = "SELECT a.*,b.OrganName"
                + ",(Case IsNull(a.WorkStatus,0) When 0 then '在岗' when 1 then '出差' when 2 then '请假' when 3 then '临时外出' end) as WorkStatusName"
                + " FROM SysUser_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";           

            sSQL = sSQL + " And a.OrganID=" + OrganID.ToString();

            sSQL = sSQL + //" And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + 
                " Order By a.SortID";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSysUserByReader(string WhereSQL)
        {
            string sSQL = "SELECT a.*"
                + " FROM V_SysUser_Info a "
                + " WHERE a.Status=0" + WhereSQL
                + " Order By a.OpCode";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetSysUserByDataSet(string WhereSQL)
        {
            string sSQL = "SELECT a.*"
                + " FROM V_SysUser_Info a "
                + " WHERE a.Status=0" + WhereSQL
                + " Order By a.OpCode";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetChildrenUserByReader(int OrganID, string WhereSQL)
        {
            string sSQL = "SELECT a.*,b.OrganName,(Case IsNull(a.WorkStatus,0) When 0 then '在岗' when 1 then '出差' when 2 then '请假' when 3 then '临时外出' end) as WorkStatusName"
                 + ",b.OrganName,(Case IsNull(a.WorkStatus,0) When 0 then 'Normal' when 1 then 'Travel' when 2 then 'Leave' when 3 then 'TemporaryOutgoing' end) as WorkStatusClass"
                 + " FROM SysUser_Info a "
                 + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "//And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                 + " WHERE a.Status=0 " + WhereSQL;
            if (OrganID > 0)
            {
                sSQL = sSQL + " And IsNull(a.OrganID,0) in (Select ID From dbo.GetOrganChildren(" + OrganID.ToString() + ", 1))";
            }
            sSQL = sSQL //+ " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " Order By a.SortID";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleUserByReader(int ID)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM V_SysUser_Info a"
                + " left join SysOrgan_Info b on a.OrganID=b.ID "// And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status =0 "//And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And a.ID=" + ID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleUserByReader(string _Guid)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysUser_Info a"
                + " left join SysOrgan_Info b on a.OrganID=b.ID "//And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status =0 "//And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And a.Guid='" + _Guid.ToString() + "'";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static int DeleteSingleUser(int _ID)//===
        {
            string sSQL = "begin Delete from SysUser_Info Where Status =0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>人员管理:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //更新用户密码
        public static Boolean CheckUserNoExists(int _ID, string OpCode)
        {
            string sSqlText = "Select 1 From SysUser_Info Where OpCode='" + OpCode + "' "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And ID<>" + _ID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static int UpdateSingleUser(int ID, string[] FieldValues, string DetailSQL)
        {
            string sSqlText = "begin ";
            if (ID > 0)
            {
                sSqlText = sSqlText + "begin UPDATE SysUser_Info SET OpCode = '" + FieldValues.GetValue(0) 
                    + "',OpName='"   + FieldValues.GetValue(1) 
                    + "',OrganID='"  + FieldValues.GetValue(2) 
                    + "',Position='" + FieldValues.GetValue(3) 
                    + "',Place='"    + FieldValues.GetValue(4) 
                    + "',Sex='"      + FieldValues.GetValue(5) 
                    + "',Phone='"    + FieldValues.GetValue(6) 
                    + "',TelNo='"    + FieldValues.GetValue(7) 
                    + "',Address='"  + FieldValues.GetValue(8) 
                    + "',UserDesc='" + FieldValues.GetValue(9)
                    + "',IDNumber='" + FieldValues.GetValue(10)
                    + "',ZipCode='" + FieldValues.GetValue(12)
                    + "',IsCanLogin='" + FieldValues.GetValue(14) 
                    + "'";
                sSqlText = sSqlText + " WHERE ID =" + ID + ";";
                string sLogText = "更新 系统管理>人员管理 ：" + FieldValues.GetValue(0) + "|" + FieldValues.GetValue(1) + " 记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            else
            {
                sSqlText = sSqlText + "begin Insert Into SysUser_Info(SystemID, Guid, OpCode, OpName,"
                    + " OrganID,Position,Place,Sex,Phone,TelNo,Address,UserDesc,IDNumber,Password,ZipCode, IsCanLogin, Status) "
                    + " Values(" + SysParams.GetPurviewSystemID().ToString() + ",'" + FieldValues.GetValue(13) + "','"
                    + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','"
                    + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "','"
                    + FieldValues.GetValue(4) + "','" + FieldValues.GetValue(5) + "','"
                    + FieldValues.GetValue(6) + "','" + FieldValues.GetValue(7) + "','"
                    + FieldValues.GetValue(8) + "','" + FieldValues.GetValue(9) + "','"
                    + FieldValues.GetValue(10) + "','" + FieldValues.GetValue(11) + "','"
                    + FieldValues.GetValue(12) + "'," + FieldValues.GetValue(14) ;
                sSqlText = sSqlText +",0 ); ";

                string sLogText = "新增 系统管理>人员管理 ： " + FieldValues.GetValue(0) + "|" + FieldValues.GetValue(1) + " 记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            sSqlText = sSqlText + DetailSQL + " end";
            return DataCommon.QueryData(sSqlText);
        }

        public static int UpdateSingleUserInfoEx(int ID, string[] FieldValues, string DetailSQL)
        {
            string sSqlText = "";
            if (ID > 0)
            {
                sSqlText = sSqlText + "UPDATE SysUser_Info SET "
                    + "OpName='" + FieldValues.GetValue(0)                    
                    + "'";
                sSqlText = sSqlText + " WHERE ID =" + ID + ";";
                sSqlText += DetailSQL;
            }            
            return DataCommon.QueryData(sSqlText);
        }

        public static string GetUserPathNameByID(int UserID)
        {
            string _Title = "";
            SqlDataReader sdr = SysUser.GetSingleUserByReader(UserID);
            if (sdr.Read())
            {
                _Title = SysOrgan.GetOrganPathNameByID(int.Parse(sdr["OrganID"].ToString()),"") + "->" + sdr["OpName"].ToString();
            }
            sdr.Close();
            return _Title;
        }

        public static int GetUserIDByOpCode(string _OpCode)
        {
            int _UserID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysUser_Info Where OpCode='" + _OpCode + "'");
            if (sdr.Read())
            {
                _UserID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _UserID;
        }

        public static int GetSelfUserIDByOpCode(string _OpCode)
        {
            int _UserID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysUser_Info Where OpCode='" + _OpCode + "'"
                + " And OrganID in (select ID From dbo.GetOrganChildren(" + SysClass.SysGlobal.CurrentUserOrganID.ToString() + "))");
            if (sdr.Read())
            {
                _UserID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _UserID;
        }

        public static string GetUserNameByOpCode(string _OpCode)
        {
            string _UserName = "";
            SqlDataReader sdr = DataCommon.GetDataByReader("Select OpName From SysUser_Info Where OpCode='" + _OpCode + "'");
            if (sdr.Read())
            {
                _UserName = sdr["OpName"].ToString();
            }
            sdr.Close();
            return _UserName;
        }

        public static string GetSelfUserNameByOpCode(string _OpCode)
        {
            string _UserName = "";
            SqlDataReader sdr = DataCommon.GetDataByReader("Select OpName From SysUser_Info Where OpCode='" + _OpCode + "'"
                + " And OrganID in (select ID From dbo.GetOrganChildren(" + SysClass.SysGlobal.CurrentUserOrganID.ToString() + "))");
            if (sdr.Read())
            {
                _UserName = sdr["OpName"].ToString();
            }
            sdr.Close();
            return _UserName;
        }

        public static string GetUserNameByUserID(int _UserID)
        {
            string _UserName = "";
            SqlDataReader sdr = GetUserInfoByReader(_UserID);
            if (sdr.Read())
            {
                _UserName = sdr["OpName"].ToString();
            }
            sdr.Close();
            return _UserName;
        }

        public static string GetOpCodeByUserID(int _UserID)
        {
            string _UserName = "";
            SqlDataReader sdr = GetUserInfoByReader(_UserID);
            if (sdr.Read())
            {
                _UserName = sdr["OpCode"].ToString();
            }
            sdr.Close();
            return _UserName;
        }        

        public static string GetRecGuidByUserID(int _UserID)
        {
            string _RecGuid = "";
            SqlDataReader sdr = GetUserInfoByReader(_UserID);
            if (sdr.Read())
            {
                _RecGuid = sdr["Guid"].ToString();
            }
            sdr.Close();
            return _RecGuid;
        }

        public static int UpdateSingleUserWorkStatus(int ID, int WorkStatus, string WorkDesc)
        {
            string sSQL = "Update SysUser_Info Set WorkStatus=" + WorkStatus.ToString() + ", WorkDesc='" + WorkDesc + "' Where ID=" + ID.ToString();
            return DataCommon.QueryData(sSQL);
        }

        public static bool CheckCanDeleteUser(String _IDs)
        {
            string sSqlText = "select top 1 1 from SysUser_Info a, UserSalary_Info b where a.OpCode=b.OpCode And a.ID in (" + _IDs + ")";

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }
    }
}
