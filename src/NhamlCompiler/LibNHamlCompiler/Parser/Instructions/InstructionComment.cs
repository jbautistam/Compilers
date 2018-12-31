using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Comentario
	/// </summary>
	internal class InstructionComment : InstructionBase
	{
		internal InstructionComment(Token token) : base(token) { }

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
		internal string Content { get; set; }
	}
}
