namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Base class for paginated responses.
/// </summary>
/// <typeparam name="T">The type of items in the paginated response</typeparam>
public abstract class PaginatedResponseBase<T>
{
    /// <summary>
    /// Gets or sets the collection of items.
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets whether there is a previous page.
    /// </summary>
    public bool HasPrevious => CurrentPage > 1;

    /// <summary>
    /// Gets whether there is a next page.
    /// </summary>
    public bool HasNext => CurrentPage < TotalPages;
}