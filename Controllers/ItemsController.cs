using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController(IItemsRepository itemsRepository) : ControllerBase
{
    private readonly IItemsRepository _itemsRepository = itemsRepository;
    
    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetAsync()
    {
        var items = (await _itemsRepository.GetAllAsync())
            .Select(item => item.AsDto());
        
        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
    {
        var item = new Item
        {
            Name = createItemDto.Name,
            Description = createItemDto.Description,
            Price = createItemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        
        await _itemsRepository.CreateAsync(item);
        
        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = await _itemsRepository.GetAsync(id);

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;
        
        await _itemsRepository.UpdateAsync(existingItem);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await _itemsRepository.GetAsync(id);

        await _itemsRepository.RemoveAsync(item.Id);
        
        return NoContent();
    }
}