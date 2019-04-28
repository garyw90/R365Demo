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
            string text;
            while ((text = reader.ReadLine()) != null)
            {
                if (text.EndsWith(","))
                    throw new ArgumentException("Input cannot end with commas at the end of the line", nameof(numbers));
                string[] parts = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
