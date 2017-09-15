using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DapperNETDemo.Utils;
using Dapper;

namespace DapperNETDemo
{
    /// <summary>
    /// 如何调用存储过程的示例
    /// </summary>
    public class QueryStoredProcedureDemo
    {
        static void Main6(string[] args)
        //static void Main(string[] args)
        {
            QueryStoredProcedureDemo clientSPQuery = new QueryStoredProcedureDemo();               

            Console.WriteLine("开始执行ProcedureWithOutAndReturnParameter1()：");
            dynamic spSingleResultDynamic = clientSPQuery.ProcedureWithOutAndReturnParameter1();
            if (spSingleResultDynamic != null)
            {
                Console.WriteLine("执行成功：{0}。", spSingleResultDynamic.NameResult);
            }
            Console.ReadLine();

            Console.WriteLine("开始执行ProcedureWithOutAndReturnParameter2()：");
            IEnumerable<dynamic> spMultiResultDynamic = clientSPQuery.ProcedureWithOutAndReturnParameter2();
            if (spMultiResultDynamic != null && spMultiResultDynamic.Count() == 2)
            {
                Console.WriteLine("执行成功：第1个结果是【{0}】，第2个结果是【{1}】。", spMultiResultDynamic.ElementAt(0).NameResult1, spMultiResultDynamic.ElementAt(1).NameResult2);
            }
            Console.ReadLine(); 
        }
        
        public dynamic ProcedureWithOutAndReturnParameter1()
        {
            int successCode = -1;
            string resultMessage = string.Empty;
            using (IDbConnection connection = Common.OpenConnection())
            {
                DynamicParameters parameter = new DynamicParameters();
                string name = "test1";
                parameter.Add("@Name", name);             
                parameter.Add("@SuccessCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameter.Add("@ResultMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                parameter.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                IEnumerable<dynamic> result = connection.Query(sql: "DapperNETDemoSP1", param: parameter, commandType: CommandType.StoredProcedure);
                                
                successCode = parameter.Get<int>("SuccessCode");
                resultMessage = parameter.Get<string>("ResultMessage");

                dynamic row = result.Single();
                return row;               
            }
        }

        /// <summary>
        /// 返回多个结果集
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> ProcedureWithOutAndReturnParameter2()
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                DynamicParameters parameter = new DynamicParameters();
                string name = "test2";
                parameter.Add("@Name", name);               
                parameter.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                IEnumerable<dynamic> resultA = null;
                IEnumerable<dynamic> resultB = null;
                using (SqlMapper.GridReader grid = connection.QueryMultiple(sql: "DapperNETDemoSP2", param: parameter, commandType: CommandType.StoredProcedure))
                {
                    resultA = grid.Read<dynamic>();
                    resultB = grid.Read<dynamic>();
                }

                List<dynamic> result = new List<dynamic>();
                result.Add(resultA.Single());
                result.Add(resultB.Single());
                return result;
            }
        }
    }
}
