using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcavationLine
{
    public class Subgrade
    {
        private int id;         //id
        private string name;    //区段名
        private double qd;      //起点里程
        private double zd;      //终点里程
        private double dh;      //路肩高程
        private double s;       //偏距
        private double h;       //高差

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double Qd
        {
            get
            {
                return qd;
            }

            set
            {
                qd = value;
            }
        }

        public double Zd
        {
            get
            {
                return zd;
            }

            set
            {
                zd = value;
            }
        }

        public double Dh
        {
            get
            {
                return dh;
            }

            set
            {
                dh = value;
            }
        }

        public double S
        {
            get
            {
                return s;
            }

            set
            {
                s = value;
            }
        }

        public double H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }
    }

    public struct Point
    {
        public double setover;
        public double height;
    }
}
