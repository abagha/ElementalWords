using Xunit;
using ElementalWords.DAL;
using ElementalWords.Models;
using System;
using System.IO;

public class ElementRepositoryTests
{
    [Fact]
    public void ElementRepository_InitialisesCorrectly()
    {
        var repository = new ElementRepository();
        Assert.NotNull(repository);
    }

    [Fact]
    public void GetElementBySymbol_ReturnsCorrectElement_ForValidSymbol()
    {
        var repository = new ElementRepository();
        var element = repository.GetElementBySymbol("H");
        Assert.NotNull(element);
        Assert.Equal("Hydrogen", element.Name);
    }

    [Fact]
    public void GetElementBySymbol_IsCaseInsensitive()
    {
        var repository = new ElementRepository();
        var element = repository.GetElementBySymbol("h");
        Assert.NotNull(element);
        Assert.Equal("Hydrogen", element.Name);
    }

    [Fact]
    public void GetElementBySymbol_ReturnsNull_ForInvalidSymbol()
    {
        var repository = new ElementRepository();
        var element = repository.GetElementBySymbol("Xx");
        Assert.Null(element);
    }

    [Fact]
    public void GetElementBySymbol_ThrowsArgumentNullException_ForNullSymbol()
    {
        var repository = new ElementRepository();
        Assert.Throws<ArgumentNullException>(() => repository.GetElementBySymbol(null));
    }
}
