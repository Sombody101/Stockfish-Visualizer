namespace Stockfish_Visualizer
{
    partial class VisualizerException
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisualizerException));
            ErrorIcon = new PictureBox();
            ErrorReason = new Label();
            ErrorReasonPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)ErrorIcon).BeginInit();
            ErrorReasonPanel.SuspendLayout();
            SuspendLayout();
            // 
            // ErrorIcon
            // 
            ErrorIcon.Image = Resources.error;
            ErrorIcon.Location = new Point(309, 58);
            ErrorIcon.Name = "ErrorIcon";
            ErrorIcon.Size = new Size(150, 150);
            ErrorIcon.SizeMode = PictureBoxSizeMode.Zoom;
            ErrorIcon.TabIndex = 0;
            ErrorIcon.TabStop = false;
            // 
            // ErrorReason
            // 
            ErrorReason.AutoSize = true;
            ErrorReason.FlatStyle = FlatStyle.Flat;
            ErrorReason.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ErrorReason.ForeColor = Color.White;
            ErrorReason.Location = new Point(73, 36);
            ErrorReason.Name = "ErrorReason";
            ErrorReason.Size = new Size(115, 21);
            ErrorReason.TabIndex = 1;
            ErrorReason.Text = "PLACEHOLDER";
            ErrorReason.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ErrorReasonPanel
            // 
            ErrorReasonPanel.BackColor = Color.FromArgb(40, 40, 40);
            ErrorReasonPanel.Controls.Add(ErrorReason);
            ErrorReasonPanel.Location = new Point(264, 271);
            ErrorReasonPanel.Name = "ErrorReasonPanel";
            ErrorReasonPanel.Size = new Size(273, 88);
            ErrorReasonPanel.TabIndex = 2;
            // 
            // VisualizerException
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(800, 450);
            Controls.Add(ErrorReasonPanel);
            Controls.Add(ErrorIcon);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "VisualizerException";
            Text = "VisualizerException";
            ((System.ComponentModel.ISupportInitialize)ErrorIcon).EndInit();
            ErrorReasonPanel.ResumeLayout(false);
            ErrorReasonPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox ErrorIcon;
        private Label ErrorReason;
        private Panel ErrorReasonPanel;
    }
}