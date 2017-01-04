using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ClickmaniaFinal
{
    class Game
    {
        // Поля класса
        private int rowCounts;
        private int columnCounts;
        private int colorsCounts;
        private int rowIndex;
        private int colIndex;
        private int[,] field; //создаем массив кубиков - игровое поле
        private Random rnd = new Random();
        private int index = 0;
        private double scoreColor = 1.2;
        private int score;

        // Индексатор и геттеры
        public int this[int rowIndex, int colIndex] { get { return field[rowIndex, colIndex]; } } // индексатор, возращает цвет 
        public int ColorsCount { get { return colorsCounts; } } 
        public int ColumnsCount { get { return columnCounts; } }
        public int RowsCount { get { return rowCounts; } }
        public int Score { get { return score; } }

        public Game(int rowCounts, int columnCounts, int colorsCounts) // Конструктор класса
        {
            this.rowCounts = rowCounts;
            this.columnCounts = columnCounts;
            this.colorsCounts = colorsCounts;

            CreateField();
        }

        public bool Click(int colIndex, int rowIndex) // Функция клика, получает индексы нажатия и выясняет строку и колонку 
        {
            this.rowIndex = rowIndex / 25;
            this.colIndex = colIndex / 25;
            if (this.rowIndex < rowCounts && this.colIndex < columnCounts)
            {
                Nieghbor(this.rowIndex, this.colIndex, field[this.rowIndex, this.colIndex]);
                FallRows();
                FallColumns();
            }
            if (index == 0)
                return false;
            index = 0;
            return true;
        }


        private void CreateField() // Создаем поле и забиваем рандомными числами(цветами)
        {
            field = new int[rowCounts, columnCounts];

            for (int rows = 0; rows < rowCounts; rows++)
                for (int columns = 0; columns < columnCounts; columns++)
                {
                    field[rows, columns] = rnd.Next(1, colorsCounts);
                }
        }

        private void Nieghbor(int rowIndex, int colIndex, int colorClick) // Определение соседних одинаковых кружков и их удаление
        {
            int[,] f = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
            for (int i = 0; i < 4; i++)
            {
                if (rowIndex + f[i, 0] < field.GetLength(0) && rowIndex + f[i, 0] >= 0 && colIndex + f[i, 1] < field.GetLength(1) && colIndex + f[i, 1] >= 0)
                    if (field[rowIndex + f[i, 0], colIndex + f[i, 1]] == colorClick && field[rowIndex + f[i, 0], colIndex + f[i, 1]] != 0)
                    {
                        field[rowIndex, colIndex] = 0;
                        index++;
                        Nieghbor(rowIndex + f[i, 0], colIndex + f[i, 1], colorClick);
                    }
            }
            if (index == 0)
                return;
            field[rowIndex, colIndex] = 0;
            score += (int)Math.Pow(scoreColor, index); // очки начисляем 
        }

        private bool IsColumnEmpty(int colIndex) // Проверяет на пустоту колонку
        {
            for (int r = 0; r < rowCounts; r++)
            {
                if (field[r, colIndex] != 0)
                    return false;
            }
            return true;
        }


        private void CopyColToCol(int colFrom, int toCol) // Копирует колонку в колонку
        {
            for (int row = 0; row < rowCounts; row++)
            {
                field[row, toCol] = field[row, colFrom];
            }
        }

        private void SetColZero(int col) // Забивает колонку нулевыми значениями
        {
            for (int row = 0; row < rowCounts; row++)
            {
                field[row, col] = 0;
            }
        }

        private void FallColumns() // Метод сдвижения колонок влево
        {
            int index1 = 0;
            for (int col = 0; col < columnCounts; col++)
            {
                if (!IsColumnEmpty(col))
                {
                    CopyColToCol(col, index1);
                    index1++;
                }
            }

            for (; index1 < columnCounts; index1++)
            {
                SetColZero(index1);
            }

        }

        private void FallRows() // Метод падения строк
        {
            for (int cols = 0; cols < columnCounts; cols++)
            {
                int index1 = rowCounts - 1;
                for (int rows = rowCounts - 1; rows >= 0; rows--)
                {
                    if (field[rows, cols] != 0)
                    {
                        field[index1, cols] = field[rows, cols];
                        index1--;
                    }
                }

                for (; index1 >= 0; index1--)
                {
                    field[index1, cols] = 0;
                }
            }
        }
    }
}
