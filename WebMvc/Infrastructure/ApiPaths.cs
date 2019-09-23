using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Infrastructure
{
    public class ApiPaths
    {
        public static class Catalog
        {
            public static string GetAllTypes(string baseUri)
            {
                return $"{baseUri}eventtypes";
            }
            public static string GetAllCities(string baseUri)
            {
                return $"{baseUri}eventcities";
            }

            public static string GetAllCatalogItems(string baseUri,int page
                ,int take,int? city,int? type)
            {
                var filterQs = string.Empty;
                if(city.HasValue || type.HasValue)
                {
                    var citiesQs = (city.HasValue) ? city.Value.ToString() : "null";
                    var typesQs = (type.HasValue) ? type.Value.ToString() : "null";
                   // filterQs = $"/type/{typesQs}/city{citiesQs}";
                    filterQs = $"/city/{citiesQs}/type{typesQs}";
                }

                return $"{baseUri}items{filterQs}?pageIndex={page}&pageSize={take}";
            }
        }

        public static class Basket
        {
            public static string GetBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
            public static string UpdateBasket(string baseUri)
            {
                return baseUri;
            }

            public static string CleanBasket(string baseUri, string basketId)
            {
                return $"{baseUri}/{basketId}";
            }
        }

        public static class Order
        {
            public static string GetOrder(string baseUri, string orderId)
            {
                return $"{baseUri}/{orderId}";
            }
            public static string GetOrders(string baseUri)
            {
                return baseUri;
            }

            public static string AddNewOrder(string baseUri)
            {
                return $"{baseUri}/new";
            }
        }
    }
}
