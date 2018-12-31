using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bau.Libraries.LibSmallCssCompiler;
using Bau.Libraries.LibSmallCssCompiler.Compiler;

namespace FormTestLibTokenizer
{
	/// <summary>
	///		Formulario principal para la librería de generación de tokens
	/// </summary>
	public partial class frmMain : Form
	{
		public frmMain()
		{ InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{ pthSource.PathName = Properties.Settings.Default.PathSource;
			pthTarget.PathName = Properties.Settings.Default.PathTarget;
			chkMinimize.Checked = Properties.Settings.Default.Minimize;
			txtSource.Text = Properties.Settings.Default.Source;
		}

		/// <summary>
		///		Interpreta los archivos de un directorio
		/// </summary>
		private void Parse()
		{ if (string.IsNullOrEmpty(pthSource.PathName) || !System.IO.Directory.Exists(pthSource.PathName))
				Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el directorio origen");
			else if (string.IsNullOrEmpty(pthTarget.PathName))
				Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el directorio destino");
			else
				{ SmallCssCompiler objCompiler = new SmallCssCompiler(pthSource.PathName, pthTarget.PathName, chkMinimize.Checked);

						// Limpia el cuadro de texto
							Clear();
						// Añade el manejador de eventos
							objCompiler.DebugInfo += (objSender, objEventArgs) => 
																						{ AddLine(objEventArgs.Message); 
																						};
						// Compila los archivos
							objCompiler.Compile();						
						// Mensaje para el usuario
							Bau.Controls.Forms.Helper.ShowMessage(this, "Compilación terminada");
				}
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		private void ParseSnippet()
		{ if (string.IsNullOrEmpty(txtSource.Text))
				Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el texto a compilar");
			else
				{ SmallCssCompiler objCompiler = new SmallCssCompiler(pthSource.PathName, pthTarget.PathName, chkMinimize.Checked);

						// Limpia el cuadro de texto
							Clear();
						// Añade el manejador de eventos
							objCompiler.DebugInfo += (objSender, objEventArgs) => 
																						{ AddLine(objEventArgs.Message); 
																						};
						// Compila los archivos
							AddLine(Environment.NewLine + Environment.NewLine + "-----------------" + Environment.NewLine + 
											objCompiler.CompileSnippet(txtSource.Text));
						// Mensaje para el usuario
							Bau.Controls.Forms.Helper.ShowMessage(this, "Compilación terminada");
				}
		}

		/// <summary>
		///		Limpia el texto de salida
		/// </summary>
		private void Clear()
		{ txtTarget.Text = "";
		}

		/// <summary>
		///		Añade una línea al texto de salida
		/// </summary>
		private void AddLine(string strLine)
		{ txtTarget.Text += strLine + Environment.NewLine;
		}

		/// <summary>
		///		Guarda la configuración al salir de la aplicación
		/// </summary>
		private void ExitApp()
		{ Properties.Settings.Default.PathSource = pthSource.PathName;
			Properties.Settings.Default.PathTarget = pthTarget.PathName;
			Properties.Settings.Default.Minimize = chkMinimize.Checked;
			Properties.Settings.Default.Source = txtSource.Text;
			Properties.Settings.Default.Save();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{ InitForm();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{ ExitApp();
		}

		private void cmdTest_Click(object sender, EventArgs e)
		{ if (tabSource.SelectedIndex == 0)
				Parse();
			else
				ParseSnippet();
		}

		/// <summary>
		///		Evento de depuración
		/// </summary>
		private void objCompiler_DebugInfo(object sender, Bau.Libraries.LibTokenizer.EventArgs.DebugEventArgs e)
		{ AddLine(e.Message);
		}
	}
}
