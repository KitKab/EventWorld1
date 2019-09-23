using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventCatalogApi.Data;
using EventCatalogApi.Domain;
using EventCatalogApi.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventCatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly EventContext _context;
        private readonly IConfiguration _config;

        public CatalogController(EventContext context,
           IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //GET api/catalog/items?pageSize=10&pageIndex=2
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items(
            [FromQuery] int pageSize = 4,
            [FromQuery] int pageIndex = 0)
        {
            //get total count
            var itemsCount = await _context.EventItems.LongCountAsync();

            var items = await _context.EventItems
                .OrderBy(c => c.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();
            items = ChangePictureUrl(items);
            var model = new PaginatedItemsViewModel<EventItem>
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Count = itemsCount,
                Data = items
            };
            return Ok(model);

        }

        // GET api/Catalog/Items/type/1/brand/null[?pageSize=4&pageIndex=0]

        [HttpGet]

        [Route("[action]/city/{EventCityId}/type/{EventTypeId}")]
        public async Task<IActionResult> Items(int? eventTypeId,
            int? eventCityId,
            [FromQuery] int pageSize = 4,
            [FromQuery] int pageIndex = 0)

        {

            var root = (IQueryable<EventItem>)_context.EventItems;
            if (eventTypeId.HasValue)
            {
                root = root.Where(c => c.EventTypeId == eventTypeId);
            }

            if (eventCityId.HasValue)
            {
                 root = root.Where(c => c.EventCityId == eventCityId);

            }

            var totalItems = await root.LongCountAsync();

            var itemsOnPage = await root

                              .OrderBy(c => c.Name)
                              .Skip(pageSize * pageIndex)
                              .Take(pageSize)
                                .ToListAsync();

            itemsOnPage = ChangePictureUrl(itemsOnPage);
            var model = new PaginatedItemsViewModel<EventItem>

            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Count = totalItems,
                Data = itemsOnPage

            };

            return Ok(model);
        }

        private List<EventItem> ChangePictureUrl(List<EventItem> items)
        {
            items.ForEach(
                c => c.PictureUrl =
                c.PictureUrl
                .Replace("http://externalcatalogbaseurltobereplaced", _config["ExternalCatalogBaseUrl"]));
            return items;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> EventTypes()
        {
            var items = await _context.EventTypes.ToListAsync();
            return Ok(items);
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> EventCities()
        {
            var items = await _context.EventCities.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemsById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Incorrect Id");
            }
            var item = await _context.EventItems
                .SingleOrDefaultAsync(c => c.Id == id);
            if (item == null)
            {
                return NotFound("Event Item not found");

            }
            item.PictureUrl = item.PictureUrl
                .Replace("http://externalcatalogbaseurltobereplaced"
                , _config["ExternalCatalogBaseUrl"]);
            return Ok(item);
        }

        //GET api/Catalog/items/withname/Wonder?pageSize=2&pageIndex=0

        [HttpGet]

        [Route("[action]/withname/{name:minlength(1)}")]

        public async Task<IActionResult> Items(string name,

            [FromQuery] int pageSize = 4,

            [FromQuery] int pageIndex = 0)

        {

            var totalItems = await _context.EventItems

                               .Where(c => c.Name.StartsWith(name))

                              .LongCountAsync();

            var itemsOnPage = await _context.EventItems

                              .Where(c => c.Name.StartsWith(name))

                              .OrderBy(c => c.Name)

                              .Skip(pageSize * pageIndex)

                              .Take(pageSize)

                              .ToListAsync();

            itemsOnPage = ChangePictureUrl(itemsOnPage);

            var model = new PaginatedItemsViewModel<EventItem>

            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Count = totalItems,
                Data = itemsOnPage

            };
            return Ok(model);
        }

        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct(
        [FromBody] EventItem product)
        {
            var item = new EventItem
            {
                EventCityId = product.EventCityId,
                EventTypeId = product.EventTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureUrl = product.PictureUrl,
                Price = product.Price
            };

            _context.EventItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemsById), new { id = item.Id });

        }

        [HttpPut]

        [Route("items")]

        public async Task<IActionResult> UpdateProduct(

      [FromBody] EventItem productToUpdate)

        {
            var eventItem = await _context.EventItems
                              .SingleOrDefaultAsync
                              (i => i.Id == productToUpdate.Id);
            if (eventItem == null)
            {
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found." });

            }

            eventItem = productToUpdate;
            _context.EventItems.Update(eventItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemsById), new { id = productToUpdate.Id });

        }

        [HttpDelete]

        [Route("{id}")]

        public async Task<IActionResult> DeleteProduct(int id)

        {
            var product = await _context.EventItems
                .SingleOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
             return NotFound();

            }

            _context.EventItems.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();



        }


    }
}