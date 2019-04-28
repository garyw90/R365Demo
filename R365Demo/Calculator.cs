using System;

namespace R365Demo
{
    public class Calculator : ICalculator
    {
        public int Add(string numbers)
        {
            if (numbers == null)
                throw new ArgumentNullException(nameof(numbers));
            int result = 0;
            string[] parts = numbers.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2)
                throw new ArgumentOutOfRangeException(nameof(numbers));
            foreach (string item in parts)
            {
                if (int.TryParse(item, out var value))
                    result += value;
            }
            return result;
        }
    }
}
