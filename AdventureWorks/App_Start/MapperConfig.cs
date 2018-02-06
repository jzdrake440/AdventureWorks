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
                c.CreateMap<Customer, CustomerDetailDto>();
                c.CreateMap<Store, StoreDto>();
                c.CreateMap<Store, StoreDetailDto>();
                c.CreateMap<SalesTerritory, SalesTerritoryDto>();
                c.CreateMap<SalesTerritory, SalesTerritoryDetailDto>();
                c.CreateMap<Person, PersonDto>();
                c.CreateMap<Person, PersonDetailDto>();
            }).CreateMapper();

        }
    }
}