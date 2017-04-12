using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcavationLine
{
    public class VerticalCurve
    {
        //给定字段
        private int id;
        private int num;
        private double varslopeUnity;    //统一里程
        private double varslopeCons;     //施工里程（不用）
        private double height;           //变坡点高程
        private double r;                //半径

        //计算字段        
        private double i1;    //直线方位角正切值(前) 
        private double i2;    //直线方位角正切值(后)

        //精确计算所需字段
        private double alpha;  //竖曲线圆心角（首尾两点设为0）
        private double a1;  //直线方位角(前) a1=Math.Atan(i1)
        private double a2;  //直线方位角(后) a1=Math.Atan(i2)
        private double t;    //切线长，首尾两点设为0，
        private int type;   //竖曲线类型，1为凸曲线(圆心在路下方)，-1为凹曲线(圆心在路上方),首尾两点设为0

        private double k1;   //变坡点距竖曲线起点里程长度
        private double k2;   //变坡点距竖曲线终点里程长度
        private double h1;   //竖曲线起点高度
        private double h2;   //竖曲线终点高度
        private double h0;   //圆心高度
        private double l;    //圆心里程

        //常用计算所需字段
        private double w;      //转坡角
        private double t1;     //切线长
        private double line;   //曲线长
        private double e;      //外矢距

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

        public double VarslopeUnity
        {
            get
            {
                return varslopeUnity;
            }

            set
            {
                varslopeUnity = value;
            }
        }

        public double VarslopeCons
        {
            get
            {
                return varslopeCons;
            }

            set
            {
                varslopeCons = value;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        public double R
        {
            get
            {
                return r;
            }

            set
            {
                r = value;
            }
        }

        public int Num
        {
            get
            {
                return num;
            }

            set
            {
                num = value;
            }
        }

        public double Alpha
        {
            get
            {
                return alpha;
            }

            set
            {
                alpha = value;
            }
        }

        public double T
        {
            get
            {
                return t;
            }

            set
            {
                t = value;
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

        public double A1
        {
            get
            {
                return a1;
            }

            set
            {
                a1 = value;
            }
        }

        public double A2
        {
            get
            {
                return a2;
            }

            set
            {
                a2 = value;
            }
        }

        public double I1
        {
            get
            {
                return i1;
            }

            set
            {
                i1 = value;
            }
        }

        public double I2
        {
            get
            {
                return i2;
            }

            set
            {
                i2 = value;
            }
        }

        public double K1
        {
            get
            {
                return k1;
            }

            set
            {
                k1 = value;
            }
        }

        public double K2
        {
            get
            {
                return k2;
            }

            set
            {
                k2 = value;
            }
        }

        public double H1
        {
            get
            {
                return h1;
            }

            set
            {
                h1 = value;
            }
        }

        public double H2
        {
            get
            {
                return h2;
            }

            set
            {
                h2 = value;
            }
        }

        public double H0
        {
            get
            {
                return h0;
            }

            set
            {
                h0 = value;
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

        public double W
        {
            get
            {
                return w;
            }

            set
            {
                w = value;
            }
        }

        public double T1
        {
            get
            {
                return t1;
            }

            set
            {
                t1 = value;
            }
        }

        public double E
        {
            get
            {
                return e;
            }

            set
            {
                e = value;
            }
        }

        public double Line
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }
        #endregion

        //public VerticalCurve()
        //{
        //    //精确计算
        //    this.Alpha = 0;
        //    this.I1 = 0;
        //    this.I2 = 0;
        //    this.A1 = 0;
        //    this.A2 = 0;
        //    this.T = 0;
        //    this.Type = 0;
        //    this.K1 = 0;
        //    this.K2 = 0;
        //    this.H1 = 0;
        //    this.H2 = 0;
        //    this.H0 = 0;
        //    this.L = 0;
        //    //常用计算
        //    this.W = 0;
        //    this.Line = 0;
        //    this.T1 = 0;
        //    this.E = 0;    
        //}

    }
}
