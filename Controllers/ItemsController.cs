using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.DTO;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    
    // GET /items
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository repository)
        {
            this._itemsRepository = repository;
        }

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = _itemsRepository.GetItems().Select(item => item.AsDto());
            return items;
        }
        
        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = _itemsRepository.GetItem(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }
        
        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            
            _itemsRepository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }
        
        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateIte(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = _itemsRepository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            
            _itemsRepository.UpdateItem(updatedItem);

            return NoContent();
        }
        
        // DELETE /Items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _itemsRepository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            _itemsRepository.DeleteItem(existingItem.Id);
            return NoContent();
        }
    }
}