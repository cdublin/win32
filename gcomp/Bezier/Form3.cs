using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Bezier
{
    //TODO: sanitize input/masked textbox

    partial class Form3 : Form
    {

        public event EventHandler PerformForm1Click;

        public Form3()
        {
            InitializeComponent();
        }

        
    //
 

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

         


            //buton reset input
            this.textBox1.Clear();
            this.textBox2.Clear();
        }




        //necesar pentru a nu deshide o noua instanta de fiecare data cand este apsata "input"
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            //my form e globala in Form1.cs
            Form1.myForm= null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
       
            Form1.xm=Int32.Parse(textBox1.Text);
            Form1.ym = 500-Int32.Parse(textBox2.Text);

            EventHandler handler = this.PerformForm1Click;
            if (handler != null)
                handler(this, EventArgs.Empty);



            this.Close(); //obligatoriu la un moment dat

        }


        

      


    }

   
}
