﻿using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules
{
	/// <summary>
	///		Regla delimitada
	/// </summary>
	public class RuleDelimited : RuleBase
	{
		public RuleDelimited(Tokens.Token.TokenType type, int? subType, string[] starts, string[] ends,
						     bool toEndLine, bool toFirstSpace, bool includeStart, bool includeEnd) : base(type, subType, toFirstSpace, true)
		{
			Starts = starts;
			Ends = ends;
			ToEndLine = toEndLine;
			ToFirstSpace = toFirstSpace;
			IncludeStart = includeStart;
			IncludeEnd = includeEnd;
		}

		/// <summary>
		///		Cadenas de inicio 
		/// </summary>
		public string[] Starts { get; set; }

		/// <summary>
		///		Cadenas de fin
		/// </summary>
		public string[] Ends { get; set; }

		/// <summary>
		///		Indica si se debe leer hasta fin de la línea
		/// </summary>
		public bool ToEndLine { get; set; }

		/// <summary>
		///		Indica si se deben incluir los caracteres de inicio en el contenido del token
		/// </summary>
		public bool IncludeStart { get; set; }

		/// <summary>
		///		Indica si se deben incluir los caracteres de fin en el contenido del token
		/// </summary>
		public bool IncludeEnd { get; set; }
	}
}
