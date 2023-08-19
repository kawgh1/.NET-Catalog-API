using System;

namespace Catalog.DTO
{
    public record ItemDto
    {
        public Guid Id { get; init; } // init only allows setting property values at initialization
        public string Name { get; set; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}