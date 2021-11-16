using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using Catalog.Entities;
using Catalog.DTO;
using System.Threading.Tasks;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        //GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        { 
            return  (await repository.GetItemsAsync())
                        .Select(item => item.AsDTO());
        }

        //GET /items/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) 
        {
            var item =  await repository.GetItemAsync(id);

            if (item is null) {
                return NotFound();
            }
            return item.AsDTO();
        }

        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new{id = item.Id}, item.AsDTO());
        }
         
        // PUT /items/:id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto item) 
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null) 
                return NotFound();
            
            Item UpdateItem = existingItem with {
                Name = item.Name,
                Price = item.Price
            };
            await repository.UpdateItemAsync(id, UpdateItem);

            return NoContent();
        }
    
    
        // DELETE /items/:id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {   
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is  null) 
               return NotFound();
            
            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }   
}   