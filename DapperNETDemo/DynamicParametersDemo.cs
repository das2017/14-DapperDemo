using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Dapper;
using DapperNETDemo.Utils;

namespace DapperNETDemo
{
    public class DynamicParametersDemo
    {
        static void Main5(string[] args)
        //static void Main(string[] args)
        {
            DynamicParametersDemo client = new DynamicParametersDemo();

            Console.WriteLine("开始执行InsertDapperDemo()：");
            DapperDemoEntity dapperDemo = new DapperDemoEntity()
            {
                ParentID = 0,
                DapperDemoName = "上海市",
                Type = Common.Type.C,
                ModifiedDate = DateTime.Now
            };
            dapperDemo.ID = client.InsertDapperDemo(dapperDemo);          
            if (dapperDemo.ID > 0)
            {
                Console.WriteLine("新增成功：新增记录的ID为{0}，ParentID为{1}，Name为{2}，Type为{3}。", dapperDemo.ID, dapperDemo.ParentID, dapperDemo.DapperDemoName, dapperDemo.Type);
            }
            else
            {
                Console.WriteLine("新增失败。");
            }

            Console.ReadLine();
        }
        
        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="dapperDemo"></param>
        /// <returns>返回新增的ID</returns>
        public int InsertDapperDemo(DapperDemoEntity dapperDemo)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql =
@"INSERT INTO dbo.DapperNETDemo(ParentID, [Name], [Type], ModifiedDate) VALUES (@ParentID, @DapperDemoName, @Type, @ModifiedDate);
  SELECT CAST(SCOPE_IDENTITY() AS INT)";              

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@ParentID", dapperDemo.ParentID, DbType.Int32);
                //注意：当数据库表字段被设计为char类型时，必须给DbType传值，且必须赋的是DbType.AnsiStringFixedLength，否则数据库访问速度会突然变得很慢。
                dp.Add("@DapperDemoName", dapperDemo.DapperDemoName, DbType.String, ParameterDirection.Input, 100);
                dp.Add("@Type", dapperDemo.Type, DbType.Int32);
                dp.Add("@ModifiedDate", dapperDemo.ModifiedDate, DbType.DateTime);

                int dapperDemoID = connection.Query<int>(sql, dp).Single();
                return dapperDemoID;
            }
        }
    }
}
