using Api.Controllers;
using Application.Dto.Request;
using Application.Dto.Request.Filters;
using Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IBookService
{
    Task<IEnumerable<BookResponse>> GetBooks(BookFilters filters);

    Task<BookResponse> CreateBook(CreateBookRequest request);

    Task UpdateBook(Guid id, UpdateBookRequest request);
}
