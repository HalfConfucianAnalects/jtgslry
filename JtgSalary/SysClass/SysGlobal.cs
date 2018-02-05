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
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using CyxPack.UserCommonOperation;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.SessionState;

namespace JtgTMS.SysClass
{
    public class SysGlobal
    {
        /// <summary>
        /// 创建新GUID
        /// </summary>
        /// <returns>GUID</returns>

        public static int CurrentUserOrganID = 0;

        public static string GetCreateGUID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string RepeatString(string str, int n)
        {

            char[] arr = str.ToCharArray();

            char[] arrDest = new char[arr.Length * n];

            for (int i = 0; i < n; i++)
            {

                Buffer.BlockCopy(arr, 0, arrDest, i * arr.Length * 2, arr.Length * 2);

            }

            return new string(arrDest);

        } 

        public static string GetComputerIP()
        {
            string IP = "";
            System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            if (addressList.Count() > 0)
            {
            IP = addressList[0].ToString();
            }
            return IP;
        }
        public static string GetComputerName()
        {
            string ComputerName;
            ComputerName = System.Net.Dns.GetHostName();
            return ComputerName;
        }
// ===↑
        public static bool GetExecSqlIsExist(string ExecSQL)
        {
            bool bFlag = false;
            SqlDataReader recc = DataCommon.GetDataByReader(ExecSQL);
            if (recc.Read())
            {
                bFlag = true;
            }
            recc.Close();
            return bFlag;
        }

        public static SqlDataReader GetMainLstByReader(string MainNo)
        {
            string sSQL = "";
            sSQL = "Select * from BaseMain_Info Where Status=0 And MainNo='" + MainNo + "'";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetDetailLstByReader(string MainNo)
        {
            string sSQL = "Select a.* from BaseDetail_Info a,BaseMain_Info b "
                + " Where a.Status=0 and b.Status=0 And a.MainID=b.ID And b.MainNo='" + MainNo.ToString() + "' Order By a.SortID";
            return DataCommon.GetDataByReader(sSQL);
        }   

        //获取基础数据，包括编号，名称 仅取名称
        public static void FullToDetailNameLst(DropDownList ddlList, string MainNo,  bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select a.* from BaseDetail_Info a,BaseMain_Info b "
                + " Where a.Status=0 and b.Status=0 And a.MainID=b.ID And b.MainNo='" + MainNo.ToString() + "' Order By a.SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Text = sdr["DetailName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取基础数据，包括编号，名称
        public static void FullToDetailDataLst(DropDownList ddlList, string MainNo, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select a.* from BaseDetail_Info a,BaseMain_Info b "
                + " Where a.Status=0 and b.Status=0 And a.MainID=b.ID And b.MainNo='" + MainNo.ToString() + "' Order By a.SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["DetailNo"].ToString();
                liItem.Text = sdr["DetailName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        public static bool GetSysIsLogined()
        {
            bool bLogined = false;
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                bLogined = true;
            }
            return bLogined;
        }

        public static void CheckSysIsLogined()
        {
            if (!GetSysIsLogined())
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Portal/Login.aspx");     
            }
        }

        public static string GetCurrentOpName()
        {
            string _OpName = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _OpName = info.OpName;
            }
            return _OpName;
        }

        public static string GetLastDate()
        {
            string _LastDate = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _LastDate = info.LastDate;
            }
            return _LastDate;
        }

        public static string GetLastIp()
        {
            string _LastIp = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _LastIp = info.LastIp;
            }
            return _LastIp;
        }

        public static string GetLastComputerName()
        {
            string _LastComputerName = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _LastComputerName = info.LastComputerName;
            }
            return _LastComputerName;
        }

        public static string GetCurrentUserRolePurview()
        {
            string _Purview = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _Purview = info.UserRolePurview;
            }
            return _Purview;
        }

        public static int GetCurrentUserOrganID()
        {
            int _OrganID = 0;

            UserInfo us = CyxPack.UserCommonOperation.UserCommonOperation.GetUserInfo();

            if (us != null)
            {
                _OrganID = us.OrganID;
            }

            return _OrganID;
        }

        public static string GetCurrentUserOrganName()
        {
            string _OrganName = "";

            UserInfo us = CyxPack.UserCommonOperation.UserCommonOperation.GetUserInfo();

            if (us != null)
            {
                _OrganName = SysOrgan.GetOrganNameByID(us.OrganID);
            }

            return _OrganName;
        }

        public static string GetCurrentOpCode()
        {
            string _OpName = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _OpName = info.OpCode;
            }
            return _OpName;
        }

        public static int GetCurrentUserID()
        {
            int _UserID = 0;
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            if (info != null)
            {
                _UserID = info.UserID;
            }
            return _UserID;
        }

        public static void UpdatePurviewSystemID(int _PurviewSystemID, string _SystemTitle)
        {
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            info.PurviewSystemID = _PurviewSystemID;
            info.PurviewSystemTitle = _SystemTitle;
            UserCommonOperation.StoreUserInfo(info);

            info.UserRolePurview = SysClass.SysUser.GetPurviewByUserID(info.UserID);
            info.Purview = info.UserRolePurview;
            UserCommonOperation.StoreUserInfo(info);
        }
        
        public static string GetHyperByUpFileName(string FullUploadFile)
        {
            string _Hyper = "";

            string[] sFileArray = Regex.Split(FullUploadFile, ",", RegexOptions.IgnoreCase);
            foreach (string tStr in sFileArray)
            {
                if (tStr.Length > 0)
                {
                    if (_Hyper.Length > 0)
                        _Hyper = _Hyper + "&nbsp;&nbsp;&nbsp;";
                    _Hyper = _Hyper + "<a href='../AdminControl/DownloadFile.aspx?FileName=../upload/"
                        + CyxPack.CommonOperation.DealwithString.GetStringPrefix(tStr)
                        + "&SaveFileName=" + CyxPack.CommonOperation.DealwithString.GetStringSuffix(tStr) + "'>"
                        + CyxPack.CommonOperation.DealwithString.GetStringSuffix(tStr) + "</a>";
                }
            }

            return _Hyper;
        }

        public static void ClearPersonRefreshTime(HttpSessionState session)
        {
            if (session != null)
            {
                UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
                //if (info != null)
                //{
                //    string SqlText = "Update TSys_Person Set FRefreshTime=null Where FGuid='" + info.CurrentUserGUID + "'";
                //    DataCommon.QueryData(SqlText);
                //}
            }
        }

        public static string GetTableOrderNo(string TableName)
        {
            string _OrderNo = "";

            string SqlText = " begin ";

            SqlText += "if not Exists (Select top 1 1 From SysPrefixCode_Info a, SysPrefixNo_Info b "
                + " Where a.TableName=b.TableName And a.TableName='" + TableName + "' and b.[Year]=year(getdate()) and b.[month]=month(getdate()))    ";
            SqlText += " begin";
            SqlText += " Insert into SysPrefixNo_Info (TableName, [Year], [Month]) Values('" + TableName + "', year(getdate()), month(getdate()))";
            SqlText += " end;";

            SqlText += " update SysPrefixNo_Info Set OrderID=IsNull(OrderID,0)+1 Where [TableName]='" + TableName + "' and [Year]=year(getdate()) and [month]=month(getdate()); ";

            SqlText += " end;";

            if (DataCommon.QueryData(SqlText) > 0)
            {
                SqlDataReader sdr = DataCommon.GetDataByReader("select IsNull(b.PrefixCode,'')+convert(varchar(4),year(getdate()))    "
                    + " + convert(varchar(2),right(replicate('0',2)+ltrim(month(getdate())),2)) + convert(varchar(5),right(replicate('0',5)+ltrim(a.OrderID),5)) as OrderNo "
                    + "  From SysPrefixNo_Info a "
                    + "  Left join SysPrefixCode_Info b on a.TableName=b.TableName "
                    + "  Where a.TableName='" + TableName + "' and a.[Year]=year(getdate()) and a.[month]=month(getdate()); ");
                if (sdr.Read())
                {
                    _OrderNo = sdr["OrderNo"].ToString();
                }
                sdr.Close();
            }

            return _OrderNo;
        }

        public static int DeleteUploadBillOpLst(int TableType, string TableRecGuid)
        {
            string SqlText = " Delete From BillOpLst_Info Where TableType = " + TableType.ToString() + " And TableRecGuid = '" + TableRecGuid + "' ";
            return DataCommon.QueryData(SqlText);
        }

        public static int AddUploadBillOpLst(int TableType, string TableRecGuid, int OrganID, int UserID)
        {
            string SqlText = "INSERT INTO BillOpLst_Info (TableType, TableRecGuid,  OrganID,  UserID) "
                 + "VALUES(" + TableType.ToString() + ",'" + TableRecGuid + "'," + OrganID.ToString()
                 + "," + UserID.ToString() + ")";
            return DataCommon.QueryData(SqlText);
        }

        public static SqlDataReader GetBillOpLstByReader(int TableType, string TableRecGuid)
        {
            string sSQL = "Select * from BillOpLst_Info Where TableType = " + TableType.ToString() + " And TableRecGuid = '" + TableRecGuid + "'";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static void RefreshRowChildrenControl(Control _PControl, System.Drawing.Color _Color)
        {
            if (_PControl.HasControls())
            {
                foreach (Control _Control in _PControl.Controls)
                {
                    if (_Control is HyperLink)
                    {
                        ((HyperLink)(_Control)).ForeColor = _Color;
                    }
                    else if (_Control is Label)
                    {
                        ((Label)(_Control)).ForeColor = _Color;
                    }
                    if (_Control.HasControls())
                    {
                        RefreshRowChildrenControl(_Control, _Color);
                    }
                }
            }
        }

        public static string ConvertToChineseMoney(double num)
        {
            string strChina = "零壹贰叁肆伍陆柒捌玖";    //0-9所对应的汉字
            string strUnit = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string strSingleNum = "";    //从原num值中取出的值
            string strNum = ""; //数字的字符串形式
            string strResult = "";    //人民币大写金额形式
            string chChina = "";    //数字的汉语读法
            string chUnit = "";    //数字位的汉语读法
            int i;    //循环变量
            int lenth;    //num的值乘以100的字符串长度
            int nZero = 0;    //用来计算连续的零值是几个
            int temp;    //从原num值中取出的值
            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            strNum = ((long)(num * 100)).ToString();    //将num乘100并转换成字符串形式
            lenth = strNum.Length;    //找出最高位
            if (lenth > 15)
            {
                return "位数过大，无法转换！";
            }
            //取出对应位数的strUnit的值。如：200.55,lenth为5所以strUnit=佰拾元角分
            strUnit = strUnit.Substring(15 - lenth);
            //循环取出每一位需要转换的值
            for (i = 0; i < lenth; i++)
            {
                strSingleNum = strNum.Substring(i, 1);    //取出需转换的某一位的值
                temp = Convert.ToInt32(strSingleNum);    //转换为数字
                if (i != (lenth - 3) && i != (lenth - 7) && i != (lenth - 11) && i != (lenth - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (strSingleNum == "0")
                    {
                        chChina = "";
                        chUnit = "";
                        nZero = nZero + 1;
                    }
                    else
                    {
                        if (strSingleNum != "0" && nZero != 0)
                        {
                            chChina = "零" + strChina.Substring(temp, 1);
                            chUnit = strUnit.Substring(i, 1);
                            nZero = 0;
                        }
                        else
                        {
                            chChina = strChina.Substring(temp, 1);
                            chUnit = strUnit.Substring(i, 1);
                            nZero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (strSingleNum != "0" && nZero != 0)
                    {
                        chChina = "零" + strChina.Substring(temp, 1);
                        chUnit = strUnit.Substring(i, 1);
                        nZero = 0;
                    }
                    else
                    {
                        if (strSingleNum != "0" && nZero == 0)
                        {
                            chChina = strChina.Substring(temp, 1);
                            chUnit = strUnit.Substring(i, 1);
                            nZero = 0;
                        }
                        else
                        {
                            if (strSingleNum == "0" && nZero >= 3)
                            {
                                chChina = "";
                                chUnit = "";
                                nZero = nZero + 1;
                            }
                            else
                            {
                                if (lenth >= 11)
                                {
                                    chChina = "";
                                    nZero = nZero + 1;
                                }
                                else
                                {
                                    chChina = "";
                                    chUnit = strUnit.Substring(i, 1);
                                    nZero = nZero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (lenth - 11) || i == (lenth - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    chUnit = strUnit.Substring(i, 1);
                }
                strResult = strResult + chChina + chUnit;
                if (i == lenth - 1 && strSingleNum == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    strResult = strResult + '整';
                }
            }
            if (num == 0)
            {
                strResult = "零元整";
            }
            return strResult;
        }
    }
}
