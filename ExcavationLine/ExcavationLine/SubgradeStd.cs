using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcavationLine
{
    public class SubgradeStd
    {
        private int id;       //自增id
        private string name;  //区段名
        private int type;     //路堑为1，路堤为-1
        private int step;     //台阶级数，从0开始
        private int lr;       //左台阶为-1，右台阶为1
        private double l;     //L
        private double p;     //P
        private double q;     //Q
        private double s1;    //排水沟宽
        //基本参数
        private double a;
        private double s;
        private double h;
        private double m;

        #region 属性

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

        public int Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public int Step
        {
            get
            {
                return step;
            }

            set
            {
                step = value;
            }
        }

        public double L
        {
            get
            {
                return l;
            }

            set
            {
                l = value;
            }
        }

        public double P
        {
            get
            {
                return p;
            }

            set
            {
                p = value;
            }
        }

        public double Q
        {
            get
            {
                return q;
            }

            set
            {
                q = value;
            }
        }

        public double S1
        {
            get
            {
                return s1;
            }

            set
            {
                s1 = value;
            }
        }

        public double A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
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

        public double M
        {
            get
            {
                return m;
            }

            set
            {
                m = value;
            }
        }

        public int Lr
        {
            get
            {
                return lr;
            }

            set
            {
                lr = value;
            }
        }
        #endregion
    }
}
