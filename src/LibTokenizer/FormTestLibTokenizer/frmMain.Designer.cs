namespace FormTestLibTokenizer
{
	partial class frmMain
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
		if(disposing && (components != null)) {
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkMinimize = new System.Windows.Forms.CheckBox();
			this.pthTarget = new Bau.Controls.Files.TextBoxSelectPath();
			this.pthSource = new Bau.Controls.Files.TextBoxSelectPath();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtTarget = new System.Windows.Forms.TextBox();
			this.cmdTest = new System.Windows.Forms.Button();
			this.tabSource = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.txtSource = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabSource.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.pthTarget);
			this.groupBox1.Controls.Add(this.pthSource);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox1.Location = new System.Drawing.Point(6, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox1.Size = new System.Drawing.Size(653, 81);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Origen";
			// 
			// chkMinimize
			// 
			this.chkMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkMinimize.AutoSize = true;
			this.chkMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkMinimize.ForeColor = System.Drawing.Color.Black;
			this.chkMinimize.Location = new System.Drawing.Point(481, 345);
			this.chkMinimize.Name = "chkMinimize";
			this.chkMinimize.Size = new System.Drawing.Size(69, 17);
			this.chkMinimize.TabIndex = 3;
			this.chkMinimize.Text = "Minimizar";
			this.chkMinimize.UseVisualStyleBackColor = true;
			// 
			// pthTarget
			// 
			this.pthTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pthTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pthTarget.ForeColor = System.Drawing.Color.Black;
			this.pthTarget.Location = new System.Drawing.Point(127, 52);
			this.pthTarget.Margin = new System.Windows.Forms.Padding(0);
			this.pthTarget.MaximumSize = new System.Drawing.Size(13611, 20);
			this.pthTarget.MinimumSize = new System.Drawing.Size(272, 20);
			this.pthTarget.Name = "pthTarget";
			this.pthTarget.PathName = "";
			this.pthTarget.Size = new System.Drawing.Size(520, 20);
			this.pthTarget.TabIndex = 2;
			// 
			// pthSource
			// 
			this.pthSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pthSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pthSource.ForeColor = System.Drawing.Color.Black;
			this.pthSource.Location = new System.Drawing.Point(127, 22);
			this.pthSource.Margin = new System.Windows.Forms.Padding(0);
			this.pthSource.MaximumSize = new System.Drawing.Size(11667, 20);
			this.pthSource.MinimumSize = new System.Drawing.Size(233, 20);
			this.pthSource.Name = "pthSource";
			this.pthSource.PathName = "";
			this.pthSource.Size = new System.Drawing.Size(520, 20);
			this.pthSource.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label2.Location = new System.Drawing.Point(9, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Directorio destino:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label1.Location = new System.Drawing.Point(9, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Directorio origen:";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.txtTarget);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox2.Location = new System.Drawing.Point(7, 376);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox2.Size = new System.Drawing.Size(671, 365);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Resultado";
			// 
			// txtTarget
			// 
			this.txtTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTarget.Location = new System.Drawing.Point(5, 18);
			this.txtTarget.Multiline = true;
			this.txtTarget.Name = "txtTarget";
			this.txtTarget.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTarget.Size = new System.Drawing.Size(661, 342);
			this.txtTarget.TabIndex = 0;
			// 
			// cmdTest
			// 
			this.cmdTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdTest.Location = new System.Drawing.Point(564, 337);
			this.cmdTest.Name = "cmdTest";
			this.cmdTest.Size = new System.Drawing.Size(116, 33);
			this.cmdTest.TabIndex = 1;
			this.cmdTest.Text = "Interpretar";
			this.cmdTest.UseVisualStyleBackColor = true;
			this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
			// 
			// tabSource
			// 
			this.tabSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabSource.Controls.Add(this.tabPage1);
			this.tabSource.Controls.Add(this.tabPage2);
			this.tabSource.Location = new System.Drawing.Point(7, 5);
			this.tabSource.Name = "tabSource";
			this.tabSource.SelectedIndex = 0;
			this.tabSource.Size = new System.Drawing.Size(673, 325);
			this.tabSource.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(665, 299);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Directorio";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.groupBox4);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(665, 299);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Snippet";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.txtSource);
			this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox4.Location = new System.Drawing.Point(6, 6);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox4.Size = new System.Drawing.Size(653, 287);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Origen";
			// 
			// txtSource
			// 
			this.txtSource.AcceptsReturn = true;
			this.txtSource.AcceptsTab = true;
			this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSource.Location = new System.Drawing.Point(5, 18);
			this.txtSource.Multiline = true;
			this.txtSource.Name = "txtSource";
			this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSource.Size = new System.Drawing.Size(643, 264);
			this.txtSource.TabIndex = 0;
			this.txtSource.WordWrap = false;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(685, 753);
			this.Controls.Add(this.chkMinimize);
			this.Controls.Add(this.cmdTest);
			this.Controls.Add(this.tabSource);
			this.Controls.Add(this.groupBox2);
			this.Name = "frmMain";
			this.Text = "Pruebas de LibTokenizer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabSource.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtTarget;
		private System.Windows.Forms.Button cmdTest;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private Bau.Controls.Files.TextBoxSelectPath pthTarget;
		private Bau.Controls.Files.TextBoxSelectPath pthSource;
		private System.Windows.Forms.CheckBox chkMinimize;
		private System.Windows.Forms.TabControl tabSource;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox txtSource;
	}
}

