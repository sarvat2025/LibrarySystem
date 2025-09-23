namespace LibrarySystem.DTOs;

public class ReaderBorrowDto
{
    public int ReaderId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CheckedOutAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
}
