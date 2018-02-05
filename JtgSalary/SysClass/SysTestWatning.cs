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
    public class SysTestWatning
    {
        public static DataSet GetTestWaringByDataSet(string v,int CategoryID, string SearchText)
        {
            string sSQL = "";
            if (v.Equals("0"))
            {
                 sSQL = "select a.* ,isnull(b.TestItem,0)as Lower,isnull(TestCycle,0)as Stock,b.ID as cid from Tool_Info a  "
                + " Left Join TestWarning b on a.ID=b.ToolID"
                + " Where a.CategoryID=" + CategoryID.ToString() + "";

                if (SearchText.Length > 0)
                {
                    sSQL = sSQL + "  And (a.ToolName Like '%" + SearchText + "%' Or a.ToolNo Like '%" + SearchText + "%')";
                }
            }
            else if (v.Equals("1"))
            {
                 sSQL = "select a.* ,isnull(b.TestItem,0)as Lower,isnull(TestCycle,0)as Stock,b.ID as cid from Tool_Info a  "
                + " Left Join TestWarning b on a.ID=b.ToolID"
                + " Where a.CategoryID=" + CategoryID.ToString() + "And isnull(b.TestItem,0)<>'0'And isnull(TestCycle,0)<>'0'";

                if (SearchText.Length > 0)
                {
                    sSQL = sSQL + "  And (a.ToolName Like '%" + SearchText + "%' Or a.ToolNo Like '%" + SearchText + "%')";
                }
            }
            else if (v.Equals("2"))
            {
                sSQL = "select a.* ,isnull(b.TestItem,0)as Lower,isnull(TestCycle,0)as Stock,b.ID as cid from Tool_Info a  "
               + " Left Join TestWarning b on a.ID=b.ToolID"
               + " Where a.CategoryID=" + CategoryID.ToString() + "And isnull(b.TestItem,0)='0'And isnull(TestCycle,0)='0'";

                if (SearchText.Length > 0)
                {
                    sSQL = sSQL + "  And (a.ToolName Like '%" + SearchText + "%' Or a.ToolNo Like '%" + SearchText + "%')";
                }
            }
            return DataCommon.GetDataByDataSet(sSQL);
        }
        public static SqlDataReader GetTestWarningInfoByID(int ToolID)
        {
            string sSQL = "select a.* ,isnull(b.TestItem,0)as Lower,isnull(TestCycle,0)as Stock,b.ID as cID from Tool_Info a  "
                + " Left Join TestWarning b on a.ID=b.ToolID"
                + " Where a.ID=" + ToolID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }
        public static int UpdateSingleTestWarning(int _ID, string[] FieldValues)
        {

            string sSql = "if Exists(Select 1 from TestWarning Where ToolID=" + _ID + ")";
            sSql = sSql + "  begin update TestWarning Set TestItem='" + FieldValues.GetValue(0) + "', TestCycle=" + FieldValues.GetValue(1) + "  where toolID=" + _ID + " ";
            sSql = sSql + "end else begin Insert into TestWarning (toolID, TestItem, TestCycle) Values(" + _ID + ", '" + FieldValues.GetValue(0) + "', '" + FieldValues.GetValue(1) + "')end";

            return DataCommon.QueryData(sSql);
        }
    }
}
