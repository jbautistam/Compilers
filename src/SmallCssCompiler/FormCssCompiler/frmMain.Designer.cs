namespace TestCssCompiler {
	partial class frmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
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
		private void InitializeComponent() {
			this.cmdCompileFile = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optCompilePath = new System.Windows.Forms.RadioButton();
			this.optCompileFile = new System.Windows.Forms.RadioButton();
			this.fnPathSource = new TestCssCompiler.UC.TextBoxSelectPath();
			this.fnFileSource = new TestCssCompiler.UC.TextBoxSelectFile();
			this.chkMinimize = new System.Windows.Forms.CheckBox();
			this.chkKillTarget = new System.Windows.Forms.CheckBox();
			this.fnPathTarget = new TestCssCompiler.UC.TextBoxSelectPath();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdCompileFile
			// 
			this.cmdCompileFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCompileFile.Location = new System.Drawing.Point(601, 194);
			this.cmdCompileFile.Name = "cmdCompileFile";
			this.cmdCompileFile.Size = new System.Drawing.Size(135, 31);
			this.cmdCompileFile.TabIndex = 2;
			this.cmdCompileFile.Text = "Compilar";
			this.cmdCompileFile.UseVisualStyleBackColor = true;
			this.cmdCompileFile.Click += new System.EventHandler(this.cmdCompile_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.optCompilePath);
			this.groupBox1.Controls.Add(this.optCompileFile);
			this.groupBox1.Controls.Add(this.fnPathSource);
			this.groupBox1.Controls.Add(this.fnFileSource);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox1.Location = new System.Drawing.Point(7, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(732, 81);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Archivo / directorio fuente";
			// 
			// optCompilePath
			// 
			this.optCompilePath.AutoSize = true;
			this.optCompilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.optCompilePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.optCompilePath.Location = new System.Drawing.Point(20, 51);
			this.optCompilePath.Name = "optCompilePath";
			this.optCompilePath.Size = new System.Drawing.Size(73, 17);
			this.optCompilePath.TabIndex = 4;
			this.optCompilePath.Text = "Directorio:";
			this.optCompilePath.UseVisualStyleBackColor = true;
			this.optCompilePath.CheckedChanged += new System.EventHandler(this.optCompileFile_CheckedChanged);
			// 
			// optCompileFile
			// 
			this.optCompileFile.AutoSize = true;
			this.optCompileFile.Checked = true;
			this.optCompileFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.optCompileFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.optCompileFile.Location = new System.Drawing.Point(20, 24);
			this.optCompileFile.Name = "optCompileFile";
			this.optCompileFile.Size = new System.Drawing.Size(64, 17);
			this.optCompileFile.TabIndex = 4;
			this.optCompileFile.TabStop = true;
			this.optCompileFile.Text = "Archivo:";
			this.optCompileFile.UseVisualStyleBackColor = true;
			this.optCompileFile.CheckedChanged += new System.EventHandler(this.optCompileFile_CheckedChanged);
			// 
			// fnPathSource
			// 
			this.fnPathSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnPathSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fnPathSource.ForeColor = System.Drawing.Color.Black;
			this.fnPathSource.Location = new System.Drawing.Point(121, 48);
			this.fnPathSource.Margin = new System.Windows.Forms.Padding(0);
			this.fnPathSource.MaximumSize = new System.Drawing.Size(10000, 20);
			this.fnPathSource.MinimumSize = new System.Drawing.Size(200, 20);
			this.fnPathSource.Name = "fnPathSource";
			this.fnPathSource.PathName = "";
			this.fnPathSource.Size = new System.Drawing.Size(603, 20);
			this.fnPathSource.TabIndex = 3;
			// 
			// fnFileSource
			// 
			this.fnFileSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnFileSource.BackColorEdit = System.Drawing.SystemColors.Window;
			this.fnFileSource.FileName = "";
			this.fnFileSource.Filter = "Todos los archivos|*.*";
			this.fnFileSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fnFileSource.ForeColor = System.Drawing.Color.Black;
			this.fnFileSource.Location = new System.Drawing.Point(121, 24);
			this.fnFileSource.Margin = new System.Windows.Forms.Padding(0);
			this.fnFileSource.MaximumSize = new System.Drawing.Size(10000, 20);
			this.fnFileSource.MinimumSize = new System.Drawing.Size(200, 20);
			this.fnFileSource.Name = "fnFileSource";
			this.fnFileSource.Size = new System.Drawing.Size(603, 20);
			this.fnFileSource.TabIndex = 1;
			this.fnFileSource.Type = TestCssCompiler.UC.TextBoxSelectFile.FileSelectType.Load;
			// 
			// chkMinimize
			// 
			this.chkMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkMinimize.AutoSize = true;
			this.chkMinimize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkMinimize.ForeColor = System.Drawing.Color.Black;
			this.chkMinimize.Location = new System.Drawing.Point(588, 58);
			this.chkMinimize.Name = "chkMinimize";
			this.chkMinimize.Size = new System.Drawing.Size(136, 17);
			this.chkMinimize.TabIndex = 3;
			this.chkMinimize.Text = "Minimizar archivos CSS";
			this.chkMinimize.UseVisualStyleBackColor = true;
			// 
			// chkKillTarget
			// 
			this.chkKillTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkKillTarget.AutoSize = true;
			this.chkKillTarget.Checked = true;
			this.chkKillTarget.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkKillTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkKillTarget.ForeColor = System.Drawing.Color.Black;
			this.chkKillTarget.Location = new System.Drawing.Point(416, 58);
			this.chkKillTarget.Name = "chkKillTarget";
			this.chkKillTarget.Size = new System.Drawing.Size(154, 17);
			this.chkKillTarget.TabIndex = 2;
			this.chkKillTarget.Text = "Eliminar archivos anteriores";
			this.chkKillTarget.UseVisualStyleBackColor = true;
			// 
			// fnPathTarget
			// 
			this.fnPathTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnPathTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fnPathTarget.ForeColor = System.Drawing.Color.Black;
			this.fnPathTarget.Location = new System.Drawing.Point(124, 27);
			this.fnPathTarget.Margin = new System.Windows.Forms.Padding(0);
			this.fnPathTarget.MaximumSize = new System.Drawing.Size(10000, 20);
			this.fnPathTarget.MinimumSize = new System.Drawing.Size(200, 20);
			this.fnPathTarget.Name = "fnPathTarget";
			this.fnPathTarget.PathName = "";
			this.fnPathTarget.Size = new System.Drawing.Size(603, 20);
			this.fnPathTarget.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label3.Location = new System.Drawing.Point(20, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(85, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Directorio salida:";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.chkMinimize);
			this.groupBox2.Controls.Add(this.fnPathTarget);
			this.groupBox2.Controls.Add(this.chkKillTarget);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox2.Location = new System.Drawing.Point(4, 100);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(732, 88);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Resultado de compilación";
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(743, 237);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmdCompileFile);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Compilador SmallCss";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdCompileFile;
		private System.Windows.Forms.GroupBox groupBox1;
		private TestCssCompiler.UC.TextBoxSelectPath fnPathTarget;
		private TestCssCompiler.UC.TextBoxSelectPath fnPathSource;
		private TestCssCompiler.UC.TextBoxSelectFile fnFileSource;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox chkMinimize;
		private System.Windows.Forms.CheckBox chkKillTarget;
		private System.Windows.Forms.RadioButton optCompilePath;
		private System.Windows.Forms.RadioButton optCompileFile;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}

