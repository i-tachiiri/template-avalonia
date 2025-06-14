using MediatR;
using Core.Domain.ValueObjects;

namespace Core.Application.Commands;

public record CreateNoteCommand(string Title, string Content) : IRequest<NoteId>;
