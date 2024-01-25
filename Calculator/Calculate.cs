using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculator
{
    internal static class Calculate
    {
        public static bool IsRadians = false;
        //Метод Calculate принимает выражение в виде строки и возвращает результат, в своей работе использует другие методы класса
        public static double Calc(string input)
        {
            string output = GetExpression(input); //Преобразовываем выражение в постфиксную запись
            double result = Counting(output); //Решаем полученное выражение
            return result; //Возвращаем результат
        }
        private static string GetExpression(string input)
        {
            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operatorsStack = new Stack<char>(); //Стек для хранения операторов

            if (input != null && input[0] == '-')
                input = '0' + input;
            IsTrigonometric(ref input);
            for (int i = 0; i < input?.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                //Если символ - цифра, то считываем все число
                if (char.IsDigit(input[i])) //Если цифра
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperator(input[i]))
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                    {
                        operatorsStack.Push(input[i]); //Записываем её в стек
                    }
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operatorsStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operatorsStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operatorsStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <=
                                GetPriority(operatorsStack
                                    .Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operatorsStack.Pop() +
                                          " "; //То добавляем последний оператор из стека в строку с выражением
                        operatorsStack.Push(
                            char.Parse(input[i]
                                .ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека
                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operatorsStack.Count > 0)
                output += operatorsStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи
        }
        private static double Counting(string input)
        {
            double result = 0; //Результат
            Stack<double> temp = new Stack<double>(); //Dhtvtyysq стек для решения

            for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
            {
                //Если символ - цифра, то читаем все число и записываем на вершину стека
                if (char.IsDigit(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsOperator(input[i])) //Пока не разделитель
                    {
                        a += input[i]; //Добавляем
                        i++;
                        if (i == input.Length) break;
                    }
                    temp.Push(double.Parse(a)); //Записываем в стек
                    i--;
                }
                else if (IsOperator(input[i])) //Если символ - оператор
                {
                    //Берем два последних значения из стека
                    double a = temp.Pop();
                    double b = 0;
                    if (input[i] != '!')
                        b = temp.Pop();

                    switch (input[i])
                    {
                        case '+':
                            result = b + a;
                            break;
                        case '-':
                            result = b - a;
                            break;
                        case '*':
                            result = b * a;
                            break;
                        case '/':
                            result = b / a;
                            break;
                        case '%':
                            result = b % a;
                            break;
                        case '^':
                            result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString()))
                                .ToString());
                            break;
                        case '!':
                            result = Factorial((int)a);
                            break;
                        default:
                            break;
                    }

                    temp.Push(result); //Результат вычисления записываем обратно в стек
                }
            }
            return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
        }

        private static string FindOperationInComplex(int beginPosition, string input)
        {
            string operations = string.Empty;
            int openBracket = 1;
            int closeBracket = 0;
            if (input == null) return operations;
            operations = "(";
            for (int i = beginPosition + 1; input[i] != ')' && openBracket - 1 == closeBracket; i++)
            {
                if (input[i] == '(')
                    openBracket++;
                else if (input[i] == ')')
                    closeBracket++;

                operations += input[i];
            }

            return (operations + ")");
        }

        private static void IsTrigonometric(ref string input)
        {
            input = input?.ToLower();
            if (input == null) return;
            for (int i = 0; i < input.Length; i++)
            {
                char latter = input[i];
                if (!char.IsLetter(latter))
                    continue;
                string operations;
                string trigonometricCalculations;
                double radians;
                double degrees;
                switch (latter)
                {
                    case 'a': //atan acos asin
                        switch (input[i + 1])
                        {
                            case 't': // atan
                                operations = FindOperationInComplex(i + 4, input);
                                i = i + 5 + operations.Length;
                                radians = Calc(operations);
                                degrees = IsRadians ? radians : radians * (Math.PI / 180);
                                degrees = degrees > 1.57 ? 1.57 : degrees < -1.57 ? -1.57 : degrees;
                                trigonometricCalculations =
                                    Math.Round(Math.Atan(degrees), 2).ToString(CultureInfo.InvariantCulture);

                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("atan" + operations, trigonometricCalculations);
                                break;

                            case 'c': // acos
                                operations = FindOperationInComplex(i + 4, input);
                                i = i + 5 + operations.Length;
                                radians = Calc(operations);
                                degrees = IsRadians ? radians : radians * (Math.PI / 180);
                                degrees = degrees > 1 ? 1 : degrees < -1 ? -1 : degrees;
                                trigonometricCalculations =
                                    Math.Round(Math.Acos(degrees), 2).ToString(CultureInfo.InvariantCulture);

                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("acos" + operations, trigonometricCalculations);
                                break;

                            case 's'://asin
                                operations = FindOperationInComplex(i + 4, input);
                                i = i + 5 + operations.Length;
                                radians = Calc(operations);
                                degrees = IsRadians ? radians : radians * (Math.PI / 180);
                                degrees = degrees > 1 ? 1 : degrees < -1 ? -1 : degrees;
                                trigonometricCalculations =
                                    Math.Round(Math.Asin(degrees), 2).ToString(CultureInfo.InvariantCulture);
                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("asin" + operations, trigonometricCalculations);
                                break;

                        }
                        break;

                    case 't'://tan
                        operations = FindOperationInComplex(i + 3, input);
                        i = i + 4 + operations.Length;
                        radians = Calc(operations);
                        degrees = IsRadians ? radians : radians * (Math.PI / 180);
                        trigonometricCalculations =
                            Math.Round(Math.Tan(degrees), 2).ToString(CultureInfo.InvariantCulture);

                        trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                        input = input.Replace("tan" + operations, trigonometricCalculations);
                        break;

                    case 's':// sin sqrt
                        switch (input[i + 1])
                        {
                            case 'i'://sin
                                operations = FindOperationInComplex(i + 3, input);
                                i = i + 4 + operations.Length;
                                radians = Calc(operations);
                                degrees = IsRadians ? radians : radians * (Math.PI / 180);
                                trigonometricCalculations =
                                    Math.Round(Math.Sin(degrees), 2).ToString(CultureInfo.InvariantCulture);

                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("sin" + operations, trigonometricCalculations);
                                break;

                            case 'q'://sqrt
                                operations = FindOperationInComplex(i + 4, input);
                                i = i + 5 + operations.Length;
                                radians = Calc(operations);
                                trigonometricCalculations = Math.Sqrt(radians).ToString(CultureInfo.InvariantCulture);

                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("sqrt" + operations, trigonometricCalculations);
                                break;
                        }
                        break;

                    case 'c'://cos
                        operations = FindOperationInComplex(i + 3, input);
                        i = i + 4 + operations.Length;
                        radians = Calc(operations);
                        degrees = IsRadians ? radians : radians * (Math.PI / 180);
                        trigonometricCalculations =
                            Math.Round(Math.Cos(degrees), 2).ToString(CultureInfo.InvariantCulture);

                        trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                        input = input.Replace("cos" + operations, trigonometricCalculations);
                        break;

                    case 'l': // log ln 
                        switch (input[i + 1])
                        {
                            case 'o'://log
                                operations = FindOperationInComplex(i + 3, input);
                                i = i + 4 + operations.Length;
                                radians = Calc(operations);
                                trigonometricCalculations =
                                    Math.Round(Math.Log10(radians), 2).ToString(CultureInfo.InvariantCulture);

                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("log" + operations, trigonometricCalculations);
                                break;

                            case 'n'://ln
                                operations = FindOperationInComplex(i + 2, input);
                                i = i + 3 + operations.Length;
                                radians = Calc(operations);
                                trigonometricCalculations =
                                    Math.Round(Math.Log(radians), 2).ToString(CultureInfo.InvariantCulture);

                                trigonometricCalculations = trigonometricCalculations.Replace('.', ',');
                                input = input.Replace("ln" + operations, trigonometricCalculations);
                                break;
                        }
                        break;

                    default:
                        throw new Exception("Введен не правильный символ");

                }
            }
        }

        private static bool IsDelimeter(char c)
        {
            return " =".IndexOf(c) != -1;
        }

        private static bool IsOperator(char с)
        {
            return "+-/*^()%!".IndexOf(с) != -1;
        }

        private static long Factorial(long n) => n == 1 ? 1 : n * Factorial(n - 1);

        private static byte GetPriority(char s)
        {
            switch (s)
            {
                case '(':
                    return 0;
                case ')':
                    return 1;
                case '+':
                    return 2;
                case '-':
                    return 3;
                case '*':
                case '/':
                    return 4;
                case '^':
                    return 5;
                case '%':
                    return 6;
                case '!':
                    return 7;
                default:
                    return 8;
            }
        }
    }
}
