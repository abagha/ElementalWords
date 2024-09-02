namespace ElementalWords.Models
{
    /// <summary>
    /// Represents a chemical Element with a name and symbol.
    /// </summary>
    public class Element
    {
        /// <summary>
        /// Name of the Element. E.g. Hydrogen
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Symbol of the Element. E.g. H
        /// </summary>
        public string Symbol { get; set; }

        public Element(string name, string symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }
}
