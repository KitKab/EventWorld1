using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service) => 
            _service = service;


        public async Task<IActionResult> Index(int? citiesFilterApplied,

            int? typesFilterApplied, int? page)
        { 
            var itemsOnPage = 3;
            var catalog = await _service.GetCatalogItemsAsync(page ?? 0,
                        itemsOnPage, citiesFilterApplied, typesFilterApplied); // checks if page is null
            var vm = new CatalogIndexViewModel
            {
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = itemsOnPage,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)

                },
                EventItems = catalog.Data,
                Cities = await _service.GetCitiesAsync(),
                Types = await _service.GetTypesAsync(),
                CityFilterApplied = citiesFilterApplied ?? 0,
                TypeFilterApplied = typesFilterApplied ?? 0
            };

            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage ==vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            return View(vm);
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

    }
}