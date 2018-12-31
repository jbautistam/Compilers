using System;

using Bau.Libraries.Compiler.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.Compiler.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Error
	/// </summary>
	public class InstructionError : InstructionBase
	{
		public InstructionError(Token token, string error = null) : base(token)
		{
			if (string.IsNullOrEmpty(error))
				ErrorDescription = $"Error en el token: {token.Value}";
			else
				ErrorDescription = error;
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected override string GetDebugInfo(int indent)
		{
			return Environment.NewLine + GetIndent(indent + 1) + "--> Error: " + ErrorDescription;
		}

		/// <summary>
		///		Error localizado
		/// </summary>
		public string ErrorDescription { get; }
	}
}
