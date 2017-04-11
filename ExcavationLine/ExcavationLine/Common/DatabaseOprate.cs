using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class DatabaseOprate
    {
        //插入竖曲线要素
        public static bool InsertVC(List<List<double>> dataList)
        {
            MySqlOperating db = new MySqlOperating();
            bool b=false;
            try
            {
                StringBuilder values=new StringBuilder();
                foreach (List<double> data in dataList)
                {
                    values.Append("(");
                    foreach (double d in data)
                    {
                        values.Append(d.ToString() + ",");
                    }
                    values.Remove(values.Length - 1, 1);
                    values.Append("),");
                }
                values.Remove(values.Length - 1, 1);
                string sql = "insert into jz_para_verticalcurve values " + values.ToString()+";";
                if (db.RunDataBase(sql))
                {
                    b = true;
                }
                else
                {
                    b = false;
                }
            }
            catch (Exception)
            {
                b = false;
            }
            finally
            {
                db.Close();                
            }
            return b;
        }
    }
}
