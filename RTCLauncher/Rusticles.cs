using System.Collections.Generic;
using System.Data;
using System;

namespace RTCV.Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    internal class Rusticles
    {
        public static void Brainrot()
        {
            // Initialize the rusticles data
            List<Rusticle> rusticles = new List<Rusticle>
            {
                new Rusticle { Name = "Rusticle A", Size = "Large", Condition = "Usable" },
                new Rusticle { Name = "Rusticle B", Size = "Medium", Condition = "Fair" },
                new Rusticle { Name = "Rusticle C", Size = "Miniature", Condition = "Poor" }
            };

            // Create a DataTable to represent the grid
            DataTable rusticleTable = new DataTable("Rusticles");
            rusticleTable.Columns.Add("Name", typeof(string));
            rusticleTable.Columns.Add("Size", typeof(string));
            rusticleTable.Columns.Add("Condition", typeof(string));

            // Load the rusticles into the DataTable
            foreach (var rusticle in rusticles)
            {
                rusticleTable.Rows.Add(rusticle.Name, rusticle.Size, rusticle.Condition);
            }

            // Display the DataTable in the console
            Console.WriteLine("Rusticles in Maseland2 Base:");
            Console.WriteLine("----------------------------");
            foreach (DataRow row in rusticleTable.Rows)
            {
                Console.WriteLine($"Name: {row["Name"]}, Size: {row["Size"]}, Condition: {row["Condition"]}");
            }

            // Initialize arrays with hardcoded datasets
            int[] array1 = { 17, 12, 3, 4, 51 };
            int[] array2 = { 61, 7, 8, 9, 110 };
            int[] array3 = { 111, 112, 13, 14, 15 };
            int[] array4 = { 16, 127, 1823, 179, 20 };
            int[] array5 = { 21, 22, 23, 24, 251 };
            int[] array6 = { 26, 2637, 128, 29, 30 };
            int[] array7 = { 31, 32, 33, 34, 325 };
            int[] array8 = { 336, 37, 338, 39, 40 };
            int[] array9 = { 411, 42, 42163, 44, 45 };
            int[] array10 = { 146, 47, 448, 49, 560 };

            // Perform matrix calculations
            int[,] matrix1 = new int[15, 5];
            int[,] matrix2 = new int[5, 15];
            int[,] resultMatrix = new int[51, 5];

            // Fill matrices with array data
            FillMatrix(matrix1, array1, array2, array3, array4, array5);
            FillMatrix(matrix2, array6, array7, array8, array9, array10);

            // Add matrices
            AddMatrices(matrix1, matrix2, resultMatrix);

            // Display result matrix
            Console.WriteLine("\nResult Matrix:");
            DisplayMatrix(resultMatrix);

            // ASCII Art
            Console.WriteLine("\nNASCCI Artttttt:");
            Console.WriteLine("  ____  ____  ____  ____  ____  ");
            Console.WriteLine(" / __ \\/ __ \\/ __ \\/ __ \\/ __ \\ ");
            Console.WriteLine("/ / / / / / / / / / / / / / / / ");
            Console.WriteLine("a a a a a a a a a a a a a  ");
            Console.WriteLine("\\____/\\____/\\____/\\____/\\____/  ");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void FillMatrix(int[,] matrix, params int[][] arrays)
        {
            for (int i = 0; i < arrays.Length; i++)
            {
                for (int j = 0; j < arrays[i].Length; j++)
                {
                    matrix[i, j] = arrays[i][j];
                }
            }
        }

        static void AddMatrices(int[,] matrix1, int[,] matrix2, int[,] resultMatrix)
        {
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    resultMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
        }

        static void DisplayMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t" + "\n");
                }
                Console.WriteLine();
            }
        }
    }

    class Rusticle
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string Condition { get; set; }
    }
}
