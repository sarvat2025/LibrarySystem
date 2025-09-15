namespace LibrarySystem.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public ICollection<Reader> Readers { get; set; } = new List<Reader>();
}