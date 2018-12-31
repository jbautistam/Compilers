using System;

namespace Bau.Libraries.LibTokenizer.Lexical.Tokens
{
	/// <summary>
	///		Clase con los datos de una palabra interpretada en el texto
	/// </summary>
	public class Token
	{
		/// <summary>
		///		Tipo de token
		/// </summary>
		public enum TokenType
		{
			Unknown,
			Error,
			String,
			Number,
			ArithmeticOperator,
			LogicalOperator,
			RelationalOperator,
			LeftParentesis,
			RightParentesis,
			Equal,
			Variable,
			Comment,
			ReservedWord,
			EndInstruction,
			StartBlock,
			EndBlock,
			EOF
		}

		public Token(TokenType type, int? subType, int row, int column, string value)
		{
			Type = type;
			SubType = subType;
			Row = row;
			Column = column;
			Indent = 0;
			Value = value;
		}

		/// <summary>
		///		Obtiene la información de depuración del token
		/// </summary>
		public virtual string GetDebugInfo()
		{
			return $"{Type.ToString()} - {SubType} (R {Row} - C {Column} - I {Indent}) : #{Value}#";
		}

		/// <summary>
		///		Tipo del token
		/// </summary>
		public TokenType Type { get; }

		/// <summary>
		///		Subtipo de token
		/// </summary>
		public int? SubType { get; }

		/// <summary>
		///		Fila
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		///		Columna
		/// </summary>
		public int Column { get; set; }


		/// <summary>
		///		Indentación
		/// </summary>
		public int Indent { get; set; }

		/// <summary>
		///		Valor de la palabra
		/// </summary>
		public string Value { get; set; }
	}
}
