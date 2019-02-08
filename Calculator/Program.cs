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
                string expression = Console.ReadLine();
                if (expression == "q") break;
                try
                {
                    Console.WriteLine(ParseString(expression));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        private static string ParseString(string expression)
        {
            LinkedList<string> expressionList = new LinkedList<string>();

            foreach (char token in expression)
            {
                expressionList.AddLast(token.ToString());
            }

            for (LinkedListNode<string> expressionNode = expressionList.First; expressionNode != null; expressionNode = expressionNode.Next)
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
                                expressionList.Remove(expressionNode.Next);
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
                                    expressionList.Remove(expressionNode.Next);
                                }
                            }
                        }
                        break;
                    default: throw new Exception("Invalid expression");
                }
            }
            return ParseBrackets(expressionList);
        }

        private static string ParseBrackets(LinkedList<string> expressionList)
        {
            for (LinkedListNode<string> expressionNode = expressionList.First; expressionNode != null; expressionNode = expressionNode.Next)
            {
                if (expressionNode.Value != ")")
                {
                    continue;
                }
                else
                {
                    LinkedList<string> bracketsList = new LinkedList<string>();
                    LinkedListNode<string> bracketNode = expressionNode.Previous;

                    while (bracketNode.Value != "(")
                    {
                        bracketsList.AddFirst(bracketNode.Value);
                        bracketNode = bracketNode.Previous;
                        expressionList.Remove(bracketNode.Next);
                    }

                    expressionList.AddBefore(expressionNode, ParseMajorOperations(bracketsList));
                    expressionNode = expressionNode.Previous;
                    expressionList.Remove(bracketNode);
                    expressionList.Remove(expressionNode.Next);
                }
            }
            return ParseMajorOperations(expressionList);
        }

        private static string ParseMajorOperations(LinkedList<string> expressionList)
        {
            for (LinkedListNode<string> expressionNode = expressionList.First; expressionNode != null; expressionNode = expressionNode.Next)
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

                    expressionList.Remove(expressionNode.Previous);
                    expressionList.Remove(expressionNode.Next);
                }
            }
            return ParseMinorOperations(expressionList);
        }

        private static string ParseMinorOperations(LinkedList<string> expressionList)
        {
            LinkedListNode<string> expressionNode = expressionList.Last;

            if (expressionList.Count > 1)
            {
                double operand = double.Parse(expressionNode.Value);
                expressionNode = expressionNode.Previous;
                expressionList.RemoveLast();

                switch (expressionNode.Value)
                {
                    case "+":
                        {
                            expressionList.RemoveLast();
                            return (double.Parse(ParseMinorOperations(expressionList)) + operand).ToString();
                        }

                    case "-":
                        {
                            expressionList.RemoveLast();
                            return (double.Parse(ParseMinorOperations(expressionList)) - operand).ToString();
                        }

                    default: throw new Exception("Invalid expression");
                }
            }
            return expressionNode.Value;
        }
    }
}
