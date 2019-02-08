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
            foreach(string lex in expressionStack)
            {
                expressionList.AddFirst(lex);
            }

            return ParseBrackets(expressionList);
        }

        private static string ParseBrackets(LinkedList<string> expressionList)
        {
            LinkedList<string> resultExpressionList = new LinkedList<string>();
            LinkedListNode<string> expressionNode;
            for (expressionNode = expressionList.First; expressionNode != null; expressionNode = expressionNode.Next)
            {
                if (expressionNode.Value != ")")
                {
                    resultExpressionList.AddLast(expressionNode.Value);
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
                    expressionList.Remove(bracketNode);
                    expressionNode = expressionNode.Next;
                    expressionList.Remove(expressionNode.Previous);

                    expressionList.AddBefore(expressionNode, ParseMajorOperations(bracketsList));
                }
            }

            return ParseMajorOperations(expressionList);
        }

        private static string ParseMajorOperations(LinkedList<string> bracketsStack)
        {
            //Queue<string> MinorOperationsQueue = new Queue<string>();
            //foreach(var lex in bracketsStack)
            //{
            //    if(lex)
            //}

            return "dummi";
        }
    }
}
