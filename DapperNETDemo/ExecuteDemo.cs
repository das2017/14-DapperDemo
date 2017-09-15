using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Dapper;
using DapperNETDemo.Utils;

namespace DapperNETDemo
{
    public class ExecuteDemo
    {
        //static void Main1(string[] args)
        static void Main(string[] args)
        {            
            ExecuteDemo clientExecute = new ExecuteDemo();
            QueryDemo clientQuery = new QueryDemo();
            IList<DapperDemoEntity> dapperDemoList = null;
            DapperDemoEntity dapperDemo = null;
            int rowAffected = -1;
            int newDapperDemoId = -1;

            int mainId = -1;
            
            #region Execute方法

            #region 新增单条记录1

            Console.WriteLine("开始执行InsertDapperDemo()：");
            dapperDemo = new DapperDemoEntity()
            {
                ParentID = 0,
                DapperDemoName = "广州市",
                Type = Common.Type.A,
                ModifiedDate = DateTime.Now
            };            
            dapperDemo.ID = clientExecute.InsertDapperDemo(dapperDemo);
            newDapperDemoId = dapperDemo.ID;
            mainId = newDapperDemoId;
            if (newDapperDemoId > 0)
            {
                Console.WriteLine("新增成功：新增记录的ID为{0}，ParentID为{1}，Name为{2}，Type为{3}。", newDapperDemoId, dapperDemo.ParentID, dapperDemo.DapperDemoName, dapperDemo.Type);
            }
            else
            {
                Console.WriteLine("新增失败。");
            }

            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 新增单条记录2

            Console.WriteLine("开始执行InsertDapperDemo()：");
            dapperDemo = new DapperDemoEntity()
            {
                ParentID = newDapperDemoId,
                DapperDemoName = "越秀区",
                Type = Common.Type.B,
                ModifiedDate = DateTime.Now
            };
            dapperDemo.ID = clientExecute.InsertDapperDemo(dapperDemo);
            newDapperDemoId = dapperDemo.ID;
            if (newDapperDemoId > 0)
            {
                Console.WriteLine("新增成功：新增记录的ID为{0}，ParentID为{1}，Name为{2}，Type为{3}。", newDapperDemoId, dapperDemo.ParentID, dapperDemo.DapperDemoName, dapperDemo.Type);
            }
            else
            {
                Console.WriteLine("新增失败。");
            }

            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");            

            #region 更新单条记录

            Console.WriteLine("开始执行UpdateDapperDemo()：");

            dapperDemoList = clientQuery.GetChildDapperDemoWithParentList().ToList();       
            if (dapperDemoList != null && dapperDemoList.Count > 0)
            {
                dapperDemo = dapperDemoList[0];
                Console.WriteLine("将更新记录ID为{0}的Name【{1}】。", dapperDemo.ID, dapperDemo.DapperDemoName);
                Console.ReadLine();

                dapperDemo.DapperDemoName += "_updated_" + dapperDemo.ID;
                dapperDemo.ModifiedDate = DateTime.Now;
                rowAffected = clientExecute.UpdateDapperDemo(dapperDemo);

                if (rowAffected == 1)
                {
                    int id = dapperDemo.ID;
                    dapperDemo = clientQuery.GetDapperDemo(id, false);
                    if (dapperDemo != null)
                    {
                        Console.WriteLine("更新成功：ID为{0}这个的Name已被更新为【{1}】。", dapperDemo.ID, dapperDemo.DapperDemoName);
                    }
                }
                else
                {
                    Console.WriteLine("更新失败。");
                }
            }
            else
            {
                Console.WriteLine("没有DapperDemo被更新。");
            }            
            
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 删除单条记录

            Console.WriteLine("开始执行DeleteDapperDemo()：");
            
            dapperDemoList = clientQuery.GetChildDapperDemoWithParentList().ToList();
            if (dapperDemoList != null && dapperDemoList.Count > 0)
            {
                dapperDemo = dapperDemoList[0];
                Console.WriteLine("将删除ID为{0}的记录（其Name为{1}）。", dapperDemo.ID, dapperDemo.DapperDemoName);
                Console.ReadLine();

                rowAffected = clientExecute.DeleteDapperDemo(dapperDemo);

                dapperDemo = clientQuery.GetDapperDemo(dapperDemo.ID, false);
                if (rowAffected == 1 && dapperDemo == null)
                {
                    Console.WriteLine("删除成功。");
                }
                else
                {
                    Console.WriteLine("删除失败。");
                }
            }
            else
            {
                Console.WriteLine("没有DapperDemo被删除。");
            }
            
            
            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 批量新增

            Console.WriteLine("开始执行InsertDapperDemoList()：");
            Console.ReadLine();

            DateTime modifiedDate = DateTime.Now;           

            dapperDemoList = new List<DapperDemoEntity>();
            dapperDemo = new DapperDemoEntity()
            {
                ParentID = 0,
                DapperDemoName = "上海市",
                Type = Common.Type.C,
                ModifiedDate = modifiedDate
            };
            dapperDemoList.Add(dapperDemo);            
            dapperDemo = new DapperDemoEntity()
            {
                ParentID = mainId,
                DapperDemoName = "天河区",
                Type = Common.Type.B,
                ModifiedDate = modifiedDate
            };            
            dapperDemoList.Add(dapperDemo);
            dapperDemo = new DapperDemoEntity()
            {
                ParentID = mainId,
                DapperDemoName = "海珠区",
                Type = Common.Type.A,
                ModifiedDate = modifiedDate
            };            
            dapperDemoList.Add(dapperDemo);

            rowAffected = clientExecute.InsertDapperDemoList(dapperDemoList);
            if (rowAffected == 3)
            {
                Console.WriteLine("共{0}条记录被批量新增。", rowAffected);
            }
            else
            {
                Console.WriteLine("批量新增失败。");
            }

            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 批量更新

            Console.WriteLine("开始执行UpdateDapperDemoList()：");

            modifiedDate = DateTime.Now;

            List<DapperDemoEntity> childList = clientQuery.GetChildDapperDemoWithParentList().ToList();
            if (childList != null && childList.Count > 0)
            {
                string updatedIDs = string.Empty;

                dapperDemoList = new List<DapperDemoEntity>();

                dapperDemo = childList[0];
                dapperDemo.DapperDemoName += "_updatedList_" + dapperDemo.ID;
                dapperDemo.ModifiedDate = modifiedDate;
                dapperDemoList.Add(dapperDemo);
                updatedIDs = dapperDemo.ID.ToString();

                if (childList.Count > 1)
                {
                    dapperDemo = childList[1];
                    dapperDemo.DapperDemoName += "_updatedList_" + dapperDemo.ID;
                    dapperDemo.ModifiedDate = modifiedDate;
                    dapperDemoList.Add(dapperDemo);

                    updatedIDs += "," + dapperDemo.ID.ToString();
                }

                Console.WriteLine("将被更新的记录ID有【{0}】。", updatedIDs);
                Console.ReadLine();

                rowAffected = clientExecute.UpdateDapperDemoList(dapperDemoList);
                if (rowAffected > 0)
                {
                    Console.WriteLine("共{0}条记录被批量更新，被更新的记录ID有【{1}】。", rowAffected, updatedIDs);
                }
                else
                {
                    Console.WriteLine("没有记录被更新。");
                }
            }
            else
            {
                Console.WriteLine("没有记录被更新。");
            }

            Console.ReadLine();

            #endregion

            Console.WriteLine("=======");

            #region 批量删除

            Console.WriteLine("开始执行DeleteDapperDemoList()：");

            modifiedDate = DateTime.Now;

            childList = clientQuery.GetChildDapperDemoWithParentList().ToList();
            if (childList != null && childList.Count > 0)
            {
                string deletedIDs = string.Empty;

                dapperDemo = childList[0];
                dapperDemoList.Add(dapperDemo);
                deletedIDs = dapperDemo.ID.ToString();

                if (childList.Count > 1)
                {
                    dapperDemo = childList[1];
                    dapperDemoList.Add(dapperDemo);

                    deletedIDs += "," + dapperDemo.ID.ToString();
                }

                Console.WriteLine("将被删除的记录ID有【{0}】。", deletedIDs);
                Console.ReadLine();

                rowAffected = clientExecute.DeleteDapperDemoList(dapperDemoList);
                if (rowAffected > 0)
                {
                    Console.WriteLine("共{0}条记录被批量删除，被删除的记录ID有【{1}】。", rowAffected, deletedIDs);
                }
                else
                {
                    Console.WriteLine("没有记录被删除。");
                }
            }
            else
            {
                Console.WriteLine("没有记录被删除。");
            }           

            Console.ReadLine();

            #endregion            

            Console.WriteLine("=======");

            #region 批量删除

            Console.WriteLine("开始执行DeleteDapperDemoList()：");

            List<DapperDemoEntity> list = clientQuery.GetDapperDemoList().ToList();
            if (list != null && list.Count > 0)
            {
                dapperDemo = list[0];
                if (dapperDemo.ParentID < 1)
                {
                    dapperDemo.ParentID = dapperDemo.ID;
                }

                Console.WriteLine("将被删除的记录ID为{0}以及其其下所有子记录。", dapperDemo.ParentID);
                Console.ReadLine();

                rowAffected = clientExecute.DeleteDapperDemoList(dapperDemo);
                if (rowAffected > 0)
                {
                    Console.WriteLine("共{0}条记录被批量删除，被删除的记录ID为{1}以及其其下所有子记录。", rowAffected, dapperDemo.ParentID);
                }
                else
                {
                    Console.WriteLine("没有记录被删除。");
                }
            }
            else
            {
                Console.WriteLine("没有记录被删除。");
            }

            #endregion

            Console.ReadLine();

            #endregion            
        }

        /// <summary>
        /// 新增单条记录
        /// </summary>
        /// <param name="dapperDemo"></param>
        /// <returns>返回新增的ID</returns>
        /// <remarks>注意：这个例子使用的是Query泛型方法，不是Execute方法</remarks>
        public int InsertDapperDemo(DapperDemoEntity dapperDemo)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql =
@"INSERT INTO dbo.DapperNETDemo(ParentID, [Name], [Type], ModifiedDate) VALUES (@ParentID, @DapperDemoName, @Type, @ModifiedDate);
  SELECT CAST(SCOPE_IDENTITY() AS INT)";

                int dapperDemoID = connection.Query<int>(sql, dapperDemo).Single();
                return dapperDemoID;
            }
        }

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <param name="dapperDemo"></param>
        /// <returns></returns>
        public int UpdateDapperDemo(DapperDemoEntity dapperDemo)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql =
@"UPDATE dbo.DapperNETDemo 
  SET ParentID = @ParentID, 
       [Name] = @DapperDemoName,
       [Type] = @Type,
       ModifiedDate = @ModifiedDate 
  WHERE ID = @ID";
                return connection.Execute(sql, dapperDemo);
            }
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <param name="dapperDemo"></param>
        /// <returns></returns>
        public int DeleteDapperDemo(DapperDemoEntity dapperDemo)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql = @"DELETE FROM dbo.DapperNETDemo WHERE ParentID > 0 AND ID = @ID";
                return connection.Execute(sql, dapperDemo);
            }
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks>没有用到Transaction</remarks>
        public int InsertDapperDemoList(IList<DapperDemoEntity> list)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql = @"INSERT INTO dbo.DapperNETDemo(ParentID, [Name], [Type], ModifiedDate) VALUES (@ParentID, @DapperDemoName, @Type, @ModifiedDate)";

                int rowsAffectd = connection.Execute(sql, list);
                return rowsAffectd;
            }
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks>没有用到Transaction</remarks>
        public int UpdateDapperDemoList(IList<DapperDemoEntity> list)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql =
@"UPDATE dbo.DapperNETDemo 
  SET ParentID = @ParentID, 
       [Name] = @DapperDemoName,
       [Type] = @Type,
       ModifiedDate = @ModifiedDate 
  WHERE ID = @ID";

                int rowsAffectd = connection.Execute(sql, list);
                return rowsAffectd;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks>没有用到Transaction</remarks>
        public int DeleteDapperDemoList(IList<DapperDemoEntity> list)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string sql = @"DELETE FROM dbo.DapperNETDemo WHERE ParentID > 0 AND ID = @ID";
                return connection.Execute(sql, list);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="dapperDemo"></param>
        /// <returns></returns>
        /// <remarks>用到了Transaction</remarks>
        public int DeleteDapperDemoList(DapperDemoEntity dapperDemo)
        {
            using (IDbConnection connection = Common.OpenConnection())
            {
                const string deleteChildSQL = @"DELETE FROM dbo.DapperNETDemo WHERE ParentID > 0 AND ParentID = @ParentID";
                const string deleteParentSQL = @"DELETE FROM dbo.DapperNETDemo WHERE ParentID < 1 AND ID = @ParentID";

                IDbTransaction transaction = connection.BeginTransaction();
                int rowsAffected = connection.Execute(deleteChildSQL, new { ParentID = dapperDemo.ParentID }, transaction);
                rowsAffected += connection.Execute(deleteParentSQL, new { ParentID = dapperDemo.ParentID }, transaction);
                transaction.Commit();
                return rowsAffected;
            }
        }  
    }
}
