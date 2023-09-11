using Stockfish_Visualizer.Classes;
using Stockfish_Visualizer.Controls;

namespace Stockfish_Visualizer.Forms;

public static class UserPrompt
{
    public static string ShowDialog(string promptText, string formTitle, params string[] options)
    {
        DraggableForm prompt = new()
        {
            FormBorderStyle = FormBorderStyle.None,
            Text = formTitle,
            StartPosition = FormStartPosition.CenterScreen,

            BackColor = Color.FromArgb(34, 34, 34),
            ForeColor = Color.White,
        };

        var header = new Label()
        {
            Font = new Font("Arial", 24, FontStyle.Regular),
            Text = promptText,
        };
        prompt.Controls.Add(header);

        // Holds the option buttons
        var optionPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
            BackColor = Color.FromArgb(40, 40, 40)
        };
        prompt.Controls.Add(optionPanel);

        string selected = "";

        // Create the option buttons
        for (int i = 0; i < options.Length; i++)
        {
            var option = options[i];

            var button = new CButton
            {
                Text = option,
                AutoSize = true,
                BorderRadius = 20,
            };

            button.Click += (sender, e) =>
            {
                selected = option;
                prompt.Close();
            };

            optionPanel.Controls.Add(button);
        }

        // Positioning
        prompt.Size = new(optionPanel.Width + 50, header.Height + optionPanel.Height + 50);
        header.Location = new((int)(prompt.Width * .05), (int)(prompt.Height * .25));
        optionPanel.Location = new(25, header.Location.Y + header.Height + 15);

        if (options.Length % 2 is 1)
        {
            var button = optionPanel.Controls[options.Length - 1];
            button.Margin = new Padding((optionPanel.Width / 2) - (button.Width / 2) - 10, 0, 0, 0);
        }

        // Better styling
        prompt.MakeCurved(20);
        optionPanel.MakeCurved(20);
        prompt.MouseDown += prompt.HandleFormMove;
        header.MouseDown += prompt.HandleFormMove;
        optionPanel.MouseDown += prompt.HandleFormMove;

        prompt.ShowDialog();

        return selected;
    }
}