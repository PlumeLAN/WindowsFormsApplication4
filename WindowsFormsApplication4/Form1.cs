using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr;
            string line;
            List<string> sz = new List<string>();
            sr = new StreamReader(@"G:\水切割3D1.nc", Encoding.GetEncoding("UTF-8"));
            while ((line = sr.ReadLine()) != null)
            {
                sz.Add(line);
            }
            sr.Dispose();

            for (int i = 0; i < sz.Count; i++)
            {
                if (sz[i].Contains("G02") || sz[i].Contains("G03"))
                {
                    string SI = (sz[i].Split('I')[1]).Split('J')[0];
                    double I = Double.Parse(SI);
                    double J = 0, Y = 0;

                    if (sz[i].Contains("F"))
                    {
                        string SJ = (sz[i].Split('J')[1]).Split('F')[0];
                        J = Double.Parse(SJ);
                    }
                    else
                    {
                        string SJ = (sz[i].Split('J')[1]);
                        J = Double.Parse(SJ);
                    }

                    string SX = (sz[i-1].Split('X')[1]).Split('Y')[0];
                    double X = Double.Parse(SX);

                    if (sz[i-1].Contains("G01") || sz[i - 1].Contains("G00"))
                    {
                        if (sz[i - 1].Contains("F"))
                        {
                            string SY = (sz[i - 1].Split('Y')[1]).Split('F')[0];
                            Y = Double.Parse(SY);
                        }
                        else
                        {
                            string SY = (sz[i - 1].Split('Y')[1]);
                            Y = Double.Parse(SY);
                        }
                    }
                    else if (sz[i - 1].Contains("G02") || sz[i - 1].Contains("G03"))
                    {
                        string SY = (sz[i - 1].Split('Y')[1]).Split('I')[0];
                        Y = Double.Parse(SY);
                    }


                    double NI = X + I;
                    double NJ = Y + J;

                    if (sz[i].Contains("F"))
                    {
                        sz[i] = sz[i].Split('I')[0] + "I" + NI + " J" + NJ + " F" + sz[i].Split('F')[1];
                    }
                    else
                    {
                        sz[i] = sz[i].Split('I')[0] + "I" + NI + " J" + NJ;
                    }

                    //MessageBox.Show(sz[i]);
                }
            }

            FileStream fs1 = new FileStream(@"G:\3DECS.txt", FileMode.Create);
            StreamWriter sw1 = new StreamWriter(fs1);
            for (int i = 0; i < sz.Count; i++)
            {
                sw1.WriteLine(sz[i]);
            }
            //清空缓冲区
            sw1.Flush();
            //关闭流
            sw1.Close();
            fs1.Close();



            //MessageBox.Show(sz[0]);
        }
    }
}
