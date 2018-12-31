using System;

using Bau.Libraries.LibTokenizer.Interpreter;
using Bau.Libraries.LibTokenizer.Interpreter.Instructions;
using Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler
{
	/// <summary>
	///		Traductor a Css
	/// </summary>
	internal class Translator
	{
		internal Translator(SmallCssCompiler manager)
		{
			Manager = manager;
		}

		/// <summary>
		///		Compila un archivo
		/// </summary>
		internal void Translate(string fileSource, string fileTarget)
		{
			string result = Translate(fileSource, true);

				// Graba el archivo
				LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileTarget));
				LibCommonHelper.Files.HelperFiles.SaveTextFile(fileTarget, result);
		}

		/// <summary>
		///		Compila un archivo
		/// </summary>
		internal string TranslateSnippet(string source)
		{
			return Translate(source, false);
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		private string Translate(string source, bool byFile)
		{
			Program program;

				// Importa los archivos origen
				if (byFile)
				{ 
					// Interpreta el archivo
					program = CompileSource(LoadFile(source));
					// Incluye los archivos origen
					while (ExistsInclude(program))
						program = IncludeFiles(source, program);
				}
				else
					program = CompileSource(source);
				// Transforma las funciones
				program = TransformFunctions(program);
				// Muestra la información de depuración
				Manager.RaiseDebug(program);
				// Ejecuta las instrucciones
				return new WriterCss(program, Manager.Minimize).Generate();
		}

		/// <summary>
		///		Compila un texto
		/// </summary>
		private Program CompileSource(string source)
		{
			TokenSmallCssCollection tokens = new Tokenizer().Parse(source);
			Program program;

				// Muestra la información de depuración
				Manager.RaiseDebug(tokens);
				// Obtiene las instrucciones
				program = new Parser().Parse(tokens);
				// Devuelve el programa
				return program;
		}

		/// <summary>
		///		Carga un archivo de texto
		/// </summary>
		private string LoadFile(string fileName)
		{
			return LibCommonHelper.Files.HelperFiles.LoadTextFile(fileName);
		}

		/// <summary>
		///		Comprueba si existe alguna instrucción include en el programa
		/// </summary>
		private bool ExistsInclude(Program program)
		{ 
			// Comprueba si existe alguna sentencia Include
			foreach (InstructionBase instruction in program.Sentences)
				if (instruction is InstructionIncludeFile)
					return true;
			// Si ha llegado hasta aquí es porque no ha encontrado ningún IncludeFile
			return false;
		}

		/// <summary>
		///		Incluye los archivos origen
		/// </summary>
		private Program IncludeFiles(string fileName, Program sourceProgram)
		{
			Program program = new Program("Main", null);

				// Añade las instrucciones al programa
				foreach (InstructionBase instruction in sourceProgram.Sentences)
					if (instruction is InstructionIncludeFile)
					{
						InstructionIncludeFile include = instruction as InstructionIncludeFile;

							if (include != null)
							{
								string newFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileName), include.FileName);

									if (System.IO.File.Exists(newFileName))
									{
										Program programIncluded = CompileSource(LoadFile(newFileName));

											// Añade las sentencias
											program.Sentences.AddRange(programIncluded.Sentences);
											// Añade las funciones
											program.Functions.AddRange(programIncluded.Functions);
									}
							}
					}
					else
						program.Sentences.Add(instruction);
				// Devuelve el programa
				return program;
		}

		/// <summary>
		///		Transforma las funciones en subprogramas
		/// </summary>
		private Program TransformFunctions(Program sourceProgram)
		{
			Program program = new Program("Main");

				// Añade las funciones originales
				program.Functions.AddRange(sourceProgram.Functions);
				// Transforma las funciones
				foreach (InstructionBase instruction in sourceProgram.Sentences)
					if (instruction is InstructionFunction)
					{
						InstructionFunction function = instruction as InstructionFunction;

							if (function != null)
								program.Functions.Add(GetFunctionProgram(function, program));
					}
					else
						program.Sentences.Add(instruction);
				// Devuelve el programa
				return program;
		}

		/// <summary>
		///		Convierte una instrucción de función en un programa
		/// </summary>
		private Program GetFunctionProgram(InstructionFunction function, Program parent)
		{
			Program program = new Program(function.Name, parent);

				// Añade los argumentos a la tabla de símbolos
				foreach (string argument in function.Arguments)
					program.Arguments.Add(argument);
				// Añade las instrucciones
				program.Sentences.AddRange(function.Sentences);
				// Devuelve el programa
				return program;
		}

		/// <summary>
		///		Manager del compilador
		/// </summary>
		private SmallCssCompiler Manager { get; }
	}
}
