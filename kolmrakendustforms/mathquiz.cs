using System;
using System.Drawing;
using System.Windows.Forms;

namespace kolmrakendustforms
{
    public class MathQuizForm : Form
    {
        Label timeLabel;
        Label addLabel, subLabel, mulLabel, divLabel;
        NumericUpDown addAns, subAns, mulAns, divAns;
        Button startButton, pauseButton, doneButton;
        ComboBox difficultyBox;
        Timer timer;
        int timeLeft;

        Random random = new Random();

        int addLeft, addRight, subLeft, subRight, mulLeft, mulRight, divLeft, divRight;
        bool isPaused = false;

        public MathQuizForm()
        {
            Text = "Math Quiz";
            Width = 450;
            Height = 350;
            StartPosition = FormStartPosition.CenterScreen;

            BackColor = Color.FromArgb(240, 248, 255);
            ForeColor = Color.DarkSlateGray;

            timeLabel = new Label()
            {
                Text = "Aeg on: 30 sec",
                Top = 10,
                Left = 20,
                Width = 150,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.MidnightBlue
            };

            difficultyBox = new ComboBox()
            {
                Left = 200,
                Top = 10,
                Width = 100,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.DarkSlateGray,
                Font = new Font("Segoe UI", 9)
            };
            difficultyBox.Items.AddRange(new string[] { "Easy", "Medium", "Hard" });
            difficultyBox.SelectedIndex = 1;

            startButton = new Button()
            {
                Text = "Start",
                Top = 250,
                Left = 20,
                Width = 100,
                BackColor = Color.FromArgb(144, 238, 144),
                ForeColor = Color.DarkGreen,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            startButton.FlatAppearance.BorderColor = Color.DarkGreen;
            startButton.Click += StartButton_Click;

            pauseButton = new Button()
            {
                Text = "Pause",
                Top = 250,
                Left = 130,
                Width = 100,
                BackColor = Color.Khaki,
                ForeColor = Color.DarkGoldenrod,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            pauseButton.FlatAppearance.BorderColor = Color.Goldenrod;
            pauseButton.Click += PauseButton_Click;

            doneButton = new Button()
            {
                Text = "Tehtud",
                Top = 250,
                Left = 240,
                Width = 100,
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            doneButton.FlatAppearance.BorderColor = Color.Firebrick;
            doneButton.Click += DoneButton_Click;

            addLabel = new Label() { Top = 60, Left = 20, Width = 100, ForeColor = Color.DarkSlateGray };
            addAns = new NumericUpDown() { Top = 60, Left = 130, Width = 60, BackColor = Color.White };

            subLabel = new Label() { Top = 90, Left = 20, Width = 100, ForeColor = Color.DarkSlateGray };
            subAns = new NumericUpDown() { Top = 90, Left = 130, Width = 60, BackColor = Color.White };

            mulLabel = new Label() { Top = 120, Left = 20, Width = 100, ForeColor = Color.DarkSlateGray };
            mulAns = new NumericUpDown() { Top = 120, Left = 130, Width = 60, BackColor = Color.White };

            divLabel = new Label() { Top = 150, Left = 20, Width = 100, ForeColor = Color.DarkSlateGray };
            divAns = new NumericUpDown() { Top = 150, Left = 130, Width = 60, BackColor = Color.White };

            Controls.AddRange(new Control[]
            {
                timeLabel, difficultyBox,
                startButton, pauseButton, doneButton,
                addLabel, addAns,
                subLabel, subAns,
                mulLabel, mulAns,
                divLabel, divAns
            });

            timer = new Timer();
            timer.Interval = 1000; // 1sec
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            GenerateQuiz();
            timeLeft = 30;
            timeLabel.Text = $"Aeg on: {timeLeft} sec";
            timer.Start();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                timer.Stop();
                isPaused = true;
                pauseButton.Text = "Resume";
            }
            else
            {
                timer.Start();
                isPaused = false;
                pauseButton.Text = "Pause";
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            CheckAnswers();
        }

        private void GenerateQuiz()
        {
            int min = 1, max = 10;
            if (difficultyBox.SelectedItem.ToString() == "Medium") { min = 10; max = 35; }
            else if (difficultyBox.SelectedItem.ToString() == "Hard") { min = 50; max = 100; }

            addLeft = random.Next(min, max);
            addRight = random.Next(min, max);
            subLeft = random.Next(min, max);
            subRight = random.Next(min, max);
            mulLeft = random.Next(1, 10);
            mulRight = random.Next(1, 10);
            divRight = random.Next(1, 10);
            divLeft = divRight * random.Next(1, 10);

            addLabel.Text = $"{addLeft} + {addRight} =";
            subLabel.Text = $"{subLeft} - {subRight} =";
            mulLabel.Text = $"{mulLeft} × {mulRight} =";
            divLabel.Text = $"{divLeft} ÷ {divRight} =";

            addAns.Value = subAns.Value = mulAns.Value = divAns.Value = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                timeLabel.Text = $"Aeg on: {timeLeft} sec";
            }
            else
            {
                timer.Stop();
                CheckAnswers();
            }
        }

        private void CheckAnswers()
        {
            int correct = 0;
            string result = "";

            if (addAns.Value == addLeft + addRight)
            {
                correct++;
                result += "Addition: Correct\n";
            }
            else
                result += $"Addition: Wrong ({addLeft}+{addRight}={addLeft + addRight}, your answer={addAns.Value})\n";

            if (subAns.Value == subLeft - subRight)
            {
                correct++;
                result += "Subtraction: Correct\n";
            }
            else
                result += $"Subtraction: Wrong ({subLeft}-{subRight}={subLeft - subRight}, your answer={subAns.Value})\n";

            if (mulAns.Value == mulLeft * mulRight)
            {
                correct++;
                result += "Multiplication: Correct\n";
            }
            else
                result += $"Multiplication: Wrong ({mulLeft}×{mulRight}={mulLeft * mulRight}, your answer={mulAns.Value})\n";

            if (divAns.Value == divLeft / divRight)
            {
                correct++;
                result += "Division: Correct\n";
            }
            else
                result += $"Division: Wrong ({divLeft}÷{divRight}={divLeft / divRight}, your answer={divAns.Value})\n";

            result += $"\nTotal correct: {correct}/4";
            MessageBox.Show(result, "Quiz Result");
        }
    }
}
