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
using System.Text;

namespace JtgTMS.SysClass
{
    public class SysPageNums
    {
        private static string GetCurrentQueryParams() 
        { 
            StringBuilder stringBuilder = new StringBuilder(); 
            string currentPath = HttpContext.Current.Request.Url.PathAndQuery; 
            int startIndex = currentPath.IndexOf("?"); 
            if (startIndex <= 0)
                return HttpContext.Current.Request.CurrentExecutionFilePath + "?"; 
            string[] nameValues = currentPath.Substring(startIndex + 1).Split('&'); 
            foreach ( string param in nameValues ) 
            {
                if ((param.ToString().ToLower().IndexOf("age=") <= 0) && (param.ToString().ToLower().IndexOf("isfirst=") < 0))
                {
                    stringBuilder.Append( param ); 
                    stringBuilder.Append( "&" ); 
                }
            }
            if (stringBuilder.ToString().Length > 0)
            {
                return HttpContext.Current.Request.CurrentExecutionFilePath + "?" + stringBuilder.ToString().TrimEnd('&');
            }
            else
            {
                return HttpContext.Current.Request.CurrentExecutionFilePath + "?";
            }
            
        }

        public static string GetCurrentQueryParams(string notParamName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string currentPath = HttpContext.Current.Request.Url.PathAndQuery;
            int startIndex = currentPath.IndexOf("?");
            if (startIndex <= 0)
                return HttpContext.Current.Request.CurrentExecutionFilePath + "?";
            string[] nameValues = currentPath.Substring(startIndex + 1).Split('&');
            foreach (string param in nameValues)
            {
                if (param.ToString().ToLower().IndexOf(notParamName.ToLower()) <= 0)
                {
                    stringBuilder.Append(param);
                    stringBuilder.Append("&");
                }
            }
            return HttpContext.Current.Request.CurrentExecutionFilePath + "?" + stringBuilder.ToString().TrimEnd('&');
        }

        public static string GetCurrentQueryMuliParams(string notParamName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string currentPath = HttpContext.Current.Request.Url.PathAndQuery;
            int startIndex = currentPath.IndexOf("?");
            if (startIndex <= 0)
                return HttpContext.Current.Request.CurrentExecutionFilePath + "?";
            string[] nameValues = currentPath.Substring(startIndex + 1).Split('&');
            string[] notnameValues = notParamName.Split(',');
            foreach (string param in nameValues)
            {
                bool bFind = false;
                foreach (string notparam in notnameValues)
                {
                    if (param.ToString().ToLower().IndexOf(notparam.ToLower()) >= 0)
                    {
                        bFind = true;
                    }
                }
                if (bFind == false)
                {
                    stringBuilder.Append(param);
                    stringBuilder.Append("&");
                }
            }
            return HttpContext.Current.Request.CurrentExecutionFilePath + "?" + stringBuilder.ToString().TrimEnd('&');
        }

        public static string GetPageNum(DataSet ds, GridView datalistname, int pagesize)
        {
            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = ds.Tables[0].DefaultView;
            objPds.AllowPaging = true;
            int total = ds.Tables[0].Rows.Count;
            objPds.PageSize = pagesize;
            int page;
            if (HttpContext.Current.Request.QueryString["page"] != null)
                page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
            else
                page = 1;
            objPds.CurrentPageIndex = page - 1;
            datalistname.DataSource = objPds;
            datalistname.DataBind();
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (page < 1) { page = 1; }
            //计算总页数
            if (pagesize != 0)
            {
                allpage = (total / pagesize);
                allpage = ((total % pagesize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; } //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "共" + allpage + "页&nbsp;&nbsp;";
            pagestr += page > 1 ? "<a href=\"" + HttpContext.Current.Request.RawUrl + "?page=1\">首页</a>&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + pre + "\">上一页</a>" : "首页 上一页";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "&nbsp;&nbsp;" + i + "" : "&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + i + "\">" + i + "</a>";
            }
            pagestr += page != allpage ? "&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + next + "\">下一页</a>&nbsp;&nbsp;<a href=\"" + HttpContext.Current.Request.CurrentExecutionFilePath + "?page=" + allpage + "\">末页</a> &nbsp;" : " 下一页 末页 &nbsp;";
            return pagestr;
        }

        //Add by lk 20151214 start
        public static string GetPageRawUrlNum(DataSet ds, GridView datalistname, int pagesize,bool isTurningPage)
        {
            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = ds.Tables[0].DefaultView;
            objPds.AllowPaging = true;
            int total = ds.Tables[0].Rows.Count;
            objPds.PageSize = pagesize;
            int page;
            if (isTurningPage)
            {
                if (HttpContext.Current.Request.QueryString["page"] != null)
                    page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
                else
                    page = 1;
            }
            else
            {
                page = 1;
            }

            int allpage = 0;
            allpage = (total / pagesize);
            if ((total % pagesize) > 0)
            {
                allpage = allpage + 1;
            }


            if (page > allpage)
            {
                page = 1;
            }
            try
            {
                objPds.CurrentPageIndex = page - 1;
                datalistname.DataSource = objPds;
                datalistname.DataBind();
            }
            catch (Exception e)
            {
            }

            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (page < 1) { page = 1; }
            //计算总页数
            if (pagesize != 0)
            {
                allpage = (total / pagesize);
                allpage = ((total % pagesize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; } //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            //pagestr += "<ul>";
            pagestr += page > 1 ? "<a href=\"" + GetCurrentQueryParams() + "&page=1\" >首页</a> | <a href=\"" + GetCurrentQueryParams() + "&page=" + pre + "\" >前页</a>" : "<a disabled='disabled'>首页</a> | <a disabled='disabled'>前页</a>";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "&nbsp;&nbsp;| <a disabled='disabled'>" + i + "</a>" : "&nbsp;&nbsp;| <a href=\"" + GetCurrentQueryParams() + "&page=" + i + "\">" + i + "</a>";
            }
            pagestr += page != allpage ? "&nbsp;&nbsp;| <a href=\"" + GetCurrentQueryParams() + "&page=" + next + "\" >后页</a>&nbsp;&nbsp;| <a href=\"" + GetCurrentQueryParams() + "&page=" + allpage + "\" >尾页</a>&nbsp;" : " | <a disabled='disabled'>后页</a> | <a disabled='disabled'>尾页&nbsp;";
            //pagestr +=  "</ul>";
            pagestr += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;共有 " + total.ToString() + " 条数据，当前第 " + page.ToString() + " 页";
            return pagestr;
        }
        //Add by lk 20151214 end

        public static string GetPageRawUrlNum(DataSet ds, GridView datalistname, int pagesize)
        {
            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = ds.Tables[0].DefaultView;
            objPds.AllowPaging = true;
            int total = ds.Tables[0].Rows.Count;
            objPds.PageSize = pagesize;
            int page;
            if (HttpContext.Current.Request.QueryString["page"] != null)
                page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
            else
                page = 1;

            int allpage = 0;
            allpage = (total / pagesize);
            if ((total % pagesize) > 0)
            {
                allpage = allpage+1;
            }
            

            if (page > allpage)
            {
                page = 1;
            }

            objPds.CurrentPageIndex = page - 1;
            datalistname.DataSource = objPds;
            datalistname.DataBind();            
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (page < 1) { page = 1; }
            //计算总页数
            if (pagesize != 0)
            {
                allpage = (total / pagesize);
                allpage = ((total % pagesize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; } //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            //pagestr += "<ul>";
            pagestr += page > 1 ? "<a href=\"" + GetCurrentQueryParams() + "&page=1\" >首页</a> | <a href=\"" + GetCurrentQueryParams() + "&page=" + pre + "\" >前页</a>" : "<a disabled='disabled'>首页</a> | <a disabled='disabled'>前页</a>";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "&nbsp;&nbsp;| <a disabled='disabled'>" + i + "</a>" : "&nbsp;&nbsp;| <a href=\"" + GetCurrentQueryParams() + "&page=" + i + "\">" + i + "</a>";
            }
            pagestr += page != allpage ? "&nbsp;&nbsp;| <a href=\"" + GetCurrentQueryParams() + "&page=" + next + "\" >后页</a>&nbsp;&nbsp;| <a href=\"" + GetCurrentQueryParams() + "&page=" + allpage + "\" >尾页</a>&nbsp;" : " | <a disabled='disabled'>后页</a> | <a disabled='disabled'>尾页</a>&nbsp;";
            //pagestr +=  "</ul>";
            pagestr += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;共有 " + total.ToString() + " 条数据，当前第 " + page.ToString() + " 页";
            return pagestr;
        }

        public static string GetPageRawUrlNum(DataSet ds, DataList datalistname, int pagesize)
        {
            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = ds.Tables[0].DefaultView;
            objPds.AllowPaging = true;
            int total = ds.Tables[0].Rows.Count;
            objPds.PageSize = pagesize;
            int page;
            if (HttpContext.Current.Request.QueryString["page"] != null)
                page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
            else
                page = 1;
            objPds.CurrentPageIndex = page - 1;
            datalistname.DataSource = objPds;
            datalistname.DataBind();
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (page < 1) { page = 1; }
            //计算总页数
            if (pagesize != 0)
            {
                allpage = (total / pagesize);
                allpage = ((total % pagesize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; } //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "";
            pagestr += page > 1 ? "<a href=\"" + GetCurrentQueryParams() + "&page=1\">1</a>&nbsp;&nbsp;<a href=\"" + GetCurrentQueryParams() + "&page=" + pre + "\"><</a>" : "<span class='disabled'> << </span> <span class='disabled'> < </span>";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "&nbsp;&nbsp;" + i + "" : "&nbsp;&nbsp;<a href=\"" + GetCurrentQueryParams() + "&page=" + i + "\">" + i + "</a>";
            }
            pagestr += page != allpage ? "&nbsp;&nbsp;<a href=\"" + GetCurrentQueryParams() + "&page=" + next + "\">></a>&nbsp;&nbsp;<a href=\"" + GetCurrentQueryParams() + "&page=" + allpage + "\">>></a>&nbsp;" : " <span class='disabled'> > </span> <span class='disabled'> >> </span>&nbsp;";
            return pagestr;
        }

        public static string GetTextPageRawUrlNum(DataSet ds, DataList datalistname, int pagesize)
        {
            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = ds.Tables[0].DefaultView;
            objPds.AllowPaging = true;
            int total = ds.Tables[0].Rows.Count;
            objPds.PageSize = pagesize;
            int page;
            if (HttpContext.Current.Request.QueryString["page"] != null)
                page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
            else
                page = 1;
            objPds.CurrentPageIndex = page - 1;
            datalistname.DataSource = objPds;
            datalistname.DataBind();
            int allpage = 0;
            int next = 0;
            int pre = 0;
            int startcount = 0;
            int endcount = 0;
            string pagestr = "";
            if (page < 1) { page = 1; }
            //计算总页数
            if (pagesize != 0)
            {
                allpage = (total / pagesize);
                allpage = ((total % pagesize) != 0 ? allpage + 1 : allpage);
                allpage = (allpage == 0 ? 1 : allpage);
            }
            next = page + 1;
            pre = page - 1;
            startcount = (page + 5) > allpage ? allpage - 9 : page - 4;//中间页起始序号
            //中间页终止序号
            endcount = page < 5 ? 10 : page + 5;
            if (startcount < 1) { startcount = 1; } //为了避免输出的时候产生负数，设置如果小于1就从序号1开始
            if (allpage < endcount) { endcount = allpage; } //页码+5的可能性就会产生最终输出序号大于总页码，那么就要将其控制在页码数之内
            pagestr = "";
            pagestr += page > 1 ? "<a href=\"" + GetCurrentQueryParams() + "&page=1\" class='first'>&nbsp;</a>&nbsp;&nbsp;" : "<span class='disabled'> <a href='' onclick='return false;' class='first'>&nbsp;</a> </span> ";
            //中间页处理，这个增加时间复杂度，减小空间复杂度
            if (startcount > 1)
            {
                pagestr += "...";
            }
            for (int i = startcount; i <= endcount; i++)
            {
                pagestr += page == i ? "&nbsp;&nbsp;<a class='current'>" + i + "</a>" : "&nbsp;&nbsp;<a href=\"" + GetCurrentQueryParams() + "&page=" + i + "\" >" + i + "</a>";
            }
            if (allpage > endcount)
            {
                pagestr += " ...";
            }
            pagestr += page != allpage ? "&nbsp;&nbsp;<a href=\"" + GetCurrentQueryParams() + "&page=" + next + "\" class='next'>下一页&nbsp;&nbsp;&nbsp;</a>&nbsp;&nbsp;" : "  <span class='disabled'>&nbsp; <a href='' onclick='return false;' class='next'>下一页&nbsp;&nbsp;&nbsp;</a> </span>&nbsp;";
            pagestr += " 共 <font color='red'> " + allpage.ToString() +  "</font> 页";
            return pagestr;
        }       
    }
}
