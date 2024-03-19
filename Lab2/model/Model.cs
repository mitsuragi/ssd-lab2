using System.Collections.ObjectModel;

namespace Lab2.model
{
    public static class Model
    {
        public static string? FindExpression(int target, ObservableCollection<int> numbers)
        {
            HashSet<int> visited = new HashSet<int>();
            Queue<(int, string)> expressions = new Queue<(int, string)>();

            foreach (int number in numbers)
            {
                expressions.Enqueue((number, number.ToString()));
            }

            while (expressions.Count > 0)
            {
                var (result, expression) = expressions.Dequeue();

                if (result == target)
                {
                    return expression;
                }

                if (visited.Contains(result))
                {
                    continue;
                }

                visited.Add(result);

                foreach (int num in numbers)
                {
                    int newResult;
                    string newExpression;

                    // Division
                    if (num != 0)
                    {
                        newResult = result / num;
                        newExpression = expression.Length > 1 ? $"({expression}) / {num}" : $"{expression} / {num}";
                        if (result % num == 0 && !visited.Contains(newResult))
                        {
                            expressions.Enqueue((newResult, newExpression));
                        }
                    }

                    // Addition
                    newResult = result + num;
                    newExpression = expression.Length > 1 ? $"({expression}) + {num}" : $"{expression} + {num}";
                    if (!visited.Contains(newResult))
                    {
                        expressions.Enqueue((newResult, newExpression));
                    }

                    // Subtraction
                    newResult = result - num;
                    newExpression = expression.Length > 1 ? $"({expression}) - {num}" : $"{expression} - {num}";
                    if (!visited.Contains(newResult))
                    {
                        expressions.Enqueue((newResult, newExpression));
                    }

                    // Multiplication
                    newResult = result * num;
                    newExpression = expression.Length > 1 ? $"({expression}) * {num}" : $"{expression} * {num}";
                    if (!visited.Contains(newResult))
                    {
                        expressions.Enqueue((newResult, newExpression));
                    }
                }
            }

            return null;
        }
    }
}
