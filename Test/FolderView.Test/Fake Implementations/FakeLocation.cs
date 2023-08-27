namespace FolderView.Test;

public class FakeLocation : ILocation
{
    public string GetAbsolutePath(IPath path)
    {
        throw new System.NotImplementedException();
    }
}
