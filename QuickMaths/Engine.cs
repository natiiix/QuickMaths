using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace QuickMaths
{
    public class Engine
    {
        private enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Power
        }

        private const string SUB_EXPR_START = "(";
        private const string SUB_EXPR_END = ")";

        private readonly string[] SourceLines;
        private readonly Dictionary<string, double> Variables;

        public Engine(string[] sourceLines)
        {
            SourceLines = sourceLines;
            Variables = new Dictionary<string, double>();
        }

        public void Run()
        {
            for (int i = 0; i < SourceLines.Length; i++)
            {
                string[] parts = SourceLines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                {
                    continue;
                }

                bool saveToVar = parts.Length >= 2 && parts[1] == "=";

                if (saveToVar)
                {
                    Variables[parts[0]] = EvaluateExpression(parts.ToList().Skip(2));
                }
                else
                {
                    Console.WriteLine($"[{i + 1}] {EvaluateExpression(parts)}");
                }
            }

            foreach (KeyValuePair<string, double> vari in Variables)
            {
                Console.WriteLine($"{vari.Key} = {vari.Value}");
            }
        }

        private double EvaluateExpression(IEnumerable<string> expression)
        {
            double value = 0;
            Operation currentOp = Operation.Add;

            for (int i = 0; i < expression.Count(); i++)
            {
                string part = expression.ElementAt(i);

                switch (part.ToLower())
                {
                    case "add":
                        currentOp = Operation.Add;
                        break;

                    case "sub":
                        currentOp = Operation.Subtract;
                        break;

                    case "mul":
                        currentOp = Operation.Multiply;
                        break;

                    case "div":
                        currentOp = Operation.Divide;
                        break;

                    case "pow":
                        currentOp = Operation.Divide;
                        break;

                    default:
                        {
                            double partValue = double.NaN;

                            // Literal value
                            if (double.TryParse(part, out partValue))
                            {
                            }
                            // Variable
                            else if (Variables.ContainsKey(part))
                            {
                                partValue = Variables[part];
                            }
                            // Sub-expression
                            else if (part == SUB_EXPR_START)
                            {
                                int end = FindEndParenthesis(expression, i);

                                if (end <= i || end >= expression.Count())
                                {
                                    throw new Exception($"Unexpected expression format \"{string.Join(' ', expression)}\"");
                                }

                                partValue = EvaluateExpression(expression.Skip(i + 1).Take(end - i - 1));
                            }
                            // Invalid expression
                            else
                            {
                                throw new Exception($"Invalid expression \"{part}\"!");
                            }

                            switch (currentOp)
                            {
                                case Operation.Add:
                                    value += partValue;
                                    break;

                                case Operation.Subtract:
                                    value -= partValue;
                                    break;

                                case Operation.Multiply:
                                    value *= partValue;
                                    break;

                                case Operation.Divide:
                                    if (partValue == 0d)
                                    {
                                        throw new DivideByZeroException();
                                    }

                                    value /= partValue;
                                    break;

                                case Operation.Power:
                                    value = Math.Pow(value, partValue);
                                    break;

                                default:
                                    throw new Exception("Invalid operation!");
                            }
                        }
                        break;
                }
            }

            return value;
        }

        private static int FindEndParenthesis(IEnumerable<string> expr, int startIdx)
        {
            if (!expr.ElementAt(startIdx).Equals(SUB_EXPR_START))
            {
                return -1;
            }

            int depth = 1;

            for (int i = startIdx + 1; i < expr.Count(); i++)
            {
                string str = expr.ElementAt(i);

                if (str.Equals(SUB_EXPR_START))
                {
                    depth++;
                }
                else if (str.Equals(SUB_EXPR_END))
                {
                    depth--;
                }

                if (depth == 0)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}