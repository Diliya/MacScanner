using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MacScanner
{
    class DBhelper
    {
        public void DBConnect()
        {
            SQLiteConnection conn = null;
            string dbPath = "Data Source =" + Environment.CurrentDirectory + "/inf.db";
            conn = new SQLiteConnection(dbPath);//创建数据库实例，指定文件位置  
            conn.Open();//打开数据库，若文件不存在会自动创建  
        }
    }
}
