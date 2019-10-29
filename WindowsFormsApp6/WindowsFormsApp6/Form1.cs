using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        Random rnd;
        static object objLock = new object();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Image im1 = Image.FromFile("car2.png");
            Size s1 = new Size(im1.Size.Width / 4, im1.Size.Height / 4);
            Bitmap b1 = new Bitmap(im1, s1);
            pictureBox1.Image = b1;
            Image im2 = Image.FromFile("car1.png");
            Size s2 = new Size(im2.Size.Width / 6, im2.Size.Height / 6);
            Bitmap b2 = new Bitmap(im2, s2);
            pictureBox2.Image = b2;
            InitialiseCar(pictureBox1, 1, true);
            InitialiseCar(pictureBox2, 2, true);

            Bitmap bmp = new Bitmap(pictureBox3.ClientSize.Width, pictureBox3.ClientSize.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawLine(Pens.Red, new Point(1, 0), new Point(1, pictureBox3.Height));
            g.DrawLine(Pens.Red, new Point(pictureBox3.Width - 1, 0), new Point(pictureBox3.Width - 1, pictureBox3.Height));
            pictureBox3.Image = bmp;

            rnd = new Random();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(() => CarGo(pictureBox1, pictureBox3, objLock, 1, rnd)));
            thread1.IsBackground = true;
            thread1.Start();

            Thread thread2 = new Thread(new ThreadStart(() => CarGo(pictureBox2, pictureBox3, objLock, 2, rnd)));
            thread2.IsBackground = true;
            thread2.Start();
        }

        private void CarGo(Control control, Control Bridge, object Lock, int moveDirection, Random random)
        {
            while (moveDirection == 1 ? (control.Location.X + control.Width < pictureBox3.Location.X) : (control.Location.X > pictureBox3.Location.X + pictureBox3.Width))
            {
                InitialiseCar((moveDirection == 1) ? pictureBox1 : pictureBox2, moveDirection, false);
                Thread.Sleep(RandomGenerator.Next(5, 20));
            }
            lock (objLock)
            {
                while (moveDirection == 1 ? (control.Location.X < pictureBox3.Location.X + pictureBox3.Width) : (control.Location.X + control.Width > pictureBox3.Location.X))
                {
                    InitialiseCar((moveDirection == 1) ? pictureBox1 : pictureBox2, moveDirection, false);
                    Thread.Sleep(RandomGenerator.Next(5, 20));
                }               
            }
        }

        private void InitialiseCar(Control control, int moveDirection, bool isInitialise)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action<Control, int, bool>(InitialiseCar), control, moveDirection, isInitialise);
            }
            else
            {
                if (isInitialise)
                {
                    control.Left = (moveDirection == 1) ? (-control.Width + 1) : (this.ClientSize.Width - 1);
                }
                else
                    control.Left += (moveDirection == 1 ? 1 : -1);
            }
        }
    }

    public static class RandomGenerator
    {
        static Random rnd = new Random();
        static object lockObj = new object();
        public static int Next(int min, int max)
        {
            int x = 0;
            lock (lockObj)
            {
                x = rnd.Next(min, max);
            }
            return x;
        }
    }
}