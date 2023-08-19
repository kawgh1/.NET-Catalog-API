using System;

namespace Catalog.Entities
{
    // Record Types
    // - User for immutable objects
    // - with-expression support
    //  value-based equality support

    public record Item
    {
        public Guid Id { get; init; } // init only allows setting property values at initialization
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}