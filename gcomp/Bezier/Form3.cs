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

        

        public Form3()
        {
            InitializeComponent();
        }

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
        //butonul "OK"

                //manipularea listbox-ului:
                //listBox1.Items.Add();
                //listBox1.SelectedIndex = listBox1.Items.Count - 1;  //autoscroll
                //listBox1.SelectedIndex = -1;  

         
        /* 
         ar trebui sa introduca in ptlist[] x si y;
         deci practic sa faca exact ce face pictureBox1_MouseClick;
         TODO: ca sa functioneze,
         variabilele ptlist, label, numpoints etc trebuiesc mutate intr-o clasa separata. 
        */


            this.Close(); //obligatoriu la un moment dat

        }


        

      


    }

   
}
