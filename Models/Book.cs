using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibrarySystem.Models;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<BookReader> BookReaders { get; set; } = new List<BookReader>();
   
    [NotMapped]
    public bool IsAvailable { get; set; }

    public int Count { get; set; }
}
