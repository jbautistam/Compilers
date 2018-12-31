using System;

namespace Bau.Libraries.NhamlCompiler
{
	/// <summary>
	///		Compilador de Nhaml
	/// </summary>
	public class Compiler
	{ 
		// Eventos públicos
		public event EventHandler<EventArgs.DebugEventArgs> Debug;

		public Compiler()
		{
			MaximumRepetitionsLoop = 500;
			LocalErrors = new Errors.CompilerErrorsCollection();
		}

		/// <summary>
		///		Interpreta una cadena
		/// </summary>
		public string Parse(string source, int maxInstructions = 0, bool isCompressed = false)
		{
			return Parse(source, new Variables.VariablesCollection(), maxInstructions, isCompressed);
		}

		/// <summary>
		///		Interpreta una cadena
		/// </summary>
		public string Parse(string source, Variables.VariablesCollection variables, int maxInstructions = 0, bool isCompressed = false)
		{
			return new Parser.Translator.Interpreter(this, variables, maxInstructions, isCompressed).Parse(source);
		}

		/// <summary>
		///		Lanza el evento de depuración
		/// </summary>
		internal void RaiseEventDebug(EventArgs.DebugEventArgs.Mode mode, string title, string message)
		{
			Debug?.Invoke(this, new EventArgs.DebugEventArgs(mode, title, message));
		}

		/// <summary>
		///		Errores
		/// </summary>
		public Errors.CompilerErrorsCollection LocalErrors { get; private set; }

		/// <summary>
		///		Número máximo de veces que se puede ejecutar un bucle
		/// </summary>
		public int MaximumRepetitionsLoop { get; set; }
	}
}
