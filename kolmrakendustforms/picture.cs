using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace kolmrakendustforms
{
    public class PictureViewerForm : Form
    {
        PictureBox pictureBox;
        Button clearButton, bgButton, closeButton, loadButton, prevButton, nextButton;
        CheckBox stretchCheckBox;

        List<Image> images = new List<Image>();
        int currentIndex = -1;

        public PictureViewerForm()
        {
            Text = "Picture Viewer";
            Width = 800;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;

            BackColor = Color.FromArgb(240, 248, 255);
            ForeColor = Color.DarkSlateGray;
            Font = new Font("Segoe UI", 9);

            pictureBox = new PictureBox()
            {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.WhiteSmoke
            };

            clearButton = new Button()
            {
                Text = "Clear the picture",
                BackColor = Color.LightCoral,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            clearButton.FlatAppearance.BorderColor = Color.Firebrick;

            bgButton = new Button()
            {
                Text = "Set the background color",
                BackColor = Color.Khaki,
                ForeColor = Color.DarkGoldenrod,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            bgButton.FlatAppearance.BorderColor = Color.Goldenrod;

            closeButton = new Button()
            {
                Text = "Close",
                BackColor = Color.Salmon,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            closeButton.FlatAppearance.BorderColor = Color.DarkRed;

            loadButton = new Button()
            {
                Text = "Load Images",
                BackColor = Color.LightGreen,
                ForeColor = Color.DarkGreen,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            loadButton.FlatAppearance.BorderColor = Color.DarkGreen;

            prevButton = new Button()
            {
                Text = "< Previous",
                BackColor = Color.LightSkyBlue,
                ForeColor = Color.MidnightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            prevButton.FlatAppearance.BorderColor = Color.SteelBlue;

            nextButton = new Button()
            {
                Text = "Next >",
                BackColor = Color.LightSkyBlue,
                ForeColor = Color.MidnightBlue,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            nextButton.FlatAppearance.BorderColor = Color.SteelBlue;

            stretchCheckBox = new CheckBox()
            {
                Text = "Stretch",
                ForeColor = Color.DarkSlateGray,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true
            };

            FlowLayoutPanel panel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Bottom,
                Height = 45,
                BackColor = Color.FromArgb(225, 235, 245),
                Padding = new Padding(5)
            };

            panel.Controls.Add(stretchCheckBox);
            panel.Controls.Add(loadButton);
            panel.Controls.Add(prevButton);
            panel.Controls.Add(nextButton);
            panel.Controls.Add(bgButton);
            panel.Controls.Add(clearButton);
            panel.Controls.Add(closeButton);

            Controls.Add(pictureBox);
            Controls.Add(panel);

            loadButton.Click += LoadImagesButton_Click;
            prevButton.Click += PrevButton_Click;
            nextButton.Click += NextButton_Click;
            clearButton.Click += (s, e) =>
            {
                pictureBox.Image = null;
                images.Clear();
                currentIndex = -1;
            };

            bgButton.Click += BgButton_Click;
            stretchCheckBox.CheckedChanged += (s, e) =>
                pictureBox.SizeMode = stretchCheckBox.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Zoom;
            closeButton.Click += (s, e) => this.Close();
        }

        private void LoadImagesButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.png";
                dlg.Multiselect = true; // mitmed fotod
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    images.Clear();
                    foreach (var file in dlg.FileNames)
                    {
                        images.Add(Image.FromFile(file));
                    }
                    if (images.Count > 0) // näitab esimene foto
                    {
                        currentIndex = 0;   
                        pictureBox.Image = images[currentIndex];
                    }
                }
            }
        }

        private void PrevButton_Click(object sender, EventArgs e) //eelmine
        {
            if (images.Count == 0) return; // me ei tagasta midagi
            currentIndex--; // vähendame piltide indeksit
            if (currentIndex < 0) currentIndex = images.Count - 1; // vähem kui 0 / tagasi viimasele pildile
            pictureBox.Image = images[currentIndex];
        }

        private void NextButton_Click(object sender, EventArgs e) //järgmine
        {
            if (images.Count == 0) return;
            currentIndex++; // lisame
            if (currentIndex >= images.Count) currentIndex = 0;
            pictureBox.Image = images[currentIndex];
        }

        private void BgButton_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog()) // avame aken
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                    pictureBox.BackColor = dlg.Color;
            }
        }
    }
}
