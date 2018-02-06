using AdventureWorks.Models.Dto;
using AdventureWorks.Models.Entity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdventureWorks
{
    public static class MapperConfig
    {
        public static IMapper CreateMapper()
        {
            return new MapperConfiguration(c =>
            {
                c.CreateMap<Customer, CustomerDto>();
            }).CreateMapper();

        }
    }
}