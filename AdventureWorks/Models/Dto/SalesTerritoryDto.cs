﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks.Models.Dto
{
    public class SalesTerritoryDto
    {
        public int? TerritoryID { get; set; }
        public string Name { get; set; }
        public string CountryRegionCode { get; set; }
        public string Group { get; set; }
    }

    public class SalesTerritoryDetailDto
    {
        public int? TerritoryID { get; set; }
        public string Name { get; set; }
        public string CountryRegionCode { get; set; }
        public string Group { get; set; }
        public decimal SalesYTD { get; set; }
        public decimal SalesLastYear { get; set; }
        public decimal CostYTD { get; set; }
        public decimal CostLastYear { get; set; }
        public DateTime ModifiedDate { get; set; }

        //public virtual CountryRegion CountryRegion { get; set; }
        //public virtual ICollection<StateProvince> StateProvinces { get; set; }
        public virtual ICollection<CustomerDto> Customers { get; set; }
        //public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
        //public virtual ICollection<SalesPerson> SalesPersons { get; set; }
        //public virtual ICollection<SalesTerritoryHistory> SalesTerritoryHistories { get; set; }
    }
}