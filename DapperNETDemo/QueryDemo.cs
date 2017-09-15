using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Dapper;
using DapperNETDemo.Utils;

namespace DapperNETDemo
{
    public class QueryDemo
    {
        static void Main2(string[] args)
        //static void Main(string[] args)
        {
            QueryDemo clientQuery = new QueryDemo();         

            IList<DapperDemoEntity> dapperDemoList = null;
            DapperDemoEntity dapperDemo = null;
            dynamic dapperDemoDynamic = null;          

            DateTime modifiedDate = DateTime.Now;

            #region Query方法
     
            #region 获得列表

            Console.WriteLine("开始执行GetDapperDemoList()：");
            dapperDemoList = clientQuery.GetDapperDemoList().ToList<DapperDemoEntity>();
            Console.WriteLine("执行成功：共有{0}条记录。", dapperDemoList.Count());
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 获得列表（需要返回列表中的每个对象所包含的另1个对象）

            Console.WriteLine("开始执行GetDapperDemoWithParentList()：");
            dapperDemoList = clientQuery.GetChildDapperDemoWithParentList().ToList<DapperDemoEntity>();
            Console.WriteLine("执行成功：共有{0}条记录。", dapperDemoList.Count());
            if (dapperDemoList != null && dapperDemoList.Any() && dapperDemoList[0].ParentDapperDemo != null)
            {
                Console.WriteLine("其中，第1条记录的Name为：{0}，Type为：{1}，ParentName为：{2}。", dapperDemoList[0].DapperDemoName, dapperDemoList[0].ParentDapperDemo.DapperDemoParentName, dapperDemoList[0].Type);
            }            
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 获得单个对象（没记录的情况）

            Console.WriteLine("开始执行GetDapperDemo()：");
            dapperDemo = clientQuery.GetDapperDemo(2000, null);
            if (dapperDemo != null)
            {
                Console.WriteLine("执行成功：ID为{0}，Name为{1}，Type为{2}。", dapperDemo.ID, dapperDemo.DapperDemoName, dapperDemo.Type);
            }
            else
            {
                Console.WriteLine("执行成功：没有ID为2000的DapperDemo。");
            }
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            List<DapperDemoEntity> list = clientQuery.GetDapperDemoList().ToList();

            #region 获得单个对象（有记录的情况）

            Console.WriteLine("开始执行GetDapperDemo()：");
          
            if (list != null && list.Count > 0)
            {
                dapperDemo = clientQuery.GetDapperDemo(list[0].ID, null);
            }            
            if (dapperDemo != null)
            {
                Console.WriteLine("执行成功：ID为{0}，Name为{1}，Type为{2}。", dapperDemo.ID, dapperDemo.DapperDemoName, dapperDemo.Type);
            }
            else
            {
                Console.WriteLine("执行成功：没有记录。");
            }

            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 获得动态对象（没记录的情况）

            Console.WriteLine("开始执行GetDapperDemoDynamic()：");
            dapperDemoDynamic = clientQuery.GetDapperDemoDynamic(2000, null);
            if (dapperDemoDynamic != null)
            {              
                Console.WriteLine("执行成功：ID为{0}，Name为{1}，Type为{2}。", dapperDemoDynamic.ID, dapperDemoDynamic.DapperDemoName, dapperDemoDynamic.Type);
            }
            else
            {
                Console.WriteLine("执行成功：没有ID为2000的DapperDemo。");
            }
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 获得动态对象（有记录的情况）

            Console.WriteLine("开始执行GetDapperDemoDynamic()：");
            if (list != null && list.Count > 0)
            {
                dapperDemoDynamic = clientQuery.GetDapperDemo(list[0].ID, null);
            }           
            if (dapperDemoDynamic != null)
            {
                Console.WriteLine("执行成功：ID为{0}，Name为{1}，Type为{2}。", dapperDemoDynamic.ID, dapperDemoDynamic.DapperDemoName, dapperDemoDynamic.Type);
            }
            else
            {
                Console.WriteLine("执行成功：没有记录。");
            }

            #endregion

            Console.ReadLine();       

            #endregion
        }     

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DapperDemoEntity> GetDapperDemoList()
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string query =
@"SELECT ID, [Name] AS DapperDemoName, ParentID, [Type], ModifiedDate 
  FROM dbo.DapperNETDemo WITH(NOLOCK)
  ORDER BY ModifiedDate DESC";
                return connection.Query<DapperDemoEntity>(query);
            }
        }

        /// <summary>
        /// 返回列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 若返回列表中的每个对象所包含的另1个对象也需要返回，则需要用到splitOn参数。
        /// 然而，如果第2个对象的分割列为Id，则可省略splitOn参数。      
        /// </remarks>
        public IEnumerable<DapperDemoEntity> GetChildDapperDemoWithParentList()
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string query =
@"SELECT child.ID, child.Name AS DapperDemoName, child.[Type], child.ModifiedDate, 
       parent.ID AS ParentID, parent.Name AS DapperDemoParentName, parent.[Type] AS ParentType
  FROM dbo.DapperNETDemo child WITH(NOLOCK)
       LEFT JOIN dbo.DapperNETDemo parent WITH(NOLOCK) ON parent.ID = child.ParentID
  WHERE parent.ID IS NOT NULL
  ORDER BY child.ModifiedDate DESC";
                return connection.Query<DapperDemoEntity, DapperDemoParentEntity, DapperDemoEntity>(query
                    , (child, parent) => { child.ParentDapperDemo = parent; child.ParentID = parent.ParentID ;return child; }, splitOn: "ParentID");
            }
        }

        /// <summary>
        /// 返回单个对象
        /// </summary>
        /// <param name="dapperDemoId"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        public DapperDemoEntity GetDapperDemo(int dapperDemoId, bool? isParent)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                string query =
@"SELECT ID, [Name] AS DapperDemoName, ParentID, [Type], ModifiedDate 
  FROM dbo.DapperNETDemo WITH(NOLOCK)
  WHERE ID = @ID";
                if (isParent.HasValue)
                {
                    if (isParent.Value)
                    {
                        query += " AND ParentID < 1";
                    }
                    else
                    {
                        query += " AND ParentID > 0";
                    }                    
                }
                //ID will be mapped to the param @ID:
                return connection.Query<DapperDemoEntity>(query, new { ID = dapperDemoId }).SingleOrDefault();                
            }
        }

        /// <summary>
        /// 返回动态对象
        /// </summary>
        /// <param name="dapperDemoId"></param>
        /// <param name="isParent"></param>
        /// <returns></returns>
        public dynamic GetDapperDemoDynamic(int dapperDemoId, bool? isParent)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                string query =
@"SELECT ID, [Name] AS DapperDemoName, ParentID, [Type], ModifiedDate 
  FROM dbo.DapperNETDemo WITH(NOLOCK)
  WHERE ID = @ID";
                if (isParent.HasValue)
                {
                    if (isParent.Value)
                    {
                        query += " AND ParentID < 1";
                    }
                    else
                    {
                        query += " AND ParentID > 0";
                    }
                }
                return connection.Query(query, new { ID = dapperDemoId }).SingleOrDefault();
            }
        }        
    }
}
