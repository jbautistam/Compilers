using System;
using System.IO;

using Bau.Libraries.LibSmallCssCompiler.Compiler;

namespace Bau.Libraries.LibSmallCssCompiler
{
	/// <summary>
	///		Compilador de archivos SmallCss
	/// </summary>
	public class SmallCssCompiler
	{ 
		// Constantes privadas
		private const string Extension = ".scss";
		private const string ExtensionTarget = ".css";
		// Eventos públicos
		public event EventHandler<LibTokenizer.EventArgs.DebugEventArgs> DebugInfo;

		public SmallCssCompiler(string source, string target, bool minimize = false, bool killTargetPath = true)
		{
			Source = source;
			Target = target;
			KillTargetPath = killTargetPath;
			Minimize = minimize;
		}

		/// <summary>
		///		Compila un texto
		/// </summary>
		public string CompileSnippet(string source)
		{
			return new Translator(this).TranslateSnippet(source);
		}

		/// <summary>
		///		Compila los archivos SmallCss
		/// </summary>
		public void Compile()
		{ 
			// Borra el directorio de salida
			if (KillTargetPath)
				DeletePath(Target);
			// Genera los archivos de salida
			Generate(Source, Target);
		}

		/// <summary>
		///		Compila un archivo o directorio
		/// </summary>
		private void Generate(string fileName, string pathTarget)
		{
			if (Directory.Exists(fileName))
			{
				string[] files = Directory.GetFiles(fileName);
				string[] paths = Directory.GetDirectories(fileName);

					// Recorre la colección de directorio compilándolos recursivamente
					foreach (string path in paths)
						if (Directory.Exists(path))
							Generate(path, Path.Combine(pathTarget, Path.GetFileName(path)));
					// Recorre la colección de archivos, si es un directorio, lo compila recursivamente
					foreach (string file in files)
						if (File.Exists(file))
							GenerateFile(file, pathTarget);
			}
			else
				GenerateFile(fileName, pathTarget);
		}

		/// <summary>
		///		Compila / copia un archivo
		/// </summary>
		private void GenerateFile(string fileName, string pathTarget)
		{
			if (!fileName.EndsWith(Extension, StringComparison.CurrentCultureIgnoreCase))
				CopyFile(fileName, pathTarget);
			else if (!Path.GetFileName(fileName).StartsWith("_")) // ... los archivos que comienzan por _ se consideran archivos a importar y no se compilan
				new Translator(this).Translate(fileName,
											   Path.Combine(pathTarget, Path.GetFileNameWithoutExtension(fileName) + ExtensionTarget));
		}

		/// <summary>
		///		Copia un archivo a un directorio
		/// </summary>
		private void CopyFile(string source, string target)
		{
			string fileName = Path.Combine(target, Path.GetFileName(source));

				// Crea el directorio
				MakePath(Path.GetDirectoryName(fileName));
				// Copia el archivo
				try
				{
					File.Copy(source, fileName, true);
				}
				catch (Exception exception)
				{
					System.Diagnostics.Debug.WriteLine("Excepción al copiar el archivo. " + exception.Message);
				}
		}

		/// <summary>
		///		Graba un archivo de texto
		/// </summary>
		private void SaveFile(string text, string fileName)
		{ 
			// Crea el directorio
			MakePath(Path.GetDirectoryName(fileName));
			// Graba el archivo
			try
			{
				using (StreamWriter stmFile = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
				{ 
					// Escribe la cadena
					stmFile.Write(text);
					// Cierra el stream
					stmFile.Close();
				}
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine($"Excepción al grabar el archivo. {exception.Message}");
			}
		}

		/// <summary>
		///		Borra un directorio
		/// </summary>
		private void DeletePath(string path)
		{
			try
			{
				if (Directory.Exists(path))
					Directory.Delete(path, true);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine($"Excepción al eliminar el directorio. {exception.Message}");
			}
		}

		/// <summary>
		///		Crea un directorio
		/// </summary>
		private void MakePath(string path)
		{
			try
			{
				if (!Directory.Exists(path))
					Directory.CreateDirectory(path);
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine($"Excepción al crear el directorio. {exception.Message}");
			}
		}

		/// <summary>
		///		Lanza la información de depuración de los tokens
		/// </summary>
		internal void RaiseDebug(TokenSmallCssCollection tokens)
		{
			if (DebugInfo != null)
			{
				RaiseDebug("Depuración de tokens");
				foreach (TokenSmallCss token in tokens)
					RaiseDebug(token.GetDebugInfo());
			}
		}

		/// <summary>
		///		Lanza la información de depuración de las instrucciones
		/// </summary>
		internal void RaiseDebug(LibTokenizer.Interpreter.Program program)
		{
			if (DebugInfo != null)
			{
				RaiseDebug("Depuración del programa" + program.ID);
				RaiseDebugInner(program);
			}
		}

		/// <summary>
		///		Rutina interna para lanzar la depuración de instrucciones de programas y funciones recursivamente
		/// </summary>
		private void RaiseDebugInner(LibTokenizer.Interpreter.Program program)
		{
			if (program.Sentences.Count == 0)
				RaiseDebug($"No se ha encontrado ninguna instrucción en el programa {program.ID}");
			else
			{ 
				// Depuración de funciones
				foreach (LibTokenizer.Interpreter.Program function in program.Functions)
				{
					RaiseDebug($"Depuración de la función {function.ID}");
					RaiseDebugInner(function);
				}
				// Depuración de instrucciones
				foreach (LibTokenizer.Interpreter.Instructions.InstructionBase instruction in program.Sentences)
					if (instruction != null)
						RaiseDebug(instruction.GetDebugString());
			}
		}

		/// <summary>
		///		Lanza un mensaje de depuración
		/// </summary>
		private void RaiseDebug(string message)
		{
			DebugInfo?.Invoke(this, new LibTokenizer.EventArgs.DebugEventArgs(message));
		}

		/// <summary>
		///		Directorio o archivo a compilar
		/// </summary>
		public string Source { get; private set; }

		/// <summary>
		///		Directorio donde se dejan los archivos de salida
		/// </summary>
		public string Target { get; private set; }

		/// <summary>
		///		Elimina el directorio antiguo antes de generar los archivos nuevos
		/// </summary>
		public bool KillTargetPath { get; set; }

		/// <summary>
		///		Indica si debe minimizar el archivo de salida
		/// </summary>
		public bool Minimize { get; set; }
	}
}
