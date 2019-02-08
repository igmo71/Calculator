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

            return ParseBrackets(expressionStack);
        }

        private static string ParseBrackets(Stack<string> expressionStack)
        {
            Stack<string> resultExpressionStack = new Stack<string>();

            foreach(string lexeme in expressionStack)
            {
                if (lexeme != ")")
                {
                    resultExpressionStack.Push(lexeme);
                    continue;
                }
                else
                {
                    Stack<string> bracketsStack = new Stack<string>();
                    string lex;
                    do
                    {
                        lex = expressionStack.Pop();
                        bracketsStack.Push(lex);
                    } while (lex != "(");
                    resultExpressionStack.Push(ParseMajorOperations(bracketsStack));
                }
            }

            return ParseMajorOperations(resultExpressionStack);
        }

        private static string ParseMajorOperations(Stack<string> bracketsStack)
        {
            Queue<string> MinorOperationsQueue = new Queue<string>();
            foreach(var lex in bracketsStack)
            {
                if(lex)
            }
        }
    }
}
