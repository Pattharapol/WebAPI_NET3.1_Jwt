using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_NET3_1.Common;
using WebAPI_NET3_1.Models;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI_NET3_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogueController : ControllerBase
    {
        private readonly MyContext _context;
        public CatalogueController(MyContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        [Route("authors")]
        // api/catalogue/authors?page=1&size=5
        // each page contain 5 records
        public async Task<IActionResult> GetAllAuthors([FromQuery]QueryParameters queryParameters)
        {
            IQueryable<Author> authors =  _context.Authors;

            authors = queryParameters.Size > 0 ? authors.Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size) : authors;

            return Ok(await authors.ToArrayAsync());
        }

        [HttpGet]
        [Route("authors/{id:int}")]
        public IActionResult GetAuthor(int id)
        {
            var author = _context.Authors.Find(id);
            if (author == null) return NotFound();
            return Ok(author);
        }



        [HttpGet]
        [Route("books")]
        [Authorize]
        
        public async Task<IActionResult> GetAllBooks([FromQuery] BookQueryParameters queryParameters)
        {
            IQueryable<Book> books = _context.Books;

            if(queryParameters.MinPrice >= 0 && queryParameters.MaxPrice > 0)
            {
                books = books.Where(b => b.Price >= queryParameters.MinPrice && b.Price <= queryParameters.MaxPrice);
            }

            if(queryParameters.Genre != null)
            {
                books = books.Where(b => string.Equals(b.BookGenre.ToString(), queryParameters.Genre, StringComparison.CurrentCultureIgnoreCase));
            }

            if(queryParameters.Title != null)
            {
                books = books.Where(b => b.Title.ToLower().Contains(queryParameters.Title.ToLower()));
            }

            if(queryParameters.SortBy != null)
            {
                var sortOrder = queryParameters.IsAscending ? "ascending" : "descending";
                books = books.AsQueryable().OrderBy(queryParameters.SortBy + " " + sortOrder);
            }

            books = queryParameters.Size > 0 ? books.Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size) : books;

            return Ok(await books.ToArrayAsync());
        }
    }
}
