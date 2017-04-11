using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcavationLine
{
    public class RuptureChain
    {
        private int id;
        private double actualMile;   //统一里程(不完全是)
        private double consMile;    //施工里程
        private double chainMile;   //断链点里程(实际)
        private int type;  //长链为-1 短链为1
        private double rupchLen;
        private double length = 0; 
        private double zh = -1;

        public double Length
        {
            get { return length; }
            set { length = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public double Zh
        {
            get { return zh; }
            set { zh = value; }
        }

        public double RupchLen
        {
            get { return rupchLen; }
            set { rupchLen = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public double ConsMile
        {
            get { return consMile; }
            set { consMile = value; }
        }

        public double ActualMile
        {
            get { return actualMile; }
            set { actualMile = value; }
        }

        public double ChainMile
        {
            get
            {
                return chainMile;
            }

            set
            {
                chainMile = value;
            }
        }

        public RuptureChain()
        { 
            
        }

        public RuptureChain(int id,double actMile,double consMile,int type,double rupchLen)
        {
            this.Id = id;
            this.ActualMile = actMile;
            this.ConsMile = consMile;
            this.Type = type;
            this.RupchLen = rupchLen;
        }
    }
}
