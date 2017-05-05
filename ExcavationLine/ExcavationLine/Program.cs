using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ExcavationLine
{
    public class Program
    {
        public static void Main(string[] args)
        {       
            List<VerticalCurve> listVC = DataAccess.GetVerticalCurvePara();
            List<RuptureChain> listRC = DataAccess.GetChainRupture();

           

            if (DataManagement.InitializeVC(ref listVC))
            {
                Console.WriteLine("初始化完成!");
            }
            double consMile = 1.5;    
            List<Subgrade> listSub = DataAccess.GetSubgrade("1001", consMile);
            double h = DataManagement.GetHeightByMileage(listVC, listRC, consMile);
            Console.WriteLine(h);

            double deviation;
            DataManagement.SlopeCalculate(listSub, 1.5, 2, 8, out deviation);
            Console.WriteLine(deviation);

            //Test(listVC,listRC);
            //ExcelW(listVC);
            //CW(listVC);       
            Console.Read();
        }

        public static void CW(List<VerticalCurve> listVC)
        {
            for (int i = 0; i < listVC.Count; i++)
            {
                //Console.WriteLine(listVC[i].Num + "\t" + listVC[i].Alpha+"\t"+ listVC[i].A2 + "\t" + listVC[i].T + "\t" + listVC[i].Type);
                //Console.WriteLine((listVC[i].VarslopeUnity-listVC[i].T)+"\t"+ (listVC[i].VarslopeUnity + listVC[i].T));
                //Console.WriteLine((listVC[i].T*2-listVC[i].Alpha*listVC[i].R));
                //Console.WriteLine(listVC[i].I1 + "\t" + listVC[i].A1);
                Console.WriteLine(listVC[i].L);
            }
            Console.WriteLine(listVC[12].VarslopeUnity-listVC[12].T1);
        }

        public static void ExcelW(List<VerticalCurve> listVC)
        {
            ExcelOprate eo = new ExcelOprate();
            List<List<double>> dataList = new List<List<double>>();
            //excel输入数据并保存
            for (int i = 0; i < listVC.Count; i++)
            {
                List<double> data = new List<double>();
                data.Add(listVC[i].I1);
                data.Add(listVC[i].I2);
                data.Add(listVC[i].A1);
                data.Add(listVC[i].A2);
                data.Add(listVC[i].Alpha);
                data.Add(listVC[i].T);
                data.Add(listVC[i].Type);
                data.Add(listVC[i].VarslopeUnity- listVC[i].K1);
                data.Add(listVC[i].VarslopeUnity+listVC[i].K2);
                data.Add(listVC[i].H1);
                data.Add(listVC[i].H2);
                data.Add(listVC[i].H0);
                data.Add(listVC[i].L);
                //data.Add(listVC[i].Alpha * listVC[i].R);
                //data.Add(listVC[i].A1);
                //data.Add(listVC[i].A2);
                dataList.Add(data);
            }
            eo.CreateExcel("数据", dataList, @"C:\Users\shui\Desktop\1234.xlsx");
        }

        public static void Test(List<VerticalCurve> listVC, List<RuptureChain> listRC)
        {
            ExcelOprate eo = new ExcelOprate(@"C:\Users\shui\Desktop\1.xlsx");
            List<List<double>> dataList1 = eo.ExcelToList("sheet1");
            List<List<double>> dataList2 = new List<List<double>>();
            foreach (List<double> data in dataList1)
            {
                List<double> data1 = new List<double>();              
                data1.Add(DataManagement.GetHeightByMileage(listVC, listRC,data[0]));
                dataList2.Add(data1);
            }         
            eo.CreateExcel("数据", dataList2, @"C:\Users\shui\Desktop\MToHTest.xlsx");
        }
    }
}
