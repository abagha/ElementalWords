using ElementalWords.BL;
using ElementalWords.DAL;
using System;

namespace ElementalWords
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var elementRepository = new ElementRepository();
            var elementalService = new ElementService(elementRepository);

            while (true)
            {
                Console.WriteLine("Enter a word to check for elemental forms (or type 'exit' to quit):");
                string word = Console.ReadLine();

                if (string.Equals(word, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    // Exit application
                    break; 
                }

                if (string.IsNullOrWhiteSpace(word))
                {
                    // Reprompt user for 'valid' (not empty) input
                    Console.WriteLine("Input cannot be empty. Please enter a valid word.");
                    continue; 
                }

                var forms = elementalService.ElementalForms(word);

                if (forms.Length > 0)
                {
                    Console.WriteLine($"Elemental forms for the word \"{word}\":");
                    foreach (var form in forms)
                    {
                        Console.WriteLine(string.Join(", ", form));
                    }
                }
                else
                {
                    Console.WriteLine($"No elemental forms found for the word \"{word}\".");
                }

                // Blank line for readability
                Console.WriteLine(); 
            }
        }
    }
}
