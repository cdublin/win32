using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using System.Linq;
namespace Bezier

 //TODO: 
//      -tratare exceptii/cazuri limita la toate componentele formului + tooltip-uri
//      -reparat reset(), undeva prin grahamscan.cs ramane ceva initializat dupa stergerea listelor cu .clear()
 //       (-numerotare triunghiuri, eventual cu I, II, II etc - optional)

{
    partial class Form1 : Form
    {
        //private List<double> ptList = new List<double>();
        public static List<double> ptList = new List<double>();

        public static Form3 myForm = null;  //form-ul pentru "manual input"
        private BezierCurve bc = new BezierCurve();

        char label = 'A';  //eticheta de start a primului punct 
        bool letter = true;    //in cazul in care s-au epuizat literele A-Z
        int numplabel = 1; //etichetarea incepe de la 1
        int numpoints = 0; //cate puncte au fost introduse 

        GrahamScan gs = new GrahamScan();
        List<PointG> listPointsG = new List<PointG>();
        int triang = 0;

        Bitmap DrawArea;

        Pen px = new Pen(Brushes.Red);
        Pen newpx = new Pen(Brushes.Black);
        Graphics g;
        

        public Form1()
        {
            InitializeComponent();
            //TODO: functie separata pt tooltip apelabila in pictureBox1_Mouse[...]
            // &cazuri separate
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = DrawArea;
            ToolTip tp = new ToolTip();
            tp.AutoPopDelay = 2000;
            tp.SetToolTip(pictureBox1, "click for new points");
        }

        //desenare puncte
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            g = Graphics.FromImage(DrawArea); //incarca tot ce a fost desenat inainte

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            numpoints++;
            Font drawFont = new Font("Arial", 13);
            if (letter == false) //s-au epuizat A-Z
            {

                ptList.Add(e.X);
                ptList.Add(e.Y);
                g.DrawRectangle(px, new Rectangle(e.X, e.Y, 2, 2));
                //etichetare varfuri cu cifre dupa epuizarea literelor
                g.DrawString("P" + numplabel.ToString(), drawFont, Brushes.Black, e.X, e.Y);
                //eticheta se incrementeaza o data cu fiecare punct 

                //test populare listbox1, codul de aici va fi in form-ul initiat de button2_Click
                //TODO: adaugare paddright/left pentru afisare corecta (aliniere)
                listBox1.Items.Add("P" + numplabel.ToString() + numpoints + ":  x" + e.X + "  y" + (500 - e.Y));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;  //autoscroll
                listBox1.SelectedIndex = -1;

                numplabel++;
                //label++
                //TODO: modificat aici totusi sa arate mai decent, A1 in loc de 1 etc
            }
            else
            {
                ptList.Add(e.X);
                ptList.Add(e.Y);
                g.DrawRectangle(px, new Rectangle(e.X, e.Y, 2, 2));

                
                // to do: etichetare varfuri cu litere din alfabet consecutive 
                g.DrawString(label.ToString(), drawFont, Brushes.Black, e.X, e.Y);

                //test populare listbox1, codul de aici va fi in form-ul initiat de button2_Click
                //TODO: adaugare paddright/left pentru afisare corecta (aliniere)
                listBox1.Items.Add(label.ToString() + ":  x" + e.X + "  y" + (500 - e.Y));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;  //autoscroll
                listBox1.SelectedIndex = -1;

                label++; //eticheta se incrementeaza o data cu fiecare punct A,B,C..
                if (label > 'Z') letter = false; //s-au epuizat A-Z
            }


            pictureBox1.Image = DrawArea; //salveaza ce s-a desenat aici()

        }

        //form3_button1() probabil la fel ca pictureBox1_MouseClick()

        //afisare coordonate in status la mouse hover
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            //p = cordonatele actuale ale pozitiei mouse-ului in picturebox 
            Point p = pictureBox1.PointToClient(Cursor.Position);
            // status label updatat sa afiseze x si y
            toolStripStatusLabel2.Text = "x = " + p.X.ToString() + " y = " + (pictureBox1.Height - p.Y).ToString();
            //(pictureBox1.Height - p.Y) = trecerea din cadranul IV (specific gdi+) in cadranul I
            // refresh pentru a nu ramane blocat pe coordonatele precedente
            statusStrip1.Refresh();

        }

        //afisare coordonate in status la mouse move
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = pictureBox1.PointToClient(Cursor.Position);
            toolStripStatusLabel2.Text = "x = " + p.X.ToString() + " y = " + (pictureBox1.Height - p.Y).ToString();
            statusStrip1.Refresh();

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "";
            statusStrip1.Refresh();
        }

        //desenare Bezier
        private void button1_Click(object sender, EventArgs e)
        {
            // how many points do you need on the curve?
            //const 
            int POINTS_ON_CURVE = 1000;
            double[] p = new double[POINTS_ON_CURVE];
            double[] ptind = new double[ptList.Count];
            ptList.CopyTo(ptind, 0);
            bc.Bezier2D(ptind, (POINTS_ON_CURVE) / 2, p);
            g = Graphics.FromImage(DrawArea);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            /* culori diferite 
            //(optional) 
            //opuleaa un array cu toate culorile posibile din .net:
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor)); 
            Random randomGen = new Random();
            if (Convert.ToBoolean(checkBox2.Checked)) //daca este bifata "colors"
            {
                KnownColor randomColorName = names[randomGen.Next(names.Length)];
                Color randomColor = Color.FromKnownColor(randomColorName);
                newpx.Color = randomColor; 
                // ia o culoare la intamplare si i-o asigneaza Pen-ului newpx
            }
            else newpx.Color = System.Drawing.Color.Black;
            */


            //desenare efectiva punctele curbei Bezier
            for (int i = 1; i != POINTS_ON_CURVE - 1; i += 2)
            {
                //trebuie warning aici daca p e vid;
                g.DrawRectangle(newpx, new Rectangle((int)p[i + 1], (int)p[i], 1, 1));
                Application.DoEvents();
            }

            button1.Enabled = false;
            
            pictureBox1.Image = DrawArea;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //g = Graphics.FromHwnd(pictureBox1.Handle);
            g = Graphics.FromImage(DrawArea);
            //initializare g=handle/pointer la pictureBox1
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            //pictureBox1.InitialImage = null;

            //g = Graphics.FromImage(DrawArea);

            g = Graphics.FromHwnd(pictureBox1.Handle); //necesar pt reset

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //Reset
            //redesenam grid-ul si background-ul
            //in design pictureBox1 ramane fix pls altfel trebuiesc refacute in totalitate for-urile urmatoare:

            //necesare pentru etichetare puncte de control:
            label = 'A';
            numplabel = 1;
            letter = true;
            numpoints = 0;


            listBox1.Items.Clear();
            g.Clear(Color.Azure);

            //checkBox2.Checked = false;
            //checkBox1.Checked = false;
            //checkBox3.Checked = false;
            //debug: needpoligon = true;

            Pen p = new Pen(Color.DarkGray);
            for (int y = 0; y < pictureBox1.Size.Height; ++y)
            {
                // linii orizontale din 50 in 50 de pixeli pt un picturebox de 500x500
                int tmp = y * 50;//ajuta la schimbarea cadranului IV -> I sa nu ma complic cu paranteze
                //Y va deveni (inaltimea_picturebox-ului - Y)
                g.DrawLine(p, 0, y * 50, pictureBox1.Size.Width * 50, y * 50);
                // eticheta 
                g.DrawString((pictureBox1.Size.Height - tmp).ToString(), this.Font, Brushes.Black, 0, y * 50);
            }
            // centrul "0"
            g.DrawString("0", this.Font, Brushes.Black, 0, pictureBox1.Height - this.Font.Height);

            //linii verticale din 50 in 50 de pixeli
            for (int x = 0; x < pictureBox1.Size.Width; ++x)
            {
                g.DrawLine(p, x * 50, 0, x * 50, pictureBox1.Size.Height * 50);
                //(pictureBox1.Height - this.Font.Height) = afisare fix deasupra axei OX:
                if (x != 1) g.DrawString((x * 50).ToString(), this.Font, Brushes.Black, x * 50 - 21, pictureBox1.Height - this.Font.Height);
                else //hack pt afisare corecta a etichetei "50" pe OX (adica in momentul in care x=1)
                    g.DrawString((x * 50).ToString(), this.Font, Brushes.Black, x * 50 - 15, pictureBox1.Height - this.Font.Height);
            }
            //la reset lista cu punctele introduse x1,y1,x2,y2 etc devine vida:
            ptList.Clear();

            //reinitializare suport DrawArea sa salvam grid-ul (doar la reset)
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);

            pictureBox1.Image = DrawArea;
        }

        private void Reset()
        //clona dupa functia initiala de reset button3_Click 
        {
            /*Reset
            /redesenam grid-ul si background-ul
            /in design pictureBox1 ramane fix pls altfel trebuiesc refacute in totalitate for-urile urmatoare: 
            */

            label = 'A';
            g.Clear(Color.Azure);
            Pen p = new Pen(Color.DarkGray);
            for (int y = 0; y < pictureBox1.Size.Height; ++y)
            {
                // linii orizontale din 50 in 50 de pixeli pt un picturebox de 500x500
                int tmp = y * 50;//ajuta la schimbarea cadranului IV -> I sa nu ma complic cu paranteze
                //Y va deveni (inaltimea_picturebox-ului - Y)
                g.DrawLine(p, 0, y * 50, pictureBox1.Size.Width * 50, y * 50);
                // eticheta 
                g.DrawString((pictureBox1.Size.Height - tmp).ToString(), this.Font, Brushes.Black, 0, y * 50);
            }
            // centrul "0"
            g.DrawString("0", this.Font, Brushes.Black, 0, pictureBox1.Height - this.Font.Height);
            //linii verticale din 50 in 50 de pixeli
            for (int x = 0; x < pictureBox1.Size.Width; ++x)
            {
                g.DrawLine(p, x * 50, 0, x * 50, pictureBox1.Size.Height * 50);
                //(pictureBox1.Height - this.Font.Height) = afisare fix deasupra axei OX:
                if (x != 1) g.DrawString((x * 50).ToString(), this.Font, Brushes.Black, x * 50 - 21, pictureBox1.Height - this.Font.Height);
                else //hack pt afisare corecta a etichetei "50" pe OX (adica in momentul in care x=1)
                    g.DrawString((x * 50).ToString(), this.Font, Brushes.Black, x * 50 - 15, pictureBox1.Height - this.Font.Height);
            }
            //la reset lista cu punctele introduse x1,y1,x2,y2 etc devine vida:
            ptList.Clear();

            gs.result.Clear();
            gs.order.Clear();
            //gs.arrSortedInt.Clear();

            listPointsG.Clear();


        }



        //optiuni de antialiasing disponibile in .net:     
        /*
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                }
         */


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        //daca functia _Paint este comentata atunci designer-ul crapa, trebuie comentata si in sursa Form1.Designer.cs
        {
            //grid initial
            //
            Pen p = new Pen(Color.DarkGray);
            for (int y = 0; y < pictureBox1.Size.Height; ++y)
            {
                int tmp = y * 50;
                e.Graphics.DrawLine(p, 0, y * 50, pictureBox1.Size.Width * 50, y * 50);
                e.Graphics.DrawString((pictureBox1.Size.Height - tmp).ToString(), this.Font, Brushes.Black, 0, y * 50);
            }
            // "0"
            e.Graphics.DrawString("0", this.Font, Brushes.Black, 0, pictureBox1.Height - this.Font.Height);
            for (int x = 0; x < pictureBox1.Size.Width; ++x)
            {
                e.Graphics.DrawLine(p, x * 50, 0, x * 50, pictureBox1.Size.Height * 50);
                if (x != 1) e.Graphics.DrawString((x * 50).ToString(), this.Font, Brushes.Black, x * 50 - 21, pictureBox1.Height - this.Font.Height);
                else //hack pt afisare corecta "50"
                    e.Graphics.DrawString((x * 50).ToString(), this.Font, Brushes.Black, x * 50 - 15, pictureBox1.Height - this.Font.Height);
            }
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            //necesar pentru a nu deshide o noua instanta 
            //de fiecare data cand este apsata "input" :

            if (myForm != null)
            {
                myForm.BringToFront();
            }

            else
            {
                myForm = new Form3();
                myForm.Show();
            }
            // myForm.Dispose();

        }



        private void button4_Click(object sender, EventArgs e)
        {
            //form separat aici
            MessageBox.Show("[TO DO: modificare coordonate punct] - cu un form separat");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //form separat aici
            MessageBox.Show("[TO DO: delete punct] - cu un form separat");
        }

        

        private void button6_Click(object sender, EventArgs e)
        {


            g = Graphics.FromImage(DrawArea);

            //necesare pt paint
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p = new Pen(Color.DarkOrchid, 1);



            //se adauga in listPointsG punctele introduse la click()
            for (int i = 0; i <= ptList.Count - 2; i += 2)
            {
                listPointsG.Add(new PointG((int)ptList[i], (int)ptList[i + 1]));
            }


            //se proceseaza listPointsG, multimea punctelor rezultate = result[]
            gs.convexHull(listPointsG);

            int k = gs.result.Count;//cate elemente sunt in result

            //se traseaza linii intre punctele convex hull
            for (int i = 0; i < gs.result.Count - 1; i++)
            {
                g.DrawLine(p, (int)gs.result[i].x, (int)gs.result[i].y, (int)gs.result[i + 1].x, (int)gs.result[i + 1].y);

            }

            //o ultima linie intre ultimul element din hull si primul (pentru a fi 'inchisa')
            g.DrawLine(p, (int)gs.result[k - 1].x, (int)gs.result[k - 1].y, (int)gs.result[0].x, (int)gs.result[0].y);

            triang = 1;
            button7.Enabled = true;

            //
            button6.Enabled = false;
            button3.Enabled = false;
            if (gs.result.Count < 3) button7.Enabled = false;

            pictureBox1.Image = DrawArea;
        }

        void DisableButtons()     
 
//TODO: reparat reset(), undeva prin grahamscan.cs ramane ceva initializat dupa stergerea listelor cu .clear()


        {
            //button1.Enabled = false;
            //button2.Enabled = false;
            //button3.Enabled = false;
            //button4.Enabled = false;
            //button5.Enabled = false;
            //button6.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //triangulare triviala, ar trebui prin ear clipping
            //TODO(?)
            g = Graphics.FromImage(DrawArea);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p = new Pen(Color.Green, 1);
            int k = 0;
            if (triang == 1)
            {
                for (int i = 2; i < gs.result.Count - 1; i++)
                {
                    g.DrawLine(p, (int)gs.result[k].x, (int)gs.result[k].y, (int)gs.result[i].x, (int)gs.result[i].y);

                }
            }
            pictureBox1.Image = DrawArea;
        }


    }
}