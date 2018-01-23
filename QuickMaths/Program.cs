using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace QuickMaths
{
    public class Program
    {
        private enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Power
        }

        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments!");
                return;
            }

            string sourcePath = args[0];

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine("Source file does not exist!");
                return;
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                Run(File.ReadAllLines(sourcePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private static void Run(string[] sourceLines)
        {
            Dictionary<string, double> variables = new Dictionary<string, double>();

            for (int i = 0; i < sourceLines.Length; i++)
            {
                string[] parts = sourceLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                {
                    continue;
                }

                bool saveToVar = parts.Length >= 2 && parts[1] == "=";

                double value = 0;
                Operation currentOp = Operation.Add;

                for (int j = saveToVar ? 2 : 0; j < parts.Length; j++)
                {
                    string part = parts[j];

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

                                if (variables.ContainsKey(part))
                                {
                                    partValue = variables[part];
                                }
                                else if (!double.TryParse(part, out partValue))
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

                if (saveToVar)
                {
                    variables[parts[0]] = value;
                }
                else
                {
                    Console.WriteLine($"[{i + 1}] {value}");
                }
            }

            foreach (KeyValuePair<string, double> vari in variables)
            {
                Console.WriteLine($"{vari.Key} = {vari.Value}");
            }
        }
    }
}