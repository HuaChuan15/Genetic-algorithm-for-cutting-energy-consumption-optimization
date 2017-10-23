using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 功率能耗计算模块
{
   
    public partial class Form3 : Form
    {
                    

        public Form3(Form1 temp)
        {
             
            double a = temp.EBASIC;
            double b = temp.EFEED;
            double c = temp.ECUT;
           
            InitializeComponent();
            double[] value = { a, b, c };    //要显示的数据
            showChart(this.chart1, value);   //显示图表
        }
            


        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void showChart(Chart chart, double[] value)
        {

            string[] xValue = { "基本能耗", "进给能耗", "切削能耗" };     //设置标签
            double[] yValue = value;    //获取要显示的值

            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;    //设置图表边框为浮雕效果
            chart.BorderlineDashStyle = ChartDashStyle.Solid;    //设置图表边框为实线
            chart.BorderlineWidth = 1;    //设置图表边框的宽度

            chart.Series[0].ChartType = SeriesChartType.Pie;    //设置图表类型为饼图
            chart.Series[0].CustomProperties = "DoughnutRadius=30, PieDrawingStyle=Concave, PieLabelStyle=Enabled,CollectedLabel=Other, MinimumRelative" + "PieSize=30";    //设置饼图的参数

            chart.Series[0].Label = "#VALX " + "#VAL{0.00}" + "J" ;    //标签加值
            chart.Series[0].LegendText = "#VALX " + "#PERCENT{P2} " + "#VAL{0.00}" + "J"; //标签加百分比
            chart.Series[0]["PieLabelStyle"] = "Inside";
            chart.Series[0].Points.DataBindXY(xValue, yValue);    //将数据绑定到图表

        }
    }
}
