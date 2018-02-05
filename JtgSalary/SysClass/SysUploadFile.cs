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

namespace JtgTMS.SysClass
{
    public class SysUploadFile
    {
        public const string UploadDirectory = "Upload";
        public const string UploadThumbDirectory = "Upload/Thumbnail";
        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        public const int CS_Notice_Type = 1;
        public const string CS_NOTICE_SUBCATEGORY = "Notice";

        public const int CS_MARKETING_UPLOAD_TYPE = 2;
        public const string CS_MARKETING_SUBCATEGORY = "Marketing";

        public const int CS_FILE_UPLOAD_TYPE = 3;
        public const string CS_FILE_SUBCATEGORY = "File";

       

        
        public static int AddUploadFiles(int MasterTableType, string MasterTableRecGuid, string sSubCategory, string UploadFileName, string FileName, string FileSize)
        {
            string sUpdateSQL = " Begin";
            
            sUpdateSQL += "INSERT INTO SysUploadFile_Info (MasterTableType, MasterTableRecGuid,SubCategory, UploadFileName,FileName,FileSize, UserID) "
                  + "VALUES(" + MasterTableType.ToString() + ",'"
                  + MasterTableRecGuid + "','"
                  + sSubCategory
                  + "','" + UploadFileName
                  + "','" + FileName + "','" + FileSize + "'," + SysGlobal.GetCurrentUserID().ToString() + ");";

            sUpdateSQL += " End;";
            return DataCommon.QueryData(sUpdateSQL);
        }

        public static int AddUploadFiles(int MasterTableType, string MasterTableRecGuid, string sSubCategory, string UploadFileName, string FileName, string FileExt, string FileSize, string sDetailSQL)
        {
            string sUpdateSQL = " Begin";

            sUpdateSQL += " INSERT INTO SysUploadFile_Info (MasterTableType"
                  + ", MasterTableRecGuid"
                  + ",SubCategory"
                  + ",UploadFileName"
                  + ",FileName"
                  + ",FileExt"
                  + ",FileSize"
                  + ",UserID) "
                  + "VALUES(" + MasterTableType.ToString() + ""
                  + ",'" + MasterTableRecGuid + "'"
                  + ",'" + sSubCategory + "'"
                  + ",'" + UploadFileName + "'"
                  + ",'" + FileName + "'"
                  + ",'" + FileExt + "'"
                  + "," + FileSize 
                  + "," + SysGlobal.GetCurrentUserID().ToString() 
                  + ");";

            sUpdateSQL += sDetailSQL + " End;";
            return DataCommon.QueryData(sUpdateSQL);
        }



        public static DataSet GetUploadFilesByDataSet(string TableRecGuid)
        {
            string SqlText = "Select * From Upload_Files Where Status=0 And TableRecGuid='" + TableRecGuid + "'";

            return DataCommon.GetDataByDataSet(SqlText);
        }

        public static SqlDataReader GetUploadFilesByReader(string TableRecGuid)
        {
            string SqlText = "Select * From v_SysUploadFile_Info Where Status=0 And MasterTableRecGuid='" + TableRecGuid + "'";

            return DataCommon.GetDataByReader(SqlText);
        }

        public static SqlDataReader GetUploadFilesByReader(int _ID)
        {
            string SqlText = "Select a.*, b.OpName As UploadOpName From Upload_Files a"
                + " left join User_Info b on a.UserID=b.ID"
                + " Where a.Status=0 And a.ID=" + _ID;

            return DataCommon.GetDataByReader(SqlText);
        }

        public static string GetHyperByUploadFiles(string TableRecGuid)
        {
            string sHyperLink = "";
            SqlDataReader sdr = GetUploadFilesByReader(TableRecGuid);
            while (sdr.Read())
            {
                sHyperLink = sHyperLink + " <a href='../Public/DownloadFile.aspx?FileName=../" 
                    + SysClass.SysUploadFile.UploadDirectory + "/" + sdr["FilePath"].ToString()
                    + "&SaveFileName=" + sdr["FileName"].ToString() + "'>"
                    + sdr["FileName"].ToString() + "(" + sdr["FileSizeName"].ToString() + ")" + "</a><br />";
            }
            sdr.Close();
            if (sHyperLink.Length == 0)
            {
                sHyperLink = "无";
            }
            return sHyperLink;
        }

        public static string GetFileSizeValue(long FileSize)
        {
            string m_strSize = "";
            long _FactSize = 0;
            _FactSize = FileSize;

            if (_FactSize < KBCount)
            {
                m_strSize = _FactSize.ToString("F2") + " Byte";
            }
            else if (_FactSize >= KBCount && _FactSize < MBCount)
            {
                m_strSize = (_FactSize/KBCount).ToString("F2") + " KB";
            }
            else if (_FactSize >= MBCount && _FactSize < GBCount)
            {
                m_strSize = (_FactSize / MBCount).ToString("F2") + " MB";
            }
            else if (_FactSize >= GBCount && _FactSize < TBCount)
            {
                m_strSize = (_FactSize / GBCount).ToString("F2") + " GB";
            }
            else if (_FactSize >= TBCount)
            {
                m_strSize = (_FactSize / TBCount).ToString("F2") + " TB";
            }
            return m_strSize;
        }
    }
}
