using System;

using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Comentario
	/// </summary>
	public class InstructionComment : InstructionBase
	{
		public InstructionComment(Token token) : base(token)
		{
			Content = token.Value;
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + GetIndent(indent + 1) + "--> Contenido: " + Content;
		}

		/// <summary>
		///		Comentario
		/// </summary>
		public string Content { get; set; }
	}
}
