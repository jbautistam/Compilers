using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Bau.Libraries.LibSmallCssCompiler;

namespace TestCssCompiler 
{
	public partial class frmMain : Form 
	{
		public frmMain() 
		{ InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{ // Carga las propiedades
				fnFileSource.FileName = Properties.Settings.Default.FileNameSource;
				fnPathSource.PathName = Properties.Settings.Default.PathSource;
				fnPathTarget.PathName = Properties.Settings.Default.PathTarget;
			// Habilita / inhabilita los controles
				EnableControls();
		}

		/// <summary>
		///		Compila las fuentes SmallCss
		/// </summary>
		private void Compile()
		{ // Asigna un directorio de salida predeterminado
				if (string.IsNullOrEmpty(fnPathTarget.PathName))
					fnPathTarget.PathName = System.IO.Path.Combine(Application.UserAppDataPath, "SmallCssCompiler\\Output");
			// Compila las fuentes
				if (optCompileFile.Checked)
					{ if (!System.IO.File.Exists(fnFileSource.FileName))
							MessageBox.Show("El archivo seleccionado no existe");
						else
							Compile(fnFileSource.FileName, fnPathTarget.PathName);
					}
				else
					{ if (!System.IO.Directory.Exists(fnPathSource.PathName))
							MessageBox.Show("El directorio seleccionado no existe");
						else
							Compile(fnPathSource.PathName, fnPathTarget.PathName);
					}
		}

		/// <summary>
		///		Compila un directorio o archivo
		/// </summary>
		private void Compile(string strSource, string strTarget)
		{ SmallCssCompiler objCompiler = new SmallCssCompiler(strSource, strTarget);

				// Compila el archivo / directorio
					objCompiler.Compile();
				// Mensaje
					MessageBox.Show("Compilación terminada");
		}

		/// <summary>
		///		Habilita / inhabilita los controles
		/// </summary>
		private void EnableControls()
		{ fnFileSource.Enabled = optCompileFile.Checked;
			fnPathSource.Enabled = !optCompileFile.Checked;
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{ // Pasa los valores a la configuración
				Properties.Settings.Default.FileNameSource = fnFileSource.FileName;
				Properties.Settings.Default.PathSource = fnPathSource.PathName;
				Properties.Settings.Default.PathTarget = fnPathTarget.PathName;
			// Graba la configuración
				Properties.Settings.Default.Save();
		}

		private void cmdCompile_Click(object sender, EventArgs e) 
		{ Compile();
		}

		private void frmMain_Load(object sender, EventArgs e) 
		{ InitForm();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e) 
		{ SaveConfiguration();
		}

		private void optCompileFile_CheckedChanged(object sender, EventArgs e)
		{ EnableControls();
		}
	}
}
