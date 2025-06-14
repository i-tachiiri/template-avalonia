using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Application;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class NoteRepository : INoteRepository
{
    private readonly AppDbContext _db;

    public NoteRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Note note, CancellationToken cancellationToken = default)
    {
        _db.Notes.Add(note);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Note>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Notes.ToListAsync(cancellationToken);
    }
}
