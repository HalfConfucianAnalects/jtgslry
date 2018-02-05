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
using CyxPack.UserCommonOperation;

namespace JtgTMS.SysClass
{
    public class SysParams
    {
        public static string ToolDetailSelectForm_SelectValues = "";
        public static string ToolSelectForm_SelectValues = "";
        public static string UserInfo_ChoiceValues = "";

        public static string BulletinClsName = "OA01";
        public static int BulletinFileTableType = 10;
        public static string BulletinFileClass = "";

        public static string DocumentClsName = "OA02";
        public static int DocumentFileTableType = 11;
        public static string DocumentFileClass = "";

        public static string TrainingDocClsName = "OA03";
        public static int TrainingDocFileTableType = 12;
        public static string TrainingDocFileClass = "";

        public static string MailClsName = "OA04";
        public static int MailFileTableType = 13;
        public static string MailFileClass = "";

        public static string OfficialDocClsName = "OA05";
        public static int OfficialDocFileTableType = 14;
        public static string OfficialDocFileClass = "";
        //public static int OfficialDocBillType = 14;

        public static string TaskClsName = "OA06";
        public static int TaskFileTableType = 15;
        public static int TaskChargeFileTableType = 16;
        public static string TaskFileClass = "";
        public static int TaskPassStatus = 0;           //草稿
        public static int TaskReplyStatus = 1;          //回复、即提交
        public static int TaskCompleteStatus = 2;       //完成
        public static int TaskReturnStatus = 3;         //退回
        public static int TaskBreakStatus = 4;          //取消
        public static int TaskDeleteStatus = -1;          //已删除

        public static string WorkPlanClsName = "OA07";
        public static int WorkPlanFileTableType = 17;
        public static string WorkPlanFileClass = "";
        public static int WorkPlanReplyStatus = 0;
        public static int WorkPlanNormalStatus = 0;
        public static int WorkPlanDeleteStatus = 4;

        public static string ProjectPlanClsName = "OA08";
        public static int ProjectPlanFileTableType = 18;
        public static int ProjectPlanChargeFileTableType = 19;
        public static string ProjectPlanFileClass = "";
        public static int ProjectPlanDraftStatus = 0;           //草稿
        public static int ProjectPlanSubmittedStatus = 1;      //已提交

        public static int ProjectPlanReplyStatus = 1;          //回复、即提交
        public static int ProjectPlanPassStatus = 2;           //已审核
        public static int ProjectPlanCompleteStatus = 3;       //完成
        public static int ProjectPlanReturnStatus = 4;         //退回
        public static int ProjectPlanBreakStatus = 5;          //取消
        public static int ProjectPlanDeleteStatus = -1;          //已删除

        public static string ServiceLogClsName = "OA09";
        public static int ServiceLogFileTableType = 20;
        public static string ServiceLogFileClass = "";
        public static int ServiceLogReplyStatus = 0;
        public static int ServiceLogNormalStatus = 0;
        public static int ServiceLogDeleteStatus = 4;      

        public static string DraftStauts = "0";             //草稿
        public static string UnAuditedStauts = "1";         //审批中
        public static string AuditedStauts = "2";           //审批通过
        public static string NotPassStauts = "3";           //审批退回
        public static string DeletedStauts = "4";           //已删除

        //权限值
        public static int PurviewOfficeDoc = 1;             //接收公文
        public static int PurviewOfficialDoc_Edit = 11;     //写公文

        public static int PurviewMail = 2;                  //电子邮件
        public static int PurviewMail_Edit = 21;            //写邮件

        public static int PurviewDocument = 3;              //规章制度
        public static int PurviewDocument_Edit = 31;        //发布规章制度
        public static int PurviewDocument_Audit = 32;       //审核规章制度

        public static int PurviewTrainingDoc = 4;           //技术资料
        public static int PurviewTrainingDoc_Edit = 41;     //发布技术资料
        public static int PurviewTrainingDoc_Audit = 42;    //审核技术资料

        public static int WorkPlanDoc = 6;           //工作日志
        public static int WorkPlanDoc_Edit = 61;     //发布工作日志
        public static int WorkPlanDoc_Audit = 62;    //审核工作日志

        public static int PurviewBulletin = 5;              //通知公告
        public static int PurviewBulletin_Edit = 51;        //发布通知公告
        public static int PurviewBulletin_Audit = 52;       //审核通知公告

        public static int PurviewTask = 6;              //工作事务
        public static int PurviewTask_Release = 61;       //发布

        public static int PurviewWorkPlan = 7;              //工作日志
        public static int PurviewWorkPlan_Release = 71;     //发布工作日志

        public static int PurviewProjectPlan = 8;              //项目计划
        public static int PurviewProjectPlan_Release = 81;     //发布项目计划
        public static int PurviewProjectPlan_Release_Audit = 1;     //审核项目计划

        public static int PurviewServiceLog = 10;              //维修日志
        public static int PurviewServiceLog_Release = 101;     //发布工作日志

        public static int PurviewAdmin = 9;                 //系统管理
        public static int PurviewAdmin_Organ = 91;           //机构部门
        public static int PurviewAdmin_User = 92;           //员工档案
        public static int PurviewAdmin_Role = 93;           //角色权限
        public static int PurviewAdmin_BulletinCls = 94;           //通知公告分类
        public static int PurviewAdmin_DocumentCls = 95;           //规章制度分类
        public static int PurviewAdmin_TrainingDocCls = 96;           //技术资料分类


        //public static int PurviewSystemID = 1;

        public static int GetPurviewSystemID()
        {
            int _PurviewSystemID = 1;
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            _PurviewSystemID = info.PurviewSystemID;
            return _PurviewSystemID;
        }

        public static string GetPurviewSystemTitle()
        {
            string _PurviewSystemTitle = "";
            UserInfo info = (UserInfo)UserCommonOperation.GetUserInfo();
            _PurviewSystemTitle = info.PurviewSystemTitle;
            return _PurviewSystemTitle;
        }

        public static string GetTitleByStatusValue(int _StatusValue)
        {
            string _StatusTitle = "";
            if (_StatusValue == 0)
            {
                _StatusTitle = "草稿";
            }
            if (_StatusValue == 1)
            {
                _StatusTitle = "审批中";
            }
            if (_StatusValue == 2)
            {
                _StatusTitle = "审批通过";
            }
            if (_StatusValue == 3)
            {
                _StatusTitle = "审批退回";
            }
            if (_StatusValue == 4)
            {
                _StatusTitle = "已删除";
            }
            return _StatusTitle;
        }
    }
}
