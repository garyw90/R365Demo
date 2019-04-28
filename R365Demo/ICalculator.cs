namespace R365Demo
{
    public enum Operation
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
    public interface ICalculator
    {
        int Add(string numbers);
        int Calculate(Operation op, string numbers);
    }
}
