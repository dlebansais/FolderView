namespace FolderView.Test;

using System;
using System.Collections.Generic;

#if NOT
public record FakePath(IList<string> Ancestors, string Name) : IPath
{
    public IPath To(string name) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IPath Up() => throw new NotImplementedException();
}
#endif
