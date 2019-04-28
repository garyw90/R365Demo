using System;
using System.CodeDom;
using System.Collections;
using System.IO;
using System.Reflection;

namespace R365Demo
{
    public class Calculator : ICalculator
    {
        public int Add(string numbers)
        {
            if (numbers == null)
                throw new ArgumentNullException(nameof(numbers));
            int result = 0;
            StringReader reader = new StringReader(numbers);
            char[] delimiter = new[] {','};
            string text;
            int lineNumber = 1;
            while ((text = reader.ReadLine()) != null)
            {
                if (text.StartsWith("//"))
                {
                    if (lineNumber != 1)
                        throw new ArgumentException("Delimiters can only appear on the first line", nameof(numbers));
                    string delimiterText = text.Substring(2).Trim();
                    if (delimiterText.Length == 0)
                        throw new ArgumentException("The delimiter character is missing", nameof(numbers));
                    if (delimiterText.Length > 1)
                        throw new ArgumentException("The delimiter can only be one character", nameof(numbers));
                    delimiter[0] = delimiterText[0];
                }
                lineNumber++;
                if (text.EndsWith(","))
                    throw new ArgumentException("Input cannot end with commas at the end of the line", nameof(numbers));
                string[] parts = text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in parts)
                {
                    if (int.TryParse(item, out var value))
                        result += value;
                }
            }
            return result;
        }
    }
}
