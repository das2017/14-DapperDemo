using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Dapper;
using DapperNETDemo.Utils;

namespace DapperNETDemo
{
    public class QueryMultipleDemo
    {
        static void Main3(string[] args)
        //static void Main(string[] args)
        {
            QueryMultipleDemo clientQueryMultiple = new QueryMultipleDemo();
            IList<DapperDemoEntity> dapperDemoList = null;
            DapperDemoEntity dapperDemo = null;

            #region QueryMultiple方法

            Console.WriteLine("开始执行GetUseQueryMultiple()：");
            dapperDemoList = clientQueryMultiple.GetUseQueryMultiple();
            if (dapperDemoList != null && dapperDemoList.Any())
            {
                dapperDemo = dapperDemoList[0];
                if (dapperDemo.ParentDapperDemo != null)
                {
                    Console.WriteLine("执行成功。第1条记录的Name为{0}（ID为{1}，Type为{2}），其ParentName为{3}（ParentID为{4}，ParentType为{5}）。", dapperDemo.DapperDemoName, dapperDemo.ID, dapperDemo.Type, dapperDemo.ParentDapperDemo.DapperDemoParentName, dapperDemo.ParentDapperDemo.ParentID, dapperDemo.ParentDapperDemo.ParentType);
                }
                else
                {
                    Console.WriteLine("执行成功。第1条记录的Name为{0}（ID为{1}，Type为{2}），但没有其ParentDapperDemo。", dapperDemo.DapperDemoName, dapperDemo.ID, dapperDemo.Type);
                }
            }
            else
            {
                Console.WriteLine("执行成功：没记录。");
            }
            Console.ReadLine();        

            #endregion
        }

        public IList<DapperDemoEntity> GetUseQueryMultiple()
        {
            List<DapperDemoParentEntity> parentDapperDemoList = null;
            List<DapperDemoEntity> dapperDemoList = null;

            using (IDbConnection connection = Common.OpenConnection())
            {
                const string query =
@"SELECT ID AS ParentID, [Name] AS DapperDemoParentName, Type, ModifiedDate 
FROM dbo.DapperNETDemo WITH(NOLOCK)
WHERE ParentID < 1;

SELECT child.ID, child.Name AS DapperDemoName, child.Type, child.ModifiedDate, parent.ID AS ParentID, parent.Name AS DapperDemoParentName, parent.Type AS ParentType
FROM dbo.DapperNETDemo child WITH(NOLOCK)
LEFT JOIN dbo.DapperNETDemo parent WITH(NOLOCK) ON parent.ID = child.ParentID
WHERE parent.ID IS NOT NULL";

                using (SqlMapper.GridReader grid = connection.QueryMultiple(query))
                {
                    parentDapperDemoList = grid.Read<DapperDemoParentEntity>().ToList();
                    dapperDemoList = grid.Read<DapperDemoEntity, DapperDemoParentEntity, DapperDemoEntity>((child, parent) => { child.ParentDapperDemo = parent; child.ParentID = parent.ParentID; return child; }, splitOn: "ParentID").ToList<DapperDemoEntity>();
                }

                return dapperDemoList;
            }
        }
    }
}
