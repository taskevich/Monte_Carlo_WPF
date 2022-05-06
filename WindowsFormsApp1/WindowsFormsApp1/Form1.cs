using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void monte_carlo(int num)
        {
            Random r = new Random();
            float cnt = 0;
            for (int i = 0; i < num; i++)
            {
                float dx = (float)r.Next(0, 100) / 100.0f;
                float dy = (float)r.Next(0, 100) / 100.0f;

                if (dx * dx + dy * dy < 1)
                    ++cnt;
            }


            float pi = (4.0f * ((float)cnt / (float)num));

            if (checkBox1.Checked || checkBox2.Checked)
                this.textBox1.BeginInvoke((MethodInvoker)delegate
                {
                    textBox2.Text = pi.ToString(CultureInfo.InvariantCulture);
                });
            else if (!checkBox1.Checked && !checkBox2.Checked)
                textBox2.Text = pi.ToString(CultureInfo.InvariantCulture);
        }

        void draw()
        {
            SolidBrush blueBrush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black, 2);
            Graphics gPanel = panel1.CreateGraphics();


            Point p1 = new Point(panel1.Width / 2, 0);
            Point p2 = new Point(panel1.Width / 2, panel1.Height);
            gPanel.DrawLine(pen, p1, p2);

            Point p3 = new Point(0, panel1.Height / 2);
            Point p4 = new Point(panel1.Width, panel1.Height / 2);
            gPanel.DrawLine(pen, p3, p4);

            gPanel.DrawEllipse(pen, panel1.Height / 25, panel1.Width / 25, 550, 550);
        }

        private void log_file(string line)
        {
            var fstream = new FileStream("log.txt", FileMode.Append);
            try
            {
                string formatted = line;
                byte[] buffer = Encoding.Default.GetBytes(formatted);
                fstream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fstream.Close();
            }
        }

        private void get_stats()
        {
            try
            {
                var stat = (File.ReadLines("log.txt")).ToList();
                foreach (var line in stat)
                    textBox3.Text += line + "\r" + "\n";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void cls()
        {
            Graphics gPanel = panel1.CreateGraphics();
            gPanel.Clear(Color.White);
            draw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            draw();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            SolidBrush blueBrush = new SolidBrush(Color.Black);
            Graphics gPanel = panel1.CreateGraphics();
            GraphicsState transState = gPanel.Save();
            Random r = new Random();
            Stopwatch sw = Stopwatch.StartNew();

            string threading_time = "";
            string tasks_time = "";
            string default_time = "";

            int num = 0;


            if (!int.TryParse(textBox1.Text, out int n))
            {
                MessageBox.Show("Нужно ввести целочисленное число!");
            }
            else
            {
                try
                {
                    num = int.Parse(textBox1.Text);

                    if (checkBox1.Checked && !checkBox2.Checked)
                    {
                        sw.Start();
                        Thread t = new Thread(() => monte_carlo(num));
                        t.Start();
                        cls();
                    }
                    else if (checkBox2.Checked && !checkBox1.Checked)
                    {
                        sw.Start();
                        Task t = new Task(() => monte_carlo(num));
                        t.Start();
                        cls();
                    }
                    else if (!checkBox1.Checked && !checkBox2.Checked)
                    {
                        sw.Start();
                        monte_carlo(num);
                        cls();
                    }
                    else if (checkBox1.Checked && checkBox2.Checked)
                    {
                        MessageBox.Show("Нельзя использовать задачу и поток одновременно");
                        num = 0;
                        cls();
                    }

                    for (int i = 0; i < num; ++i)
                    {
                        int rnd = r.Next(100, 500);


                        int x1 = r.Next(600);
                        int x2 = r.Next(600);

                        Rectangle rect = new Rectangle(x2, x1, 5, 5);
                        gPanel.FillRectangle(blueBrush, rect);
                    }
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    if (checkBox1.Checked)
                    {
                        threading_time = "Threading Time " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + "\n";
                        log_file(threading_time);
                    }
                    else if (checkBox2.Checked)
                    {
                        tasks_time = "Tasks Time " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + "\n";
                        log_file(tasks_time);
                    }
                    else 
                    {
                        default_time = "Default Time " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10) + "\n";
                        log_file(default_time);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Нужно обязательно ввести количество точек");
                }
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            cls();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            get_stats();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string caption = "Information";
            string message = "Программист - Однобурцев\t\n" +
                             "Документатор - Горбунов\t\t";
            MessageBoxButtons button = MessageBoxButtons.OK;
            DialogResult result;

            result = MessageBox.Show(this, message, caption, button,
            MessageBoxIcon.None, MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RightAlign);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
