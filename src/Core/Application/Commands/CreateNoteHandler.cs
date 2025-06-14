using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Core.Domain.Entities;
using Core.Domain.ValueObjects;

namespace Core.Application.Commands;

public class CreateNoteHandler : IRequestHandler<CreateNoteCommand, NoteId>
{
    private readonly INoteRepository _repository;

    public CreateNoteHandler(INoteRepository repository)
    {
        _repository = repository;
    }

    public async Task<NoteId> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        var noteId = NoteId.New();
        var note = new Note(noteId, request.Title, request.Content);
        await _repository.AddAsync(note, cancellationToken);
        return noteId;
    }
}
