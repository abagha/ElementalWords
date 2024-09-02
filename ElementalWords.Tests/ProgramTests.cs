using Xunit;
using System;
using System.IO;
using ElementalWords;

public class ProgramTests
{
    [Fact]
    public void Program_HandlesValidInput()
    {
        var input = "Snack\nexit\n";
        var output = RunProgramWithInput(input);

        Assert.Contains("Elemental forms for the word \"Snack\":", output);
    }

    [Fact]
    public void Program_HandlesExitInput()
    {
        var input = "exit\n";
        var output = RunProgramWithInput(input);

        Assert.DoesNotContain("Elemental forms for the word", output);
    }

    [Fact]
    public void Program_PromptsAgainForEmptyInput()
    {
        var input = "\nSnack\nexit\n";
        var output = RunProgramWithInput(input);

        Assert.Contains("Input cannot be empty. Please enter a valid word.", output);
    }

    private string RunProgramWithInput(string input)
    {
        using (var sw = new StringWriter())
        {
            using (var sr = new StringReader(input))
            {
                Console.SetOut(sw);
                Console.SetIn(sr);

                Program.Main(new string[0]);

                return sw.ToString();
            }
        }
    }
}
