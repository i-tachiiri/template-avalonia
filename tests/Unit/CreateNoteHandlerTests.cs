using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Application;
using Core.Application.Commands;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Xunit;

namespace Unit.Tests;

public class CreateNoteHandlerTests
{
    private class InMemoryNoteRepository : INoteRepository
    {
        public List<Note> Notes { get; } = new();

        public Task AddAsync(Note note, CancellationToken cancellationToken = default)
        {
            Notes.Add(note);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult((IReadOnlyList<Note>)Notes);
        }
    }

    [Fact]
    public async Task Handle_Adds_Note_To_Repository()
    {
        var repo = new InMemoryNoteRepository();
        var handler = new CreateNoteHandler(repo);
        var command = new CreateNoteCommand("title", "content");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Single(repo.Notes);
        var note = repo.Notes[0];
        Assert.Equal(result, note.Id);
        Assert.Equal("title", note.Title);
        Assert.Equal("content", note.Content);
    }
}
