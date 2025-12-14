using Api.Controllers;
using Application.Contracts;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using AutoMapper;
using Domain;
using Entities;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _bookRepo;
    private readonly IMapper _mapper;

    public BookService(
        IRepository<Book> bookRepo,
        IMapper mapper)
    {
        _bookRepo = bookRepo;
        _mapper = mapper;
    }

    public async Task<BookResponse> CreateBook(CreateBookRequest request)
    {
        await using var memoryStream = new MemoryStream();
        await request.CoverImage.CopyToAsync(memoryStream);
        var imageData = memoryStream.ToArray();

        var book = _mapper.Map<Book>(request);
        book.CoverImage = imageData;

        await _bookRepo.AddAsync(book);
        await _bookRepo.SaveChangesAsync();

        return _mapper.Map<BookResponse>(book);
    }

    public async Task<IEnumerable<BookResponse>> GetBooks(BookFilters filters)
    {
        var books = await _bookRepo.GetPagedAsync(
            filters.PageNumber,
            filters.PageSize,
            book =>
                (string.IsNullOrEmpty(filters.Title) || book.Title.Contains(filters.Title)) &&
                (string.IsNullOrEmpty(filters.AuthorName) || book.Authors.Any(x => x.Contains(filters.AuthorName))) &&
                (string.IsNullOrEmpty(filters.GenreName) || book.Genres.Any(x => x.Contains(filters.GenreName))) &&
                (string.IsNullOrEmpty(filters.PublisherName) || book.Publisher.Contains(filters.PublisherName)) &&
                (!filters.MinPrice.HasValue || book.Price >= filters.MinPrice.Value) &&
                (!filters.MaxPrice.HasValue || book.Price <= filters.MaxPrice.Value) &&
                (!filters.PublicationYearFrom.HasValue || book.PublicationYear >= filters.PublicationYearFrom.Value) &&
                (!filters.PublicationYearTo.HasValue || book.PublicationYear <= filters.PublicationYearTo.Value) &&
                (!filters.MinPageCount.HasValue || book.PageCount >= filters.MinPageCount.Value) &&
                (!filters.MaxPageCount.HasValue || book.PageCount <= filters.MaxPageCount.Value) &&
                (!filters.InStock.HasValue || book.Quantity > 0 == filters.InStock.Value) &&
                (!filters.MinRating.HasValue || book.Reviews.Average(r => r.Rating) >= filters.MinRating.Value)
        );

        return _mapper.Map<IEnumerable<BookResponse>>(books);
    }

    public async Task UpdateBook(Guid id, UpdateBookRequest request)
    {
        var book = await _bookRepo.GetByIdAsync(id);
        book.Description = request.Description ?? book.Description;
        book.Price = request.Price ?? book.Price;

        await _bookRepo.UpdateAsync(book);
        await _bookRepo.SaveChangesAsync();
    }
}
