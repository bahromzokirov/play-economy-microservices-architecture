using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> Items =
    [
        new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of health", 5, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Iron Sword", "A basic sword made of iron", 7, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Steel Shield", "Provides protection against attacks", 5, DateTimeOffset.UtcNow)
    ];

    [HttpGet]
    public IEnumerable<ItemDto> Get()
    {
        return Items;
    }

    [HttpGet("{id}")]
    public ItemDto? GetById(Guid id)
    {
        var item = Items.SingleOrDefault(i => i.Id == id);
        return item;
    }

    [HttpPost]
    public ActionResult<ItemDto> Post(CreateItemDto createItemDto)
    {
        var item  = new ItemDto(
            Guid.NewGuid(),
            createItemDto.Name,
            createItemDto.Description,
            createItemDto.Price,
            DateTimeOffset.UtcNow);
        Items.Add(item);
       
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = Items.SingleOrDefault(i => i.Id == id);
        
        if (existingItem != null)
        {
            var updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };
        
            var index = Items.FindIndex(itemDto => itemDto.Id == id);
            Items[index] = updatedItem;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var index = Items.FindIndex(itemDto => itemDto.Id == id);
        Items.RemoveAt(index);
        
        return NoContent();
    }
}