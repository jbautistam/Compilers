using System;

using Bau.Libraries.LibTokenizer.Lexical.Tokens;

namespace Bau.Libraries.LibTokenizer.Interpreter.Instructions
{
	/// <summary>
	///		Base para las clases de instrucción
	/// </summary>
	public class InstructionBase
	{
		public InstructionBase(Token token)
		{
			Token = token;
		}

		/// <summary>
		///		Obtiene una cadena con el tipo
		/// </summary>
		public string GetDebugString(int indent = 0)
		{
			string message = GetIndent(indent);

				// Añade los datos básicos
				message += $"{Token} ({ToString()})";
				// Añade el error
				if (IsError)
					message += $" Error: {Error}";
				// Añade la depuración interna
				message += GetDebugInfo(indent);
				// Devuelve la información de depuración
				return message;
		}

		/// <summary>
		///		Obtiene una cadena para indentar
		/// </summary>
		protected string GetIndent(int indent)
		{
			return new string('\t', indent);
		}

		/// <summary>
		///		Obtiene la información de depuración
		/// </summary>
		protected virtual string GetDebugInfo(int indent)
		{
			return "";
		}

		/// <summary>
		///		Token al que se asocia la instrucción
		/// </summary>
		public Token Token { get; }

		/// <summary>
		///		Indica si la instrucción tiene un error
		/// </summary>
		public bool IsError
		{
			get { return Error != null; }
		}

		/// <summary>
		///		Indica el error de la instrucción
		/// </summary>
		public InstructionError Error { get; set; }
	}
}
