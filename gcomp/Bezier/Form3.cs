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

    public partial class Form3 : Form
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
         this.Close(); //obligatoriu la un moment dat
        // ar trebui sa introduca in ptlist[] x si y
        // rescrie valorile existente sau adauga, cu reset() sau nu;
        }


        

      


    }

   
}
