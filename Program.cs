using System;
using System.IO;

namespace Diagram
{
    class Program
    {
        const string path = "output.txt";
        static void Main()
        {
           // Объявление переменных.
            string symbol;
            double minEl, maxEl;

            do // Цикл повтора решения.
            {
                Console.WriteLine("Введите числа через пробел:");
                double[] array = ReadData();
                Console.WriteLine("Введите символ: ");
                ReadSymbol(out symbol);

                (maxEl, minEl) = FindMinMax(array);
                int[] normArray = CreateNormArray(array, minEl, maxEl);
                string[,] BarChartArray = CreateBarChart(normArray, symbol);
                FormArrayOutput(BarChartArray, normArray.Length, minEl, maxEl);

                Console.WriteLine("Для завершения программы нажмите Escape...");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Находит минимум и максимум в исходном массиве.
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        /// <returns>Максимальный и минимальный элемент.</returns>
        static (double, double) FindMinMax(double[] array)
        {
            double minEl = array[0];
            double maxEl = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (minEl > array[i])
                    minEl = array[i];
                if (maxEl < array[i])
                    maxEl = array[i];
            }
            return (maxEl, minEl);
        }
        
        /// <summary>
        /// Нормирует числа исходного массива и проекцирует их на отрезок [0,10].
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        /// <param name="minEl">Минимальный элемент массива.</param>
        /// <param name="maxEl">Максимальный элемент массива.</param>
        /// <returns></returns>
        static int[] CreateNormArray (double[] array, double minEl, double maxEl)
        {
            int[] normArray = new int[array.Length];
            if (maxEl != minEl)
            {
                for (int i = 0; i < array.Length; i++)   
                    normArray[i] = (int)Math.Round(10 * (array[i] - minEl) / (maxEl - minEl));
                
            }
            else
            {       
                for (int i = 0; i < array.Length; i++)
                    normArray[i] = 10;                          
            }
            return normArray;
        }
      
        /// <summary>
        /// Считывает символ для отрисовки гистограммы. Если введен некорректно, запрашивает снова.
        /// </summary>
        /// <param name="symbol">Символ.</param>
        static void ReadSymbol(out string symbol)
        {
            symbol = Console.ReadLine();
            if (symbol.Length != 1)
            {
                Logger("Требуется ОДИН символ. Повторите ввод." + Environment.NewLine);
                ReadSymbol(out symbol);
            }
            if (symbol == " ")
            {
                Logger("Введите не пробел: " + Environment.NewLine);
                ReadSymbol(out symbol);
            }
        }

        /// <summary>
        /// Считывает входную строку и содает массив вещ. чисел. Если введена некорректно, запрашивает снова.
        /// </summary>
        /// <returns>Массив вещественных чисел.</returns>
        static double[] ReadData()
        {
            string[] separators = new string[] { " " };
            string inputString = Console.ReadLine();
            // Если в строке есть лишние пробелы, считается правильно.
            string[] strArr = inputString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (strArr.Length > 120 && strArr.Length == 0)
            {
                Logger("Некорректное количество элементов в строке. Повторите ввод:" + Environment.NewLine);
                ReadData();
            }
            double[] arrayOfDouble = new double[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {            
                if (!Check(out arrayOfDouble[i], strArr[i]))
                {
                    Logger("Введено не число. Введите строку снова." + Environment.NewLine);
                    ReadData();
                }
                if (!CheckRange(arrayOfDouble[i]))
                {
                    Logger("Введено число вне границ. Введите строку снова." + Environment.NewLine);
                    ReadData();
                }
            }
            return arrayOfDouble;     
        }

        /// <summary>
        /// Создание массива для отрисовки гистограммы.
        /// </summary>
        /// <param name="normArray">Массив нормированных чисел.</param>
        /// <param name="symbol">Символ для отрисовки.</param>
        /// <returns>Двумерный массив символов для гистограммы.</returns>
        static string[,] CreateBarChart(int[] normArray, string symbol)
        {
            string[,] BarChartArray = new string[10, normArray.Length];

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < normArray.Length; j++)
                    BarChartArray[i, j] = " ";
            }

            for (int j = 0; j < normArray.Length; j++)
            {
                for (int i = 9; i > -1; i--)
                {
                    if (i > 9 - normArray[j])
                        BarChartArray[i, j] = symbol;
                    else
                        break;
                }
            }
            return BarChartArray;
        }

        /// <summary>
        /// Проверяет входное число на принадлежность отрезку [-1024;1024].
        /// </summary>
        /// <param name="el">Проверяемый параметр.</param>
        /// <returns>true, если принадлежит.</returns>
        static bool CheckRange(double el) => (el <= 1024 && el >= -1024);

        /// <summary>
        /// Проверяет, что введены вещественные числа.
        /// </summary>
        /// <param name="elOfDoubleArr">Число.</param>
        /// <param name="elOfStringArr">Проверяемый параметр.</param>
        /// <returns>true, если введено число.</returns>
        static bool Check(out double elOfDoubleArr, string elOfStringArr) 
            => double.TryParse(elOfStringArr, out elOfDoubleArr);
        
        /// <summary>
        /// Формирует вывод и запись в файл гистограммы по шаблону.
        /// </summary>
        /// <param name="BarChartArray">Массив символов гистограммы.</param>
        /// <param name="numOfEl">Кол-во чисел в исходном массиве.</param>
        /// <param name="min">Минимальный элемент исходного массива.</param>
        /// <param name="max">Максимальный элемент исходного массива.</param>
        static void FormArrayOutput(string[,] BarChartArray, int numOfEl, double min, double max)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                    Logger($"{max:f2}");
                if (i == 9)
                    Logger($"{min:f2}");
                Logger("\t|\t");              
                for (int j = 0; j < numOfEl; j++)
                    Logger(BarChartArray[i, j]);
                Logger(Environment.NewLine);
            }
            Logger("============================" + Environment.NewLine);
        }

        /// <summary>
        /// Записывает выходную строку в консоль и файл.
        /// </summary>
        /// <param name="outputStr">Выходная строка.</param>
        static void Logger(string outputStr)
        {
            Console.Write(outputStr);
            try
            {
                File.AppendAllText(path, outputStr);
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка записи в файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
