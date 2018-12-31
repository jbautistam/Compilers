using System;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions
{
	/// <summary>
	///		Instrucción para llamada a un mixin
	/// </summary>
	public class InstructionMixinCall : LibTokenizer.Interpreter.Instructions.InstructionBaseBlock
	{
		public InstructionMixinCall(TokenSmallCss token) : base(token)
		{
			Parameters = new TokenSmallCssCollection();
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			string debug = Environment.NewLine;

				// Añade la información de depuración del token
				debug = base.GetIndent(indent);
				debug += $"Llamada a {Name} --> Parámetros: ";
				foreach (TokenSmallCss token in Parameters)
					debug += token.GetDebugForInstructions() + " ";
				// Añade la información de depuración de las instrucciones
				debug += Sentences.GetDebugString(indent + 1);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Nombre del mixin que se va a insertar
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Parámetros de la llamada
		/// </summary>
		public TokenSmallCssCollection Parameters { get; private set; }
	}
}
