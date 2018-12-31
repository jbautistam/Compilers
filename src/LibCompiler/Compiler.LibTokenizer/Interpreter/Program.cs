using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter
{
	/// <summary>
	///		Clase con los datos de un programa
	/// </summary>
	public class Program
	{
		public Program(string id, Program parent = null)
		{
			ID = id;
			Parent = parent;
			SymbolsTable = new Symbols.SymbolTable(ID);
		}

		/// <summary>
		///		Escribe la información de depuración
		/// </summary>
		public void Debug()
		{
			foreach (Instructions.InstructionBase sentence in Sentences)
				System.Diagnostics.Debug.WriteLine(sentence.GetDebugString());
		}

		/// <summary>
		///		Identificador del programa / función
		/// </summary>
		public string ID { get; set; }

		/// <summary>
		///		Programa padre
		/// </summary>
		public Program Parent { get; set; }

		/// <summary>
		///		Instrucciones
		/// </summary>
		public Instructions.InstructionsBaseCollection Sentences { get; } = new Instructions.InstructionsBaseCollection();

		/// <summary>
		///		Funciones del programa
		/// </summary>
		public ProgramCollection Functions { get; } = new ProgramCollection();

		/// <summary>
		///		Argumentos
		/// </summary>
		public System.Collections.Generic.List<string> Arguments { get; } = new System.Collections.Generic.List<string>();

		/// <summary>
		///		Tabla de variables
		/// </summary>
		public Symbols.SymbolTable SymbolsTable { get; }
	}
}
