﻿using System;

namespace Bau.Libraries.NhamlCompiler.Parser.Tokens
{
	/// <summary>
	///		Clase con los datos de un token
	/// </summary>
	internal class Token
	{
		/// <summary>
		///		Tipo de token
		/// </summary>
		internal enum TokenType
		{
			Unknown,
			Error,
			String,
			Number,
			Literal,
			StartSentenceBlock,
			EndSentenceBlock,
			Sentence,
			TagHTML,
			LeftTagHTMLInner,
			RightTagHTMLInner,
			Arithmeticoperation,
			Logicaloperation,
			Relationaloperation,
			LeftParentesis,
			RightParentesis,
			LeftCorchete,
			RightCorchete,
			VariablePointer,
			Equal,
			LeftLlave,
			RightLlave,
			Variable,
			StartComment,
			EndComment,
			EOF
		}

		/// <summary>
		///		Pasa el token a una cadena
		/// </summary>
		public override string ToString()
		{
			return $"{Content} - {Row},{Column}:{Indent} -- Type: {Type}";
		}

		/// <summary>
		///		Tipo de token
		/// </summary>
		internal TokenType Type { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		internal string Content { get; set; }

		/// <summary>
		///		Fila
		/// </summary>
		internal int Row { get; set; }

		/// <summary>
		///		Columna
		/// </summary>
		internal int Column { get; set; }

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; set; }

		/// <summary>
		///		Indica si el token es parte de una expresión
		/// </summary>
		internal bool IsExpressionPart
		{
			get
			{
				return Type == TokenType.Arithmeticoperation ||
						Type == TokenType.Logicaloperation ||
						Type == TokenType.Number ||
						Type == TokenType.Relationaloperation ||
						Type == TokenType.String ||
						Type == TokenType.Variable ||
						Type == TokenType.LeftParentesis || 
						Type == TokenType.RightParentesis;
			}
		}
	}
}
