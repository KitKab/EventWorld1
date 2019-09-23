using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;
using WebMvc.ViewModels;

namespace WebMvc.ViewModels
{
    public class CatalogIndexViewModel
    {
        public  PaginationInfo PaginationInfo { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<EventItem> EventItems { get; set; }

        public int? CityFilterApplied { get; set; }
        public int? TypeFilterApplied { get; set; }
    }
}
