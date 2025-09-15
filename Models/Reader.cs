namespace LibrarySystem.Models;

public class Reader
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsBlocked { get; set; } = false;
    public ICollection<Book> Books { get; set; } = new List<Book>();
}