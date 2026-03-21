using System.Collections;

namespace ExpressionEvaluator.Core;

public class Evaluator
{
    public static double Evaluate(string infix)
    {
        //1.modification:sanitize the input by removing all whitespace characters
        infix = infix.Replace(" ", "");
        /************************************************************************/
        var postfix = InfixToPostfix(infix);
        return EvaluatePostfix(postfix);
    }

    private static string InfixToPostfix(string infix)
    {
        var postFix = string.Empty;
        var stack = new Stack<char>();
        //2.modification: chang the loop to a normal for so we can access the index of the current character and check if the next character is a digit or a dot to handle multi-digit numbers and decimal numbers.
        for (int i = 0; i < infix.Length; i++)
        {
            char item = infix[i];
        /************************************************************************/
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
                        while (stack.Peek() != '(')
                        {
                            postFix += stack.Pop() + " ";
                            Console.WriteLine(postFix);
                        } 
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
                            while(stack.Count > 0 && stack.Peek() != '(' && PriorityStack(stack.Peek()) >= PriorityInfix(item))
                            {
                                postFix += stack.Pop() + " ";
                            }
                            stack.Push(item);
                        }
                    }
                }
            }
            else
            {
                //4.modification: change the way we handle numbers to handle multi-digit numbers and decimal numbers by checking if the current character is a digit or a dot and if the next character is also a digit or a dot and concatenating them until we reach a multiple-digit number and/or a decimal number.
                var wholeNumber = string.Empty;
                //postFix += item;
                if (IsDigitOrDot(item) == true)
                {
                    wholeNumber += infix[i];
                    i++;
                    while (i < infix.Length && IsDigitOrDot(infix[i]))
                    {
                        wholeNumber += infix[i];
                        i++;
                    }
                    postFix += wholeNumber + " "; //add a space after each number to separate them in the postfix expression, so we can use them properly.
                }
                else
                {
                    postFix += item + " ";
                }
                /************************************************************************/
            }
        }
        while (stack.Count > 0)
        {
            //6. modification: make sure to pop the parentheses from the stack and not add them to the postfix expression.
            var popped = stack.Pop();
            if (popped != '(' && popped != ')')
            {
                postFix += popped + " ";
            }
            /*****************************************************************/
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
        //Console.WriteLine($"Postfix: {postfix}");
        var stack = new Stack<double>();
        //5.modification: change the way we handle numbers in the postfix expression to handle multi-digit numbers and decimal numbers by splitting the postfix expression by spaces and checking if each item is an operator or a number.
        var postFixArray = postfix.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        /************************************************************************/
        foreach (var item in postFixArray)
        {
            if (item.Length == 1 && IsOperator(item[0]))
            {
                var b = stack.Pop();
                var a = stack.Pop();
                stack.Push(item switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new Exception("Sintax error."),
                });
            }
            else
            {
                stack.Push(double.Parse(item));
            }
        }
        return stack.Pop();
    }

    private static bool IsOperator(char item) => "+-*/^()".Contains(item);

    //3.modification: boolean method to check if the current character is a digit or a dot.
    private static bool IsDigitOrDot(char item) => ".".Contains(item) || char.IsDigit(item);
}