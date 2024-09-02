using Xunit;
using ElementalWords.BL;
using ElementalWords.DAL;
using System.Linq;

public class ElementServiceTests
{
    [Fact]
    public void ElementalForms_ReturnsCorrectCombinations_ForSimpleWord()
    {
        var repository = new ElementRepository();
        var service = new ElementService(repository);
        var forms = service.ElementalForms("More");

        Assert.NotEmpty(forms);

        var expectedCombination = new List<string> { "Molybdenum (Mo)", "Rhenium (Re)" };

        Assert.Contains(forms, form => form.SequenceEqual(expectedCombination));
    }


    [Fact]
    public void ElementalForms_ReturnsMultipleCombinations()
    {
        var repository = new ElementRepository();
        var service = new ElementService(repository);
        var forms = service.ElementalForms("Snack");

        Assert.True(forms.Length > 1);
    }

    [Fact]
    public void ElementalForms_ReturnsEmptyArray_ForUnmatchableWord()
    {
        var repository = new ElementRepository();
        var service = new ElementService(repository);
        var forms = service.ElementalForms("Xyz");

        Assert.Empty(forms);
    }

    [Fact]
    public void ElementalForms_ReturnsEmptyArray_ForNullOrEmptyInput()
    {
        var repository = new ElementRepository();
        var service = new ElementService(repository);
        var formsNull = service.ElementalForms(null);
        var formsEmpty = service.ElementalForms(string.Empty);

        Assert.Empty(formsNull);
        Assert.Empty(formsEmpty);
    }
}
