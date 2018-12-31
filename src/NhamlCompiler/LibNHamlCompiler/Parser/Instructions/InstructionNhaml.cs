using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Instrucción de código Nhaml
	/// </summary>
	internal class InstructionNhaml : InstructionBase
	{
		internal InstructionNhaml(Token token) : base(token) { }

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = "";

				// Añade información de los atributos
				if (Attributes.Count > 0)
				{ 
					// Cabecera
					debug = Environment.NewLine + GetIndent(indent + 1) + "---> Parámetros: ";
					// Parámetros
					foreach (Parameter parameter in Attributes)
						debug += $"{parameter.Name} = {parameter.Variable.GetDebugInfo()}";
				}
				// Devuelve la cadena informativa
				return debug;
		}

		/// <summary>
		///		Indica si es una subinstrucción de HTML
		/// </summary>
		internal bool IsInner
		{
			get { return Token.Content.StartsWith("#"); }
		}

		/// <summary>
		///		Atributos
		/// </summary>
		internal ParametersCollection Attributes { get; } = new ParametersCollection();
	}
}
