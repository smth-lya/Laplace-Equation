using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaplaceSOR.Solving
{
    internal class Program
    {
        static void Main()
        {
            int width = 200;
            int height = 200;

            //Console.SetWindowSize(width + 2, height + 1);

            double[,] grid = new double[height, width];

            LaplaceEquationSolver solver = new LaplaceEquationSolver(grid);
            solver.ResetGrid();
            solver.GridUpdated += () => Console.WriteLine(1);
            solver.SolveSequentially();
            Console.WriteLine(1);
        }

        public static void PrintGrid(double[,] grid)
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            Thread.Sleep(10);

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    Console.Write(GetColor(grid[y, x]));
                }
                Console.WriteLine();
            }

            Console.WriteLine($"\x1B[48;2;{0};{0};{0}m ");
        }

        static string GetColor(double value)
        {
            int r = (int)(255 * value / 100.0);
            int b = 255 - (int)(255 * value / 100.0);       
            int g = 0;                                  

            return $"\x1B[48;2;{r};{g};{b}m ";
        }

        static string GetColor(int value, int minValue, int maxValue)
        {
            double percent = (double)(value - minValue) / (maxValue - minValue);
            percent = Math.Max(0, Math.Min(1, percent)); 

            int r = (int)(255 * (1 - percent)); 
            int b = (int)(255 * percent);      
            int g = 0;                          

            return $"\x1B[48;2;{r};{g};{b}m";
        }
    }
}

