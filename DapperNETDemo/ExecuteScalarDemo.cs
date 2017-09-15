using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using DapperNETDemo.Utils;
using Dapper;

namespace DapperNETDemo
{
    public class ExecuteScalarDemo
    {
        static void Main4(string[] args)
        //static void Main(string[] args)
        {
            ExecuteScalarDemo client = new ExecuteScalarDemo();
            object expectedResult;
            int result = -1;

            #region 泛型ExecuteScalar方法的使用

            Console.WriteLine("开始执行GetChildDapperDemoCount()：");
            expectedResult = (long)client.UseGenericExecuteScalar<long>("bigint");
            Console.WriteLine("执行成功：结果为{0}，类型为{1}。", expectedResult, expectedResult.GetType().Name);
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 非泛型ExecuteScalar方法的使用

            Console.WriteLine("开始执行GetChildDapperDemoCount()：");
            result = client.GetChildDapperDemoCount();
            Console.WriteLine("执行成功：共有{0}条Child DapperDemo记录。", result);
            Console.ReadLine();

            #endregion
        }

        /// <summary>
        /// 泛型ExecuteScalar方法的使用
        /// </summary>
        /// <typeparam name="T"></typeparam>      
        /// <param name="dbType"></param>
        /// <returns></returns>
        public T UseGenericExecuteScalar<T>(string dbType)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                T scalar = connection.ExecuteScalar<T>("SELECT CAST(1 AS " + dbType + ")");
                return scalar;
            }
        }

        /// <summary>
        /// 非泛型ExecuteScalar方法的使用
        /// </summary>
        /// <returns></returns>
        public int GetChildDapperDemoCount()
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql = 
@"SELECT COUNT(child.ID)
FROM dbo.DapperNETDemo child WITH(NOLOCK)
LEFT JOIN dbo.DapperNETDemo parent WITH(NOLOCK) ON parent.ID = child.ParentID
WHERE parent.ID IS NOT NULL";
                return (int)connection.ExecuteScalar(sql);
            }
        }
    }
}
