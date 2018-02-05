using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using JtgTMS.SysClass;
using System.Text;
using System.Web;
using CyxPack.OperateSqlServer;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace JtgSalary.MobilePlatform
{
    public class GetData
    {
        static Dictionary<string, string> dictionaryToJson = new Dictionary<string, string>();

        public static string DataToJson(Dictionary<string, string> dictionary)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            foreach (var kvp in dictionary)
            {
                jsonBuilder.Append("\"");
                jsonBuilder.Append(kvp.Key);
                jsonBuilder.Append("\":\"");
                jsonBuilder.Append(kvp.Value);
                jsonBuilder.Append("\",");
            }
            if (!jsonBuilder.ToString().EndsWith("{"))
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("}");

            return jsonBuilder.ToString();
        }

        public static string DataToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName.ToString());
            jsonBuilder.Append("\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            if (!jsonBuilder.ToString().EndsWith("["))
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]}");
            return jsonBuilder.ToString();
        }

        public static string GetName(string PhoneNum)
        {
            try
            {
                dictionaryToJson.Clear();
                string sSQL = "Select a.*,b.Column_手机号码 from SysUser_Info a"
                              + " left join SysUserExt_Info b on b.UserID=a.ID"
                              + " Where b.Column_手机号码='" + PhoneNum + "'";
                SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
                if (sdr.HasRows && sdr.Read())
                {
                    dictionaryToJson.Add("userID", sdr["ID"].ToString());
                    dictionaryToJson.Add("OpCode", sdr["OpCode"].ToString());
                    dictionaryToJson.Add("OpName", sdr["OpName"].ToString());
                    dictionaryToJson.Add("OrganID", sdr["OrganID"].ToString());
                    dictionaryToJson.Add("PhoneNum", sdr["Column_手机号码"].ToString());
                }
                else
                {
                    sSQL = "Select a.*,b.Column_手机号码 from SysUser_Info a"
                              + " left join SysUserExt_Info b on b.UserID=a.ID"
                              + " Where a.OpCode='" + PhoneNum + "'";

                    SqlDataReader sdrOpcode = DataCommon.GetDataByReader(sSQL);
                    if (sdrOpcode.HasRows && sdrOpcode.Read())
                    {
                        dictionaryToJson.Add("userID", sdrOpcode["ID"].ToString());
                        dictionaryToJson.Add("OpCode", sdrOpcode["OpCode"].ToString());
                        dictionaryToJson.Add("OpName", sdrOpcode["OpName"].ToString());
                        dictionaryToJson.Add("OrganID", sdrOpcode["OrganID"].ToString());
                        dictionaryToJson.Add("PhoneNum", sdrOpcode["Column_手机号码"].ToString());
                    }
                    sdrOpcode.Close();
                }
                sdr.Close();
            }
            catch (Exception ex)
            {

            }
            return DataToJson(dictionaryToJson);
        }

        public static string UserLogin(string UserID, string Password)
        {

            dictionaryToJson.Clear();

            string sSQL = "Select Password from SysUser_Info Where ID='" + UserID + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            
            if (sdr.Read())
            {
                string userPassword = sdr["Password"].ToString();
                
                if (userPassword == Password)
                {

                    UpdateLoginState(UserID, 1);
                    if (IsNeedCode(UserID))
                    {
                        string sSqlText = "update SysUserMobile_Info set NeedCode=0 where UserID='" + UserID + "'";
                        DataCommon.QueryData(sSqlText);
                    }
                    dictionaryToJson.Add("Result", "True");
                }
                else
                {
                    UpdateLoginState(UserID, 0);
                    dictionaryToJson.Add("Result", "False");
                }
            }
            else
            {
                UpdateLoginState(UserID, 0);
                dictionaryToJson.Add("Result", "False");
            }
            sdr.Close();
            return DataToJson(dictionaryToJson);
        }

        public static string UserLoginByPhoneCode(string UserID, string Password, string PhoneCode)
        {
            dictionaryToJson.Clear();

            if (ValidateCode(UserID, PhoneCode))
            {
                return UserLogin(UserID, Password);
            }
            else
            {
                UpdateLoginState(UserID, 0);
                dictionaryToJson.Add("Result", "False");
                return DataToJson(dictionaryToJson);
            }
        }

        public static string GetUserProfile(string sID)
        {
            dictionaryToJson.Clear();
            int ID = Convert.ToInt32(sID);
            SqlDataReader sdr = SysUser.GetUserInfoByReader(ID);
            if (sdr.Read())
            {
                dictionaryToJson.Add("OpCode", sdr["OpCode"].ToString());
                dictionaryToJson.Add("OpName", sdr["OpName"].ToString());
            }

            DataSet ds = SysCustomField.GetCustomFieldLstByDataset(" And TableNo='UserInfo'");
            DataTable dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string key = dr["FieldName"].ToString();
                string value = sdr["Column_" + key].ToString();
                dictionaryToJson.Add(key, value);
            }
            return DataToJson(dictionaryToJson);
        }

        public static string GetSalary(string OpCode, string SalaryYears, bool isReadSign)
        {
            string sWhereSQL = " And a.OpCode='" + OpCode;
            if (isReadSign)
            {
                sWhereSQL += "' And SignStatus=1";
            }
            else
            {
                sWhereSQL += "' And SignStatus=0";
            }
            sWhereSQL += " And a.SalaryYears='" + SalaryYears + "'";
            sWhereSQL +=
                " And a.SalaryRecGuid in (Select SalaryRecGuid From UserUserImportRec_Info where ApprovalStatus=1)";

            DataSet ds = SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL);
            DataToJson(ds.Tables[0]);
            return DataToJson(ds.Tables[0]);
        }

        public static string GetSalaryDetail(string SalaryID, string SalaryYears)
        {
            string DetailKey = "";
            string DetailValue = "";

            string SalaryRecGuid = "";

            string result = "";
            dictionaryToJson.Clear();
            SqlDataReader sdrValue = SysUserSalary.GetSingleUserSalaryByReader(Convert.ToInt32(SalaryID));
            if (sdrValue.Read())
            {
                SalaryRecGuid = sdrValue["SalaryRecGuid"].ToString();
                SqlDataReader sdrKey = SysUserSalary.GetUserSalaryFieldsLstByReader(SalaryYears, SalaryRecGuid, "");
                result = "{\"DetailResult\": [";
                while (sdrKey.Read())
                {
                    string FieldName = sdrKey["FieldName"].ToString();
                    DetailKey = sdrKey["UserFieldTitle"].ToString();
                    DetailValue = sdrValue[FieldName].ToString();
                    dictionaryToJson.Add(DetailKey, DetailValue);
                    result += DataToJson(dictionaryToJson) + ",";
                    dictionaryToJson.Clear();
                }

                if (result.EndsWith("["))
                    result = "";
                if (result.EndsWith(","))
                    result = result.Substring(0, result.Length - 1);
                result += "]}";
            }
            return result;
        }

        public static string UpdateSalary(string ID, bool isToSign, string Description)
        {
            string sSqlText = "begin";
            if (isToSign)
            {
                sSqlText = sSqlText + " UPDATE UserSalary_Info SET SignPlatform=1, SignStatus=1, SignDate=GetDate(), Description='" + Description + "'";
                sSqlText = sSqlText + " WHERE ID in (" + ID + ")" + ";";
            }
            else
            {
                sSqlText += " Update UserSalary_Info Set SignStatus=0 Where ID in (" + ID + "); ";
            }
            sSqlText += " end;";

            dictionaryToJson.Clear();
                if (1 == DataCommon.QueryData(sSqlText))
                {
                    dictionaryToJson.Add("Result", "True");
                }
                else
                {
                    dictionaryToJson.Add("Result", "False");
            }
            
            return DataToJson(dictionaryToJson);

        }

        public static string GetUserNotice(string userID, string userOrganID)
        {
            string sSQL = "Select a.*,IsNull(b.SelfClickNum, 0) As SelfClickNum,c.OpName from Notice_Info a "
                          + " left join (Select MasterID, Sum(ClickNum) As SelfClickNum From SysClick_Info Where ClickUserID="
                          + userID + " Group by MasterID) b on b.MasterID=a.Id"
                          + " left join Sysuser_info c on a.CreateUserID=c.ID"
                          + " Where a.Status=0" + " And a.OrganID in (select ID from [GetParentOrganByID](" + userOrganID +
                          "))" + " Order By a.Createdtime desc,a.SortID";
            DataSet ds = DataCommon.GetDataByDataSet(sSQL);
            return DataToJson(ds.Tables[0]);
        }

        public static string GetUserNoticeDetail(string userID, int noticeID)
        {
            UpdateClickNum(noticeID.ToString(), userID);

            SqlDataReader sdr = SysNotice.GetSingleToolsNoticeByReader(noticeID);
            dictionaryToJson.Clear();
            if (sdr.Read())
            {
                dictionaryToJson.Add("NoticeTitle", sdr["NoticeTitle"].ToString());
                dictionaryToJson.Add("NoticeBody", sdr["NoticeBody"].ToString());
                dictionaryToJson.Add("CreatedTime", sdr["CreatedTime"].ToString());
            }
            sdr.Close();
            return DataToJson(dictionaryToJson);
        }

        public static string UpdateUserPhoneNum(string UserID, string PhoneNum, string PhoneCode)
        {
            dictionaryToJson.Clear();

            if (ValidateCode(UserID, PhoneCode))
            {
                string sSqlText = "begin UPDATE SysUserExt_Info SET Column_手机号码='" + PhoneNum + "' WHERE UserID=" + UserID + "; End;";

                if (1 == DataCommon.QueryData(sSqlText))
                {
                    dictionaryToJson.Add("Result", "True");
                    return DataToJson(dictionaryToJson);
                }
            }
            dictionaryToJson.Add("Result", "False");

            return DataToJson(dictionaryToJson);
        }

        public static string GetPhoneCode(string UserID, string mobile)
        {
            Random _random = new Random();
            string _tradeno = DateTime.Now.ToString("yyyyMMddHHmmssfff") + _random.Next(100, 1000);
            string _username = "hzdongxiaofan";
            string _password = "jK2qVBRX";
            Random rad = new Random();
            int mobile_code = rad.Next(100000, 1000000);
            string _content = "首次登陆或者更换手机时，需要短信验证。您的验证码是：" + mobile_code + "，请谨慎保存。";
            string _phone = mobile.Trim();
            string json = JsonConvert.SerializeObject(new
            {
                userPassword = _password,
                tradeNo = _tradeno,
                phones = _phone,
                etnumber = "",
                userName = _username,
                content = _content,
            });
            string _sign = AESEncrypt(json, "DTFU6E7KWaotNz8N", "DTFU6E7KWaotNz8N");
            _password = MD5Encrypt(_password);
            string parm_json = JsonConvert.SerializeObject(new
            {
                tradeNo = _tradeno,
                userName = _username,
                userPassword = _password,
                phones = _phone,
                content = _content,
                etnumber = "",
                sign = _sign
            });

            string PostUrl = "http://apis.hzfacaiyu.com/sms/openCard";
            string returnStr = HttpHelper.Post(PostUrl, parm_json);

            dictionaryToJson.Clear();
            if (returnStr.Contains("提交成功"))
            {
                if (UpdatePhoneCode(UserID, mobile_code.ToString()))
                    dictionaryToJson.Add("Result", "True");
                else
                    dictionaryToJson.Add("Result", "False");
            }
            else
            {
                dictionaryToJson.Add("Result", "False");
            }
            return DataToJson(dictionaryToJson);
        }

        public static string GetUserHeadPortrait(string UserID)
        {
            dictionaryToJson.Clear();

            try
            {
                string filePath = ConfigurationManager.AppSettings["HeadPortraitFilePath"];
                string strHeadPortrait = File.ReadAllText(filePath + UserID + ".txt");
                dictionaryToJson.Add("Column_头像", strHeadPortrait);
            }
            catch (Exception e)
            {
                dictionaryToJson.Add("Column_头像", "");
            }

            return DataToJson(dictionaryToJson);
        }

        public static string UpdateUserHeadPortrait(string UserID, string HeadPortrait)
        {
            dictionaryToJson.Clear();
            try
            {
                string filePath = ConfigurationManager.AppSettings["HeadPortraitFilePath"];
                File.Delete(filePath + UserID + ".txt");
                using (StreamWriter sw = File.CreateText(filePath + "\\" + UserID + ".txt"))//AppendText(filePath + "\\" + UserID + ".txt"))
                {
                    sw.Write(HeadPortrait);
                }
                dictionaryToJson.Add("Result", "True");
            }
            catch (Exception e)
            {
                dictionaryToJson.Add("Result", "False");
                //LogHelper(e.Message);
            }
            return DataToJson(dictionaryToJson);
        }

        public static bool MobileIsLogin(string UserID)
        {
            bool result = false;
            string sSQL = "Select * from SysUserMobile_Info"
                          + " Where UserID='" + UserID + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);

            if (sdr.Read())
            {
                int LoginState = Convert.ToInt32(sdr["LoginState"].ToString());
                DateTime LoginTime = Convert.ToDateTime(sdr["LoginTime"].ToString());
                TimeSpan ts = DateTime.Now - LoginTime;
                if (LoginState == 1 && ts.TotalSeconds < 600)
                {
                    UpdateLoginState(UserID, 1);
                    result = true;
                }
            }
            sdr.Close();
            return result;
        }

        public static bool IsNeedCode(string UserID)
        {
            //return false;//ios上传store时，做测试使用

            bool result = false;
            string sSQL = "Select * from SysUserMobile_Info"
                          + " Where UserID='" + UserID + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);

            if (sdr.Read())
            {
                int NeedCode = Convert.ToInt32(sdr["NeedCode"].ToString());
                if (NeedCode == 1)
                {
                    result = true;
                }
            }
            sdr.Close();
            return result;
        }

        public static string UpdateRegistrationID(string userID, string newRegistrationID)
        {
            /*************ios上传store时，做测试使用********************/
            //dictionaryToJson.Clear();

            //dictionaryToJson.Add("Result", "False");

            //return DataToJson(dictionaryToJson);
            /*************ios上传store时，做测试使用********************/

            bool result = false;
            string sSQL = "Select * from SysUserMobile_Info"
                          + " Where UserID='" + userID + "'";

            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);

            if (sdr.Read())
            {
                if (sdr["NeedCode"].ToString() == "1")
                {
                    result = true;
                }
                if (sdr["RegistrationID"].ToString() != newRegistrationID)
                {
                    string sSqlText = "update SysUserMobile_Info set RegistrationID='" + newRegistrationID +
                                      "', NeedCode=1 where UserID='" + userID + "'";
                    result = 1 == DataCommon.QueryData(sSqlText);
                }
            }
            sdr.Close();
            dictionaryToJson.Clear();
            if (result)
            {
                dictionaryToJson.Add("Result", "True");
            }
            else
            {
                dictionaryToJson.Add("Result", "False");
            }

            return DataToJson(dictionaryToJson);
        }

        #region 私有函数

        private static string UpdateClickNum(string noticeID, string userID)
        {
            string sSQL = " Begin ";
            sSQL += " Update Notice_Info Set ClickNum=IsNull(ClickNum,0) + 1 Where ID=" + noticeID + ";";

            sSQL += " if Exists(Select 1 From SysClick_Info Where MasterType=1"
                    + " And MasterID=" + noticeID + " And ClickUserID=" + userID + ")";

            sSQL += " begin";

            sSQL += " Update SysClick_Info Set ClickNum=IsNull(ClickNum,0) + 1 Where MasterType=1"
                    + " And MasterID=" + noticeID
                    + " And ClickUserID=" + userID + "";
            sSQL += " end";
            sSQL += " else";
            sSQL += " begin";
            sSQL += " Insert Into SysClick_Info (MasterType, MasterID, ClickUserID) Values(" + 1
                    + "," + noticeID + "," + userID + ");";
            sSQL += " Update Notice_Info Set ClickUserNum=IsNull(ClickUserNum,0) + 1 Where ID=" + noticeID.ToString() + ";";
            sSQL += " end";

            sSQL = sSQL + " end";

            dictionaryToJson.Clear();
            if (1 == DataCommon.QueryData(sSQL))
            {

                dictionaryToJson.Add("Result", "True");
            }
            else
            {
                dictionaryToJson.Add("Result", "False");
            }
            return DataToJson(dictionaryToJson);
        }

        private static bool UpdatePhoneCode(string UserID, string PhoneCode)
        {
            string sSqlText = "update SysUserMobile_Info set PhoneCode='" + PhoneCode + "', CodeTime=GetDate()" +
                              " where UserID='" + UserID + "'";
            return 1 == DataCommon.QueryData(sSqlText);
        }

        private static bool UpdateLoginState(string UserID, int LoginState)
        {
            if (LoginState == 1)
            {
                UpdateNeedCodeState(UserID, 0);
            }
            string sSqlText = "update SysUserMobile_Info set LoginState=" + LoginState + ", LoginTime=GetDate()" + " where UserID='" + UserID + "'";

            return 1 == DataCommon.QueryData(sSqlText);
        }

        private static void UpdateNeedCodeState(string userID, int isNeed)
        {
            string sSqlText = "update SysUserMobile_Info set  NeedCode=" + isNeed + " where UserID='" + userID + "'";
            DataCommon.QueryData(sSqlText);
        }

        private static bool ValidateCode(string userID, string PhoneCode)
        {
            string sSQL = "Select * from SysUserMobile_Info" + " Where UserID='" + userID + "'";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);

            if (sdr.Read())
            {
                string userPhoneCode = sdr["PhoneCode"].ToString();
                DateTime CodeTime = Convert.ToDateTime(sdr["CodeTime"].ToString());

                TimeSpan ts = DateTime.Now - CodeTime;
                if (ts.TotalSeconds < 600 && PhoneCode == userPhoneCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 功能辅助函数

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        private static string AESEncrypt(string toEncrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return byteToHexStr(resultArray);
        }

        /// <summary>
        /// 字节转换成十六进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// MD5加密，返回MD5 16位或32位加密后的字符串，默认返回32位。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string MD5Encrypt(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
        #endregion

        #region 测试用函数

        public static void LogHelper(string log)
        {
            //using (StreamWriter sw = File.AppendText("C:\\work\\www\\JtgSalaryWWW\\Logs\\asplog.txt"))//C:\work\www\JtgSalaryWWW\Logs
            //{
            //    sw.WriteLine(log);
            //}
            using (StreamWriter sw = File.AppendText("E:\\asplog.txt"))//C:\work\www\JtgSalaryWWW\Logs
            {
                sw.WriteLine(log);
            }
        }

        public static string ShowDataSetContent(DataSet ds)
        {
            string sSqlText = "Table:\r\n";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    sSqlText += ds.Tables[0].Rows[i][j].ToString() + " ; ";
                }

                sSqlText += "\r\n";
            }
            return sSqlText;
        }
        public static string ShowDataSetContent(DataTable dt)
        {
            string sSqlText = "Table:\r\n";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sSqlText += dt.Rows[i][j].ToString() + " ; ";
                }

                sSqlText += "\r\n";
            }
            return sSqlText;
        } 
        #endregion

    }
}
