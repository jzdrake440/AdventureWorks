﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks.Models.DataTables
{
    public class DataTableServerSideRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataTableColumn[] Columns { get; set; }
        public DataTableSearch Search { get; set; }

        public class DataTableColumn
        {
            public string Data { get; set; }
            public string Name { get; set; }
            public bool Searchable { get; set; }
            public bool Orderable { get; set; }
            public DataTableSearch Search { get; set; }
        }

        public class DataTableSearch
        {
            public string Value { get; set; }
            public bool Regex { get; set; }
        }
    }
}