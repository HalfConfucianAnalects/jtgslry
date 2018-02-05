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
    public class SysToolCategory
    {
        //产品分类
        public static string ToolCategory_SearchText = "";
        public static bool ViewChildren = false;
        public static int ToolCategory_PageNo = 1;

        //获取地图列表首个ID
        public static int GetTopCategoryID(int PCategoryID)
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From ToolCategory_Info Where Status=0 and PCategoryID=" + PCategoryID.ToString() + " Order By CategoryNo, SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }

        //判断机构编号是否重复
        public static Boolean CheckCategoryNoExists(int CategoryID, string CategoryNo)
        {
            string sSqlText = "Select 1 From ToolCategory_Info Where CategoryNo='" + CategoryNo + "' And ID<>" + CategoryID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static string GetCategoryNameByID(int CategoryID)
        {
            string _CategoryName = "";
            SqlDataReader sdr = GetSingleToolsCategoryByReader(CategoryID);
            if (sdr.Read())
            {
                _CategoryName = sdr["CategoryName"].ToString();
            }
            sdr.Close();
            return _CategoryName;
        }
        //获取专家所属车间列表
        public static string GetCategoryNameByGuid(string _ToolsCategoryGuid)
        {
            string _CategoryName = "";
            SqlDataReader sdr = GetSingleCategoryNameByReader(_ToolsCategoryGuid);
            if (sdr.Read())
            {
                _CategoryName = sdr["CategoryName"].ToString();
            }
            sdr.Close();
            return _CategoryName;
        }        
      
        //判断机构编号已被使用
        public static bool CheckToolsCategoryIsUse(int CategoryID)
        {
            string sSqlText = "Select top 1 1 From User_Info Where Status=0 And CategoryID=" + CategoryID.ToString()
                + " union all "
                + " Select top 1 1 From ToolCategory_Info Where Status=0 And PCategoryID=" + CategoryID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static SqlDataReader GetToolsCategoryByTreelist()
        {
            string sSQL = "DECLARE @t_Level TABLE(ID char(3),Level int,Sort varchar(8000))  "
                + " DECLARE @Level int  "
                + " SET @Level=0  "
                + " INSERT @t_Level SELECT ID,@Level,ID  "
                + " FROM ToolCategory_Info  "
                + " WHERE IsNull(PCategoryID,0) =0"
                + " order by sortID "
                + " WHILE @@ROWCOUNT>0  "
                + " BEGIN  "
                    + " SET @Level=@Level+1  "
                + "     INSERT @t_Level SELECT a.ID,@Level,b.Sort+a.ID  "
                + "     FROM ToolCategory_Info a,@t_Level b  "
                + "     WHERE a.PCategoryID=b.ID"
                + "         AND b.Level=@Level-1"
                + "     order by a.sortID"
                + " END  "

                + " SELECT a.ID, REPLICATE('&nbsp;',b.Level*4)+''+a.CategoryName as CategoryName  "
                + " FROM ToolCategory_Info a,@t_Level b  "
                + " WHERE a.ID=b.ID "
                + " ORDER BY b.Sort  ";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static void LoadToolsCategoryByTreeList(DropDownList ddlList, int PCategoryID, int Level)
        {
            if (Level == 0)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "<请选择工具分类>";
                ddlList.Items.Add(liItem);
            }

            SqlDataReader sdr = SysClass.SysToolCategory.GetToolsCategoryLstByReader(PCategoryID);
            while (sdr.Read())
            {
                ListItem newItem = new ListItem();
                newItem.Value = sdr["ID"].ToString();
                newItem.Text = HttpUtility.HtmlDecode(SysClass.SysGlobal.RepeatString("　", Level) + "" + sdr["CategoryName"].ToString());
                ddlList.Items.Add(newItem);

                LoadToolsCategoryByTreeList(ddlList, int.Parse(sdr["ID"].ToString()), Level + 1);
            }
            sdr.Close();
        } 

        //获取车站列表至控件
        public static void FullToToolsCategoryLstByTreeList(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "<请选择车间>";
                ddlList.Items.Add(liItem);
            }

            SqlDataReader sdr = GetToolsCategoryByTreelist();
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = HttpUtility.HtmlDecode(sdr["CategoryName"].ToString());
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取车站列表至控件
        public static void FullToToolsCategoryLst(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from ToolCategory_Info Where Status=0 Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["CategoryName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取车间列表
        public static DataSet GetToolsCategoryLstByDataSet(int PCategoryID, string SearchText)
        {
            string sSQL = "Select *  from ToolCategory_Info Where Status=0"
                + " And PCategoryID=" + PCategoryID.ToString() ;
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (CategoryNo Like '%" + SearchText + "%' Or CategoryName Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取车间列表
        public static DataSet GetToolsCategoryLstByDataSet(string WhereSQL)
        {
            string sSQL = "Select *  from ToolCategory_Info Where Status=0" + WhereSQL;
            
            sSQL = sSQL + " Order By CategoryNo, SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetToolsCategoryLstByReader()
        {
            string sSQL = "Select *  from ToolCategory_Info Where Status=0 And PCategoryID=0 Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetToolsCategoryLstByReader(int PCategoryID)
        {
            string sSQL = "Select *  from ToolCategory_Info Where Status=0 And PCategoryID=" + PCategoryID.ToString() + " Order By CategoryNo, SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取单个车间信息
        public static SqlDataReader GetSingleToolsCategoryByReader(int _ID)
        {
            string sSQL = "Select * from ToolCategory_Info Where Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleCategoryNameByReader(string _ToolsCategoryGuid)
        {
            string sSQL = "Select * from ToolCategory_Info Where Status=0 And Guid='" + _ToolsCategoryGuid + "'";
            return DataCommon.GetDataByReader(sSQL);
        }

        //获取单个车间信息
        public static int DeleteSingleToolsCategory(int _ID)
        {
            string sSQL = "begin Delete from ToolCategory_Info Where Status=0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>人员管理 机构部门：ID:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //更新车间信息
        public static int UpdateSingleToolsCategory(int _ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE ToolCategory_Info SET PCategoryID=" + FieldValues.GetValue(0) + ", CategoryNo='" + FieldValues.GetValue(1)
                     + "',CategoryName='" + FieldValues.GetValue(2) + "',Description='" + FieldValues.GetValue(3) + "'";                
                sSqlText = sSqlText + ",SortID=" + FieldValues.GetValue(4) + " WHERE ID=" + _ID + "" + ";";
                string sLogText = "更新 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into ToolCategory_Info ( PCategoryID, CategoryNo, CategoryName, Description, SortID) Values("
                   + FieldValues.GetValue(0) + ",'" + FieldValues.GetValue(1) + "','"
                   + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "'," + FieldValues.GetValue(4)  + ")" + ";";
                string sLogText = "新增 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }

        public static string GetImageSrcByCategoryNo(string CategoryNo)
        {
            string sImgSrc = "";
            string sSQL = "Select ImageSrc from ToolCategory_Info where Status=0 And CategoryNo='" + CategoryNo + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                sImgSrc = "../Upload/"+ sdr["ImageSrc"].ToString();
            }
            sdr.Close();
            return sImgSrc;
        }                

        public static string GetToolsCategoryPathNameByID(int CategoryID, string _ToolsCategoryPath)
        {
            string _Title = _ToolsCategoryPath;
            SqlDataReader sdr = SysToolCategory.GetSingleToolsCategoryByReader(CategoryID);
            if (sdr.Read())
            {
                if (_Title.Length > 0)
                {
                    _Title = sdr["CategoryName"].ToString() + "->" + _Title;
                }
                else
                {
                    _Title = sdr["CategoryName"].ToString();
                }
                if (int.Parse(sdr["PCategoryID"].ToString()) > 0)
                {
                    _Title = GetToolsCategoryPathNameByID(int.Parse(sdr["PCategoryID"].ToString()), _Title);
                }
            }
            sdr.Close();
            return _Title;
        }
    }
}
