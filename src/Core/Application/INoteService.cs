namespace Core.Application;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;

public interface INoteService
{
    Task<IReadOnlyList<Note>> GetNotesAsync(CancellationToken cancellationToken = default);
    Task<NoteId> CreateAsync(string title, string content, CancellationToken cancellationToken = default);
}
