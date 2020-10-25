# NovaPagedList
This library is the spiritual successor to [troygoode](https://github.com/troygoode)'s PagedList project, which is no longer maintained. However, NovaPagedList, made from scratch with the latest .NET technologies, targets today's projects which support .NET Standard 2.0 or newer platforms. This library is highly optimized to be a lightweight but modern and robust tool for any LINQ pagination uses.

## Examples

### Synchronous query

```csharp
public class ProductService : IProductService
{
    public IPagedList<Product> ListProducts(int page, int pageSize)
    {
        return dbContext.Products.OrderBy(p => p.Name).ToPagedList(page, pageSize);
    }
}
```

### Asynchronous approach

```csharp
public class ProductService : IProductService
{
    public Task<IPagedList<Product>> ListProductsAsync(int page, int pageSize)
    {
        return dbContext.Products.OrderBy(p => p.Name).ToPagedListAsync(page, pageSize);
    }
}
```

### Using IAsyncEnumerable with Entity Framework Core

```csharp
public class ProductService : IProductService
{
    public Task<IAsyncPagedList<Product>> ListProductsAsync(int page, int pageSize)
    {
        return dbContext.Products.OrderBy(p => p.Name).ToAsyncPagedListAsync(page, pageSize);
    }
}

public class ProductConsumer
{
    public async Task ListProductsAsync(int page, int pageSize)
    {
        var products = await productService.ListProductsAsync(page, pageSize);

        await foreach (var product in products)
        {
            Console.WriteLine($"{product.Name}: {product.Price:c}");
        }
    }
}
```
