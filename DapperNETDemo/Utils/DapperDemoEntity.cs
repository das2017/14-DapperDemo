using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperNETDemo.Utils
{
    public class DapperDemoEntity
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string DapperDemoName { get; set; }
        public Common.Type Type { get; set; }
        public DateTime ModifiedDate { get; set; }        
        public DapperDemoParentEntity ParentDapperDemo { get; set; }
    }

    public class DapperDemoParentEntity
    {
        public int ParentID { get; set; }
        public string DapperDemoParentName { get; set; }
        public Common.Type ParentType { get; set; }
    }    
}
