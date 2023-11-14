namespace WInForm無邊框釘在桌面上
{
    partial class Form1
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
            textShow = new TextBox();
            textInput = new TextBox();
            SuspendLayout();
            // 
            // textShow
            // 
            textShow.BackColor = SystemColors.Desktop;
            textShow.ForeColor = SystemColors.Window;
            textShow.Location = new Point(12, 9);
            textShow.Name = "textShow";
            textShow.Size = new Size(776, 396);
            textShow.TabIndex = 0;
            textShow.Text = "你好\n您好";
            textShow.Multiline = true;
            textShow.ScrollBars = ScrollBars.Vertical;
            // 
            // textInput
            // 
            textInput.BackColor = SystemColors.Desktop;
            textInput.ForeColor = SystemColors.Window;
            textInput.Location = new Point(12, 408);
            textInput.Name = "textInput";
            textInput.Size = new Size(776, 30);
            textInput.TabIndex = 1;
            textInput.KeyDown += textInput_KeyDown;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlText;
            ClientSize = new Size(800, 450);
            Controls.Add(textInput);
            Controls.Add(textShow);
            Location = new Point(3000, 1000);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textShow;
        private TextBox textInput;
    }
}