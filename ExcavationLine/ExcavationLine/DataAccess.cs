using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Common;

namespace ExcavationLine
{
    public class DataAccess
    {
        //获取竖曲线要素
        public static List<VerticalCurve> GetVerticalCurvePara()
        {
            try
            {
                MySqlOperating db = new MySqlOperating();
                string sql = "select id,num,varslope_cons,varslope_uni,varslope_cons,h,r from jz_para_verticalcurve";
                DataSet ds = db.GetDataSet(sql);
                if (ds != null)
                {
                    List<VerticalCurve> listVC = new List<VerticalCurve>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        VerticalCurve vc = new VerticalCurve();
                        vc.Id = Convert.ToInt32(row["id"] == DBNull.Value ? 0 : row["id"]);
                        vc.Num = Convert.ToInt32(row["num"] == DBNull.Value ? 0 : row["num"]);                      
                        vc.VarslopeCons = Convert.ToDouble(row["varslope_cons"] == DBNull.Value ? 0 : row["varslope_cons"]);
                        vc.VarslopeUnity = Convert.ToDouble(row["varslope_uni"] == DBNull.Value ? 0 : row["varslope_uni"]);
                        vc.Height = Convert.ToDouble(row["h"] == DBNull.Value ? 0 : row["h"]);
                        vc.R = Convert.ToDouble(row["r"] == DBNull.Value ? 0 : row["r"]);
                        //剔除非变坡点
                        //if (vc.R>=0)
                        //{
                        //    listVC.Add(vc);
                        //} 
                        listVC.Add(vc);
                    }
                    db.Close();
                    return listVC;
                }
                else
                {
                    db.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetVerticalCurvePara:" + e.Message);
                return null;
            }
        }

        //获取断链
        public static List<RuptureChain> GetChainRupture()
        {
            try
            {
                MySqlOperating db = new MySqlOperating();
                string sql = "select id,act_mile,cons_mile,chain_length,type from jz_para_chain order by act_mile";
                DataSet ds = db.GetDataSet(sql);
                if (ds != null)
                {
                    List<RuptureChain> listRC = new List<RuptureChain>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        RuptureChain ruptureChain = new RuptureChain();
                        ruptureChain.Id = Convert.ToInt32(row["id"] == DBNull.Value ? 0 : row["id"]);
                        ruptureChain.ChainMile = ruptureChain.ActualMile = Convert.ToDouble(row["act_mile"] == DBNull.Value ? 0 : row["act_mile"]);
                        ruptureChain.ConsMile = Convert.ToDouble(row["cons_mile"] == DBNull.Value ? 0 : row["cons_mile"]);
                        ruptureChain.RupchLen = Convert.ToDouble(row["chain_length"] == DBNull.Value ? 0 : row["chain_length"]);
                        ruptureChain.Type = Convert.ToInt32(row["type"] == DBNull.Value ? 0 : row["type"]);
                        listRC.Add(ruptureChain);
                    }
                    db.Close();
                    return listRC;
                }
                else
                {
                    db.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetChainRupture:" + e.Message);
                return null;
            }
        }

        //获取路基断面标准参数
        public static List<SubgradeStd> GetSubgradeStd(string name)
        {
            if (name!=null)
            {
                try
                {
                    MySqlOperating db = new MySqlOperating();
                    string sql = "select id,name,type,l_or_r,step,l,p,q,s1,a,s,h,m from jz_para_subgrade where name ='" + name + "' order by id";
                    DataSet ds = db.GetDataSet(sql);
                    if (ds != null)
                    {
                        List<SubgradeStd> listSS = new List<SubgradeStd>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            SubgradeStd subgradeStd = new SubgradeStd();
                            subgradeStd.Id = Convert.ToInt32(row["id"] == DBNull.Value ? 0 : row["id"]);
                            subgradeStd.Name = name;
                            subgradeStd.Type = Convert.ToInt32(row["type"] == DBNull.Value ? 0 : row["type"]);
                            subgradeStd.Lr = Convert.ToInt32(row["l_or_r"] == DBNull.Value ? 0 : row["l_or_r"]);
                            subgradeStd.Step = Convert.ToInt32(row["step"] == DBNull.Value ? 0 : row["step"]);
                            subgradeStd.L = Convert.ToDouble(row["l"] == DBNull.Value ? 0 : row["l"]);
                            subgradeStd.P = Convert.ToDouble(row["p"] == DBNull.Value ? 0 : row["p"]);
                            subgradeStd.Q = Convert.ToDouble(row["q"] == DBNull.Value ? 0 : row["q"]);
                            subgradeStd.S1 = Convert.ToDouble(row["s1"] == DBNull.Value ? 0 : row["s1"]);
                            subgradeStd.A = Convert.ToDouble(row["a"] == DBNull.Value ? 0 : row["a"]);
                            subgradeStd.S = Convert.ToDouble(row["s"] == DBNull.Value ? 0 : row["s"]);
                            subgradeStd.H = Convert.ToDouble(row["h"] == DBNull.Value ? 0 : row["h"]);
                            subgradeStd.M = Convert.ToDouble(row["m"] == DBNull.Value ? 0 : row["m"]);

                            listSS.Add(subgradeStd);
                        }
                        db.Close();
                        return listSS;
                    }
                    else
                    {
                        db.Close();
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("GetInputData:" + e.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
