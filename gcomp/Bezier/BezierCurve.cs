using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Bezier

    //TODO: comentat in intregime algoritmul  ("Forma Bernstein a curbelor Bezier")


{
    class BezierCurve
    {
        

        //inlocuit tot ce tine de calculul factorial! cu formula lui Stirling.
        
        

        private double factorialS(int n)
        {
            double Stirling;
            if (n == 0.0 || n == 1.0) Stirling = 1.0;
            else Stirling = (Math.Sqrt(2 * Math.PI * n)) * (Math.Pow((n / Math.E), n));
            return Stirling;

        }

        //mai eficient si nu mai exista restrictie la n (nr de puncte introduse)





        //   N!/(i!*(n-i)!)

        private double Ni(int n, int i)
        {
            double ni;
            double a1 = factorialS(n);
            double a2 = factorialS(i);
            double a3 = factorialS(n - i);
            ni =  a1/ (a2 * a3);
            return ni;
        }

        // Calculate Bernstein basis
        private double Bernstein(int n, int i, double t)
        {
            double basis;
            double ti; /* t^i */
            double tni; /* (1 - t)^(n-i) */

            /* Prevent problems with pow */

            if (t == 0.0 && i == 0) 
                ti = 1.0; 
            else 
                ti = Math.Pow(t, i);

            if (n == i && t == 1.0) 
                tni = 1.0; 
            else 
                tni = Math.Pow((1 - t), (n - i));

            //Bernstein basis
            basis = Ni(n, i) * ti * tni; 
            return basis;
        }

        public void Bezier2D(double[] b, int cpts, double[] p)
        {
            int npts = (b.Length) / 2;
            int icount, jcount;
            double step, t;

            // Calculate points on curve

            icount = 0;
            t = 0;
            step = (double)1.0 / (cpts - 1);

            for (int i1 = 0; i1 != cpts; i1++)
            { 
                if ((1.0 - t) < 5e-6) 
                    t = 1.0;

                jcount = 0;
                p[icount] = 0.0;
                p[icount + 1] = 0.0;
                for (int i = 0; i != npts; i++)
                {
                    double basis = Bernstein(npts - 1, i, t);
                    p[icount] += basis * b[jcount];
                    p[icount + 1] += basis * b[jcount + 1];
                    jcount = jcount +2;
                }

                icount += 2;
                t += step;
            }
        }
    }
}
