using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcavationLine
{
    public class DataManagement
    {
        #region 竖曲线计算

        /// <summary>
        /// 竖曲线初始化
        /// </summary>
        /// <param name="listVC">竖曲线要素</param>
        /// <returns>是否初始化完成</returns>
        public static bool InitializeVC(ref List<VerticalCurve> listVC)
        {
            if (listVC != null)
            {
                int len = listVC.Count;

                //计算方位角                              
                for (int i = 0; i < len - 1; i++)
                {
                    VerticalCurve vc = listVC[i + 1];
                    if ((vc.VarslopeUnity - listVC[i].VarslopeUnity) == 0)
                    {
                        continue;   //断链点(里程一样) 为0
                    }
                    double d = (vc.Height - listVC[i].Height) / (vc.VarslopeUnity - listVC[i].VarslopeUnity);
                    if (Math.Abs(d) > 1)
                    {
                        return false;
                    }
                    listVC[i].I2 = listVC[i + 1].I1 = d;
                    listVC[i].A2 = listVC[i + 1].A1 = Math.Atan(d);
                }

                #region 计算
                for (int i = 1; i < len - 1; i++)
                {
                    //精确计算圆心角，切线长，竖曲线类型等                
                    double a = listVC[i].A2 - listVC[i].A1;
                    listVC[i].Alpha = Math.Abs(a);
                    listVC[i].Type = a >= 0 ? -1 : 1;
                    if (listVC[i].R == 0)
                    {
                        listVC[i].T = 0;
                        //常用计算
                        listVC[i].E = 0;
                    }
                    else
                    {
                        listVC[i].T = listVC[i].R * Math.Tan(listVC[i].Alpha / 2);
                        //常用计算
                        listVC[i].E = Math.Pow(listVC[i].T1, 2) / (2 * listVC[i].R);
                    }
                    listVC[i].K1 = listVC[i].T * Math.Cos(listVC[i].A1);
                    listVC[i].K2 = listVC[i].T * Math.Cos(listVC[i].A2);
                    listVC[i].H1 = listVC[i].Height - listVC[i].T * Math.Sin(listVC[i].A1);
                    listVC[i].H2 = listVC[i].Height + listVC[i].T * Math.Sin(listVC[i].A2);
                    listVC[i].H0 = listVC[i].H1 - listVC[i].Type * listVC[i].R * Math.Cos(listVC[i].A1);
                    listVC[i].L = listVC[i].VarslopeUnity - listVC[i].K1 + listVC[i].Type *  listVC[i].R * Math.Sin(listVC[i].A1);

                    //常用计算
                    listVC[i].W = listVC[i].I2 - listVC[i].I1;
                    listVC[i].Line = Math.Abs(listVC[i].W) * listVC[i].R;
                    listVC[i].T1 = listVC[i].Line / 2;
                }
                #endregion               
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到统一里程
        /// </summary>
        /// <param name="listRC">断链数据</param>
        /// <param name="consMileage">施工里程</param>
        /// <returns></returns>
        public static double GetUnityMileage(List<RuptureChain> listRC, double consMileage)
        {
            double unityMileage = Math.Abs(consMileage);
            double m = Math.Abs(consMileage);
            foreach (RuptureChain rc in listRC)
            {
                if (consMileage < 0)
                {
                    if (m >= rc.ConsMile)
                    {
                        unityMileage = unityMileage - rc.RupchLen * rc.Type;
                    }
                }
                else
                {
                    if (m >= rc.ChainMile)
                    {
                        unityMileage = unityMileage - rc.RupchLen * rc.Type;
                    }
                }
            }
            return unityMileage;
        }

        /// <summary>
        /// 由施工里程计算高程
        /// </summary>
        /// <param name="listVC">初始化后的竖曲线要素</param>
        /// <param name="unityMileage">里程</param>
        /// <returns>高程</returns>
        public static double GetHeightByMileage(List<VerticalCurve> listVC, List<RuptureChain> listRC, double consMile)
        {
            double height = 0;
            //获得统一里程
            double unityMileage = DataManagement.GetUnityMileage(listRC, consMile);           
            bool b = false;  //是否精确计算
            int n;
            if (b)
            {
                //精确计算
                n = GetPosition(listVC, unityMileage);
            }
            else
            {
                //常用计算
                n = GetPositionCom(listVC, unityMileage);
            }        
            if (n < 0 || n >= listVC.Count)
            {
                height = 0;
            }
            else
            {
                VerticalCurve vc = listVC[n];
                //精确或常用计算
                height = b ? AccurateHeight(vc, unityMileage) : CommonHeight(vc, unityMileage);
            }
            return height;
        }

        /// <summary>
        /// 常用计算高程
        /// </summary>
        /// <param name="vc">变坡点</param>
        /// <param name="unityMileage">统一里程</param>
        /// <returns>高程</returns>
        public static double CommonHeight(VerticalCurve vc, double unityMileage)
        {
            double height = 0;
            double l2 = vc.VarslopeUnity + vc.T1;
            if (l2 <= unityMileage)
            {
                height = vc.Height + (unityMileage - vc.VarslopeUnity) * vc.I2;
            }
            else if (vc.VarslopeUnity <= unityMileage)
            {
                double h = vc.Height - (vc.VarslopeUnity - unityMileage) * vc.I2;
                double dh = Math.Pow(unityMileage - l2, 2) / 2 / vc.R;
                height = h - dh * vc.Type;
            }
            else
            {
                double h = vc.Height - (vc.VarslopeUnity - unityMileage) * vc.I1;
                double dh = Math.Pow(unityMileage - vc.VarslopeUnity + vc.T1, 2) / 2 / vc.R;
                height = h - dh * vc.Type;
            }
            return height;
        }

        /// <summary>
        /// 精确计算高程
        /// </summary>
        /// <param name="vc">变坡点</param>
        /// <param name="unityMileage">统一里程</param>
        /// <returns>高程</returns>
        public static double AccurateHeight(VerticalCurve vc, double unityMileage)
        {
            double height = 0;
            double l2 = vc.VarslopeUnity + vc.K2;
            if (l2 <= unityMileage)
            {
                height = vc.Height + (unityMileage - vc.VarslopeUnity) * vc.I2;
            }
            else
            {
                double d = vc.L - unityMileage;
                double dh = Math.Sqrt(Math.Pow(vc.R, 2) - Math.Pow(d, 2));
                height = vc.H0 + dh * vc.Type;
            }
            return height;
        }

        /// <summary>
        /// 根据里程确定该点在哪个变坡点内(常用)
        /// </summary>
        /// <param name="listVC">初始化后的竖曲线要素</param>
        /// <param name="unityMileage">里程</param>
        /// <returns>确定变坡点Num</returns>
        public static int GetPositionCom(List<VerticalCurve> listVC, double unityMileage)
        {
            int n = -1;
            if (listVC != null)
            {
                int len = listVC.Count;
                if (listVC[0].VarslopeUnity > unityMileage || listVC[len - 1].VarslopeUnity < unityMileage)
                {
                    n = -1;
                }
                else
                {
                    for (int i = 0; i < len - 1; i++)
                    {
                        if ((listVC[i].VarslopeUnity - listVC[i].T1) <= unityMileage && (listVC[i + 1].VarslopeUnity - listVC[i + 1].T1) >= unityMileage)
                        {
                            n = i;
                            break;
                        }
                    }
                }
            }
            return n;
        }

        /// <summary>
        /// 根据里程确定该点在哪个变坡点内(精确)
        /// </summary>
        /// <param name="listVC">初始化后的竖曲线要素</param>
        /// <param name="unityMileage">里程</param>
        /// <returns>确定变坡点Num</returns>
        public static int GetPosition(List<VerticalCurve> listVC, double unityMileage)
        {
            int n = -1;
            if (listVC != null)
            {
                int len = listVC.Count;
                if (listVC[0].VarslopeUnity > unityMileage || listVC[len - 1].VarslopeUnity < unityMileage)
                {
                    n = -1;
                }
                else
                {
                    for (int i = 0; i < len - 1; i++)
                    {
                        if ((listVC[i].VarslopeUnity - listVC[i].K1) <= unityMileage && (listVC[i + 1].VarslopeUnity - listVC[i + 1].K1) >= unityMileage)
                        {
                            n = i;
                            break;
                        }
                    }
                }
            }
            return n;
        }
        #endregion

        #region 路基开挖和填方计算
        
        /// <summary>
        /// 初始化路基标准断面数据（外部类调用此接口）
        /// </summary>
        /// <param name="listSS">路基标准断面数据</param>
        /// <returns>路基标准断面各点的偏距高数据</returns>
        public static SubgradeSH InitializeSS(List<SubgradeStd> listSS)
        {
            if (listSS!=null)
            {                
                //分类
                List<SubgradeStd> lh, ll, rh, rl;
                ClassifySS(listSS, out lh, out ll, out rh, out rl);

                //排序
                QuickSortSS(ref lh, 0, lh.Count - 1);
                QuickSortSS(ref ll, 0, ll.Count - 1);
                QuickSortSS(ref rh, 0, rh.Count - 1);
                QuickSortSS(ref rl, 0, rl.Count - 1);

                //处理
                SubgradeSH ssh = GetSubgradeSH(lh, ll, rh, rl);
                return ssh;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断是否路基标准断面上（外部类调用此接口）
        /// </summary>
        /// <param name="ss">路基节点数据</param>
        /// <param name="setover">偏距</param>
        /// <param name="h">高</param>
        public static void HeightJudge(SubgradeSH ss, double setover,double h)
        {
            double hh, lh;
            double dh1, dh2;
            //路堑
            if (GetSubgradeHeight(ss, setover, 1, out hh))
            {
                dh1 = h - hh;
                //未完待写
            }
            //路基
            if (GetSubgradeHeight(ss, setover, -1, out lh))
            {
                dh2 = h - lh;
                //未完待写
            }
        }

        /// <summary>
        /// 获得指定偏距处标准路基的高
        /// </summary>
        /// <param name="ss">路基节点数据</param>
        /// <param name="setover">偏距</param>
        /// <param name="type">路基类型，路堑为1，路堤为-1</param>
        /// <param name="h">高</param>
        /// <returns>是否计算成功</returns>               
        public static bool GetSubgradeHeight(SubgradeSH ss,double setover,int type,out double h)
        {
            bool b = true;
            h = 0;
            double s0 = ss.S0;
            double h0 = ss.H0;
            List<NodeData> listND;
            if (setover <= s0)
            {
                listND = type > 0 ? ss.LhList : ss.LlList;
                if (setover >= listND[listND.Count - 1].setover)
                {
                    h = GetHeightByListND(listND, setover, s0, h0, -1);
                }
                else
                {
                    b = false;
                }
            }
            else
            {
                listND = type > 0 ? ss.RhList : ss.RlList;
                if (setover <= listND[listND.Count - 1].setover)
                {
                    h = GetHeightByListND(listND, setover, s0, h0, 1);
                }
                else
                {
                    b = false;
                }
            }
            return b;
        }              

        /// <summary>
        /// 根据节点数据获得指定偏距处标准路基的高
        /// </summary>
        /// <param name="listND">节点数据</param>
        /// <param name="setover">偏距</param>
        /// <param name="s0">原始点偏距</param>
        /// <param name="h0">原始点高</param>
        /// <param name="lr">左或右</param>
        /// <returns>高</returns>
        public static double GetHeightByListND(List<NodeData> listND, double setover, double s0, double h0, int lr)
        {
            double h = 0;
            int m = listND.Count;
            if (lr < 0)
            {
                if (setover >= listND[0].setover)
                {
                    h = h0 / (s0 - listND[0].setover) * (setover - listND[0].setover);
                }
                else
                {
                    for (int i = 1; i < m - 1; i++)
                    {
                        if (setover >= listND[i].setover)
                        {
                            h = listND[i].height + (listND[i - 1].height - listND[i].height) / (listND[i - 1].setover - listND[i].setover) * (setover - listND[i].setover);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (setover <= listND[0].setover)
                {
                    h = h0 / (s0 - listND[0].setover) * (setover - listND[0].setover);
                }
                else
                {
                    for (int i = 1; i < m - 1; i++)
                    {
                        if (setover <= listND[i].setover)
                        {
                            h = listND[i].height + (listND[i - 1].height - listND[i].height) / (listND[i - 1].setover - listND[i].setover) * (setover - listND[i].setover);
                            break;
                        }
                    }
                }
            }
            return h;
        }



        #region 初始化计算函数
        /// <summary>
        /// 路基标准断面数据分类
        /// </summary>
        /// <param name="listSS">路基标准断面数据</param>
        /// <param name="lh">左上</param>
        /// <param name="ll">左下</param>
        /// <param name="rh">右上</param>
        /// <param name="rl">右下</param>
        public static void ClassifySS(List<SubgradeStd> listSS, out List<SubgradeStd> lh, out List<SubgradeStd> ll, out List<SubgradeStd> rh, out List<SubgradeStd> rl)
        {
            int len = listSS.Count;
            lh = ll = rh = rl = new List<SubgradeStd>();
            for (int i = 0; i < len - 1; i++)
            {
                SubgradeStd ss = listSS[i];
                if (ss.Type > 0)
                {
                    if (ss.Lr < 0)
                    {
                        lh.Add(ss);
                    }
                    else
                    {
                        rh.Add(ss);
                    }
                }
                else
                {
                    if (ss.Lr < 0)
                    {
                        ll.Add(ss);
                    }
                    else
                    {
                        rl.Add(ss);
                    }
                }
            }
        }

        /// <summary>
        /// 对分类后的路基标准断面数据快速排序
        /// </summary>
        /// <param name="lss">分类后的路基标准断面数据</param>
        /// <param name="left">初始索引</param>
        /// <param name="right">最大索引</param>
        public static void QuickSortSS(ref List<SubgradeStd> lss, int left, int right)
        {
            if (left < right)
            {
                SubgradeStd keySS = lss[(left + right) / 2];
                int i = left - 1;
                int j = right + 1;
                while (true)
                {
                    while (lss[++i].Step < keySS.Step && i < right) ;
                    while (lss[--j].Step > keySS.Step && j > 0) ;
                    if (i >= j)
                    {
                        break;
                    }
                    SubgradeStd ss = lss[i];
                    lss[i] = lss[i];
                    lss[j] = ss;
                }
                QuickSortSS(ref lss, left, i - 1);
                QuickSortSS(ref lss, j + 1, right);
            }
        }

        /// <summary>
        /// 计算偏距高标准断面数据
        /// </summary>
        /// <param name="lh">左上</param>
        /// <param name="ll">左下</param>
        /// <param name="rh">右上</param>
        /// <param name="rl">右下</param>
        /// <returns>偏距高标准断面数据</returns>
        public static SubgradeSH GetSubgradeSH(List<SubgradeStd> lh, List<SubgradeStd> ll, List<SubgradeStd> rh, List<SubgradeStd> rl)
        {
            SubgradeSH ssh = new SubgradeSH();
            ssh.Name = lh[0].Name;
            double l = lh[0].L;
            double p = lh[0].P;
            double q = lh[0].Q;
            double s1 = lh[0].S1;
            //计算初始点
            ssh.S0 = -l + (l + p + q) / 2;
            ssh.H0 = ll[0].A / 100 * (l + p + q) / 2;
            //计算各点SH
            ssh.LhList = GetCuttingSH(lh);
            ssh.RhList = GetCuttingSH(rh);
            ssh.LlList = GetEmbankmentSH(ll, ssh.S0, ssh.H0);
            ssh.RlList = GetEmbankmentSH(rl, ssh.S0, ssh.H0);

            return ssh;
        }

        /// <summary>
        /// 计算路堑偏距高
        /// </summary>
        /// <param name="listCut">路堑标准断面数据</param>
        /// <returns>路堑偏距高</returns>
        public static List<NodeData> GetCuttingSH(List<SubgradeStd> listCut)
        {
            List<NodeData> listND = new List<NodeData>();
            int lr = listCut[0].Lr;
            NodeData nd = new NodeData();
            nd.height = 0;
            if (lr < 0)
            {
                nd.setover = -listCut[0].L;
            }
            else
            {
                nd.setover = listCut[0].P + listCut[0].Q;
            }
            listND.Add(nd);
            int lastIndex = 0;    //总是指向listND最后一个数据
            int n = listCut.Count;
            //排水沟前偏距高计算
            nd.setover = listND[lastIndex].setover + listCut[0].H * listCut[0].M * lr;
            nd.height = listND[lastIndex].height - listCut[0].H;
            listND.Add(nd);
            lastIndex += 1;
            nd.setover = listND[lastIndex].setover + listCut[0].S * lr;
            nd.height = listND[lastIndex].height - listCut[0].S * listCut[0].A;
            listND.Add(nd);
            lastIndex += 1;
            nd.setover += listCut[0].S1 * lr;
            listND.Add(nd);
            //排水沟之后计算
            for (int i = 1; i < n - 1; i++)
            {
                lastIndex += 1;
                nd.setover = listND[lastIndex].setover + listCut[0].S * lr;
                nd.height = listND[lastIndex].height + listCut[0].S * listCut[0].A;
                listND.Add(nd);
                lastIndex += 1;
                nd.setover = listND[lastIndex].setover + listCut[0].H * listCut[0].M * lr;
                nd.height = listND[lastIndex].height + listCut[0].H;
                listND.Add(nd);
            }

            return listND;
        }

        /// <summary>
        /// 计算路堤偏距高
        /// </summary>
        /// <param name="listEbm">路堤标准断面数据</param>
        /// <param name="s0">初始点偏距</param>
        /// <param name="h0">初始点高</param>
        /// <returns>路堤偏距高</returns>
        public static List<NodeData> GetEmbankmentSH(List<SubgradeStd> listEbm, double s0, double h0)
        {
            List<NodeData> listND = new List<NodeData>();
            int lr = listEbm[0].Lr;
            NodeData nd = new NodeData();
            nd.height = s0;
            nd.height = h0;
            listND.Add(nd);
            int n = listEbm.Count;
            int lastIndex = -1;    //总是指向listND最后一个数据
            for (int i = 0; i < n - 1; i++)
            {
                lastIndex += 1;
                nd.setover = listND[lastIndex].setover + listEbm[i].S * lr;
                nd.height = listND[lastIndex].height - listEbm[i].S * listEbm[i].A;
                listND.Add(nd);
                lastIndex += 1;
                nd.setover = listND[lastIndex].setover + listEbm[i].H * listEbm[i].M * lr;
                nd.height = listND[lastIndex].height - listEbm[i].H;
                listND.Add(nd);
            }
            listND.RemoveAt(0);
            return listND;
        }
        #endregion

        #endregion
    }
}
