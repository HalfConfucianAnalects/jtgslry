using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace JtgTMS.WarmingSalary
{
    /// <summary>
    /// 公共类合并Grv单元格
    /// </summary>
    public class OperateGridView
    {
        #region 合并行(相同值)-普通列
        /// <summary>
        /// 合并行(普通列)
        /// </summary>
        /// <param name=“gv”>所对应的GridView对象</param>
        /// <param name=“columnIndex”>所对应要合并的列的索引</param>
        public static void UnitRow(GridView gv, int columnIndex)
        {
            int i;
            string lastType;
            int lastCell;
            if (gv.Rows.Count > 0)
            {
                lastType = gv.Rows[0].Cells[columnIndex].Text;
                gv.Rows[0].Cells[columnIndex].RowSpan = 1;
                lastCell = 0;
                for (i = 1; i < gv.Rows.Count; i++)
                {
                    if (gv.Rows[i].Cells[columnIndex].Text == lastType)
                    {
                        gv.Rows[i].Cells[columnIndex].Visible = false;
                        gv.Rows[lastCell].Cells[columnIndex].RowSpan++;
                    }
                    else
                    {
                        lastType = gv.Rows[i].Cells[columnIndex].Text;
                        lastCell = i;
                        gv.Rows[i].Cells[columnIndex].RowSpan = 1;
                    }
                }
            }
        }
        #endregion
        #region 合并行(相同值)-模板列
        /// <summary>
        /// 合并行(模板列)
        /// </summary>
        /// <param name=“gv”>所对应的GridView对象</param>
        /// <param name=“columnIndex”>所对应要合并的列的索引</param>   
        /// <param name=“lblName”>模板列里面Lable的Id</param>
        public static void UnitRow(GridView gv, int columnIndex, string lblName)
        {
            int i;
            string lastType;
            int lastCell;
            if (gv.Rows.Count > 0)
            {
                lastType = (gv.Rows[0].Cells[columnIndex].FindControl(lblName) as Label).Text;
                gv.Rows[0].Cells[columnIndex].RowSpan = 1;
                lastCell = 0;
                for (i = 1; i < gv.Rows.Count; i++)
                {
                    if ((gv.Rows[i].Cells[columnIndex].FindControl(lblName) as Label).Text == lastType)
                    {
                        gv.Rows[i].Cells[columnIndex].Visible = false;
                        gv.Rows[lastCell].Cells[columnIndex].RowSpan++;
                    }
                    else
                    {
                        lastType = (gv.Rows[i].Cells[columnIndex].FindControl(lblName) as Label).Text;
                        lastCell = i;
                        gv.Rows[i].Cells[columnIndex].RowSpan = 1;
                    }
                }
            }
        }
        #endregion 

        #region 合并列(相同值)-普通列
        /// <summary>
        /// 合并列(普通列)
        /// </summary>
        /// <param name="gv">所对应的GridView对象</param>
        /// <param name="columnIndex">所对应要合并的列的索引</param>
        public static void UnitCell(GridView gv, int columnIndex)
        {
            int i;
            string lastType;
            if (gv.Rows.Count > 0)
            {
                for (i = 1; i < gv.Rows.Count; i++)
                {
                    lastType = gv.Rows[i].Cells[columnIndex].Text;
                    gv.Rows[i].Cells[columnIndex].ColumnSpan = 1;
                    if (gv.Rows[i].Cells[columnIndex + 1].Text == lastType)
                    {
                        gv.Rows[i].Cells[columnIndex + 1].Visible = false;
                        gv.Rows[i].Cells[columnIndex].ColumnSpan++;
                    }
                    else
                    {
                        lastType = gv.Rows[i].Cells[columnIndex].Text;
                        gv.Rows[i].Cells[columnIndex].ColumnSpan = 1;
                    }
                }
            }
        }
        #endregion
        #region 合并列(相同值)-模板列
        /// <summary>
        /// 合并行(模板列)
        /// </summary>
        /// <param name=“gv”>所对应的GridView对象</param>
        /// <param name=“columnIndex”>所对应要合并的列的索引</param>   
        /// <param name=“lblName1”>模板列里面Lable1的Id</param>
        /// /// <param name=“lblName1”>模板列里面Lable2的Id</param>
        public static void UnitCell(GridView gv, int columnIndex, string lblName1, string lblName2)
        {
            int i;
            string lastType;
            if (gv.Rows.Count > 0)
            {
                for (i = 0; i < gv.Rows.Count; i++)
                {
                    lastType = (gv.Rows[i].Cells[columnIndex].FindControl(lblName1) as Label).Text;
                    gv.Rows[i].Cells[columnIndex].ColumnSpan = 1;
                    if ((gv.Rows[i].Cells[columnIndex + 1].FindControl(lblName2) as Label).Text == lastType)
                    {
                        gv.Rows[i].Cells[columnIndex + 1].Visible = false;
                        gv.Rows[i].Cells[columnIndex].ColumnSpan++;
                    }
                    else
                    {
                        lastType = (gv.Rows[i].Cells[columnIndex].FindControl(lblName1) as Label).Text;
                        gv.Rows[i].Cells[columnIndex].ColumnSpan = 1;
                    }
                }
            }
        }
        #endregion
    }
}
