using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
namespace Bezier
    
    
{
    class BezierCurve
    {
        //precizie 1/12n si nu mai exista restrictie la nr de puncte introduse
        private double factorialS(int n)
        {
            double Stirling;
            if (n == 0.0 || n == 1.0) Stirling = 1.0;
            else Stirling = (Math.Sqrt(2 * Math.PI * n)) * (Math.Pow((n / Math.E), n));
            return Stirling;
        }
        
        //calculeaza n!/(i!*(n-i)!)
        private double raport(int n, int i)
        {
            double temp;
            double a1 = factorialS(n);
            double a2 = factorialS(i);
            double a3 = factorialS(n - i);
            temp =  a1/ (a2 * a3);
            return temp;
        }

        //
        private double Bernstein(int n, int i, double t)
        {
            double baza;
            double t_pow_i; /* t^i */
            double t_pow_ni; /* (1 - t)^(n-i) */
           //daca exponentul este nul:
            if (t == 0.0 && i == 0) 
                t_pow_i = 1.0; 
            else 
                t_pow_i = Math.Pow(t, i);

            if (n == i && t == 1.0) 
                t_pow_ni = 1.0; 
            else 
                t_pow_ni = Math.Pow((1 - t), (n - i));
            
            baza = raport(n, i) * t_pow_i * t_pow_ni; 
            return baza;
        }


        /*
        populeaza p cu coordonatele punctelor curbei in ordine
        x1,y1,x2,y2 etc

        primeste b=punctele introduse 
        x1,y1,x2,y2...

        si cpts = din cate puncte este formata curba
        */


        public void points2bezier(double[] b, int cpts, double[] p)
        {
            int npts = (b.Length) / 2;
            int ib; //iterator pentru punctele bezier
            int ip; //iterator pentru punctele de intrare
            double t_step, t;
             
            ib = 0;
            t = 0; //care parcurge [0,1]
            t_step = (double)1.0 / (cpts - 1); //..cu pasul 1/("nr_de_puncte_pe_curba"-1)

            for (int i1 = 0; i1 != cpts; i1++) //pentru fiecare punct al curbei
            {
                if ((1.0 - t) < 1e-6) t = 1.0; // limita minima 10^-6
                ip = 0;
                p[ib] = 0.0;
                p[ib + 1] = 0.0;
                for (int i = 0; i != npts; i++) //pentru fiecare punct de intrare
                {
                    double baza = Bernstein(npts - 1, i, t);
                    p[ib] += baza * b[ip];
                    p[ib + 1] += baza * b[ip + 1];
                    ip = ip +2;
                }
                ib += 2;
                t += t_step;
            }
        }

    }

}
