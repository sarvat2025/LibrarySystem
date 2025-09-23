using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibrarySystem.Models;

public class Reader
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    [JsonIgnore] public ICollection<BookReader> BookReaders { get; set; } = new List<BookReader>();
    [NotMapped] public IEnumerable<Book> CurrentBooks { get; set; } = Enumerable.Empty<Book>();
}
