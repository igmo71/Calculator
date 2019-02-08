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
                Console.WriteLine("Please enter an arithmetic expression (output \"q\"):");
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
            Stack<string> expressionStack = new Stack<string>();
            bool isNewDigit = true;

            if (expression[0] == '-')
            {
                expressionStack.Push(expression[0].ToString());
                expression = expression.Substring(1);
                isNewDigit = false;
            }

            foreach (char token in expression)
            {
                if (char.IsNumber(token))
                {
                    if (isNewDigit)
                    {
                        expressionStack.Push(token.ToString());
                        isNewDigit = false;
                    }
                    else
                    {
                        expressionStack.Push(expressionStack.Pop() + token);
                    }
                }
                else if (token == '(' || token == ')' || token == '+' || token == '-' || token == '*' || token == '/')
                {
                    expressionStack.Push(token.ToString());
                    isNewDigit = true;
                }
                else continue;
            }

            LinkedList<string> expressionList = new LinkedList<string>();
            foreach (string lex in expressionStack)
            {
                expressionList.AddFirst(lex);
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
                }

            }

            return expressionNode.Value;
        }
    }
}
