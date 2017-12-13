namespace Kurs
{
    partial class MainForm
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
            this.textBox_inputInfo = new System.Windows.Forms.TextBox();
            this.button_simulate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_inputInfo
            // 
            this.textBox_inputInfo.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_inputInfo.Location = new System.Drawing.Point(15, 25);
            this.textBox_inputInfo.Multiline = true;
            this.textBox_inputInfo.Name = "textBox_inputInfo";
            this.textBox_inputInfo.ReadOnly = true;
            this.textBox_inputInfo.Size = new System.Drawing.Size(596, 133);
            this.textBox_inputInfo.TabIndex = 1;
            // 
            // button_simulate
            // 
            this.button_simulate.Location = new System.Drawing.Point(12, 164);
            this.button_simulate.Name = "button_simulate";
            this.button_simulate.Size = new System.Drawing.Size(282, 23);
            this.button_simulate.TabIndex = 0;
            this.button_simulate.Text = "Запустить симуляцию (Линейный закон)";
            this.button_simulate.UseVisualStyleBackColor = true;
            this.button_simulate.Click += new System.EventHandler(this.button_simulate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Входные параметры:";
            // 
            // textBox_output
            // 
            this.textBox_output.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_output.Location = new System.Drawing.Point(12, 222);
            this.textBox_output.Multiline = true;
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.ReadOnly = true;
            this.textBox_output.Size = new System.Drawing.Size(596, 369);
            this.textBox_output.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 193);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(596, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(328, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(280, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Запустить симуляцию (Экспоненциальный закон)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 603);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBox_output);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_simulate);
            this.Controls.Add(this.textBox_inputInfo);
            this.Name = "MainForm";
            this.Text = "Вычислительная Система";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_inputInfo;
        private System.Windows.Forms.Button button_simulate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_output;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
    }
}

