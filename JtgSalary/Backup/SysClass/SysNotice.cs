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
    public class SysNotice
    {
        //产品分类
        public static string Notice_SearchText = "";
        public static bool ViewChildren = false;
        public static int Notice_PageNo = 1;

        public const int cs_Notice_MasterType = 1;

        public static string GetRecGuidByNoticeID(int GardenerID)
        {
            string _RecGuid = "";
            SqlDataReader sdr = GetSingleToolsNoticeByReader(GardenerID);
            if (sdr.Read())
            {
                _RecGuid = sdr["RecGuid"].ToString();
            }
            sdr.Close();

            return _RecGuid;
        }

        //更新阅读数量
        public static int UpdateClickNum(int MasterID)
        {
            string sSQL = " Begin ";
            sSQL += " Update Notice_Info Set ClickNum=IsNull(ClickNum,0) + 1 Where ID=" + MasterID.ToString() + ";";

            sSQL += " if Exists(Select 1 From SysClick_Info Where MasterType=" + cs_Notice_MasterType.ToString()
                + " And MasterID=" + MasterID.ToString() + " And ClickUserID=" + SysClass.SysGlobal.GetCurrentUserID().ToString() + ")";

            sSQL += " begin";

            sSQL += " Update SysClick_Info Set ClickNum=IsNull(ClickNum,0) + 1 Where MasterType=" + cs_Notice_MasterType.ToString()
                + " And MasterID=" + MasterID.ToString()
                + " And ClickUserID=" + SysClass.SysGlobal.GetCurrentUserID().ToString() + "";
            sSQL += " end";
            sSQL += " else";
            sSQL += " begin";
            sSQL += " Insert Into SysClick_Info (MasterType, MasterID, ClickUserID) Values(" + cs_Notice_MasterType.ToString()
                + "," + MasterID.ToString()
                + "," + SysClass.SysGlobal.GetCurrentUserID().ToString() + ");";
            sSQL += " Update Notice_Info Set ClickUserNum=IsNull(ClickUserNum,0) + 1 Where ID=" + MasterID.ToString() + ";";
            sSQL += " end";

            sSQL = sSQL + " end";
            return DataCommon.QueryData(sSQL);
        }

        //获取地图列表首个ID
        public static int GetTopNoticeID(int PNoticeID)
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From Notice_Info Where Status=0 and PNoticeID=" + PNoticeID.ToString() + " Order By NoticeNo, SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }

        //判断机构编号是否重复
        public static Boolean CheckNoticeNoExists(int NoticeID, string NoticeNo)
        {
            string sSqlText = "Select 1 From Notice_Info Where NoticeNo='" + NoticeNo + "' And ID<>" + NoticeID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static string GetNoticeTitleByID(int NoticeID)
        {
            string _NoticeTitle = "";
            SqlDataReader sdr = GetSingleToolsNoticeByReader(NoticeID);
            if (sdr.Read())
            {
                _NoticeTitle = sdr["NoticeTitle"].ToString();
            }
            sdr.Close();
            return _NoticeTitle;
        }
        //获取专家所属车间列表
        public static string GetNoticeTitleByGuid(string _ToolsNoticeGuid)
        {
            string _NoticeTitle = "";
            SqlDataReader sdr = GetSingleNoticeTitleByReader(_ToolsNoticeGuid);
            if (sdr.Read())
            {
                _NoticeTitle = sdr["NoticeTitle"].ToString();
            }
            sdr.Close();
            return _NoticeTitle;
        }        
      
        //判断机构编号已被使用
        public static bool CheckToolsNoticeIsUse(int NoticeID)
        {
            string sSqlText = "Select top 1 1 From User_Info Where Status=0 And NoticeID=" + NoticeID.ToString()
                + " union all "
                + " Select top 1 1 From Notice_Info Where Status=0 And PNoticeID=" + NoticeID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static SqlDataReader GetToolsNoticeByTreelist()
        {
            string sSQL = "DECLARE @t_Level TABLE(ID char(3),Level int,Sort varchar(8000))  "
                + " DECLARE @Level int  "
                + " SET @Level=0  "
                + " INSERT @t_Level SELECT ID,@Level,ID  "
                + " FROM Notice_Info  "
                + " WHERE IsNull(PNoticeID,0) =0"
                + " order by sortID "
                + " WHILE @@ROWCOUNT>0  "
                + " BEGIN  "
                    + " SET @Level=@Level+1  "
                + "     INSERT @t_Level SELECT a.ID,@Level,b.Sort+a.ID  "
                + "     FROM Notice_Info a,@t_Level b  "
                + "     WHERE a.PNoticeID=b.ID"
                + "         AND b.Level=@Level-1"
                + "     order by a.sortID"
                + " END  "

                + " SELECT a.ID, REPLICATE('&nbsp;',b.Level*4)+''+a.NoticeTitle as NoticeTitle  "
                + " FROM Notice_Info a,@t_Level b  "
                + " WHERE a.ID=b.ID "
                + " ORDER BY b.Sort  ";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static void LoadToolsNoticeByTreeList(DropDownList ddlList, int PNoticeID, int Level)
        {
            if (Level == 0)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "<请选择通知>";
                ddlList.Items.Add(liItem);
            }

            SqlDataReader sdr = SysClass.SysNotice.GetToolsNoticeLstByReader(PNoticeID);
            while (sdr.Read())
            {
                ListItem newItem = new ListItem();
                newItem.Value = sdr["ID"].ToString();
                newItem.Text = HttpUtility.HtmlDecode(SysClass.SysGlobal.RepeatString("　", Level) + "" + sdr["NoticeTitle"].ToString());
                ddlList.Items.Add(newItem);

                LoadToolsNoticeByTreeList(ddlList, int.Parse(sdr["ID"].ToString()), Level + 1);
            }
            sdr.Close();
        } 

        //获取车站列表至控件
        public static void FullToToolsNoticeLstByTreeList(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "<请选择车间>";
                ddlList.Items.Add(liItem);
            }

            SqlDataReader sdr = GetToolsNoticeByTreelist();
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = HttpUtility.HtmlDecode(sdr["NoticeTitle"].ToString());
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取车站列表至控件
        public static void FullToToolsNoticeLst(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from Notice_Info Where Status=0 Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["NoticeTitle"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取车间列表
        public static DataSet GetToolsNoticeLstByDataSet(int PNoticeID, string SearchText)
        {
            string sSQL = "Select *  from Notice_Info Where Status=0"
                + " And PNoticeID=" + PNoticeID.ToString() ;
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (NoticeNo Like '%" + SearchText + "%' Or NoticeTitle Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取车间列表
        public static DataSet GetToolsNoticeLstByDataSet(string WhereSQL)
        {
            string sSQL = "Select a.*,IsNull(b.SelfClickNum, 0) As SelfClickNum,c.OpName  from Notice_Info a "
                + " left join (Select MasterID, Sum(ClickNum) As SelfClickNum From SysClick_Info Where ClickUserID=" + SysClass.SysGlobal.GetCurrentUserID().ToString()
                + " And MasterType=" + cs_Notice_MasterType + " Group by MasterID) b on b.MasterID=a.Id"
                + " left join Sysuser_info c on a.CreateUserID=c.ID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By a.Createdtime desc,a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取车间列表
        public static DataSet GetTopToolsNoticeLstByDataSet(int TopNum,  string WhereSQL)
        {

            string sSQL = "Select top "+ TopNum.ToString() + " a.*,IsNull(b.SelfClickNum, 0) As SelfClickNum, c.OpName  from Notice_Info a "
                + " left join (Select MasterID, Sum(ClickNum) As SelfClickNum From SysClick_Info Where ClickUserID=" + SysClass.SysGlobal.GetCurrentUserID().ToString()
                + " And MasterType=" + cs_Notice_MasterType + " Group by MasterID) b on b.MasterID=a.Id"
                + " left join Sysuser_info c on a.CreateUserID=c.ID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By a.Createdtime desc, a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetToolsNoticeLstByReader()
        {
            string sSQL = "Select *  from Notice_Info Where Status=0 And PNoticeID=0 Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取车间列表
        public static SqlDataReader GetToolsNoticeLstByReader(int PNoticeID)
        {
            string sSQL = "Select *  from Notice_Info Where Status=0 And PNoticeID=" + PNoticeID.ToString() + " Order By NoticeNo, SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取单个车间信息
        public static SqlDataReader GetSingleToolsNoticeByReader(int _ID)
        {
            string sSQL = "Select * from Notice_Info Where Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleNoticeTitleByReader(string _ToolsNoticeGuid)
        {
            string sSQL = "Select * from Notice_Info Where Status=0 And Guid='" + _ToolsNoticeGuid + "'";
            return DataCommon.GetDataByReader(sSQL);
        }

        //获取单个车间信息
        public static int DeleteSingleToolsNotice(int _ID)
        {
            string sSQL = "begin Delete from Notice_Info Where Status=0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>人员管理 机构部门：ID:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //更新车间信息
        public static int UpdateSingleToolsNotice(int _ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE Notice_Info SET "
                     + " OrganID=" + FieldValues.GetValue(0)
                     + ",NoticeTitle='" + FieldValues.GetValue(1) 
                     + "',NoticeBody='" + FieldValues.GetValue(2) + "'";                
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText += " end;";
            }
            else
            {
                sSqlText = "begin Insert Into Notice_Info (OrganID, NoticeTitle, NoticeBody,CreateUserID, RecGuid) Values("
                   + FieldValues.GetValue(0) + ",'" + FieldValues.GetValue(1) + "','" + FieldValues.GetValue(2) + "'," + SysClass.SysGlobal.GetCurrentUserID().ToString() + ",'" + FieldValues.GetValue(3) + "')" + ";";
                sSqlText = sSqlText + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }

        public static string GetImageSrcByNoticeNo(string NoticeNo)
        {
            string sImgSrc = "";
            string sSQL = "Select ImageSrc from Notice_Info where Status=0 And NoticeNo='" + NoticeNo + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                sImgSrc = "../Upload/"+ sdr["ImageSrc"].ToString();
            }
            sdr.Close();
            return sImgSrc;
        }                

        public static string GetToolsNoticePathNameByID(int NoticeID, string _ToolsNoticePath)
        {
            string _Title = _ToolsNoticePath;
            SqlDataReader sdr = SysNotice.GetSingleToolsNoticeByReader(NoticeID);
            if (sdr.Read())
            {
                if (_Title.Length > 0)
                {
                    _Title = sdr["NoticeTitle"].ToString() + "->" + _Title;
                }
                else
                {
                    _Title = sdr["NoticeTitle"].ToString();
                }
                if (int.Parse(sdr["PNoticeID"].ToString()) > 0)
                {
                    _Title = GetToolsNoticePathNameByID(int.Parse(sdr["PNoticeID"].ToString()), _Title);
                }
            }
            sdr.Close();
            return _Title;
        }
    }
}
