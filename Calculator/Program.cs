using System;
using System.Collections.Generic;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please enter an arithmetic expression (quit - \"q\"):");
                string expressionString = Console.ReadLine();
                if (expressionString == "q") break;
                try
                {
                    Console.WriteLine(ParseString(expressionString));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        /// <summary>
        /// Разбор строки арифметического выражения
        /// </summary>
        /// <param name="expressionString"></param>
        /// <returns></returns>
        private static string ParseString(string expressionString)
        {
            LinkedList<string> expression = new LinkedList<string>();

            foreach (char token in expressionString)
            {
                expression.AddLast(token.ToString());
            }

            for (LinkedListNode<string> expressionNode = expression.First; expressionNode != null; expressionNode = expressionNode.Next)
            {
                switch (expressionNode.Value)
                {
                    case "(":
                    case ")":
                    case "*":
                    case "/":
                    case "+":
                    case "-":
                        break;
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case ",":
                        {
                            if (expressionNode.Previous == null)
                                break;
                            else if (double.TryParse(expressionNode.Previous.Value, out double d))
                            {
                                expressionNode.Previous.Value += expressionNode.Value;
                                expressionNode = expressionNode.Previous;
                                expression.Remove(expressionNode.Next);
                            }
                            else if (expressionNode.Previous.Value == "-")
                            {
                                if (expressionNode.Previous.Previous == null
                                    || expressionNode.Previous.Previous.Value == "("
                                    || expressionNode.Previous.Previous.Value == "*"
                                    || expressionNode.Previous.Previous.Value == "/"
                                    || expressionNode.Previous.Previous.Value == "+"
                                    || expressionNode.Previous.Previous.Value == "-")
                                {
                                    expressionNode.Previous.Value += expressionNode.Value;
                                    expressionNode = expressionNode.Previous;
                                    expression.Remove(expressionNode.Next);
                                }
                            }
                        }
                        break;
                    default: throw new Exception("Invalid expression");
                }
            }
            return ParseBrackets(expression);
        }

        /// <summary>
        /// Разбор скобок
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string ParseBrackets(LinkedList<string> expression)
        {
            for (LinkedListNode<string> expressionNode = expression.First; expressionNode != null; expressionNode = expressionNode.Next)
            {
                if (expressionNode.Value != ")")
                {
                    continue;
                }
                else
                {
                    LinkedList<string> subExpression = new LinkedList<string>();
                    LinkedListNode<string> subExpressionNode = expressionNode.Previous;

                    while (subExpressionNode.Value != "(")
                    {
                        subExpression.AddFirst(subExpressionNode.Value);
                        subExpressionNode = subExpressionNode.Previous;
                        expression.Remove(subExpressionNode.Next);
                    }

                    expression.AddBefore(expressionNode, ParseMajorOperations(subExpression));
                    expressionNode = expressionNode.Previous;
                    expression.Remove(subExpressionNode);
                    expression.Remove(expressionNode.Next);
                }
            }
            return ParseMajorOperations(expression);
        }

        /// <summary>
        /// Разбор стакрших операций - умножение, деление
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string ParseMajorOperations(LinkedList<string> expression)
        {
            for (LinkedListNode<string> expressionNode = expression.First; expressionNode != null; expressionNode = expressionNode.Next)
            {
                if (!(expressionNode.Value == "*" || expressionNode.Value == "/"))
                {
                    continue;
                }
                else
                {
                    switch (expressionNode.Value)
                    {
                        case "*":
                            expressionNode.Value = (double.Parse(expressionNode.Previous.Value) * double.Parse(expressionNode.Next.Value)).ToString();
                            break;

                        case "/":
                            expressionNode.Value = (double.Parse(expressionNode.Previous.Value) / double.Parse(expressionNode.Next.Value)).ToString();
                            break;

                        default: throw new Exception("Invalid expression");

                    }

                    expression.Remove(expressionNode.Previous);
                    expression.Remove(expressionNode.Next);
                }
            }
            return ParseMinorOperations(expression);
        }

        /// <summary>
        /// Разбор младших операций - сложение, вычитание
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string ParseMinorOperations(LinkedList<string> expression)
        {
            LinkedListNode<string> expressionNode = expression.Last;

            if (expression.Count > 1)
            {
                double operand = double.Parse(expressionNode.Value);
                expressionNode = expressionNode.Previous;
                expression.RemoveLast();

                switch (expressionNode.Value)
                {
                    case "+":
                        {
                            expression.RemoveLast();
                            return (double.Parse(ParseMinorOperations(expression)) + operand).ToString();
                        }

                    case "-":
                        {
                            expression.RemoveLast();
                            return (double.Parse(ParseMinorOperations(expression)) - operand).ToString();
                        }

                    default: throw new Exception("Invalid expression");
                }
            }
            return expressionNode.Value;
        }
    }
}
