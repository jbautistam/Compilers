using System;

using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Instructions
{
	/// <summary>
	///		Base para las clases de instrucción
	/// </summary>
	internal class InstructionBase
	{ 
		// Variables privadas
		private string error;

		internal InstructionBase(Token token)
		{
			Token = token;
			Instructions = new InstructionsBaseCollection();
			IsError = false;
		}

		/// <summary>
		///		Obtiene una cadena con el tipo
		/// </summary>
		internal string GetDebugString(int indent = 0)
		{
			string message = GetIndent(indent);

				// Añade los datos básicos
				message += $"{Token.ToString()} ({ToString()})";
				// Añade el error
				if (IsError)
					message += $" Error: {Error}";
				// Añade la depuración de las instrucciones
				message += Instructions.GetDebugString(indent + 1);
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
		internal Token Token { get; private set; }

		/// <summary>
		///		Indica si la instrucción tiene un error
		/// </summary>
		internal bool IsError { get; set; }

		/// <summary>
		///		Indica el error de la instrucción
		/// </summary>
		internal string Error
		{
			get
			{
				if (string.IsNullOrWhiteSpace(error))
					return "Error no definido";
				else
					return error;
			}
			set
			{
				error = value;
				IsError = true;
			}
		}

		/// <summary>
		///		Instrucciones
		/// </summary>
		internal InstructionsBaseCollection Instructions { get; set; }
	}
}
