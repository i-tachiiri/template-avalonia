using Core.Domain.ValueObjects;

namespace Core.Domain.Entities;

public class Note
{
    public NoteId Id { get; private set; } = default!;
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;

    public Note(NoteId id, string title, string content)
    {
        Id = id;
        Title = title;
        Content = content;
    }
}
