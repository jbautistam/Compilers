using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Lexical.Rules
{
	/// <summary>
	///		Definición de una regla
	/// </summary>
	public abstract class RuleBase
	{
		protected RuleBase(Tokens.Token.TokenType type, int? subType, bool toFirstSpace, bool mustTrim)
		{
			Type = type;
			SubType = subType;
			ToFirstSpace = toFirstSpace;
			MustTrim = mustTrim;
		}

		/// <summary>
		///		Tipo de token
		/// </summary>
		public Tokens.Token.TokenType Type { get; set; }

		/// <summary>
		///		Subtipo de token
		/// </summary>
		public int? SubType { get; set; }

		/// <summary>
		///		Indica si se debe hacer un trim del texto
		/// </summary>
		public bool MustTrim { get; set; }

		/// <summary>
		///		Indica si se debe leer hasta el primer espacio
		/// </summary>
		public bool ToFirstSpace { get; set; }
	}
}
