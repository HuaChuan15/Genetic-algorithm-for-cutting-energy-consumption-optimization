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

namespace 功率能耗计算模块
{
    public partial class Form2 : Form
    {
        public static Form2 S = null;
        public Form2()
        {
            InitializeComponent();
            S = this;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            FileStream Q = new FileStream("C:\\Users\\hcgg\\Desktop\\切一刀的加工信息.txt", FileMode.Open);
            StreamReader q = new StreamReader(Q);
            textBox1.Text = q.ReadToEnd();
            Q.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
