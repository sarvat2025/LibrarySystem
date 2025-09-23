using LibrarySystem.Models;
using LibrarySystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LibrarySystem.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;
    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BookDto>> GetBooks()
    {
        var books = _context.Books.ToList();
        var result = books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Count = b.Count,
            IsAvailable = b.Count > 0
        }).ToList();

        return Ok(result);
    }
    [HttpGet("{id}")]
    public ActionResult<BookDto> GetBook(int id)
    {
        // var book = _context.Books.FirstOrDefault(b => b.Id == id);
        var book = _context.Books.Find(id);
        if (book == null) return NotFound();

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Count = book.Count,
            IsAvailable = book.Count > 0
        };
    }
    [HttpPost]
    public ActionResult<BookDto> PostBook(BookDto dto)
    {
        var book = new Book { Title = dto.Title, Author = dto.Author, Count = dto.Count };
        _context.Books.Add(book);
        _context.SaveChanges();

        dto.Id = book.Id;
        dto.IsAvailable = book.Count > 0;
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, dto);
    }
    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, BookDto dto)
    {
        var book = _context.Books.Find(id);
        if (book == null) return NotFound();

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.Count = dto.Count;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PatchBook(int id, [FromBody] JsonElement patch)
    {
        var book = _context.Books.Find(id);
        if (book == null) return NotFound();

        if (patch.TryGetProperty("title", out var title))
            book.Title = title.GetString();
        if (patch.TryGetProperty("author", out var author))
            book.Author = author.GetString();
        if (patch.TryGetProperty("count", out var count))
            book.Count = count.GetInt32();
       // if (patch.TryGetProperty("isAvailable", out var isAvailable))
         //   book.IsAvailable = isAvailable.GetBoolean();
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _context.Books.Include(b => b.BookReaders).FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound();
        _context.BookReaders.RemoveRange(book.BookReaders);
        _context.Books.Remove(book);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpGet("available")]
    public ActionResult<IEnumerable<BookDto>> GetAvailableBooks()
    {
        var books = _context.Books.Where(b => b.Count > 0).ToList();
        var result = books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Count = b.Count,
            IsAvailable = b.Count > 0
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}/history")]
    public ActionResult<BookHistoryDto> GetBookHistory(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null) return NotFound();

        var history = _context.BookReaders
            .Include(br => br.Reader)
            .Where(br => br.BookId == id)
            .OrderBy(br => br.CheckedOutAt)
            .ToList();

        
        var result = new BookHistoryDto
        {
            BookId = book.Id,
            Title = book.Title,
            Author = book.Author,
            Borrowers = history.Select(br => new ReaderBorrowDto
            {
                ReaderId = br.ReaderId,
                FullName = br.Reader?.FullName ?? string.Empty,
                Email = br.Reader?.Email ?? string.Empty,
                CheckedOutAt = br.CheckedOutAt,
                ReturnedAt = br.ReturnedAt
            }).ToList()
        };

        return Ok(result);
    }
}