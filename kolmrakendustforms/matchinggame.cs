using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace kolmrakendustforms
{
    public class MatchingGameForm : Form
    {
        TableLayoutPanel table = new() { Dock = DockStyle.Fill, BackColor = Color.LightSlateGray };
        Panel topPanel = new() { Height = 50, Dock = DockStyle.Top, BackColor = Color.LightGray };

        Button restartButton = new() { Text = "Restart", Left = 10, Width = 100, Top = 10 };
        Button iconSetButton = new() { Text = "Kasuta loomad", Left = 120, Width = 160, Top = 10 };
        Button difficultyButton = new() { Text = "Lihtne / Raske", Left = 290, Width = 140, Top = 10 };

        List<string> icons = new() { "!", "N", ",", "k", "b", "v", "w", "z", "x", "y", "m", "t", "u", "o", "p" };
        List<string> loomad = new() { "🐶", "🐱", "🐸", "🐻", "🦊", "🐭", "🐼", "🐰", "🦁", "🐯", "🐨", "🐵", "🐔", "🐧", "🐦" };

        List<string> currentIcons;
        Label firstClicked = null, secondClicked = null; // esimene kart ja teine kart
        Random random = new();
        bool allowClick = true;

        int rows = 4, columns = 4;

        public MatchingGameForm()
        {
            Text = "Matching Game";
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;

            BackColor = Color.FromArgb(240, 248, 255);
            ForeColor = Color.DarkSlateGray;
            Font = new Font("Segoe UI", 9);

            topPanel.BackColor = Color.FromArgb(225, 235, 245);
            topPanel.Padding = new Padding(10);

            restartButton.BackColor = Color.LightGreen;
            restartButton.ForeColor = Color.DarkGreen;
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            restartButton.FlatAppearance.BorderColor = Color.DarkGreen;

            iconSetButton.BackColor = Color.LightSkyBlue;
            iconSetButton.ForeColor = Color.MidnightBlue;
            iconSetButton.FlatStyle = FlatStyle.Flat;
            iconSetButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            iconSetButton.FlatAppearance.BorderColor = Color.SteelBlue;

            difficultyButton.BackColor = Color.Khaki;
            difficultyButton.ForeColor = Color.DarkGoldenrod;
            difficultyButton.FlatStyle = FlatStyle.Flat;
            difficultyButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            difficultyButton.FlatAppearance.BorderColor = Color.Goldenrod;

            currentIcons = icons;

            Controls.Add(table);
            Controls.Add(topPanel);
            topPanel.Controls.AddRange(new Control[] { restartButton, iconSetButton, difficultyButton });

            restartButton.Click += (s, e) => RestartGame();
            iconSetButton.Click += (s, e) => ToggleIconSet();
            difficultyButton.Click += (s, e) => ToggleDifficulty();

            CreateTable();
            AddCardsToTable();
        }

        void CreateTable()
        {
            table.Controls.Clear();
            table.RowStyles.Clear();
            table.ColumnStyles.Clear();

            table.RowCount = rows;
            table.ColumnCount = columns;

            for (int i = 0; i < rows; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            for (int j = 0; j < columns; j++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
        }

        void AddCardsToTable()
        {
            table.Controls.Clear();

            int totalCards = rows * columns; // 16
            if (totalCards % 2 != 0) // нечетное
            {
                totalCards--;
            }

            int pairs = totalCards / 2;
            var iconsToUse = currentIcons.Take(pairs).ToList();

            var cardIcons = iconsToUse.Concat(iconsToUse).OrderBy(_ => random.Next()).ToList();

            foreach (var icon in cardIcons)
            {
                var card = new Label
                {
                    Tag = icon, // icon
                    Font = new Font("Segoe UI Emoji", 36, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    BackColor = Color.AliceBlue,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = ""
                };
                card.Click += Card_Click;
                table.Controls.Add(card);
            }
        } // <- закрыли AddCardsToTable

        void Card_Click(object sender, EventArgs e)
        {
            if (!allowClick) return;
            var clicked = sender as Label;
            if (clicked == null || clicked.Text != "") return;

            clicked.Text = clicked.Tag.ToString();

            if (firstClicked == null)
            {
                firstClicked = clicked;
                clicked.BackColor = Color.LightCyan;
                return;
            }

            secondClicked = clicked;
            clicked.BackColor = Color.LightCyan;

            if (firstClicked.Text == secondClicked.Text)
            {
                firstClicked.BackColor = secondClicked.BackColor = Color.LightGreen;
                firstClicked = secondClicked = null;

                if (table.Controls.OfType<Label>().All(c => c.Text != ""))
                    MessageBox.Show("Palju õnne! Leidsite kõik paarid!");
            }
            else
            {
                allowClick = false;
                var timer = new Timer { Interval = 700 };
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    firstClicked.BackColor = secondClicked.BackColor = Color.AliceBlue;
                    firstClicked.Text = secondClicked.Text = "";
                    firstClicked = secondClicked = null;
                    allowClick = true;
                };
                timer.Start();
            }
        }

        void RestartGame()
        {
            firstClicked = secondClicked = null;
            allowClick = true;
            CreateTable();
            AddCardsToTable();
        }

        void ToggleIconSet()
        {
            if (currentIcons.SequenceEqual(icons))
            {
                currentIcons = loomad;
                iconSetButton.Text = "Kasuta sümbolid";
            }
            else
            {
                currentIcons = icons;
                iconSetButton.Text = "Kasuta loomad";
            }
            RestartGame();
        }

        void ToggleDifficulty()
        {
            if (rows == 4)
            {
                rows = 5;
                columns = 6;
            }
            else
            {
                rows = columns = 4;
            }
            RestartGame();
        }
    }
}
