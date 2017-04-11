using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcavationLine
{
    public class SubgradeSH
    {        
        private const double dh = 0.972;   //路肩高程        
        private string name;     //区段名

        //路基对称中心点的偏距高程（作初始点计算）
        private double s0;
        private double h0;
        //路基标准点偏距高程
        private List<NodeData> lhList;    //左上
        private List<NodeData> llList;    //左下
        private List<NodeData> rhList;    //右上
        private List<NodeData> rlList;    //右下

        #region 属性
        public double S0
        {
            get
            {
                return s0;
            }

            set
            {
                s0 = value;
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

        public List<NodeData> LhList
        {
            get
            {
                return lhList;
            }

            set
            {
                lhList = value;
            }
        }

        public List<NodeData> LlList
        {
            get
            {
                return llList;
            }

            set
            {
                llList = value;
            }
        }

        public List<NodeData> RhList
        {
            get
            {
                return rhList;
            }

            set
            {
                rhList = value;
            }
        }

        public List<NodeData> RlList
        {
            get
            {
                return rlList;
            }

            set
            {
                rlList = value;
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

        public static double Dh
        {
            get
            {
                return dh;
            }
        }
        #endregion

    }

    public struct NodeData
    {
        public double setover;
        public double height;
    }
}
