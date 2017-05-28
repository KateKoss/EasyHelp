using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.Models
{
    public class DataPoint
    {
        //public double a1 { get; set; }
        //public double a2 { get; set; }
        //public int Cluster { get; set; }

        //public DataPoint(double a1, double a2)
        //{
        //    this.a1 = a1;
        //    this.a2 = a2;
        //    Cluster = 0;
        //}

        //public DataPoint()
        //{

        //}

        //public override string ToString()
        //{
        //    return string.Format("{{{0}; {1}}}", a1.ToString("f" + 1), a2.ToString("f" + 1));
        //}



        private double n { get; set; }
        public double[] a { get; set; }
        //public double a2 { get; set; }
        public int Cluster { get; set; }
        public string pointId { get; set; }
        private static Random r = new Random();

        public DataPoint(int n)
        {
            this.n = n;
            a = new double[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = r.Next(0, 100);                
            }           
            
            Cluster = 0;
        }

        public DataPoint()
        {

        }

        public override string ToString()
        {
            string str = "";
            //string str2 = "";
            for (int i = 0; i < n; i++)
            {
                str += "{" + a[i].ToString("f" + 1) + "}; ";
            }
            return str;
        }

    }
}
