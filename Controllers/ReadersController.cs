using LibrarySystem.DTOs;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LibrarySystem.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReadersController : ControllerBase
{
    private readonly LibraryContext _context;
    public ReadersController(LibraryContext context)
    {
        _context = context;
    }

    // GET: api/Readers
    [HttpGet]
    public ActionResult<IEnumerable<ReaderDto>> GetReaders()
    {
        var readers = _context.Readers
            .Include(r => r.BookReaders)
                .ThenInclude(br => br.Book)
            .ToList();

        var result = readers.Select(r => new ReaderDto
        {
            Id = r.Id,
            FullName = r.FullName,
            Email = r.Email,
            IsBlocked = r.IsBlocked,
            BorrowHistory = r.BookReaders.Select(br => new BorrowDto
            {
                BookId = br.BookId,
                Title = br.Book!.Title,
                Author = br.Book!.Author,
                CheckedOutAt = br.CheckedOutAt.ToString("yyyy-MM-dd HH:mm:ss"),
                ReturnedAt = br.ReturnedAt == null ? "Читатель не вернул книгу" : br.ReturnedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList()
        }).ToList();

        return Ok(result);
    }

    // GET: api/Readers/5
    [HttpGet("{id}")]
    public ActionResult<ReaderDto> GetReader(int id)
    {
        var reader = _context.Readers.Include(r => r.BookReaders).ThenInclude(br => br.Book).FirstOrDefault(r => r.Id == id);
        if (reader == null) return NotFound();

        var dto = new ReaderDto
        {
            Id = reader.Id,
            FullName = reader.FullName,
            Email = reader.Email,
            IsBlocked = reader.IsBlocked,
            BorrowHistory = reader.BookReaders.Select(br => new BorrowDto
            {
                BookId = br.BookId,
                Title = br.Book!.Title,
                Author = br.Book!.Author,
                CheckedOutAt = br.CheckedOutAt.ToString("yyyy-MM-dd HH:mm:ss"),
                ReturnedAt = br.ReturnedAt == null ? "Читатель не вернул книгу" : br.ReturnedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList()
        };
        return Ok(dto);
    }

    // POST: api/Readers
    [HttpPost]
    public ActionResult<ReaderDto> PostReader(ReaderDto dto)
    {
        var reader = new Reader { FullName = dto.FullName, Email = dto.Email, IsBlocked = dto.IsBlocked };
        _context.Readers.Add(reader);
        _context.SaveChanges();

        dto.Id = reader.Id;
        return CreatedAtAction(nameof(GetReader), new { id = reader.Id }, dto);
    }

    // PUT: api/Readers/5
    [HttpPut("{id}")]
    public IActionResult UpdateReader(int id, ReaderDto dto)
    {
        var reader = _context.Readers.Find(id);
        if (reader == null) return NotFound();

        reader.FullName = dto.FullName;
        reader.Email = dto.Email;
        reader.IsBlocked = dto.IsBlocked;
        _context.SaveChanges();
        return NoContent();
    }

    // PATCH: api/Readers/5
    [HttpPatch("{id}")]
    public IActionResult PatchReader(int id, [FromBody] JsonElement patch)
    {
        var reader = _context.Readers.Find(id);
        if (reader == null) return NotFound();

        if (patch.TryGetProperty("fullName", out var fullName))
            reader.FullName = fullName.GetString();
        if (patch.TryGetProperty("email", out var email))
            reader.Email = email.GetString();
        if (patch.TryGetProperty("isBlocked", out var isBlocked))
            reader.IsBlocked = isBlocked.GetBoolean();

        _context.SaveChanges();
        return NoContent();
    }
    // Block   /Unblock Reader
    [HttpPost("{id}/block")]
    public IActionResult BlockReader(int id)
    {
        var reader = _context.Readers.Find(id);
        if (reader == null) return NotFound();

        reader.IsBlocked = true;
        _context.SaveChanges();
        return Ok(new { Message = "Читатель заблокирован" });
    }

    [HttpPost("{id}/unblock")]
    public IActionResult UnblockReader(int id)
    {
        var reader = _context.Readers.Find(id);
        if (reader == null) return NotFound();

        reader.IsBlocked = false;
        _context.SaveChanges();
        return Ok(new { Message = "Читатель разблокирован" });
    }

    // Checkout / Return Book
    [HttpPost("{readerId}/checkout/{bookId}")]
    public IActionResult CheckoutBook(int readerId, int bookId)
    {
        var reader = _context.Readers.Find(readerId);
        var book = _context.Books.Find(bookId);
        if (reader == null || book == null) return NotFound();
        if (reader.IsBlocked) return BadRequest("Читатель заблокирован");

        var checkedOutCount = _context.BookReaders.Count(br => br.BookId == bookId && br.ReturnedAt == null);
        if (checkedOutCount >= book.Count)
            return BadRequest("Нет доступных экземпляров этой книги");

        _context.BookReaders.Add(new BookReader { BookId = bookId, ReaderId = readerId, CheckedOutAt = DateTime.UtcNow});
        _context.SaveChanges();
        return Ok(new { Message = "Книга успешно выдана" });
    }
    [HttpPost("{readerId}/return/{bookId}")]
    public IActionResult ReturnBook(int readerId, int bookId)
    {
        var br = _context.BookReaders
            .Where(x => x.BookId == bookId && x.ReaderId == readerId && x.ReturnedAt == null)
            .OrderByDescending(x => x.CheckedOutAt)
            .FirstOrDefault();
        if (br == null) return NotFound();
        br.ReturnedAt = DateTime.UtcNow;
        _context.SaveChanges();
        return Ok(new { Message = "Книга успешно возвращена" });
    }

    // Get borrow history for a specific reader
    [HttpGet("{id}/history")]
    public ActionResult<IEnumerable<object>> GetReaderHistory(int id)
    {
        var readerExists = _context.Readers.Any(r => r.Id == id);
        if (!readerExists) return NotFound();

        var history = _context.BookReaders
            .Include(br => br.Book)
            .Where(br => br.ReaderId == id)
            .OrderByDescending(br => br.CheckedOutAt)
            .ToList();

        var result = history.Select(br => new
        {
            BookId = br.BookId,
            Title = br.Book?.Title ?? string.Empty,
            Author = br.Book?.Author ?? string.Empty,
            CheckedOutAt = br.CheckedOutAt.ToString("yyyy-MM-dd HH:mm:ss"),
            ReturnedAt = br.ReturnedAt == null ? "Читатель не вернул книгу" : br.ReturnedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();

        return Ok(result);
    }
}
