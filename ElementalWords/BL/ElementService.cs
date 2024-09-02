using ElementalWords.DAL;
using System;
using System.Collections.Generic;

namespace ElementalWords.BL
{
    public class ElementService
    {
        private readonly ElementRepository _elementRepository;

        public ElementService(ElementRepository elementRepository)
        {
            _elementRepository = elementRepository ?? throw new ArgumentNullException(nameof(elementRepository));
        }

        /// <summary>
        /// This method finds all possible elemental representations of a given word.
        /// </summary>
        /// <param name="word">Word to be created using Elemental symbols</param>
        /// <returns>An array of strings, where it makes up the word provided by the user input.</returns>
        public string[][] ElementalForms(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                // Return an empty array if the input word is null or empty
                return Array.Empty<string[]>(); 
            }

            var results = new List<List<string>>();
            word = word.ToLower();

            FindElementalForms(word, 0, new List<string>(), results);

            // Convert the list of lists to an array
            return results.ConvertAll(subList => subList.ToArray()).ToArray();
        }

        /// <summary>
        /// This method finds all possible elemental forms for a given word.
        /// </summary>
        /// <param name="word">The word being processed.</param>
        /// <param name="index">The current index in the word.</param>
        /// <param name="currentPath">The current path of elements being formed.</param>
        /// <param name="results">The list of successful elemental forms.</param>
        private void FindElementalForms(string word, int index, List<string> currentPath, List<List<string>> results)
        {
            // If the current index has reached the end of the word we've found
            // a valid combination, so it's added to the results
            if (index == word.Length)
            {
                results.Add(new List<string>(currentPath));
                return;
            }

            // Try to match either one or two characters as a symbol from the current index
            // This loop will handle up-to 3 letter element symbols
            for (int length = 1; length <= 3; length++)
            {
                // Ensure we don't go out of bounds
                if (index + length > word.Length)
                    break;

                // Extract a potential element symbol of length 1 or 2 from the current position
                string symbol = word.Substring(index, length);

                // Attempt to retrieve the corresponding element from the repository
                var element = _elementRepository.GetElementBySymbol(symbol);

                // If a valid element is found, add it to the current path and search
                // for the next part of the word starting from the new index
                if (element != null)
                {
                    // Add the element in the required format to the current path
                    currentPath.Add($"{element.Name} ({element.Symbol})");

                    // Pprocess the next part of the word
                    FindElementalForms(word, index + length, currentPath, results);

                    // Remove the last element added, to check for other possibilities
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }

    }
}
