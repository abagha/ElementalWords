using ElementalWords.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace ElementalWords.DAL
{
    public class ElementRepository
    {
        // Dictionary of Elements with Symbol
        private readonly Dictionary<string, Element> elements;
        private readonly string elementHeader;
        private readonly string symbolHeader;

        public ElementRepository()
        {
            // Initialising elements Dictionary
            elements = new Dictionary<string, Element>();

            // Initialses the configuration system to load settings from the appsetting.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Finding and settings CSV headers from appsettings.json
            elementHeader = configuration["CSVHeaders:Element"] ?? throw new InvalidOperationException("CSV header for 'Element' is missing or empty in the configuration.");
            symbolHeader = configuration["CSVHeaders:Symbol"] ?? throw new InvalidOperationException("CSV header for 'Symbol' is missing or empty in the configuration.");

            ReadElementsFromCSV(configuration["ElementsCSVFilePath"] ?? throw new InvalidOperationException("The configuration setting 'ElementsCSVFilePath' is missing or empty."));
        }

        /// <summary>
        /// This method reads and parses Elements from a CSV file specified by the file path in appsettings.json. It checks
        /// the existence of the CSV file, verifies headers and loads the Elements into a Dictionary indexed by the Elements
        /// Symbol.
        /// </summary>
        /// <param name="elementCSVFilePath">~File path of CSV</param>
        /// <exception cref="FileNotFoundException">Thrown when the specified CSV file is not found</exception>
        /// <exception cref="InvalidDataException">Thrown when the CSV file is empty or does not contain the required headers</exception>
        private void ReadElementsFromCSV(string elementCSVFilePath)
        {
            // Setting the 'full' file path to the Elements CSV File
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, elementCSVFilePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found: {fullPath}");
            }

            var lines = File.ReadAllLines(fullPath);
            if (lines.Length == 0)
            {
                throw new InvalidDataException("The CSV file is empty.");
            }

            // Seperates and stores the headers from the CSV file in an array
            var headers = lines[0].Split(',');

            // Finds the positions of the headers in the CSV relative to the header defined in appsettings.json
            int elementIndex = Array.IndexOf(headers, elementHeader);
            int symbolIndex = Array.IndexOf(headers, symbolHeader);

            // If the headers can't be found
            if (elementIndex == -1 || symbolIndex == -1)
            {
                throw new InvalidDataException($"Required headers '{elementHeader}' and '{symbolHeader}' not found.");
            }

            // Process the CSV data and store in the elements dictionary
            for (int i = 1; i < lines.Length; i++)
            {
                // Split data by the ,
                var parts = lines[i].Split(',');

                // Check to see if the current line has enough columns to contain both Element name and symbol
                // If not, skip this line
                if (parts.Length <= Math.Max(elementIndex, symbolIndex))
                {
                    continue;
                }

                // Extract the element name and symbol from the corresponding columns
                string elementName = parts[elementIndex];
                string symbol = parts[symbolIndex];

                // If Element name and symbol contain data, create Element object and add to elements dictionary
                if (!string.IsNullOrEmpty(symbol) && !string.IsNullOrEmpty(elementName))
                {
                    elements[symbol.ToLower()] = new Element(elementName, symbol);
                }
            }
        }

        /// <summary>
        /// This method returns an Element object based on the symbol provided.
        /// </summary>
        /// <param name="symbol">Symbol of the element you want to find. E.g. h</param>
        /// <returns>Element object</returns>
        /// <exception cref="ArgumentNullException">Symbol cannot be null or empty</exception>
        public Element? GetElementBySymbol(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentNullException(nameof(symbol), "Symbol cannot be null or empty.");
            }

            // Attempts to retrieve the Element from the given symbol (case-insensitive).
            elements.TryGetValue(symbol.ToLower(), out var element);
            return element;
        }
    }
}
