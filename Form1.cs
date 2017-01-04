using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickmaniaFinal
{
    public partial class Form1 : Form
    {
        // Поля класса
        private Game game;
        private Random rnd;
        // Набор цветов для кружков
        private Brush[] brushes = new[] {Color.LightGray, Color.Blue, Color.Red, Color.Orange, Color.Green, Color.Magenta}.Select(c => new SolidBrush(c)).ToArray();

        // Конструктор класса
        public Form1()
        {
            InitializeComponent(); // Инициализируем графику
            rnd = new Random();
            for (int i = 2; i <= 5; i++) // Добавляет количество цветов на выбор
                ColorsCounts.Items.Add(i);
        }
        
        private void StartGame_Click(object sender, EventArgs e) // Реакция на клик по кнопке
        {
            int rowCounts = (int)RowCountsNumeric.Value;
            int columnCounts = (int)ColumnCountsNumeric.Value;
            int colorsCounts = Convert.ToInt32(ColorsCounts.Text);

            ScoreText.Text = Convert.ToString(0);
            game = new Game(rowCounts, columnCounts, colorsCounts+1); //создает Game
            panelGame.Invalidate(); // обновляет paint
        }
        
        private void panelGame_Paint(object sender, PaintEventArgs e) // Метод отрисовки графики
        {
            if (game == null) // Если игра не создана, то выходит из метода
                return;
            ScoreText.Text = Convert.ToString(game.Score);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Сглаживания шариков
            
            for (int y = 0; y < game.RowsCount; y++)
                for (int x = 0; x < game.ColumnsCount; x++)
                {
                    e.Graphics.FillEllipse(brushes[game[y, x]], x * 25, y * 25, 25, 25); // Рисует кружок
                }
        }
        
        private void panelGame_MouseClick(object sender, MouseEventArgs e) // Нажатие мышки на поле с шариками
        {
            if(game.Click(e.X, e.Y))
                panelGame.Invalidate();
        }
    }
}

