using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace 功率能耗计算模块
{
    public partial class MyWindows : Form
    {
        private Thread myThread;//遗传算法对应线程

        public MyWindows()
        {
            InitializeComponent();
        }
        public MyWindows(string temp1,string temp2,string temp3)
        {
            InitializeComponent();
            t1 = temp1;
            t2 = temp2;
            M1 = temp3;
        }
        string t1,t2,M1;
       
        /************************************************
        * 参数：分别表示当前进度与总进度
        * 返回值：无
        * 功能：完成进程框的设定
        * ***********************************************/
        public delegate void setProgressInvoke(int num, int MAXGENS);
        public void setProgressBar(int num, int MAXGENS)
        {
            if (this.InvokeRequired)
            {
                setProgressInvoke _setInvoke = new setProgressInvoke(setProgressBar);
                this.Invoke(_setInvoke, new object[] { num, MAXGENS });
            }
            else
            {
                label13.Text = Math.Round((100.0 * num / MAXGENS), 3).ToString("f1") + "%";
                progressBar1.Value = (int)((int)((1.0 * num / MAXGENS) * 100) % (100 + 1));
            }
        }

        /************************************************
      * 参数：分别表示文本框待显示的值与对应文本框归属
      * 返回值：无
      * 功能：完成6个文本框的设定
      * ***********************************************/
        public delegate void setListInvoke(double val, int flag);
        public void setList(double val, int flag)
        {
            if (this.InvokeRequired)
            {
                setListInvoke _setInvoke = new setListInvoke(setList);
                this.Invoke(_setInvoke, new object[] { val, flag });
            }
            else
            {
                switch (flag)
                {
                    case 1:
                        textBox1.Text = val.ToString();
                        break;
                    case 2:
                        textBox2.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 3:
                        textBox3.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 4:
                        textBox4.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 5:
                        textBox5.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 6:
                        textBox6.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 7:
                        textBox11.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 8:
                        textBox12.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 9:
                        textBox13.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    case 10:
                        textBox14.Text = Math.Round(val, 8).ToString("f8");
                        break;
                    default:
                        break;
                }
            }
        }


        /************************************************
        *“开始”按钮
        * ***********************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            //组合框为空，未选择数据集，重新选择
            if (comboBox1.Text == "")
            {
                MessageBox.Show("请选择约束条件数据集!");
                return;
            }

            //当前线程未结束，直接结束该线程
            if (null != myThread)
                myThread.Abort();
            Population myPopulation = new Population(Convert.ToInt32(textBox7.Text),
                Convert.ToInt32(textBox8.Text), Convert.ToDouble(textBox10.Text),
                Convert.ToDouble(textBox9.Text), comboBox1.Text, this);
            myThread = new Thread(myPopulation.GeneticAlgorithm);
            myThread.Start();
        }

        /************************************************
        *“暂停”按钮
        * ***********************************************/
        private void button2_Click(object sender, EventArgs e)
        {
            if (null == myThread)
            {
                MessageBox.Show("请确认程序正在运行!");
                return;
            }
            myThread.Suspend();
        }

        /************************************************
         *“退出”按钮
         * ***********************************************/
        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你按了退出键");

            //当前线程未结束，直接结束该线程
            if (null != myThread)
                myThread.Abort();
            Application.Exit();
        }

        /************************************************
        *“继续”按钮
        * ***********************************************/
        private void button4_Click(object sender, EventArgs e)
        {
            //保证当前线程处在挂起状态
            if (ThreadState.Suspended == myThread.ThreadState)
                myThread.Resume();
            else
            {
                MessageBox.Show("请确认当前程序处于暂停状态!");
            }
        }

        /************************************************
       *初始化窗口
       * ***********************************************/
        private void MyWindows_Load(object sender, EventArgs e)
        {
            this.textBox7.Text = "100";
            this.textBox8.Text = "1000";
            this.textBox9.Text = "0.15";
            this.textBox10.Text = "0.80";
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 F3 = new Form1(t1,t2,M1);
            F3.Show();
            F3.textBox1.Text = this.textBox11.Text;
            F3.textBox2.Text = this.textBox12.Text;
            F3.textBox3.Text = this.textBox13.Text;
            F3.textBox4.Text = this.textBox14.Text;
            double MRR2 = Convert.ToDouble(textBox12.Text)*Convert.ToDouble(textBox13.Text)* Convert.ToDouble(textBox14.Text)/60;
            double MRR1 = Convert.ToDouble(M1);
            double time1 = Convert.ToDouble(t1);
            double time2 = Convert.ToDouble(t2);
            double newt1 = time1 *( MRR1 / MRR2);
            double newt2 = newt1 + (time2-time1);
            F3.textBox15.Text = Convert.ToString(newt1);
            F3.textBox16.Text = Convert.ToString(newt2); 

        }
    }
}