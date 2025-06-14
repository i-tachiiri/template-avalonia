using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;

namespace Core.Application;

public interface INoteRepository
{
    Task AddAsync(Note note, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken cancellationToken = default);
}
