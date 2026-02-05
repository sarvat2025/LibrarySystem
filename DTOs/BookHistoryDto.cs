namespace LibrarySystem.DTOs;

public class BookHistoryDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public List<ReaderBorrowDto> Borrowers { get; set; } = new();
}
