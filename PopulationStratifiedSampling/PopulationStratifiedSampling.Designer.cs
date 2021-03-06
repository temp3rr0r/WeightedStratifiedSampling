﻿namespace PopulationStratifiedSampling
{
    partial class PopulationStratifiedSampling
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
            this.btnSample = new System.Windows.Forms.Button();
            this.txtSamples = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkShowTextLog = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCellCountX = new System.Windows.Forms.ComboBox();
            this.cmbCellCountY = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbMaxGlobalPopulationDensity = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbMaxLocalPopulationDensity = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbLocalSamplingProbability = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvStrata = new System.Windows.Forms.DataGridView();
            this.MinX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Samples = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ttxStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlbExecutionTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tpbProgress = new System.Windows.Forms.ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrata)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSample
            // 
            this.btnSample.Location = new System.Drawing.Point(1006, 549);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(79, 23);
            this.btnSample.TabIndex = 0;
            this.btnSample.Text = "Sample";
            this.btnSample.UseVisualStyleBackColor = true;
            this.btnSample.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSamples
            // 
            this.txtSamples.Location = new System.Drawing.Point(11, 25);
            this.txtSamples.Multiline = true;
            this.txtSamples.Name = "txtSamples";
            this.txtSamples.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSamples.Size = new System.Drawing.Size(268, 194);
            this.txtSamples.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(285, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(691, 41);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 400);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Random Population Density";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(688, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Random Stratified Sampling (Population Density weighted)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Final Sampling Log:";
            // 
            // chkShowTextLog
            // 
            this.chkShowTextLog.AutoSize = true;
            this.chkShowTextLog.Checked = true;
            this.chkShowTextLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowTextLog.Location = new System.Drawing.Point(6, 19);
            this.chkShowTextLog.Name = "chkShowTextLog";
            this.chkShowTextLog.Size = new System.Drawing.Size(94, 17);
            this.chkShowTextLog.TabIndex = 7;
            this.chkShowTextLog.Text = "Show text Log";
            this.chkShowTextLog.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Cell Count X:";
            // 
            // cmbCellCountX
            // 
            this.cmbCellCountX.FormattingEnabled = true;
            this.cmbCellCountX.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "200"});
            this.cmbCellCountX.Location = new System.Drawing.Point(204, 37);
            this.cmbCellCountX.Name = "cmbCellCountX";
            this.cmbCellCountX.Size = new System.Drawing.Size(54, 21);
            this.cmbCellCountX.TabIndex = 9;
            this.cmbCellCountX.Text = "40";
            // 
            // cmbCellCountY
            // 
            this.cmbCellCountY.FormattingEnabled = true;
            this.cmbCellCountY.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "200"});
            this.cmbCellCountY.Location = new System.Drawing.Point(204, 64);
            this.cmbCellCountY.Name = "cmbCellCountY";
            this.cmbCellCountY.Size = new System.Drawing.Size(54, 21);
            this.cmbCellCountY.TabIndex = 11;
            this.cmbCellCountY.Text = "40";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Cell Count Y:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkShowTextLog);
            this.groupBox1.Controls.Add(this.cmbCellCountY);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbCellCountX);
            this.groupBox1.Location = new System.Drawing.Point(15, 225);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 97);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // cmbMaxGlobalPopulationDensity
            // 
            this.cmbMaxGlobalPopulationDensity.FormattingEnabled = true;
            this.cmbMaxGlobalPopulationDensity.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "50",
            "100",
            "200"});
            this.cmbMaxGlobalPopulationDensity.Location = new System.Drawing.Point(204, 19);
            this.cmbMaxGlobalPopulationDensity.Name = "cmbMaxGlobalPopulationDensity";
            this.cmbMaxGlobalPopulationDensity.Size = new System.Drawing.Size(54, 21);
            this.cmbMaxGlobalPopulationDensity.TabIndex = 13;
            this.cmbMaxGlobalPopulationDensity.Text = "50";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Global Max:";
            // 
            // cmbMaxLocalPopulationDensity
            // 
            this.cmbMaxLocalPopulationDensity.FormattingEnabled = true;
            this.cmbMaxLocalPopulationDensity.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "50",
            "100",
            "200"});
            this.cmbMaxLocalPopulationDensity.Location = new System.Drawing.Point(204, 46);
            this.cmbMaxLocalPopulationDensity.Name = "cmbMaxLocalPopulationDensity";
            this.cmbMaxLocalPopulationDensity.Size = new System.Drawing.Size(54, 21);
            this.cmbMaxLocalPopulationDensity.TabIndex = 15;
            this.cmbMaxLocalPopulationDensity.Text = "100";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Local Max:";
            // 
            // cmbLocalSamplingProbability
            // 
            this.cmbLocalSamplingProbability.FormattingEnabled = true;
            this.cmbLocalSamplingProbability.Items.AddRange(new object[] {
            "0.001",
            "0.01",
            "0.1",
            "1.0"});
            this.cmbLocalSamplingProbability.Location = new System.Drawing.Point(204, 73);
            this.cmbLocalSamplingProbability.Name = "cmbLocalSamplingProbability";
            this.cmbLocalSamplingProbability.Size = new System.Drawing.Size(54, 21);
            this.cmbLocalSamplingProbability.TabIndex = 17;
            this.cmbLocalSamplingProbability.Text = "0.3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Local Sampling Probability:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cmbLocalSamplingProbability);
            this.groupBox2.Controls.Add(this.cmbMaxGlobalPopulationDensity);
            this.groupBox2.Controls.Add(this.cmbMaxLocalPopulationDensity);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(15, 328);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 113);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Population Density";
            // 
            // dgvStrata
            // 
            this.dgvStrata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStrata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MinX,
            this.MaxX,
            this.MinY,
            this.MaxY,
            this.Samples});
            this.dgvStrata.Location = new System.Drawing.Point(15, 460);
            this.dgvStrata.Name = "dgvStrata";
            this.dgvStrata.Size = new System.Drawing.Size(331, 125);
            this.dgvStrata.TabIndex = 14;
            // 
            // MinX
            // 
            this.MinX.HeaderText = "MinX";
            this.MinX.Name = "MinX";
            this.MinX.Width = 50;
            // 
            // MaxX
            // 
            this.MaxX.HeaderText = "MaxX";
            this.MaxX.Name = "MaxX";
            this.MaxX.Width = 50;
            // 
            // MinY
            // 
            this.MinY.HeaderText = "MinY";
            this.MinY.Name = "MinY";
            this.MinY.Width = 50;
            // 
            // MaxY
            // 
            this.MaxY.HeaderText = "MaxY";
            this.MaxY.Name = "MaxY";
            this.MaxY.Width = 50;
            // 
            // Samples
            // 
            this.Samples.HeaderText = "Samples";
            this.Samples.Name = "Samples";
            this.Samples.Width = 80;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 444);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(182, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Strata Bounding Boxes and Samples:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ttxStatus,
            this.toolStripStatusLabel3,
            this.tlbExecutionTime,
            this.toolStripStatusLabel5,
            this.tpbProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 608);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1099, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // ttxStatus
            // 
            this.ttxStatus.Name = "ttxStatus";
            this.ttxStatus.Size = new System.Drawing.Size(38, 17);
            this.ttxStatus.Text = "Done.";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(91, 17);
            this.toolStripStatusLabel3.Text = "Execution Time:";
            // 
            // tlbExecutionTime
            // 
            this.tlbExecutionTime.Name = "tlbExecutionTime";
            this.tlbExecutionTime.Size = new System.Drawing.Size(13, 17);
            this.tlbExecutionTime.Text = "0";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(23, 17);
            this.toolStripStatusLabel5.Text = "ms";
            // 
            // tpbProgress
            // 
            this.tpbProgress.MarqueeAnimationSpeed = 20;
            this.tpbProgress.Name = "tpbProgress";
            this.tpbProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // PopulationStratifiedSampling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 630);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dgvStrata);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtSamples);
            this.Controls.Add(this.btnSample);
            this.Name = "PopulationStratifiedSampling";
            this.Text = "Random Stratified Sampling (Population density weighted)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrata)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSample;
        private System.Windows.Forms.TextBox txtSamples;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkShowTextLog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCellCountX;
        private System.Windows.Forms.ComboBox cmbCellCountY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbMaxGlobalPopulationDensity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbMaxLocalPopulationDensity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbLocalSamplingProbability;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvStrata;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinX;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxX;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinY;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxY;
        private System.Windows.Forms.DataGridViewTextBoxColumn Samples;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel ttxStatus;
        private System.Windows.Forms.ToolStripProgressBar tpbProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlbExecutionTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
    }
}

