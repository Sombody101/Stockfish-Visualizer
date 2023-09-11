namespace Stockfish_Visualizer
{
    partial class Visualizer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Visualizer));
            AppTitleLabel = new Label();
            Titlebar = new Panel();
            MinimizeButton = new Button();
            ExitButton = new Button();
            GamePresenter = new RichTextBox();
            Titlebar.SuspendLayout();
            SuspendLayout();
            // 
            // AppTitleLabel
            // 
            AppTitleLabel.AutoSize = true;
            AppTitleLabel.Dock = DockStyle.Left;
            AppTitleLabel.Location = new Point(0, 0);
            AppTitleLabel.Margin = new Padding(10, 10, 3, 0);
            AppTitleLabel.Name = "AppTitleLabel";
            AppTitleLabel.Padding = new Padding(5, 8, 0, 0);
            AppTitleLabel.Size = new Size(112, 23);
            AppTitleLabel.TabIndex = 0;
            AppTitleLabel.Text = "Stockfish Visualizer";
            AppTitleLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // Titlebar
            // 
            Titlebar.BackColor = Color.FromArgb(40, 40, 40);
            Titlebar.Controls.Add(MinimizeButton);
            Titlebar.Controls.Add(ExitButton);
            Titlebar.Controls.Add(AppTitleLabel);
            Titlebar.Dock = DockStyle.Top;
            Titlebar.Location = new Point(0, 0);
            Titlebar.Name = "Titlebar";
            Titlebar.Size = new Size(1300, 30);
            Titlebar.TabIndex = 1;
            // 
            // MinimizeButton
            // 
            MinimizeButton.Dock = DockStyle.Right;
            MinimizeButton.FlatAppearance.BorderSize = 0;
            MinimizeButton.FlatStyle = FlatStyle.Flat;
            MinimizeButton.ForeColor = Color.Transparent;
            MinimizeButton.Location = new Point(1220, 0);
            MinimizeButton.Name = "MinimizeButton";
            MinimizeButton.Size = new Size(40, 30);
            MinimizeButton.TabIndex = 2;
            MinimizeButton.Text = "_";
            MinimizeButton.TextAlign = ContentAlignment.TopCenter;
            MinimizeButton.UseVisualStyleBackColor = true;
            // 
            // ExitButton
            // 
            ExitButton.Dock = DockStyle.Right;
            ExitButton.FlatAppearance.BorderSize = 0;
            ExitButton.FlatStyle = FlatStyle.Flat;
            ExitButton.Location = new Point(1260, 0);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new Size(40, 30);
            ExitButton.TabIndex = 1;
            ExitButton.Text = "X";
            ExitButton.UseVisualStyleBackColor = true;
            // 
            // GamePresenter
            // 
            GamePresenter.BackColor = Color.FromArgb(30, 30, 30);
            GamePresenter.BorderStyle = BorderStyle.None;
            GamePresenter.Dock = DockStyle.Left;
            GamePresenter.ForeColor = Color.White;
            GamePresenter.Location = new Point(0, 30);
            GamePresenter.Name = "GamePresenter";
            GamePresenter.ReadOnly = true;
            GamePresenter.Size = new Size(233, 590);
            GamePresenter.TabIndex = 2;
            GamePresenter.Text = "";
            // 
            // Visualizer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 34, 34);
            ClientSize = new Size(1300, 620);
            Controls.Add(GamePresenter);
            Controls.Add(Titlebar);
            DoubleBuffered = true;
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Visualizer";
            Text = "Visualizer";
            Load += Visualizer_Load;
            Titlebar.ResumeLayout(false);
            Titlebar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label AppTitleLabel;
        private Button MinimizeButton;
        private Button ExitButton;
        public RichTextBox GamePresenter;
        public Panel Titlebar;
    }
}