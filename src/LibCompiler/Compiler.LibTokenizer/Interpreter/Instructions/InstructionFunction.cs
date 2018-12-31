using System;
using System.Collections.Generic;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Instrucción con el contenido de una función
	/// </summary>
	public class InstructionFunction : InstructionBaseBlock
	{
		public InstructionFunction(Lexical.Tokens.Token token) : base(token)
		{
			Arguments = new List<string>();
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine + GetIndent(indent + 1) + "--> Función: " + Name;

				// Añade la información de las expresiones
				debug += "(";
				for (int index = 0; index < Arguments.Count; index++)
				{ 
					// Añade la coma si es necesario
					if (index > 0)
						debug += ", ";
					// Añade el nombre del argumento
					debug += Arguments[index];
				}
				debug += ")";
				debug += Sentences.GetDebugString(indent + 1);
				// Devuelve la información de depuración
				return debug;
		}

		/// <summary>
		///		Nombre de la función
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Argumentos de la función
		/// </summary>
		public List<string> Arguments { get; set; }
	}
}
