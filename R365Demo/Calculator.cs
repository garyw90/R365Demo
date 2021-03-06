﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace R365Demo
{
    public class Calculator : ICalculator
    {
        class Delimiters
        {
            private char[] Chars { get; set; }
            private List<string> LongDelimiter { get; set; }

            public Delimiters()
            {
                Chars = new[] {','};
            }
            public Delimiters(string text)
            {
                string delimiterText = text.Substring(2).Trim();
                if (delimiterText.Length == 0)
                    throw new ArgumentException("The delimiter character is missing", nameof(text));
                if (delimiterText[0] == '[' && delimiterText[delimiterText.Length - 1] == ']')
                {
                    LongDelimiter = new List<string>();
                    int startIndex = 1;
                    int index = delimiterText.IndexOf(']');
                    while (index > 0)
                    {
                        LongDelimiter.Add(delimiterText.Substring(startIndex, index - startIndex));
                        startIndex = index + 1;
                        if (startIndex == delimiterText.Length)
                            index = -1;
                        else
                        {
                            if (delimiterText[startIndex] != '[')
                                throw new ArgumentException("Invalid multiple delimiter format, [ expected", nameof(text));
                            index = delimiterText.IndexOf(']', ++startIndex);
                        }
                    }
                }
                else
                {
                    if (delimiterText.Length > 1)
                        throw new ArgumentException("The delimiter can only be one character", nameof(text));
                    Chars = new [] { delimiterText[0] };
                }
            }

            private string FindDelimiter(string text, int startIndex)
            {
                return LongDelimiter.FirstOrDefault(x => text.IndexOf(x, startIndex) >= 0);
            }
            public string[] GetNumbers(string text)
            {
                string[] parts;
                if (LongDelimiter != null)
                {
                    List<string> numberList = new List<string>();
                    int index = 0;
                    while (index >= 0)
                    {
                        int startIndex = index;
                        while (index < text.Length && char.IsDigit(text[index]))
                            index++;
                        numberList.Add(text.Substring(startIndex, index - startIndex));
                        if (index == text.Length)
                            index = -1;
                        else
                        {
                            startIndex = index;
                            while (index < text.Length && !char.IsDigit(text[index]))
                                index++;
                            string delimiter = text.Substring(startIndex, index - startIndex);
                            if (LongDelimiter.FirstOrDefault(x => x == delimiter) == null)
                                throw new ArgumentException("Invalid characters found, not a valid delimiter", nameof(text));
                        }
                    }
                    parts = numberList.ToArray();
                }
                else
                    parts = text.Split(Chars, StringSplitOptions.RemoveEmptyEntries);

                return parts;
            }
        }

        private void GetNumbers(string numbers, Action<int> operation)
        {
            if (numbers == null)
                throw new ArgumentNullException(nameof(numbers));
            StringReader reader = new StringReader(numbers);
            Delimiters delimiter = new Delimiters();
            string text;
            int lineNumber = 1;
            while ((text = reader.ReadLine()) != null)
            {
                if (text.StartsWith("//"))
                {
                    if (lineNumber != 1)
                        throw new ArgumentException("Delimiters can only appear on the first line", nameof(numbers));
                    delimiter = new Delimiters(text);
                }
                else
                {
                    if (text.EndsWith(","))
                        throw new ArgumentException("Input cannot end with commas at the end of the line", nameof(numbers));
                    string[] parts = delimiter.GetNumbers(text);
                    foreach (string item in parts)
                    {
                        if (int.TryParse(item.Trim(), out var value))
                        {
                            operation(value);
                        }
                    }
                }
                lineNumber++;
            }
        }
        public int Add(string numbers)
        {
            int result = 0;
            List<int> negativeNumbers = new List<int>();
            GetNumbers(numbers, (value) =>
            {
                if (value < 0)
                    negativeNumbers.Add(value);
                else if (value <= 1000)
                    result += value;
            });
            if (negativeNumbers.Any())
            {
                throw new ArgumentException($"Negative numbers are not allowed: {string.Join(",", negativeNumbers)}", nameof(numbers));
            }
            return result;
        }

        public int Calculate(Operation op, string numbers)
        {
            int? result = null;
            GetNumbers(numbers, (value) =>
            {
                switch (op)
                {
                    case Operation.Add:
                        if (!result.HasValue)
                            result = 0;
                        result += value;
                        break;
                    case Operation.Subtract:
                        if (!result.HasValue)
                            result = value;
                        else
                            result -= value;
                        break;
                    case Operation.Multiply:
                        if (!result.HasValue)
                            result = 1;
                        result *= value;
                        break;
                    case Operation.Divide:
                        if (!result.HasValue)
                            result = value;
                        else
                            result /= value;
                        break;
                }
            });
            return result.GetValueOrDefault();
        }
    }
}
