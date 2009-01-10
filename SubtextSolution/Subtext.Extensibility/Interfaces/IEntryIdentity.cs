using System;

namespace Subtext.Extensibility.Interfaces
{
    public interface IEntryIdentity : IIdentifiable
    {
        string EntryName { get; }
        DateTime DateCreated { get; }
        PostType PostType { get; }
    }
}
