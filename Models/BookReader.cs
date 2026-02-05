using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models;

public class BookReader
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int ReaderId { get; set; }
    public Reader? Reader { get; set; }
    public DateTime CheckedOutAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnedAt { get; set; }
}
