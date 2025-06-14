using System;

namespace Core.Domain.ValueObjects;

public readonly struct NoteId
{
    public Guid Value { get; }

    public NoteId(Guid value)
    {
        Value = value;
    }

    public static NoteId New() => new NoteId(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
