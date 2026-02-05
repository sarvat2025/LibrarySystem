namespace LibrarySystem.DTOs;

public class BorrowDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string CheckedOutAt { get; set; } = string.Empty;
    public string ReturnedAt { get; set; } = string.Empty;
}
