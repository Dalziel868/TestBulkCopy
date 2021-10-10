using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBulkCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            string connection = ConfigurationManager.ConnectionStrings["BulkSource"].ConnectionString;
            var dts = new DataSet();
            var dtb = new DataTable();
            dtb = dts.Tables.Add("ttmpData");
            dtb.Columns.Add("Id", typeof(int));
            //dtb.Columns.Add("Name", typeof(string));
            dtb.Columns.Add("Desc", typeof(string));

            Console.WriteLine("Add some data to table...");
            for(int i=1;i<=100000;i++)
            {
                dtb.Rows.Add(i, $"Product NO.{i}");
            }
            Console.WriteLine("Open connection ...");
            SqlConnection sqlCon = new SqlConnection(connection);
            SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlCon);
            sqlCon.Open();
            
            sqlBulk.DestinationTableName = "Products";
            sqlBulk.BulkCopyTimeout = 0;
            Console.WriteLine("Wringting data...");
            sqlBulk.ColumnMappings.Add("Id", "Id");
            sqlBulk.ColumnMappings.Add("Desc", "Desc");
            sqlBulk.WriteToServer(dtb);
            sqlCon.Close();
            sqlCon.Dispose();
            dts.Dispose();
            dtb.Dispose();
            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
