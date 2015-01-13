using System;
using System.Collections.Generic;
using System.Text;

namespace Bezier
{
    //clasa helper pentru punctele folosite
    // p.getX=coordonata X a punctului p 

    public class PointG
    {
        public int y;
        public int x;
        public PointG(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
    }


    public class GrahamScan
    {
        const int TURN_LEFT = 1;
        const int TURN_RIGHT = -1;
        const int TURN_NONE = 0;

        //liste de puncte publice pentru a fi afectate de reset // TODO: reparat reset()

        public List<PointG> result = new List<PointG>();
        public List<PointG> order = new List<PointG>();

        //test de orientare clasic Delta(p,q,r)
        //mai multe detalii in cursul pdf de geometrie de pe grup 2014-2015, pag 4 in partea de jos.

        public int turn(PointG p, PointG q, PointG r)
        {
            return ((q.getX() - p.getX()) * (r.getY() - p.getY()) - (r.getX() - p.getX()) * (q.getY() - p.getY())).CompareTo(0);
        }

        //
        public void keepLeft(List<PointG> hull, PointG r)
        {
            //se testeaza daca ultimele doua elemente din hull impreuna cu un punct nou r efectueaza 
            //un viraj la stanga. daca nu, se elimina ultimul element din hull pana conditia este indeplinita.
            //(sau pana hull este vida atunci r e default acceptat)

            while (hull.Count > 1 && turn(hull[hull.Count - 2], hull[hull.Count - 1], r) != TURN_LEFT)
            {

                hull.RemoveAt(hull.Count - 1);
            }
            if (hull.Count == 0 || hull[hull.Count - 1] != r)
            {

                hull.Add(r);
            }


        }

        //unghiul segmentului p1p2 fata de axa OX (grade)
        //&&helper pt mergesort - criteriul principal de sortare

        public double getAngle(PointG p1, PointG p2)
        {
            float xDiff = p2.getX() - p1.getX();
            float yDiff = p2.getY() - p1.getY();
            return Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;
        }


        public List<PointG> MergeSort(PointG p0, List<PointG> arrPoint)
        //probabil inlocuita cu qsort(....) insa mergesort e mai eficienta pe liste
        {
            if (arrPoint.Count == 1)
            {
                return arrPoint;
            }
            List<PointG> arrSortedInt = new List<PointG>();
            int middle = (int)arrPoint.Count / 2;
            List<PointG> leftArray = arrPoint.GetRange(0, middle);
            List<PointG> rightArray = arrPoint.GetRange(middle, arrPoint.Count - middle);
            leftArray = MergeSort(p0, leftArray);
            rightArray = MergeSort(p0, rightArray);
            int leftptr = 0;
            int rightptr = 0;
            for (int i = 0; i < leftArray.Count + rightArray.Count; i++)
            {
                if (leftptr == leftArray.Count)
                {
                    arrSortedInt.Add(rightArray[rightptr]);
                    rightptr++;
                }
                else if (rightptr == rightArray.Count)
                {
                    arrSortedInt.Add(leftArray[leftptr]);
                    leftptr++;
                }
                else if (getAngle(p0, leftArray[leftptr]) < getAngle(p0, rightArray[rightptr]))

                //"arrSortedInt va contine punctele ordonate dupa unghiurile polare in jurul lui p0"
                //(Cormen RO pag. 771 - scanarea Graham)
                {
                    arrSortedInt.Add(leftArray[leftptr]);
                    leftptr++;
                }
                else
                {
                    arrSortedInt.Add(rightArray[rightptr]);
                    rightptr++;
                }
            }
            return arrSortedInt;
        }

        public void convexHull(List<PointG> points)
        {


            PointG p0 = null;

            foreach (PointG value in points)
            {
                if (p0 == null)
                    p0 = value;
                else
                {
                    if (p0.getY() > value.getY())
                        p0 = value;
                }
            }

            //p0 acum este punctul cu coordonata Y minima (cel mai de jos)


            foreach (PointG value in points)
            {
                if (p0 != value)
                    order.Add(value);
            }

            //s-au adaugat restul punctelor

            order = MergeSort(p0, order);

            //s-au sortat dupa dupa unghiurile polare in jurul lui p0
            result.Add(p0);
            result.Add(order[0]);
            result.Add(order[1]);

            //se sterg order[0] si order[1] din order[] pt ca au fost introduse deja in result[]
            order.RemoveAt(0);
            order.RemoveAt(0);


            //se introduc/valideaza restul punctelor respectand conditia 'viraj la stanga'

            foreach (PointG value in order)
            {
                keepLeft(result, value);
            }

            //result este acoperirea convexa, de forma  p[i].x si p[i].y; adica 5 puncte introduse = 5 elemente.
            //lista ptList din Form1 este de forma p[i]=x, p[i+1]=y (5 puncte = 10 elemente, se itereaza cu i+=2)

        }

    }
}
