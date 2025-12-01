namespace ConsoleApp1
{
    partial class Form1
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
            this.glControlSlots = new OpenTK.GLControl();
            this.btnPull = new System.Windows.Forms.Button();
            this.numericUpDownCycles = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycles)).BeginInit();
            this.SuspendLayout();
            // 
            // glControlSlots
            // 
            this.glControlSlots.BackColor = System.Drawing.Color.Black;
            this.glControlSlots.ForeColor = System.Drawing.SystemColors.ControlText;
            this.glControlSlots.Location = new System.Drawing.Point(97, 12);
            this.glControlSlots.Name = "glControlSlots";
            this.glControlSlots.Size = new System.Drawing.Size(643, 219);
            this.glControlSlots.TabIndex = 0;
            this.glControlSlots.VSync = false;
            // 
            // btnPull
            // 
            this.btnPull.Location = new System.Drawing.Point(363, 257);
            this.btnPull.Name = "btnPull";
            this.btnPull.Size = new System.Drawing.Size(101, 58);
            this.btnPull.TabIndex = 1;
            this.btnPull.Text = "TRAGE";
            this.btnPull.UseVisualStyleBackColor = true;
            // 
            // numericUpDownCycles
            // 
            this.numericUpDownCycles.Location = new System.Drawing.Point(72, 267);
            this.numericUpDownCycles.Name = "numericUpDownCycles";
            this.numericUpDownCycles.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownCycles.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.numericUpDownCycles);
            this.Controls.Add(this.btnPull);
            this.Controls.Add(this.glControlSlots);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControlSlots;
        private System.Windows.Forms.Button btnPull;
        private System.Windows.Forms.NumericUpDown numericUpDownCycles;
    }
}