using System.Collections;

namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var postFix = string.Empty;
        var stack = new Stack<char>();
        foreach (var item in infix)
        {
            if (IsOperator(item))
            {
                if (stack.Count == 0)
                {
                    stack.Push(item);
                }
                else
                {
                    if (item == ')')
                    {
                        do
                        {
                            postFix += stack.Pop();
                        } while (stack.Peek() != '(');
                        stack.Pop();
                    }
                    else
                    {
                        if (PriorityInfix(item) > PriorityStack(stack.Peek()))
                        {
                            stack.Push(item);
                        }
                        else
                        {
                            postFix += stack.Pop();
                            stack.Push(item);
                        }
                    }
                }
            }
            else
            {
                postFix += item;
            }
        }
        while (stack.Count > 0)
        {
            postFix += stack.Pop();
        }
        return postFix;
    }

    private static int PriorityStack(char item) => item switch
    {
        '^' => 3,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 0,
        _ => throw new Exception("Sintax error."),
    };

    private static int PriorityInfix(char item) => item switch
    {
        '^' => 4,
        '*' => 2,
        '/' => 2,
        '+' => 1,
        '-' => 1,
        '(' => 5,
        _ => throw new Exception("Sintax error."),
    };

    private static double EvaluatePostfix(string postfix)
    {
        var stack = new Stack<double>();
        foreach (char item in postfix)
        {
            if (IsOperator(item))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(item switch
                {
                    '+' => a + b,
                    '-' => a - b,
                    '*' => a * b,
                    '/' => a / b,
                    '^' => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                var number = item.ToString();
                if (IsDigitOrDot(item))
                {
                    while (postfix.IndexOf(item) + 1 < postfix.Length && IsDigitOrDot(postfix[postfix.IndexOf(item) + 1]))
                    {
                        number += postfix[++postfix.IndexOf(item)];
                    }
                    stack.Push(double.Parse(number));
                }
            }
        }
        return stack.Pop();
    }

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);

    //make it so it can parse numbers with more than one digit and decimal numbers
    private static bool IsDigitOrDot(char item) => ".".Contains(item) || char.IsDigit(item);
}