namespace LibrarySystem.DTOs;

public class ReaderDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsBlocked { get; set; }
    public List<BorrowDto> BorrowHistory { get; set; } = new();
}
