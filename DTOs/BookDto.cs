namespace LibrarySystem.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public int Count { get; set; }
}
