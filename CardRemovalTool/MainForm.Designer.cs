namespace CardRemovalTool
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
            this.btnShowCardDistribution = new System.Windows.Forms.Button();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.btnAddFromClipboard = new System.Windows.Forms.Button();
            this.btnGetCurrentWithRemoval = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOmaha = new System.Windows.Forms.RadioButton();
            this.rbHoldem = new System.Windows.Forms.RadioButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.outputGroupBox = new System.Windows.Forms.GroupBox();
            this.rbCSV = new System.Windows.Forms.RadioButton();
            this.rbPPT = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.outputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnShowCardDistribution
            // 
            this.btnShowCardDistribution.Location = new System.Drawing.Point(3, 105);
            this.btnShowCardDistribution.Name = "btnShowCardDistribution";
            this.btnShowCardDistribution.Size = new System.Drawing.Size(262, 23);
            this.btnShowCardDistribution.TabIndex = 2;
            this.btnShowCardDistribution.Text = "Show distrubution of cards in the deck";
            this.btnShowCardDistribution.UseVisualStyleBackColor = true;
            this.btnShowCardDistribution.Click += new System.EventHandler(this.cardDistributionClick);
            // 
            // outputTextBox
            // 
            this.outputTextBox.BackColor = System.Drawing.Color.White;
            this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.outputTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextBox.Location = new System.Drawing.Point(269, 0);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(464, 413);
            this.outputTextBox.TabIndex = 10;
            // 
            // btnAddFromClipboard
            // 
            this.btnAddFromClipboard.Location = new System.Drawing.Point(3, 47);
            this.btnAddFromClipboard.Name = "btnAddFromClipboard";
            this.btnAddFromClipboard.Size = new System.Drawing.Size(262, 23);
            this.btnAddFromClipboard.TabIndex = 0;
            this.btnAddFromClipboard.Text = "Add next range from clipboard";
            this.btnAddFromClipboard.UseVisualStyleBackColor = true;
            this.btnAddFromClipboard.Click += new System.EventHandler(this.btnAddFromClipboard_Click);
            // 
            // btnGetCurrentWithRemoval
            // 
            this.btnGetCurrentWithRemoval.Location = new System.Drawing.Point(3, 76);
            this.btnGetCurrentWithRemoval.Name = "btnGetCurrentWithRemoval";
            this.btnGetCurrentWithRemoval.Size = new System.Drawing.Size(262, 23);
            this.btnGetCurrentWithRemoval.TabIndex = 1;
            this.btnGetCurrentWithRemoval.Text = "Update range from clipboard with card removal";
            this.btnGetCurrentWithRemoval.UseVisualStyleBackColor = true;
            this.btnGetCurrentWithRemoval.Click += new System.EventHandler(this.btnGetCurrentWithRemoval_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(3, 134);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(262, 23);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset all";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 243);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(262, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "prepare ranges";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOmaha);
            this.groupBox1.Controls.Add(this.rbHoldem);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(139, 38);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // rbOmaha
            // 
            this.rbOmaha.AutoSize = true;
            this.rbOmaha.Location = new System.Drawing.Point(73, 15);
            this.rbOmaha.Name = "rbOmaha";
            this.rbOmaha.Size = new System.Drawing.Size(59, 17);
            this.rbOmaha.TabIndex = 1;
            this.rbOmaha.Text = "Omaha";
            this.rbOmaha.UseVisualStyleBackColor = true;
            this.rbOmaha.CheckedChanged += new System.EventHandler(this.rbOmaha_CheckedChanged);
            // 
            // rbHoldem
            // 
            this.rbHoldem.AutoSize = true;
            this.rbHoldem.Checked = true;
            this.rbHoldem.Location = new System.Drawing.Point(6, 15);
            this.rbHoldem.Name = "rbHoldem";
            this.rbHoldem.Size = new System.Drawing.Size(61, 17);
            this.rbHoldem.TabIndex = 0;
            this.rbHoldem.TabStop = true;
            this.rbHoldem.Text = "Holdem";
            this.rbHoldem.UseVisualStyleBackColor = true;
            this.rbHoldem.CheckedChanged += new System.EventHandler(this.rbHoldem_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(3, 416);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(733, 23);
            this.progressBar.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.outputGroupBox);
            this.panel1.Controls.Add(this.outputTextBox);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnShowCardDistribution);
            this.panel1.Controls.Add(this.btnAddFromClipboard);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.btnGetCurrentWithRemoval);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(733, 413);
            this.panel1.TabIndex = 13;
            // 
            // outputGroupBox
            // 
            this.outputGroupBox.Controls.Add(this.rbCSV);
            this.outputGroupBox.Controls.Add(this.rbPPT);
            this.outputGroupBox.Location = new System.Drawing.Point(148, 3);
            this.outputGroupBox.Name = "outputGroupBox";
            this.outputGroupBox.Size = new System.Drawing.Size(117, 38);
            this.outputGroupBox.TabIndex = 12;
            this.outputGroupBox.TabStop = false;
            this.outputGroupBox.Text = "output";
            this.outputGroupBox.Visible = false;
            // 
            // rbCSV
            // 
            this.rbCSV.AutoSize = true;
            this.rbCSV.Location = new System.Drawing.Point(58, 15);
            this.rbCSV.Name = "rbCSV";
            this.rbCSV.Size = new System.Drawing.Size(46, 17);
            this.rbCSV.TabIndex = 1;
            this.rbCSV.Text = "CSV";
            this.rbCSV.UseVisualStyleBackColor = true;
            this.rbCSV.CheckedChanged += new System.EventHandler(this.rbCSV_CheckedChanged);
            // 
            // rbPPT
            // 
            this.rbPPT.AutoSize = true;
            this.rbPPT.Checked = true;
            this.rbPPT.Location = new System.Drawing.Point(6, 15);
            this.rbPPT.Name = "rbPPT";
            this.rbPPT.Size = new System.Drawing.Size(46, 17);
            this.rbPPT.TabIndex = 0;
            this.rbPPT.TabStop = true;
            this.rbPPT.Text = "PPT";
            this.rbPPT.UseVisualStyleBackColor = true;
            this.rbPPT.CheckedChanged += new System.EventHandler(this.rbPPT_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 272);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(262, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "subsctruct ranges";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 442);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Card removal tool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.outputGroupBox.ResumeLayout(false);
            this.outputGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowCardDistribution;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button btnAddFromClipboard;
        private System.Windows.Forms.Button btnGetCurrentWithRemoval;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbOmaha;
        private System.Windows.Forms.RadioButton rbHoldem;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox outputGroupBox;
        private System.Windows.Forms.RadioButton rbCSV;
        private System.Windows.Forms.RadioButton rbPPT;
        private System.Windows.Forms.Button button2;
    }
}

