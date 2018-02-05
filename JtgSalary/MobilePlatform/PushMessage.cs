using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.device;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using cn.jpush.api.schedule;
using CyxPack.OperateSqlServer;
using Quartz;
using Quartz.Impl;

namespace JtgSalary.MobilePlatform
{
    public class basePushMessage
    {
        public string app_key = "d86bc64e56ead188c611d7d1";
        public string master_secret = "f15ff2ccaf5fe6ea2e343b97";
        public string message = "";

        public virtual string SendMessage()
        {
            return "failed";
        }

        public PushPayload PushObject_All_All_Alert()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.all();
            pushPayload.audience = Audience.all();
            pushPayload.notification = new Notification().setAlert(message);
            return pushPayload;
        }

        public PushPayload PushObject_android_and_ios()
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.all();

            var notification = new Notification().setAlert(message);

            notification.AndroidNotification = new AndroidNotification().setTitle("Android Title");

            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("extra_key", "extra_value");
            pushPayload.notification = notification.Check();
            return pushPayload;
        }
    }

    public class PushMessage : basePushMessage
    {
        public string[] allAlias = null;
        public int maxSendCount = 950; //一次最多发送1000个消息，为了冗余，程序内设置最大次数950
        public override string SendMessage()
        {
            JPushClient client = new JPushClient(app_key, master_secret);

            PushPayload payload_all = PushObject_All_All_Alert();
            try
            {
                var result = client.SendPush(payload_all);
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);

                //如需查询上次推送结果执行下面的代码
                var apiResult = client.getReceivedApi(result.msg_id.ToString());
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());

            }
            catch
            {
                return "failed";
            }

            PushPayload payload_android_and_ios = PushObject_android_and_ios();
            try
            {
                var result = client.SendPush(payload_android_and_ios);
                //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                System.Threading.Thread.Sleep(10000);

                //如需查询上次推送结果执行下面的代码
                var apiResult = client.getReceivedApi(result.msg_id.ToString());
                var apiResultv3 = client.getReceivedApi_v3(result.msg_id.ToString());

            }
            catch
            {
                return "failed";
            }

            return "succeed";
        }

        public void GetAllAlias()
        {
            ArrayList aliasList = new ArrayList();
            string sSQL = "select distinct OpCode from UserSalary_Info Where Status=0 And SignStatus=0";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);

            for (; sdr.Read();)
            {
                if (sdr["OpCode"].ToString() != "")
                    aliasList.Add(sdr["OpCode"].ToString());
            }

            allAlias = new string[aliasList.Count];
            allAlias = (string[])aliasList.ToArray(typeof(string));
        }

        public PushPayload PushObject_all_alias_alert(string[] alias)
        {
            PushPayload pushPayload = new PushPayload();
            pushPayload.platform = Platform.android_ios();
            pushPayload.audience = Audience.s_alias(alias);
            pushPayload.notification = new Notification().setAlert(message);
            return pushPayload;
        }

        public string SendAllAliasMessage()
        {
            GetAllAlias();
            if (allAlias.Length<= maxSendCount)
            {
                return SendSomeAliasMessage(allAlias);
            }

            string[] someAlias = new string[maxSendCount];
            int i = 0;
            for (i = 0; i < allAlias.Length / maxSendCount; i++)
            {
                someAlias = new string[maxSendCount];
                Array.Copy(allAlias, maxSendCount * i, someAlias, 0, maxSendCount);
                SendSomeAliasMessage(someAlias);
            }
            someAlias = new string[(allAlias.Length - maxSendCount * i)];
            Array.Copy(allAlias, maxSendCount * i, someAlias, 0, allAlias.Length - maxSendCount * i);
            return SendSomeAliasMessage(someAlias);
        }

        public string SendSomeAliasMessage(string[] someAlias)
        {
            JPushClient client = new JPushClient(app_key, master_secret);

            PushPayload payload_alias = PushObject_all_alias_alert(someAlias);
            try
            {
                client.SendPush(payload_alias);
            }
            catch
            {
                return "failed";
            }

            return "succeed";
        }
    }

    public class SchedulePushMessage : basePushMessage
    {
        public static String NAME = "Test";
        public static bool ENABLED = true;
        public static String TIME = "2017-09-10 10:05:00";

        public static String START = "2017-09-08 14:30:00";
        public static String END = "2017-10-20 12:30:00";
        public static String TIME_PERIODICAL = "12:00:00";

        public static String TIME_UNIT = "week";
        public static int FREQUENCY = 1;
        public static String[] POINT = new String[] { "sat", "sun" };

        public override string SendMessage()
        {
            ScheduleClient scheduleclient = new ScheduleClient(app_key, master_secret);
            PushPayload payload_all = PushObject_All_All_Alert();

            //init a TriggerPayload
            TriggerPayload triggerConstructor = new TriggerPayload(START, END, TIME_PERIODICAL, TIME_UNIT, FREQUENCY, POINT);
            //init a SchedulePayload
            SchedulePayload schedulepayloadperiodical = new SchedulePayload(NAME, ENABLED, triggerConstructor, payload_all);

            try
            {
                var result = scheduleclient.sendSchedule(schedulepayloadperiodical);
            }
            catch
            {
                return "failed";
            }

            SchedulePayload schedulepayloadsingle = new SchedulePayload();
            TriggerPayload triggersingle = new TriggerPayload(TIME);

            schedulepayloadsingle.setPushPayload(payload_all);
            schedulepayloadsingle.setTrigger(triggersingle);
            schedulepayloadsingle.setName(NAME);
            schedulepayloadsingle.setEnabled(ENABLED);

            try
            {
                var result = scheduleclient.sendSchedule(schedulepayloadsingle);
            }
            catch
            {
                return "failed";
            }

            return "succeed";
        }
    }

    public class PushMessageJob : IJob
    {
        //Push the Message
        public virtual void Execute(IJobExecutionContext context)
        {
            PushMessage pm = new PushMessage();
            pm.message = "请及时签收工资信息。";
            pm.SendAllAliasMessage();
        }
    }

    public class PushMessageJobScheduler
    {
        public static string GetTargetTime()
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddDays(10);
            string TargetTime = "0 30 9 " + dt.Day + " " + dt.Month + " ? " + dt.Year;
            //dt = dt.AddSeconds(30);
            //string TargetTime = dt.Second + " " + dt.Minute + " " + dt.Hour + " " + dt.Day + " " + dt.Month + " ? " + dt.Year;
            return TargetTime;
        }

        public static void Start()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = sf.GetScheduler();

            IJobDetail job = JobBuilder.Create<PushMessageJob>()
                .WithIdentity("JtgJob", "JtgGroup")
                .Build();

            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("JtgTrigger", "JtgGroup")
                .WithCronSchedule(GetTargetTime())
                .Build();

            DateTimeOffset ft = sched.ScheduleJob(job, trigger);

            sched.Start();
        }
    }
}