using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace 功率能耗计算模块
{
    public partial class Form1 : Form
    {
        public double EBASIC;
        public double EFEED;
        public double ECUT;

        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string tmp1,string tmp2,string tmp3)
        {
            InitializeComponent();
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //设置打开文件的格式
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox14.Text = string.Empty;
                //使用“打开”对话框中选择的文件实例化StreamReader对象
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                //调用ReadToEnd方法读取选中文件的全部内容
                textBox14.Text = sr.ReadToEnd();
                //关闭当前文件读取流
                sr.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox14.Text == string.Empty)
            {
                MessageBox.Show("要写入的G代码不能为空");
            }
            else
            {
                //设置保存文件的格式
                saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //使用“另存为”对话框中输入的文件名实例化StreamWriter对象
                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, true);
                    //向创建的文件中写入内容
                    sw.WriteLine(textBox14.Text);
                    //关闭当前文件写入流
                    sw.Close();
                    textBox14.Text = string.Empty;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty || textBox4.Text == string.Empty)
            {
                MessageBox.Show("请先读取读取或填入基本参数");
            }
            else
            {
                string a = textBox1.Text;
                string b = textBox2.Text;
                string c = textBox3.Text;
                string d = textBox4.Text;

                double n = Convert.ToDouble(a) / 60;
                double f = Convert.ToDouble(b) / 60;
                double ap = Convert.ToDouble(c);
                double ae = Convert.ToDouble(d);

                double r = f * ap * ae;
                string R = Convert.ToString(r);
                textBox5.Text = R;

                double g = 460 + 8.18 * n;
                string Pbasic = Convert.ToString(g);
                textBox6.Text = Pbasic;

                double h = 461.807 + 7.3524 * n + 4.8723 * f;
                string Pair = Convert.ToString(h);
                textBox7.Text = Pair;

                double i = 478.31 + 7.9209 * n + 4.8557 * r;
                string Pm = Convert.ToString(i);
                textBox8.Text = Pm;

                double l = 5.1175 + 7.7875 * n / r + 478.797 / r;
                string SEC = Convert.ToString(l);
                textBox11.Text = SEC;              

                double cutting_time = Convert.ToDouble(textBox15.Text) / 1000 / 60 * 3.6 * 1000000;
                double machine_time = Convert.ToDouble(textBox16.Text) / 1000 / 60 * 3.6 * 1000000;
                double ebasic = machine_time * g;
                string Ebasic = Convert.ToString(ebasic);
                textBox9.Text = Ebasic;
                double eair = (machine_time - cutting_time) * h;
                string Eair = Convert.ToString(eair);
                textBox10.Text = Eair;
                double ecut = cutting_time * i;
                string Ecut = Convert.ToString(ecut);
                textBox12.Text = Ecut;
                double etotle = eair + ecut;
                string Etotle = Convert.ToString(etotle);
                textBox13.Text = Etotle;

                double Pcut = i - h;
                double Pfeed = h - g;
                ECUT = Pcut * cutting_time;
                EFEED = Pfeed * machine_time;
                EBASIC = ebasic;

                Form3 f3 = new Form3(this);
                f3.Show();

            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {   //使得textBox1只能填入数字
            if ((!Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox14.Text == string.Empty)
            {
                MessageBox.Show("请先读取数控代码");
            }
            else
            {

                string content = textBox14.Text;
                Regex a = new Regex(@"Tool Diameter\s*:\s*([\d\.]+)\s*(mm)");
                Match match1 = a.Match(content);
                string ToolDiameter = match1.Groups[1].Value;
                textBox4.Text = ToolDiameter;

                Regex b = new Regex(@"Spindle Speed\s*:\s*([\d\.]+)\s*(r/min)");
                Match match2 = b.Match(content);
                string SpindleSpeed = match2.Groups[1].Value;
                textBox1.Text = SpindleSpeed;

                Regex c = new Regex(@"Feed Cut Value\s*:\s*([\d\.]+)\s*(mm/min)");
                Match match3 = c.Match(content);
                string FeedCutValue = match3.Groups[1].Value;
                textBox2.Text = FeedCutValue;

                Regex d = new Regex(@"Depth Per Cut\s*:\s*([\d\.]+)\s*(mm)");
                Match match4 = d.Match(content);
                string DepthPerCut = match4.Groups[1].Value;
                textBox3.Text = DepthPerCut;

                Regex time1 = new Regex(@"Cutting Time\s*:\s*([\d\.]+)\s*(min)");
                Match match5 = time1.Match(content);
                string CuttingTime = match5.Groups[1].Value;
                Regex time2 = new Regex(@"Total Machine Time\s*:\s*([\d\.]+)\s*(min)");
                Match match6 = time2.Match(content);
                string TotalMachineTime = match6.Groups[1].Value;
                textBox15.Text = CuttingTime;
                textBox16.Text = TotalMachineTime;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult TS = MessageBox.Show("退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (TS == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2();
            F2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog2.Filter = "文本文件(*.txt)|*.txt";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //使用“另存为”对话框中输入的文件名实例化StreamWriter对象
                StreamWriter st = new StreamWriter(saveFileDialog2.FileName, true);
                //向创建的文件中写入内容
                st.WriteLine(textBox14.Text+"\r\n分析结果："+ "\r\n基本参数\r\n主轴转速n(r/min)： "+textBox1.Text+ "\r\n进给率f(mm/min)： "+textBox2.Text+ "\r\n切削深度ap(mm)： "+ textBox3.Text + "\r\n切削宽度ae(mm)： "+ textBox4.Text+ "\r\n材料去除率MRR(mm3/s)： "+ textBox5.Text+ "\r\n功率计算\r\n基本功率Pbasic(w)： "+ textBox6.Text+ "\r\n空进给功率Pair(w)： "+ textBox7.Text+ "\r\n材料去除总功率Pm(w)： "+ textBox8.Text+ "\r\n能耗计算\r\n基本能耗Ebasic(J)： "+ textBox9.Text+ "\r\n空进给总能耗Eair(J)： "+ textBox10.Text+ "\r\n单位切削能耗SEC(J/mm3)： "+ textBox11.Text + "\r\n材料去除总能耗Em(J)： "+ textBox12.Text+ "\r\n加工过程总能耗E(J)： "+textBox13.Text+"\r\n占比分析\r\n总基本能耗(J)： "+ textBox9.Text+"\r\n用于进给的能耗(J)： "+EFEED+"\r\n用于材料去除的能耗(J)： "+ECUT);
                //关闭当前文件写入流
                st.Close();
                
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MyWindows F4 = new MyWindows(textBox15.Text, textBox16.Text, textBox5.Text);
            F4.Show();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }
        
    }
    }

