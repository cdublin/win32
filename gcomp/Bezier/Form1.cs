using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bezier
{
    public partial class Form1 : Form
    {
        private List<double> ptList = new List<double>();
        private BezierCurve bc = new BezierCurve();

        public Form1()
        {
            InitializeComponent();
            //TODO: functie separata pt tooltip apelabila in pictureBox1_Mouse[...]
            // &cazuri separate
            
            ToolTip tp = new ToolTip();
            tp.AutoPopDelay = 2000;
            tp.SetToolTip(pictureBox1, "click for new points then push 'FIT BEZIER' \n (32 max)");

        }

        //desenare puncte

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            ptList.Add(e.X);
            ptList.Add(e.Y);

            g.DrawRectangle(px, new Rectangle(e.X, e.Y, 2, 2));
            
        }

        //afisare coordonate in status la mouse hover

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {

            //p = cordonatele actuale ale pozitiei mouse-ului in picturebox 
            Point p = pictureBox1.PointToClient(Cursor.Position);
            // status label updatat sa afiseze x si y
            toolStripStatusLabel2.Text = "X = " + p.X.ToString() + " Y = " + p.Y.ToString();
            // refresh pentru a nu ramane blocat pe coordonatele precedente
            statusStrip1.Refresh();
        }

        //afisare coordonate in status la mouse move

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = pictureBox1.PointToClient(Cursor.Position);
            toolStripStatusLabel2.Text = "X = " + p.X.ToString() + " Y = " + p.Y.ToString();
            statusStrip1.Refresh();
            
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
            statusStrip1.Refresh();
        }



        Pen px = new Pen(Brushes.Red);
        Pen newpx = new Pen(Brushes.Black);
        Graphics g ;
        
               
        private void button1_Click(object sender, EventArgs e)
        {
            
            // how many points do you need on the curve?
            const int POINTS_ON_CURVE = 1000;

            double[] ptind = new double[ptList.Count];
            double[] p = new double[POINTS_ON_CURVE];
            ptList.CopyTo(ptind, 0);
            bc.Bezier2D(ptind, (POINTS_ON_CURVE) / 2, p);
         
            // draw points
            

            for (int i = 1; i != POINTS_ON_CURVE-1; i += 2)
            {
                g.DrawRectangle(newpx, new Rectangle((int)p[i + 1], (int)p[i], 1, 1));
                Application.DoEvents();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = Graphics.FromHwnd(pictureBox1.Handle);
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {


        }

        private void button3_Click(object sender, EventArgs e)
        {

            g.Clear(Color.Azure);  
            
            //TO DO: reset form. probabil un g.Flush() aici si/sau o stergere totala a listei cu puncte.

            ptList.Clear();

            //DONE.
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //form separat aici
            MessageBox.Show("[TO DO: input] - cu un form separat");
        }

        
        //TODO: (in paint sau nu) antialiasing ceva gen        
/*
            //Invalidate();
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        }
 */

        //TODO: grid initial:
        //private void pictureBox1_Paint(object sender, PaintEventArgs e)
        //{
        //    Pen p = new Pen(Color.Gray);
        //    for (int y = 0; y < pictureBox1.Size.Height; ++y)
        //    {
        //        g.DrawLine(p, 0, y *5, pictureBox1.Size.Width *5, y *5);
        //    }

        //    for (int x = 0; x < pictureBox1.Size.Width; ++x)
        //    {
        //        g.DrawLine(p, x *5, 0, x *5, pictureBox1.Size.Height *5);
        //    }
        //}

        /* daca pictureBox1_Paint e comentata aici atunci trebuie
         comentata si in Form1.Designer.cs la linia 57 
         altfel designerul general da eroare.
        */

       
    }
}