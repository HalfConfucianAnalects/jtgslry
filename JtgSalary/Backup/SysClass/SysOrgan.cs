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
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;

namespace JtgTMS.SysClass
{
    public class SysOrgan
    {
        public static int _TopOrganID = 11;
        public const int OrganType_Value = 0, WorkshopType_Value = 1, DeptType_Value = 2, TeamType_Value = 3, SupplierType_Value = 4;

        public static int GetWorkShopByOrganID(int OrganID)
        {
            int _OrganID = 0;
            SqlDataReader sdr = GetSingleOrganByReader(OrganID);
            if (sdr.Read())
            {
                //if (int.Parse(sdr["POrganID"].ToString()) == GetTopOrganID(0))
                if ((int.Parse(sdr["OrganType"].ToString()) == WorkshopType_Value) || (int.Parse(sdr["OrganType"].ToString()) == SupplierType_Value)
                    || (int.Parse(sdr["OrganType"].ToString()) == OrganType_Value))
                {
                    _OrganID = OrganID;
                }
                else
                {
                    _OrganID = GetWorkShopByOrganID(int.Parse(sdr["POrganID"].ToString()));
                }
            }
            else
            {
                _OrganID =  GetTopOrganID(0);
            }
            sdr.Close();
            return _OrganID;
        }

        //获取顶层ID
        public static int GetTopOrganID(int POrganID)
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and POrganID=" + POrganID.ToString() + " Order By SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }

        //获取地图列表首个ID
        public static int GetTopWorkShopID(int POrganID)
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysOrgan_Info Where Status=0 And IsNull(OrganType,0)=1"// And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and POrganID=" + POrganID.ToString() + " Order By SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }

        //获取地图列表首个ID
        public static int GetTopDeptID(int POrganID)
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysOrgan_Info Where Status=0 And IsNull(OrganType,0)=2 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and POrganID=" + POrganID.ToString() + " Order By SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }

        //判断机构编号是否重复
        public static Boolean CheckOrganNoExists(int OrganID, string OrganNo)
        {
            string sSqlText = "Select 1 From SysOrgan_Info Where OrganNo='" + OrganNo //+ "' And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + "' And ID<>" + OrganID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static string GetOrganNameByID(int OrganID)
        {
            string _OrganName = "";
            SqlDataReader sdr = GetSingleOrganByReader(OrganID);
            if (sdr.Read())
            {
                _OrganName = sdr["OrganName"].ToString();
            }
            sdr.Close();
            return _OrganName;
        }
        //获取专家所属车间列表
        public static string GetOrganNameByGuid(string _OrganGuid)
        {
            string _OrganName = "";
            SqlDataReader sdr = GetSingleOrganNameByReader(_OrganGuid);
            if (sdr.Read())
            {
                _OrganName = sdr["OrganName"].ToString();
            }
            sdr.Close();
            return _OrganName;
        }        
      
        //判断机构编号已被使用
        public static bool CheckOrganIsUse(int OrganID)
        {
            string sSqlText = "Select top 1 1 From User_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And OrganID=" + OrganID.ToString()
                + " union all "
                + " Select top 1 1 From SysOrgan_Info Where Status=0"// And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And POrganID=" + OrganID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //public static SqlDataReader GetOrganByTreelist()
        //{
        //    string sSQL = "DECLARE @t_Level TABLE(ID char(3),Level int,Sort varchar(8000))  "
        //        + " DECLARE @Level int  "
        //        + " SET @Level=0  "
        //        + " INSERT @t_Level SELECT ID,@Level,ID  "
        //        + " FROM SysOrgan_Info  "
        //        + " WHERE IsNull(POrganID,0) =0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
        //        + " order by sortID "
        //        + " WHILE @@ROWCOUNT>0  "
        //        + " BEGIN  "
        //            + " SET @Level=@Level+1  "
        //        + "     INSERT @t_Level SELECT a.ID,@Level,b.Sort+a.ID  "
        //        + "     FROM SysOrgan_Info a,@t_Level b  "
        //        + "     WHERE a.POrganID=b.ID"// And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
        //        + "         AND b.Level=@Level-1"
        //        + "     order by a.sortID"
        //        + " END  "

        //        + " SELECT a.ID, REPLICATE('&nbsp;',b.Level*4)+''+a.OrganName as OrganName  "
        //        + " FROM SysOrgan_Info a,@t_Level b  "
        //        + " WHERE a.ID=b.ID And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
        //        + " ORDER BY b.Sort  ";

        //    return DataCommon.GetDataByReader(sSQL);
        //}

        public static SqlDataReader GetOrganByTreelist()
        {
            string sSQL = "DECLARE @t_Level TABLE(ID char(3),Level int,Sort varchar(8000))  "
                + " DECLARE @Level int  "
                + " SET @Level=0  "
                + " INSERT @t_Level SELECT ID,@Level,ID  "
                + " FROM SysOrgan_Info  "
                + " WHERE IsNull(POrganID,0) =0 And OrganType in (0,1, 2)"//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " order by sortID "
                + " WHILE @@ROWCOUNT>0  "
                + " BEGIN  "
                    + " SET @Level=@Level+1  "
                + "     INSERT @t_Level SELECT a.ID,@Level,b.Sort+a.ID  "
                + "     FROM SysOrgan_Info a,@t_Level b  "
                + "     WHERE a.POrganID=b.ID And OrganType in (0,1, 2)"// And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + "         AND b.Level=@Level-1"
                + "     order by a.sortID"
                + " END  "

                + " SELECT a.ID, REPLICATE('&nbsp;',b.Level*4)+''+a.OrganName as OrganName  "
                + " FROM SysOrgan_Info a,@t_Level b  "
                + " WHERE a.ID=b.ID "//And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " ORDER BY b.Sort  ";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取车站列表至控件
        public static void FullToOrganLstByTreeList(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "请选择车间";
                ddlList.Items.Add(liItem);
            }

            SqlDataReader sdr = GetOrganByTreelist();
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = HttpUtility.HtmlDecode(sdr["OrganName"].ToString());
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取组织机构列表至控件
        public static void FullOrganToRadioButtonList(RadioButtonList rblList, int OrganType, bool HasAll)
        {           
            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                rblList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysOrgan_Info Where Status=0 And OrganType=" + OrganType.ToString() 
                //+ "  And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["OrganName"].ToString();
                rblList.Items.Add(liItem);
            }
            sdr.Close();

            if (rblList.Items.Count > 0)
            {
                rblList.SelectedIndex = 0;
            }
        }

        //获取组织机构列表至控件
        public static void FullOrganToDropDownList(DropDownList ddlList, int OrganType, bool HasAll)
        {
            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysOrgan_Info Where Status=0 And OrganType=" + OrganType.ToString()
                //+ "  And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["OrganName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取组织机构列表至控件
        public static void FullToOrganLst(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["OrganName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取车站列表至控件
        public static void FullToSupplierLst(RadioButtonList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "请选择车间";
                ddlList.Items.Add(liItem);
            }

            SqlDataReader sdr = GetSupplierLstByReader("");
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = HttpUtility.HtmlDecode(sdr["OrganName"].ToString());
                ddlList.Items.Add(liItem);
            }
            sdr.Close();

            if (ddlList.Items.Count > 0)
            {
                ddlList.SelectedIndex = 0;
            }
        }

        //获取车间列表
        public static DataSet GetOrganLstByDataSet(int POrganID, int OrganType,  string SearchText)
        {
            string sSQL = "Select *  from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And POrganID=" + POrganID.ToString() + " And OrganType=" + OrganType.ToString();
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (OrganNo Like '%" + SearchText + "%' Or OrganName Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取供应商列表
        public static SqlDataReader GetSupplierLstByReader(string SearchText)
        {
            string sSQL = "Select *  from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And OrganType=" + SupplierType_Value.ToString();
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (OrganNo Like '%" + SearchText + "%' Or OrganName Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetOrganLstByReader()
        {
            string sSQL = "Select *  from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And POrganID=0 Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetOrganLstByReader(int POrganID)
        {
            string sSQL = "Select *  from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And POrganID=" + POrganID.ToString() + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetOrganLstByReader(int POrganID, int OrganType)
        {
            string sSQL = "Select *  from SysOrgan_Info Where Status=0 "
                //+ " And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And POrganID=" + POrganID.ToString();
            if (OrganType >= 0)
            {
                sSQL += " And OrganType=" + OrganType.ToString();
            }
            sSQL +=" Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取组织机构列表
        public static SqlDataReader GetOrganListByReader(int OrganType)
        {
            string sSQL = "Select *  from SysOrgan_Info Where Status=0 And OrganType=" + OrganType.ToString()
                //+ " And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取单个车间信息
        public static SqlDataReader GetSingleOrganByReader(int _ID)
        {
            string sSQL = "Select * from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleOrganNameByReader(string _OrganGuid)
        {
            string sSQL = "Select * from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And Guid='" + _OrganGuid + "'";
            return DataCommon.GetDataByReader(sSQL);
        }

        //删除部门
        public static int DeleteSingleOrgan(string _IDs)
        {
            string sSQL = "begin Delete from SysOrgan_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And ID in (" + _IDs.ToString() + "); ";

            //Del by lk 20151214 start
            //sSQL += " Delete From ToolStock_Info Where OrganID in (" + _IDs.ToString() + ")";
            //Del by lk 20151214 end

            string sLogText = "删除 系统管理>人员管理 机构部门：ID:" + _IDs.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //更新车间信息
        public static int UpdateSingleOrgan(int _ID, int _OrganType, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE SysOrgan_Info SET OrganType=" + _OrganType.ToString() + ",POrganID=" + FieldValues.GetValue(0) + ", OrganNo='" + FieldValues.GetValue(1)
                     + "',OrganName='" + FieldValues.GetValue(2) + "',Description='" + FieldValues.GetValue(3) + "'";                
                sSqlText = sSqlText + ",SortID=" + FieldValues.GetValue(4) + " WHERE ID=" + _ID + "" + ";";
                string sLogText = "更新 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into SysOrgan_Info (SystemID, OrganType, POrganID, OrganNo, OrganName, Description, SortID) Values(" + SysParams.GetPurviewSystemID().ToString() + ","
                   + _OrganType.ToString()  + "," + FieldValues.GetValue(0) + ",'" + FieldValues.GetValue(1) + "','"
                   + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "'," + FieldValues.GetValue(4)  + ")" + ";";
                string sLogText = "新增 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }

            //Del by lk 20151214 start
            //sSqlText += "Insert Into ToolStock_Info (OrganID, ToolID)"
            //    + " select "
            //    + " a.ID As OrganID, b.ID As ToolID from SysOrgan_Info a, Tool_Info b "
            //    + " Where a.OrganType in (0, 1) and a.OrganNo='" + FieldValues.GetValue(1) + "' and a.OrganType in (" + OrganType_Value.ToString() + "," + WorkshopType_Value.ToString() + ")"
            //    + " And Convert(varchar(10),a.ID) + '-' + Convert(varchar(10),b.ID) not in ("
            //    + " Select Convert(varchar(10),a.OrganID) + '-' + Convert(varchar(10),a.ToolID) From ToolStock_Info a, SysOrgan_Info b "
            //    + " where a.OrganID=b.ID and b.OrganNo='" + FieldValues.GetValue(1) + "')";
            //Del by lk 20151214 end

            return DataCommon.QueryData(sSqlText);
        }

        public static string GetImageSrcByOrganNo(string OrganNo)
        {
            string sImgSrc = "";
            string sSQL = "Select ImageSrc from SysOrgan_Info where Status=0"// And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and OrganNo='" + OrganNo + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                sImgSrc = "../Upload/"+ sdr["ImageSrc"].ToString();
            }
            sdr.Close();
            return sImgSrc;
        }                

        public static string GetOrganPathNameByID(int OrganID, string _OrganPath)
        {
            string _Title = _OrganPath;
            SqlDataReader sdr = SysOrgan.GetSingleOrganByReader(OrganID);
            if (sdr.Read())
            {
                if (_Title.Length > 0)
                {
                    _Title = sdr["OrganName"].ToString() + "->" + _Title;
                }
                else
                {
                    _Title = sdr["OrganName"].ToString();
                }
                if (int.Parse(sdr["POrganID"].ToString()) > 0)
                {
                    _Title = GetOrganPathNameByID(int.Parse(sdr["POrganID"].ToString()), _Title);
                }
            }
            sdr.Close();
            return _Title;
        }

        public static int GetOrganIDByOrganName(string _OrganName)
        {
            int _UserID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysOrgan_Info Where OrganName='" + _OrganName + "'");
            if (sdr.Read())
            {
                _UserID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _UserID;
        }
    }
}
