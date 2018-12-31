using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.NhamlCompiler.Parser.Lexical
{
	/// <summary>
	///		Clase para obtener los caracteres de una cadena
	/// </summary>
	internal class StringWord
	{
		/// <summary>
		///		Modo de interpretación
		/// </summary>
		internal enum ParseMode
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Nhaml</summary>
			Nhaml,
			/// <summary>Sentencias de código</summary>
			Code,
			/// <summary>Sentencias de bloque de código</summary>
			CodeBlock,
			/// <summary>Comentario</summary>
			Comment,
			/// <summary>Expresión en líneas de código Nhaml</summary>
			ExpressionNhaml,
			/// <summary>Expresión en líneas de código</summary>
			ExpressionCode
		}

		/// <summary>
		///		Estructura con los datos de una palabra
		/// </summary>
		internal struct WordStruct
		{
			internal int Row, Column, Indent;
			internal bool IsEof;
			internal string Content;
		}

		internal StringWord(string source)
		{
			Source = source;
			Mode = ParseMode.Nhaml;
			Row = 1;
			Column = 1;
			IndexActualChar = 0;
			PreviousChar = ' ';
			PreviousWord = new WordStruct();
		}

		/// <summary>
		///		Obtiene la siguiente palabra
		/// </summary>
		internal WordStruct GetNextWord()
		{
			WordStruct word = new WordStruct();

				// Obtiene la siguiente cadena (o null si ha terminado con el archivo)
				if (IsEof())
					word.IsEof = true;
				else
				{ 
					// Salta los espacios
					SkipSpaces();
					// Asigna la fila y columna (cuando se han saltado los espacios)
					word.Row = Row;
					word.Column = Column;
					word.Indent = Indent;
					// Dependiendo del modo ...
					switch (Mode)
					{
						case ParseMode.Nhaml:
								word.Content = GetNextStringNhaml();
							break;
						case ParseMode.Comment:
								word.Content = GetNextStringComment();
							break;
						case ParseMode.ExpressionCode:
						case ParseMode.ExpressionNhaml:
								word.Content = GetNextStringExpression();
							break;
						case ParseMode.Code:
								word.Content = GetNextStringCode();
							break;
						case ParseMode.CodeBlock:
								word.Content = GetNextStringBlockCode();
							break;
					}
				}
				// Guarda la palabra anterior
				PreviousWord = word;
				// Devuelve la palabra
				return word;
		}

		/// <summary>
		///		Obtiene la siguiente cadena para código Nhaml
		/// </summary>
		private string GetNextStringNhaml()
		{
			char nextChar = GetChar();
			string nhaml = nextChar.ToString();

				// Dependiendo del carácter inicial, lee el resto de los caracteres
				if (nextChar == '\\') // carácter especial para no interpretar el siguiente carácter
				{ 
					// Quita el carácter especial por el que se ha puesto la \ (lo añade a la cadena para que cuando se obtenga
					// el token se vuelva a preguntar por la \
					nhaml += GetChar().ToString();
					// Lee hasta el final de la palabra
					nhaml += ReadToEndWord();
				}
				else if (nextChar == '\"')
					nhaml += ReadToEndString();
				else if (nextChar == '#' || nextChar == '%' || nextChar == '·' || nextChar == '&')
					nhaml += ReadToEndNHaml();
				else if (nextChar == '$')
					nhaml += ReadToEndVariable();
				else if (nextChar == '[')
					Mode = ParseMode.ExpressionNhaml;
				else if (nextChar == '-' && GetFirstChars(1, false) == ">")
				{
					nhaml = "->";
					SkipChars(1);
				}
				else if (nextChar == '<' && GetFirstChars(3, false) == "!--")
				{ 
					nhaml = "<!--";
					SkipChars(3); // ... no 4 porque el primero ya está
					Mode = ParseMode.Comment;
				}
				else if (nextChar == '<' && GetFirstChars(7, false).EqualsIgnoreCase("%code%>"))
				{ 
					nhaml = "<%code%>";
					SkipChars(7);
					Mode = ParseMode.CodeBlock;
				}
				else if (nextChar == '<' && GetFirstChars(1, false) == "%")
				{ 
					nhaml = "<%";
					SkipChars(1); // ... no 2 porque el primero ya no está en el buffer
					Mode = ParseMode.Code;
				}
				else if (!IsEndCharNHaml(nextChar.ToString())) // ... si no está entre los caracteres finales
					nhaml += ReadToEndWord();
				// Devuelve la cadena Nhaml
				return nhaml;
		}

		/// <summary>
		///		Obtiene la sigiente cadena para código
		/// </summary>
		private string GetNextStringCode()
		{
			char actualChar = GetChar();
			string result = actualChar.ToString();
			char nextChar = GetNextChar();

				// Dependiendo del carácter inicial, lee el resto de los caracteres
				if (actualChar == '\"')
					result += ReadToEndString();
				else if (actualChar == '$')
					result += ReadToEndVariable();
				else if (actualChar == '[')
					Mode = ParseMode.ExpressionCode;
				else if (actualChar == '-' && GetFirstChars(1, false) == ">")
				{
					result = "->";
					SkipChars(1);
				}
				else if (actualChar == '%' && nextChar == '>')
				{ 
					result = "%>";
					SkipChars(1); // ... no 2 porque el primero ya no está en el buffer
								  // Cambia el modo de lectura
					Mode = ParseMode.Nhaml;
				}
				else if (IsDigit(actualChar.ToString()))
					result += ReadToEndNumber();
				else if ((IsLogicoperation(actualChar) && nextChar == '=') || // <= ó >=
								 (actualChar == '=' && IsLogicoperation(nextChar)) || // => ó <=
								 (actualChar == '=' && nextChar == '=') || // ==
								 (actualChar == '!' && nextChar == '=') || // !=
								 (actualChar == '&' && nextChar == '&') || // &&
								 (actualChar == '|' && nextChar == '|') // ||
								)
				{
					result += nextChar.ToString();
					SkipChars(1);
				}
				else if (!IsArithmeticoperation(actualChar) && !IsLogicoperation(actualChar))
					result += ReadToEndWord();
				// Devuelve la cadena de código
				return result;
		}

		/// <summary>
		///		Obtiene la siguiente cadena de comentario
		/// </summary>
		private string GetNextStringComment()
		{
			string result = "";

				// Busca la cadena final para el comentario
				if (GetFirstChars(3, false) == "-->")
				{ 
					// Asigna la cadena final y las quita del buffer
					result = "-->";
					SkipChars(3);
					// Cambia el modo de lectura
					Mode = ParseMode.Nhaml;
				}
				else // ... obtiene el contenido del comentario
					while (!IsEof() && GetFirstChars(3, false) != "-->")
						result += GetChar();
				// Devuelve el comentrario
				return result;
		}

		/// <summary>
		///		Obtiene una cadena con un bloque de código
		/// </summary>
		private string GetNextStringBlockCode()
		{
			string result = "";

				// Obtiene la cadena hasta el final del bloque de código
				while (!IsEof() && !GetFirstChars(7, false).EqualsIgnoreCase("<%end%>"))
					result += GetChar();
				// Cambia el modo de lectura a código (para que se lea correctamente el end
				Mode = ParseMode.Nhaml;
				// Devuelve el comentrario
				return result;
		}

		/// <summary>
		///		Obtiene la siguiente cadena de una expresión
		/// </summary>
		private string GetNextStringExpression()
		{
			char actual = GetChar();
			char next = GetNextChar();
			string result = actual.ToString();

				// Obtiene la siguiente cadena de la expresión
				if (actual == ']') // ... fin de expresión, cambia el modo
				{
					if (Mode == ParseMode.ExpressionCode)
						Mode = ParseMode.Code;
					else
						Mode = ParseMode.Nhaml;
				}
				else if (actual == '[') // ... inicio de índice
					result = "["; // ... no es necesario, se añade por claridad
				else if (IsDigit(actual.ToString()))
					result += ReadToEndNumber();
				else if ((IsLogicoperation(actual) && next == '=') || // <= ó >=
								 (actual == '=' && IsLogicoperation(next)) || // => ó <=
								 (actual == '=' && next == '=') || // ==
								 (actual == '&' && next == '&') || // &&
								 (actual == '|' && next == '|') // ||
						)
				{
					result += next.ToString();
					SkipChars(1);
				}
				else if (!IsArithmeticoperation(actual) && !IsLogicoperation(actual))
					result += ReadToEndWord();
				// Devuelve la cadena de la expresión
				return result;
		}

		/// <summary>
		///		Lee hasta el final de la palabra (letras y dígitos)
		/// </summary>
		private string ReadToEndWord()
		{
			string result = "";
			string nextChar = GetFirstChars(1, false);

				// Busca el carácter final para la palabra (letras y dígitos)
				while (!IsEof() && (IsAlphabetic(nextChar) || IsDigit(nextChar)))
				{ 
					// Añade el carácter al resultado
					result += GetCharSkipBars(false).ToString();
					// Obtiene el siguiente carácter por adelantado
					nextChar = GetFirstChars(1, false);
				}
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Lee hasta el final de la variable (letras, dígitos y cadena ->)
		/// </summary>
		private string ReadToEndVariable()
		{
			string result = "";
			string nextChar = GetFirstChars(1, false);

				// Busca el carácter final para la variable (letras y dígitos y los caracteres _)
				while (!IsEof() && (IsAlphabetic(nextChar) || IsDigit(nextChar) || nextChar == "_"))
				{ 
					// Añade el carácter al resultado
					result += GetCharSkipBars(false).ToString();
					// Añade el > si estamos en un -
					if (nextChar == "-")
						result += GetCharSkipBars(false).ToString();
					// Obtiene el siguiente carácter por adelantado
					nextChar = GetFirstChars(1, false);
				}
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Busca un carácter final para Nhaml (espacio o carácter de fin de Nhaml)
		/// </summary>
		private string ReadToEndNHaml()
		{
			string result = "";
			string nextChar = GetFirstChars(1, false);

				// Busca el carácter final para Nhaml
				while (!IsEof() && (IsAlphabetic(nextChar) || IsDigit(nextChar) || nextChar == "-" || nextChar == "_"))
				{ 
					// Añade el carácter al resultado
					result += GetCharSkipBars(false).ToString();
					// Obtiene el siguiente carácter por adelantado
					nextChar = GetFirstChars(1, false);
				}
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Lee hasta el final de la cadena
		/// </summary>
		private string ReadToEndString()
		{
			string result = "";
			string nextChar = GetFirstChars(1, true);

				// Busca el carácter final para la cadena
				while (!IsEof() && nextChar != "\"")
				{ 
					// Añade el carácter al resultado
					result += GetChar();
					// Obtiene el siguiente carácter por adelantado
					nextChar = GetFirstChars(1, false);
				}
				// Añade las comillas finales
				if (nextChar == "\"")
					result += GetChar();
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Lee hasta el final del número
		/// </summary>
		private string ReadToEndNumber()
		{
			string result = "";
			string nextChar = GetFirstChars(1, true);

				// Busca el carácter final para la cadena
				while (!IsEof() && (IsDigit(nextChar) || nextChar == "."))
				{ 
					// Añade el carácter al resultado
					result += GetChar();
					// Obtiene el siguiente carácter por adelantado
					nextChar = GetFirstChars(1, false);
				}
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Salta los espacios
		/// </summary>
		private void SkipSpaces()
		{
			if (!IsEof())
			{
				string nextChars = GetFirstChars(1, false);

					// Se salta los espacios
					while (!IsEof() && IsSpace(nextChars))
					{
						char actual = GetChar();

							// Incrementa el número de tabuladores
							if (actual == '\t')
								Indent++;
							// Obtiene el siguiente carácter (sin sacarlo del buffer)
							nextChars = GetFirstChars(1, false);
					}
			}
		}

		/// <summary>
		///		Se salta una serie de caracteres
		/// </summary>
		private void SkipChars(int length)
		{
			for (int index = 0; index < length; index++)
				GetChar();
		}

		/// <summary>
		///		Obtiene el carácter actual
		/// </summary>
		private char GetChar()
		{
			char actual = ' ';

				// Obtiene el siguiente carácter (si existe)
				if (!IsEof())
				{ 
					// Obtiene el carácter
					actual = Source[IndexActualChar];
					// Incrementa el índice actual
					IndexActualChar++;
					// Incrementa filas y columnas
					if (actual == '\r' || (actual == '\n' && PreviousChar != '\r'))
					{
						Row++;
						Column = 1;
						Indent = 0;
					}
					else
						Column++;
					// Guarda el carácter en los caracteres previos
					PreviousChar = actual;
				}
				// Devuelve el carácter
				return actual;
		}

		/// <summary>
		///		Obtiene un carácter saltándose las barras invertidas
		/// </summary>
		private char GetCharSkipBars(bool atString)
		{
			char chrChar = GetChar();

				// Se salta las barras invertidas
				if (chrChar == '\\' && !atString)
					chrChar = GetChar();
				// Devuelve el carácter
				return chrChar;
		}

		/// <summary>
		///		Obtiene los primeros n caracteres (sin sacarlos del buffer)
		/// </summary>
		private string GetFirstChars(int length, bool atString)
		{
			string nextChars = "";
			int startPosition = IndexActualChar;

				// Obtiene los siguientes carácter (si existe)
				for (int index = 0; index < length; index++)
					if (startPosition + index < Source.Length)
					{ 
						// Se salta la barra actual
						if (!atString && Source[startPosition + index] == '\\')
							startPosition++;
						// Obtiene el carácter
						nextChars += Source[startPosition + index];
					}
				// Devuelve la cadena de siguientes caracteres
				return nextChars;
		}

		/// <summary>
		///		Obtiene el siguiente carácter sin sacarlo del buffer
		/// </summary>
		private char GetNextChar()
		{
			string nextChars = GetFirstChars(1, false);

				// Obtiene el primer carácter
				if (!string.IsNullOrWhiteSpace(nextChars))
					return nextChars[0];
				else
					return ' ';
		}

		/// <summary>
		///		Indica si es final de archivo
		/// </summary>
		private bool IsEof()
		{
			return IndexActualChar >= Source.Length;
		}

		/// <summary>
		///		Comprueba si un carácter es un espacio
		/// </summary>
		private bool IsSpace(string strChar)
		{
			return strChar == " " || strChar == "\t" || strChar == "\r" || strChar == "\n";
		}

		/// <summary>
		///		Comprueba si una cadena es un carácter final en Nhaml
		/// </summary>
		private bool IsEndCharNHaml(string nextChar)
		{
			return nextChar == "{" || nextChar == "}" || nextChar == "#" || nextChar == "=" || nextChar == "[" || nextChar == "]";
		}

		/// <summary>
		///		Comprueba si es un dígito
		/// </summary>
		private bool IsDigit(string nextChar)
		{
			return !string.IsNullOrWhiteSpace(nextChar) && nextChar.Length > 0 && char.IsDigit(nextChar[0]);
		}

		/// <summary>
		///		Comprueba si es un carácter alfabético
		/// </summary>
		private bool IsAlphabetic(string nextChar)
		{
			return !string.IsNullOrWhiteSpace(nextChar) && nextChar.Length > 0 && char.IsLetter(nextChar[0]);
		}

		/// <summary>
		///		Comprueba si es un operador lógico
		/// </summary>
		private bool IsLogicoperation(char chrChar)
		{
			return chrChar == '<' || chrChar == '>';
		}

		/// <summary>
		///		Comprueba si es un operador aritmético
		/// </summary>
		private bool IsArithmeticoperation(char chrChar)
		{
			return chrChar == '+' || chrChar == '-' || chrChar == '/' || chrChar == '*' || chrChar == '\\' || chrChar == '(' || chrChar == ')';
		}

		/// <summary>
		///		Texto original
		/// </summary>
		internal string Source { get; private set; }

		/// <summary>
		///		Fila actual
		/// </summary>
		internal int Row { get; private set; }

		/// <summary>
		///		Columna actual
		/// </summary>
		internal int Column { get; private set; }

		/// <summary>
		///		Carácter anterior
		/// </summary>
		internal char PreviousChar { get; private set; }

		/// <summary>
		///		Palabra anterior
		/// </summary>
		internal WordStruct PreviousWord { get; private set; }

		/// <summary>
		///		Carácter actual
		/// </summary>
		internal int IndexActualChar { get; private set; }

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; private set; }

		/// <summary>
		///		Modo de interpretación
		/// </summary>
		internal ParseMode Mode { get; set; }

		/// <summary>
		///		Indica si el modo de interpretación es uno de los de código (código o expresión)
		/// </summary>
		internal bool IsCodeMode
		{
			get { return Mode == ParseMode.Code || Mode == ParseMode.ExpressionCode || Mode == ParseMode.ExpressionNhaml; }
		}
	}
}
