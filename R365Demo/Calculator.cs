using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            string longDelimiter = null;
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
                    if (delimiterText[0] == '[' && delimiterText[delimiterText.Length - 1] == ']')
                        longDelimiter = delimiterText.Substring(1, delimiterText.Length - 2);
                    else
                    {
                        if (delimiterText.Length > 1)
                            throw new ArgumentException("The delimiter can only be one character", nameof(numbers));
                        delimiter[0] = delimiterText[0];
                    }
                }
                else
                {
                    if (text.EndsWith(","))
                        throw new ArgumentException("Input cannot end with commas at the end of the line", nameof(numbers));
                    string[] parts;
                    if (longDelimiter != null)
                    {
                        List<string> numberList = new List<string>();
                        int startIndex = 0;
                        int index = text.IndexOf(longDelimiter);
                        while (index >= 0)
                        {
                            numberList.Add(text.Substring(startIndex, index - startIndex));
                            startIndex = index + longDelimiter.Length;
                            index = text.IndexOf(longDelimiter, startIndex);
                        }
                        if (startIndex < text.Length)
                            numberList.Add(text.Substring(startIndex));
                        parts = numberList.ToArray();
                    }
                    else
                        parts = text.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
                    List<int> negativeNumbers = new List<int>();
                    foreach (string item in parts)
                    {
                        if (int.TryParse(item.Trim(), out var value))
                        {
                            if (value < 0)
                                negativeNumbers.Add(value);
                            else if (value <= 1000)
                                result += value;
                        }
                    }
                    if (negativeNumbers.Any())
                    {
                        throw new ArgumentException($"Negative numbers are not allowed: {string.Join(",", negativeNumbers)}", nameof(numbers));
                    }
                }
                lineNumber++;
            }
            return result;
        }
    }
}
