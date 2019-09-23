using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebMvc.Infrastructure;
using WebMvc.Models;

namespace WebMvc.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpClient _client;
        private readonly string _baseUri;

        public CatalogService(IHttpClient httpClient, IConfiguration config)
        {
            _client = httpClient;
         // _baseUri = $"{config["CatalogUrl"]}/api/eventcatalog"; // recheck
            _baseUri = $"{config["CatalogUrl"]}/api/catalog/";
        }

        public async Task<Catalog> GetCatalogItemsAsync(int page, int size, int? city, int? type)
        {
            var catalogItemsUri = ApiPaths.Catalog
                .GetAllCatalogItems(_baseUri, page, size, city, type);
            var dataString = await _client.GetStringAsync(catalogItemsUri);
            var response = JsonConvert.DeserializeObject<Catalog>(dataString);
            return response;

        }

        public async Task<IEnumerable<SelectListItem>> GetCitiesAsync()
        {
            var cityUri = ApiPaths.Catalog.GetAllCities(_baseUri);
            var dataString = await _client.GetStringAsync(cityUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected= true
               
                }
            };
            var cities = JArray.Parse(dataString);
            foreach(var city in cities)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = city.Value<string>("id"),
                        Text = city.Value<string>("city")
                    }
                    );
            }
            return items;

        }



        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typeUri = ApiPaths.Catalog.GetAllTypes(_baseUri);
            var dataString = await _client.GetStringAsync(typeUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value=null,
                    Text="All",
                    Selected= true

                }
            };
            var types = JArray.Parse(dataString);
            foreach (var type in types)
            {
                items.Add(
                    new SelectListItem
                    {
                        Value = type.Value<string>("id"),
                        Text = type.Value<string>("type")
                    }
                    );
            }
            return items;
        }
    }
}
