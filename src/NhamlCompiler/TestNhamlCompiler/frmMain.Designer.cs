namespace TestNhamlCompiler
{
	partial class frmMain
	{
		/// <summary>
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
		protected override void Dispose(bool disposing)
		{
		if(disposing && (components != null)) {
		components.Dispose();
		}
		base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.txtSource = new System.Windows.Forms.TextBox();
			this.cmdParse = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.txtTarget = new System.Windows.Forms.TextBox();
			this.txtEvents = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.fnFile = new Bau.Controls.Files.TextBoxSelectFile();
			this.cmdSave = new System.Windows.Forms.Button();
			this.chkCompress = new System.Windows.Forms.CheckBox();
			this.cmdLoad = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtSource
			// 
			this.txtSource.AcceptsReturn = true;
			this.txtSource.AcceptsTab = true;
			this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSource.Location = new System.Drawing.Point(0, 0);
			this.txtSource.Multiline = true;
			this.txtSource.Name = "txtSource";
			this.txtSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtSource.Size = new System.Drawing.Size(571, 617);
			this.txtSource.TabIndex = 0;
			this.txtSource.Text = resources.GetString("txtSource.Text");
			// 
			// cmdParse
			// 
			this.cmdParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdParse.Location = new System.Drawing.Point(761, 656);
			this.cmdParse.Name = "cmdParse";
			this.cmdParse.Size = new System.Drawing.Size(103, 31);
			this.cmdParse.TabIndex = 3;
			this.cmdParse.Text = "Interpretar";
			this.cmdParse.UseVisualStyleBackColor = true;
			this.cmdParse.Click += new System.EventHandler(this.cmdParse_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(2, 33);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.txtSource);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(860, 617);
			this.splitContainer1.SplitterDistance = 571;
			this.splitContainer1.TabIndex = 2;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.txtTarget);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.txtEvents);
			this.splitContainer2.Size = new System.Drawing.Size(285, 617);
			this.splitContainer2.SplitterDistance = 302;
			this.splitContainer2.TabIndex = 2;
			// 
			// txtTarget
			// 
			this.txtTarget.AcceptsReturn = true;
			this.txtTarget.AcceptsTab = true;
			this.txtTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtTarget.Location = new System.Drawing.Point(0, 0);
			this.txtTarget.Multiline = true;
			this.txtTarget.Name = "txtTarget";
			this.txtTarget.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtTarget.Size = new System.Drawing.Size(285, 302);
			this.txtTarget.TabIndex = 0;
			// 
			// txtEvents
			// 
			this.txtEvents.AcceptsReturn = true;
			this.txtEvents.AcceptsTab = true;
			this.txtEvents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtEvents.Location = new System.Drawing.Point(0, 0);
			this.txtEvents.Multiline = true;
			this.txtEvents.Name = "txtEvents";
			this.txtEvents.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEvents.Size = new System.Drawing.Size(285, 311);
			this.txtEvents.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Archivo:";
			// 
			// fnFile
			// 
			this.fnFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnFile.BackColorEdit = System.Drawing.SystemColors.Window;
			this.fnFile.FileName = "";
			this.fnFile.Filter = "Todos los archivos|*.*";
			this.fnFile.Location = new System.Drawing.Point(69, 7);
			this.fnFile.Margin = new System.Windows.Forms.Padding(0);
			this.fnFile.MaximumSize = new System.Drawing.Size(10000, 20);
			this.fnFile.MinimumSize = new System.Drawing.Size(100, 20);
			this.fnFile.Name = "fnFile";
			this.fnFile.Size = new System.Drawing.Size(627, 20);
			this.fnFile.TabIndex = 1;
			this.fnFile.Type = Bau.Controls.Files.TextBoxSelectFile.FileSelectType.Load;
			// 
			// cmdSave
			// 
			this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdSave.Location = new System.Drawing.Point(785, 5);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(76, 23);
			this.cmdSave.TabIndex = 2;
			this.cmdSave.Text = "Guardar";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// chkCompress
			// 
			this.chkCompress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkCompress.AutoSize = true;
			this.chkCompress.Location = new System.Drawing.Point(7, 661);
			this.chkCompress.Name = "chkCompress";
			this.chkCompress.Size = new System.Drawing.Size(112, 17);
			this.chkCompress.TabIndex = 4;
			this.chkCompress.Text = "Comprimir la salida";
			this.chkCompress.UseVisualStyleBackColor = true;
			// 
			// cmdLoad
			// 
			this.cmdLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdLoad.Location = new System.Drawing.Point(699, 5);
			this.cmdLoad.Name = "cmdLoad";
			this.cmdLoad.Size = new System.Drawing.Size(84, 23);
			this.cmdLoad.TabIndex = 2;
			this.cmdLoad.Text = "Cargar";
			this.cmdLoad.UseVisualStyleBackColor = true;
			this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
			// 
			// frmMain
			// 
			this.AcceptButton = this.cmdParse;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(866, 690);
			this.Controls.Add(this.chkCompress);
			this.Controls.Add(this.fnFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.cmdLoad);
			this.Controls.Add(this.cmdSave);
			this.Controls.Add(this.cmdParse);
			this.Name = "frmMain";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtSource;
		private System.Windows.Forms.Button cmdParse;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox txtTarget;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.TextBox txtEvents;
		private System.Windows.Forms.Label label1;
		private Bau.Controls.Files.TextBoxSelectFile fnFile;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.CheckBox chkCompress;
		private System.Windows.Forms.Button cmdLoad;
	}
}

