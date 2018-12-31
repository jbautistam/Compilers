using System;
using System.Collections.Generic;

namespace Bau.Libraries.Compiler.LibTokenizer.Errors
{
	/// <summary>
	///		Colección de <see cref="CompilerError"/>
	/// </summary>
	public class CompilerErrorsCollection : List<CompilerError>
	{
		/// <summary>
		///		Añade un error a partir de un token
		/// </summary>
		internal void Add(Lexical.Tokens.Token token, string error)
		{
			CompilerError compilerError = new CompilerError();

				// Asigna las propiedades
				compilerError.Token = token.Value;
				compilerError.Row = token.Row;
				compilerError.Column = token.Column;
				compilerError.Description = error;
				// Añade el error
				Add(compilerError);
		}
	}
}
