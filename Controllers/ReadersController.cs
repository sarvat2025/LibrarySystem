using LibrarySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadersController : ControllerBase
{
    private readonly LibraryContext _context;

    public ReadersController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reader>>> GetReaders()
    {
        return await _context.Readers
            .Include(r => r.Books)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reader>> GetReader(int id)
    {
        var reader = await _context.Readers
            .Include(r => r.Books)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reader == null) return NotFound();
        return reader;
    }

    [HttpPost]
    public async Task<ActionResult<Reader>> PostReader(Reader reader)
    {
        _context.Readers.Add(reader);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReader), new { id = reader.Id }, reader);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutReader(int id, Reader reader)
    {
        if (id != reader.Id) return BadRequest();

        _context.Entry(reader).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReaderExists(id)) return NotFound();
            else throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReader(int id)
    {
        var reader = await _context.Readers.FindAsync(id);
        if (reader == null) return NotFound();

        _context.Readers.Remove(reader);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPost("{readerId}/assign-book/{bookId}")]
    public async Task<IActionResult> AssignBookToReader(int readerId, int bookId)
    {
        var reader = await _context.Readers
            .Include(r => r.Books)
            .FirstOrDefaultAsync(r => r.Id == readerId);
        var book = await _context.Books
            .Include(b => b.Readers)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (reader == null || book == null) return NotFound();
        if (reader.IsBlocked) return BadRequest("Читатель заблокирован");

        if (!reader.Books.Contains(book))
        {
            reader.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        return Ok(new { Message = "Книга успешно привязана к читателю" });
    }
    [HttpPost("{readerId}/unassign-book/{bookId}")]
    public async Task<IActionResult> UnassignBookFromReader(int readerId, int bookId)
    {
        var reader = await _context.Readers
            .Include(r => r.Books)
            .FirstOrDefaultAsync(r => r.Id == readerId);
        var book = await _context.Books
            .Include(b => b.Readers)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (reader == null || book == null) return NotFound();

        if (reader.Books.Contains(book))
        {
            reader.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        return Ok(new { Message = "Книга возвращена в библиотеку" });
    }

    private bool ReaderExists(int id) => _context.Readers.Any(e => e.Id == id);
}