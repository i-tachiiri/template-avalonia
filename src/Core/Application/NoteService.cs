namespace Core.Application;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;
using Core.Application.Commands;

public class NoteService : INoteService
{
    private readonly IMediator _mediator;
    private readonly INoteRepository _repository;

    public NoteService(IMediator mediator, INoteRepository repository)
    {
        _mediator = mediator;
        _repository = repository;
    }

    public Task<IReadOnlyList<Note>> GetNotesAsync(CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync(cancellationToken);
    }

    public Task<NoteId> CreateAsync(string title, string content, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new CreateNoteCommand(title, content), cancellationToken);
    }
}
