namespace FolderView.Test;

using System;
using NUnit.Framework;

[TestFixture]
public class TestLocation
{
    [Test]
    public void TestFakeLocation()
    {
        ILocation Location = new FakeLocation();
        Assert.Throws<ArgumentException>(() => Path.RootFolderFrom(Location));
    }

    private class FakeLocation : ILocation
    {
    }
}
