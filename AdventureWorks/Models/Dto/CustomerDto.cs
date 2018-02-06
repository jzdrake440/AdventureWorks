using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks.Models.Dto
{
    public class CustomerDto
    {
        public int? CustomerID { get; set; }
        public int? PersonID { get; set; }
        public int? StoreID { get; set; }
        public int? TerritoryID { get; set; }
        public string AccountNumber { get; set; }
        
        public DateTime ModifiedDate { get; set; }

        //public virtual Person Person { get; set; }
        //public virtual SalesTerritory SalesTerritory { get; set; }
        //public virtual Store Store { get; set; }
        
        //public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}