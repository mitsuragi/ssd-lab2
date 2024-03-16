using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.model
{
    static class Model
    {
        public static string? FindExpression(int target, ObservableCollection<int> numbers, string currentExpression = "", int currentValue = 0, int index = 0)
        {
            if (index == numbers.Count)
            {
                if (currentValue == target)
                {
                    return currentExpression;
                }
                else
                {
                    return null;
                }
            }

            int num = numbers[index];

            // попытка добавить число
            string? exprWithAddition;
            if (currentExpression == "")
            {
                exprWithAddition = FindExpression(target, numbers, $"{num}", currentValue + num, index + 1);
            }
            else
            {
                exprWithAddition = FindExpression(target, numbers, $"{currentExpression}+{num}", currentValue + num, index + 1);
            }
            if (exprWithAddition != null)
                return exprWithAddition;

            // попытка вычесть число
            string? exprWithSubtraction = FindExpression(target, numbers, $"{currentExpression}-{num}", currentValue - num, index + 1);
            if (exprWithSubtraction != null)
                return exprWithSubtraction;

            // попытка умножить число
            if (currentExpression != "")
            {
                string? exprWithMultiplication = FindExpression(target, numbers, $"({currentExpression})*{num}", currentValue * num, index + 1);
                if (exprWithMultiplication != null)
                    return exprWithMultiplication;
            }

            // попытка разделить число (если оно не ноль)
            if (num != 0 && currentValue % num == 0 && currentExpression != "")
            {
                string? exprWithDivision = FindExpression(target, numbers, $"({currentExpression})/{num}", currentValue / num, index + 1);
                if (exprWithDivision != null)
                    return exprWithDivision;
            }

            // Если не найдено решения
            return null;
        }
    }
}
