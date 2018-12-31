using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.NhamlCompiler.Parser.Instructions;
using Bau.Libraries.NhamlCompiler.Parser.Tokens;

namespace Bau.Libraries.NhamlCompiler.Parser.Lexical
{
	/// <summary>
	///		Intérprete
	/// </summary>
	internal class ParserManager
	{ 
		// Enumerados privados
		/// <summary>
		///		Modo del lector de expresions
		/// </summary>
		private enum ExpressionReaderMode
		{
			/// <summary>En una sección de atributos</summary>
			AtAttributes,
			/// <summary>En una variable</summary>
			AtVariable,
			/// <summary>En el resto del texto</summary>
			Normal
		}
		// Variables privadas
		private Evaluator.ExpressionConversorRpn _expressionEvaluator;

		internal ParserManager(Compiler compiler)
		{
			Compiler = compiler;
			Tokens = new TokensCollection();
			_expressionEvaluator = new Evaluator.ExpressionConversorRpn();
		}

		/// <summary>
		///		Interpreta una cadena
		/// </summary>
		internal void Parse(string source)
		{
			Token lastToken;

				// Crea la colección de tokens
				Tokens = new StringTokenizer(source.TrimEnd()).GetAllTokens();
				// Depura la colección de tokens
				DebugTokens();
				// Lee las instrucciones
				lastToken = ReadInstructions(Instructions);
				// Depura las instrucciones
				DebugInstructions();
		}

		/// <summary>
		///		Lee las instrucciones
		/// </summary>
		private Token ReadInstructions(InstructionsBaseCollection instructions)
		{
			Token nextToken = GetToken(true);
			int firstIndent;
			bool end = false;

				// Quita las instrucciones de fin y de inicio de comando
				while (nextToken.Type == Token.TokenType.EndSentenceBlock)
				{
					GetToken();
					nextToken = GetToken(true);
				}
				// Obtiene la indentación inicial
				firstIndent = nextToken.Indent;
				// Lee las instrucciones
				while (nextToken.Type != Token.TokenType.EOF && nextToken.Indent >= firstIndent && !end)
				{
					InstructionsBaseCollection innerInstructions = new InstructionsBaseCollection();

						// Obtiene el token real
						nextToken = GetToken();
						// Trata las instrucciones
						switch (nextToken.Type)
						{
							case Token.TokenType.StartComment:
									innerInstructions.Add(ReadComment(nextToken));
								break;
							case Token.TokenType.StartSentenceBlock:
							case Token.TokenType.EndSentenceBlock:
									// ... no hace nada, simplemente se las salta
								break;
							case Token.TokenType.TagHTML:
									innerInstructions.Add(ReadHtml(nextToken));
								break;
							case Token.TokenType.Sentence:
									innerInstructions.Add(ReadSentence(nextToken));
								break;
							default:
									innerInstructions.Add(new InstructionBase(nextToken));
								break;
						}
						// Trata las instrucciones
						if (innerInstructions.Count > 0)
						{
							bool error = false;

								// Si hay algún error lo añade
								foreach (InstructionBase instruction in innerInstructions)
									if (instruction.IsError)
									{
										Compiler.LocalErrors.Add(instruction.Token, instruction.Error);
										error = true;
									}
								// Se recupera de los errores
								if (error)
									RecoverError();
								// En cualquier caso, mete las instrucciones en el buffer
								instructions.AddRange(innerInstructions);
						}
						// Obtiene el siguiente token
						nextToken = GetToken(true);
						// Termina si es una sentencia else
						if (IsElseCommand(nextToken))
							end = true;
				}
				// Devuelve el último token leído
				return nextToken;
		}

		/// <summary>
		///		Lee un comentario
		/// </summary>
		private InstructionComment ReadComment(Token token)
		{
			InstructionComment instruction = new InstructionComment(token);

				// Supone que hay algún error
				instruction.IsError = true;
				// Lee el siguiente token (cuerpo del comentario)
				token = GetToken();
				if (token.Type != Token.TokenType.EndComment)
				{ // Lee el contenido del comentario
					instruction.Content = token.Content;
					// Lee el siguiente token (fin del comentario)
					token = GetToken();
				}
				// Comprueba si es un fin de comentario
				if (token.Type == Token.TokenType.EndComment)
					instruction.IsError = false;
				else
					instruction.Error = "No se ha encontrado la etiqueta de fin de comentario";
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee el Html de una página
		/// </summary>
		private InstructionNhaml ReadHtml(Token token)
		{
			InstructionNhaml instruction = new InstructionNhaml(token);
			Token nextToken = GetToken(true); // ... lee el siguiente token sin sacarlo del buffer
			bool end = false;

				// Si es una llave de apertura, lee los argumentos
				if (nextToken.Type == Token.TokenType.LeftLlave)
				{ 
					// Lee los atributos 
					ReadAttributes(instruction);
					// Lee el siguiente token
					nextToken = GetToken(true);
				}
				// Recorre los tokens de la instrucción
				while (!IsEof(nextToken) && !end && !instruction.IsError)
				{ 
					// Comprueba si lo siguiente es una instrucción interna o si ya se ha terminado
					if (IsNextInstructionInternal(instruction, nextToken))
					{
						if (instruction.IsInner)
							instruction.Error = "Se ha detectado una instrucción dentro de una etiqueta HTML";
						else
							nextToken = ReadInstructions(instruction.Instructions);
					}
					else if (IsNextInstruction(nextToken))
					{
						if (instruction.IsInner)
							instruction.Error = "Se ha detectado una instrucción dentro de una etiqueta HTML";
						else
							end = true;
					}
					else
					{ 
						// Lee el token
						token = GetToken();
						// Trata el token
						switch (token.Type)
						{
							case Token.TokenType.LeftTagHTMLInner:
									instruction.Instructions.Add(ReadHtml(token));
								break;
							case Token.TokenType.RightTagHTMLInner:
									if (instruction.IsInner)
										end = true;
									else
										instruction.Instructions.Add(new InstructionLiteral(token));
								break;
							case Token.TokenType.Variable:
									instruction.Instructions.Add(ReadVariableIdentifier(token));
								break;
							default:
									instruction.Instructions.Add(new InstructionLiteral(token));
								break;
						}
						// Lee el siguiente token sin sacarlo del buffer
						nextToken = GetToken(true);
					}
				}
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee una instrucción de código
		/// </summary>
		private InstructionBase ReadCommand(Token token)
		{
			InstructionBase instruction = null;

				// Lee el siguiente token
				token = GetToken();
				// Interpreta la sentencia
				if (token.Type != Token.TokenType.Sentence)
					instruction = GetInstructionError(token, "Sentencia desconocida");
				else
					instruction = ReadSentence(token);
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee una sentencia
		/// </summary>
		private InstructionBase ReadSentence(Token token)
		{
			switch ((token.Content ?? "").ToUpper())
			{
				case "CODE":
				case "<%CODE%>":
					return ReadCommandCode(token);
				case "END":
					return GetInstructionError(token, "Sentencia 'end' sin inicio de bloque");
				case "ELSE":
					return GetInstructionError(token, "Sentencia 'else' sin 'if'");
				case "IF":
					return ReadCommandIf(token);
				case "LET":
					return ReadCommandLet(token);
				case "FOREACH":
					return ReadCommandForEach(token);
				case "FOR":
					return ReadCommandFor(token);
				case "WHILE":
					return ReadCommandWhile(token);
				default:
					return GetInstructionError(token, "Sentencia desconocida");
			}
		}

		/// <summary>
		///		Lee una sentencia if
		/// </summary>
		private InstructionIf ReadCommandIf(Token token)
		{
			InstructionIf instruction = new InstructionIf(token);

				// Lee la expresión
				instruction.Condition = ReadExpression(ExpressionReaderMode.Normal, out string error);
				instruction.ConditionRPN = _expressionEvaluator.ConvertToRPN(instruction.Condition);
				if (!string.IsNullOrWhiteSpace(error))
					instruction.Error = error;
				// Lee el resto de datos
				if (!instruction.IsError)
				{
					Token nextToken = GetToken(true);

						// Comprueba si es un error
						if (nextToken.Type != Token.TokenType.EndSentenceBlock && nextToken.Type != Token.TokenType.Sentence)
							instruction.Error = "No se ha encontrado el final de la instrucción";
						else if (!IsEndCommand(nextToken)) // ... no es un if vacío
						{
							bool atElse = false;

								// Lee las instrucciones de la parte
								nextToken = ReadInstructions(instruction.Instructions);
								// Comprueba si la siguiente es una sentencia else
								if (IsElseCommand(nextToken))
								{ 
									// Indica que es un else
									atElse = true;
									// Quita los tokens del else
									if (nextToken.Type == Token.TokenType.StartSentenceBlock)
									{
										GetToken();
										nextToken = GetToken(true);
									}
									if (nextToken.Type == Token.TokenType.Sentence && nextToken.Content.Equals("else", StringComparison.CurrentCultureIgnoreCase))
									{
										GetToken();
										nextToken = GetToken(true);
									}
									if (nextToken.Type == Token.TokenType.EndSentenceBlock)
									{
										GetToken();
										nextToken = GetToken(true);
									}
								}
								// Lee la parte else
								if (atElse)
									ReadInstructions(instruction.InstructionsElse);
						}
				}
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee una sentencia while
		/// </summary>
		private InstructionWhile ReadCommandWhile(Token token)
		{
			InstructionWhile instruction = new InstructionWhile(token);

				// Lee la expresión
				instruction.Condition = ReadExpression(ExpressionReaderMode.Normal, out string error);
				instruction.ConditionRPN = _expressionEvaluator.ConvertToRPN(instruction.Condition);
				if (!string.IsNullOrWhiteSpace(error))
					instruction.Error = error;
				// Lee las instrucciones del bucle
				if (!instruction.IsError)
					ReadInstructions(instruction.Instructions);
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Elimina los tokens de fin de comando (<%end%>)
		/// </summary>
		private void RemoveTokensEndCommand()
		{
			Token token = GetToken();

				if (token.Type == Token.TokenType.StartSentenceBlock)
					RemoveTokens(2);
				else if (token.Type == Token.TokenType.Sentence &&
						 token.Content.Equals("end", StringComparison.CurrentCultureIgnoreCase))
				{ 
					// Quita la sentencia end
					token = GetToken(true);
					// Quita el final de bloque
					if (token.Type == Token.TokenType.EndSentenceBlock)
						GetToken();
				}
		}

		/// <summary>
		///		Elimina una serie de tokens
		/// </summary>
		private void RemoveTokens(int count)
		{
			for (int index = 0; index < count; index++)
				GetToken();
		}

		/// <summary>
		///		Lee una instrucción de un identificador de variable
		/// </summary>
		private InstructionVariableIdentifier ReadVariableIdentifier(Token token)
		{
			InstructionVariableIdentifier instruction = new InstructionVariableIdentifier(token);

				// Lee la expresión
				instruction.Variable = ReadVariableIdentifier(token, out string error);
				if (!string.IsNullOrWhiteSpace(error))
					instruction.Error = error;
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee un identificador de variable
		/// </summary>
		private ExpressionVariableIdentifier ReadVariableIdentifier(Token token, out string error)
		{
			ExpressionVariableIdentifier expression = new ExpressionVariableIdentifier(token);
			Token nextToken = GetToken(true);

				// Inicializa los argumentos de salida
				error = "";
				// Asigna el nombre de la variable
				expression.Name = token.Content;
				// Comprueba si hay un corchete, es decir, es una variable de array
				if (nextToken.Type == Token.TokenType.LeftCorchete)
				{ 
					// Quita el token
					GetToken();
					// Asigna la expresión
					expression.IndexExpressions = ReadExpression(ExpressionReaderMode.AtVariable, out error);
					expression.IndexExpressionsRPN = _expressionEvaluator.ConvertToRPN(expression.IndexExpressions);
					// Si no ha habido ningún error ...
					if (string.IsNullOrWhiteSpace(error))
					{ 
						// Obtiene el siguiente token
						nextToken = GetToken(true);
						// Si no es un corchete, añade un error, si es un corchete, quita el token
						if (nextToken.Type != Token.TokenType.RightCorchete)
							error = "Falta el corchete final en la definición de variable";
						else
						{ 
							// Quita el token
							GetToken();
							// ... y deja leído el siguiente para comprobar si es un miembro de variable
							nextToken = GetToken(true);
						}
					}
				}
				// Comprueba si hay un signo ->, es decir, tenemos una variable miembro
				if (nextToken.Type == Token.TokenType.VariablePointer)
				{ 
					// Quita el token
					GetToken();
					// Obtiene el siguiente token sin sacarlo del buffer
					nextToken = GetToken(true);
					// Si es un literal, lo considera una variable (para los casos en $a->b en lugar de $a->$b)
					if (nextToken.Type == Token.TokenType.Literal)
					{
						nextToken.Content = "$" + nextToken.Content;
						nextToken.Type = Token.TokenType.Variable;
					}
					// Comprueba que sea una variable
					if (nextToken.Type == Token.TokenType.Variable) // ... si es así, obtiene el identificador del miembro
						expression.Member = ReadVariableIdentifier(GetToken(), out error);
					else // ... si no es así, indica el error
						error = "Falta el identificador de la variable miembro";
				}
				// Devuelve la expresión
				return expression;
		}

		/// <summary>
		///		Lee las expresiones
		/// </summary>
		private ExpressionsCollection ReadExpression(ExpressionReaderMode mode, out string error)
		{
			ExpressionsCollection expressions = new ExpressionsCollection();
			Token nextToken = GetToken(true);

				// Inicializa los valores de salida
				error = "";
				// Lee las expresiones
				while (!IsEof(nextToken) && nextToken.IsExpressionPart && string.IsNullOrWhiteSpace(error))
				{ 
					// Añade el token a la colección de expresiones
					if (nextToken.Type == Token.TokenType.Variable)
						expressions.Add(ReadVariableIdentifier(GetToken(), out error));
					else
						expressions.Add(new ExpressionBase(GetToken()));
					// Obtiene el siguiente token
					nextToken = GetToken(true);
				}
				// Comprueba si ha habido algún error
				switch (mode)
				{
					case ExpressionReaderMode.Normal:
							if (nextToken.Type != Token.TokenType.EndSentenceBlock &&
									nextToken.Type != Token.TokenType.RightLlave && nextToken.Type != Token.TokenType.Sentence)
								error = "Falta el final de sentencia en la expresión";
						break;
					case ExpressionReaderMode.AtVariable:
							if (nextToken.Type != Token.TokenType.RightCorchete)
								error = "Falta el corchete final en la definición de variable";
						break;
					case ExpressionReaderMode.AtAttributes:
							if (nextToken.Type != Token.TokenType.RightLlave && nextToken.Type != Token.TokenType.Literal)
								error = "Falta el final de sentencia en la definición de atributos";
						break;
				}
				// Devuelve la colección de expresiones
				return expressions;
		}

		/// <summary>
		///		Lee una sentencia Let
		/// </summary>
		private InstructionLet ReadCommandLet(Token token)
		{
			InstructionLet instruction = new InstructionLet(token);
			Token nextToken = GetToken(true);

				// Lee la variable
				if (nextToken.Type == Token.TokenType.Variable)
				{ 
					// Asigna la variable
					instruction.Variable = ReadVariableIdentifier(GetToken(), out string error);
					// Comprueba si hay algún error antes de continuar
					if (!string.IsNullOrWhiteSpace(error))
						instruction.Error = error;
					else
					{ 
						// Signo igual
						nextToken = GetToken(true);
						if (nextToken.Type == Token.TokenType.Equal)
						{ 
							// Quita el igual
							GetToken();
							// Lee las expresiones
							instruction.Expressions = ReadExpression(ExpressionReaderMode.Normal, out error);
							instruction.ExpressionsRPN = _expressionEvaluator.ConvertToRPN(instruction.Expressions);
							if (!string.IsNullOrEmpty(error))
								instruction.Error = error;
						}
						else
							instruction.Error = "No se encuentra el signo igual";
					}
				}
				else
					instruction.Error = "Debe existir una variable en la parte izquierda";
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee una sentencia Code
		/// </summary>
		private InstructionCode ReadCommandCode(Token token)
		{
			InstructionCode instruction = new InstructionCode(token);
			Token nextToken = GetToken(true);

				// Comprueba si es un final de sentencia
				if (nextToken.Type != Token.TokenType.EndSentenceBlock && !token.Content.EndsWith("%>"))
					instruction.Error = "Falta el marcador de final de sentencia '%>' en la instrucción 'code'";
				else
				{ 
					// Quita el token de fin de bloque y lee el siguiente
					if (!token.Content.EndsWith("%>"))
						GetToken();
					nextToken = GetToken();
					// Si es un literal, se añade al contenido
					if (nextToken.Type == Token.TokenType.Literal)
					{ 
						// Guarda el contenido
						instruction.Content = nextToken.Content;
						// Lee el siguiente token
						nextToken = GetToken(true);
						// Comprueba que sea el final de sentencia
						if (IsEndCommand(nextToken))
							RemoveTokensEndCommand();
						else
							AddError(nextToken, "No se ha encontrado el final de la sentencia <%code%>");
					}
					else
						AddError(nextToken, "No se ha encontrado el contenido de la sentencia <%code%>");
				}
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee una sentencia for
		/// </summary>
		private InstructionBase ReadCommandFor(Token token)
		{
			InstructionFor instruction = new InstructionFor(token);
			Token nextToken = GetToken(true);

				// Lee la variable
				if (nextToken.Type == Token.TokenType.Variable)
				{ 
					// Asigna la variable
					instruction.IndexVariable = ReadVariableIdentifier(GetToken(), out string error);
					// Comprueba si hay algún error antes de continuar
					if (!string.IsNullOrWhiteSpace(error))
						instruction.Error = error;
					else
					{ 
						// Signo igual
						nextToken = GetToken(true);
						if (nextToken.Type != Token.TokenType.Equal)
							instruction.Error = "Falta el signo igual en el bucle for";
						else
						{ 
							// Quita el igual
							GetToken();
							// Lee el valor inicial
							instruction.StartValue = ReadExpression(ExpressionReaderMode.Normal, out error);
							instruction.StartValueRPN = _expressionEvaluator.ConvertToRPN(instruction.StartValue);
							if (!string.IsNullOrEmpty(error))
								instruction.Error = error;
							else
							{ 
								// Sentencia to
								nextToken = GetToken(true);
								if (nextToken.Type != Token.TokenType.Sentence ||
										!nextToken.Content.Equals("to", StringComparison.CurrentCultureIgnoreCase))
									instruction.Error = "Falta la sentencia to en el bucle for";
								else
								{ 
									// Quita el to
									nextToken = GetToken();
									// Lee el valor final
									instruction.EndValue = ReadExpression(ExpressionReaderMode.Normal, out error);
									instruction.EndValueRPN = _expressionEvaluator.ConvertToRPN(instruction.EndValue);
									if (!string.IsNullOrEmpty(error))
										instruction.Error = error;
									else
									{ 
										// Sentencia step
										nextToken = GetToken(true);
										// Obtiene el valor del incremento (si es necesario)
										if (nextToken.Type == Token.TokenType.Sentence &&
												nextToken.Content.Equals("step", StringComparison.CurrentCultureIgnoreCase))
										{ 
											// Quita el step
											GetToken();
											// Lee el valor del step
											instruction.StepValue = ReadExpression(ExpressionReaderMode.Normal, out error);
											instruction.StepValueRPN = _expressionEvaluator.ConvertToRPN(instruction.StepValue);
											// Comprueba el error y lee las instrucciones
											if (!string.IsNullOrEmpty(error))
												instruction.Error = error;
										}
										// Lee las instrucciones del for
										if (!instruction.IsError)
											token = ReadInstructions(instruction.Instructions);
									}
								}
							}
						}
					}
				}
				else
					instruction.Error = "Debe existir una variable después del for";
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee una sentencia foreach
		/// </summary>
		private InstructionForEach ReadCommandForEach(Token token)
		{
			InstructionForEach instruction = new InstructionForEach(token);
			Token nextToken = GetToken(true);

				// Lee los datos de la instrucción
				if (nextToken.Type == Token.TokenType.Variable)
				{ 
					// Lee la variable
					instruction.IndexVariable = ReadVariableIdentifier(nextToken, out string error);
					// Comprueba los errores
					if (!string.IsNullOrEmpty(error))
						instruction.Error = "Error al leer la variable índice: " + error;
					else
					{ 
						// Lee la parte in
						nextToken = GetToken(true);
						if (nextToken.Type == Token.TokenType.Literal && (nextToken.Content ?? "").Equals("in", StringComparison.CurrentCultureIgnoreCase))
						{ 
							// Quita el in de la cadena de tokens
							GetToken();
							// Lee el siguiente token
							nextToken = GetToken(true);
							// Comprueba si es la variable de lista
							if (nextToken.Type == Token.TokenType.Variable)
							{ 
								// Guarda la variable de lista
								instruction.ListVariable = ReadVariableIdentifier(nextToken, out error);
								// Comprueba los errores antes de continuar
								if (!string.IsNullOrEmpty(error))
									instruction.Error = "Error al leer la variable del bucle: " + error;
								else
									ReadInstructions(instruction.Instructions);
							}
							else
								instruction.Error = "Falta la variable de lista";
						}
						else
							instruction.Error = "Falta la sentencia in";
					}
				}
				else
					instruction.Error = "No se ha definido la variable índice";
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Lee los atributos de una instrucción
		/// </summary>
		private void ReadAttributes(InstructionNhaml instruction)
		{
			Token token = GetToken();
			bool withError = false; // ... supone que hay algún error

				// Lee el primer token (el anterior sólo ha quitado la llave)
				token = GetToken();
				// Lee los atributos
				while (!IsEof(token) && token.Type != Token.TokenType.RightLlave && !withError)
				{
					Parameter attribute = new Parameter();

					// Obtiene el atributo
					if (token.Type == Token.TokenType.Literal)
					{ 
						// Nombre del atributo
						attribute.Name = token.Content;
						// El nombre de un atributo puede ser del tipo http-equiv, es decir, puede tener un guión intermedio,
						// aquí obtiene el resto
						token = GetToken(true);
						if (token.Type == Token.TokenType.Literal && token.Content.StartsWith("-"))
						{ 
							// Quita el token
							token = GetToken();
							// Añade el guión al nombre del atributo
							attribute.Name += token.Content;
						}
						// Signo igual
						token = GetToken();
						if (token.Type == Token.TokenType.Equal)
						{ 
							// Literal
							token = GetToken(true);
							// Comprueba y lo añade
							if (token.IsExpressionPart)
							{
								// Obtiene las expresiones de la variable
								attribute.Variable = ReadExpression(ExpressionReaderMode.AtAttributes, out string error);
								attribute.VariableRPN = _expressionEvaluator.ConvertToRPN(attribute.Variable);
								// Indica si hay un error
								if (!error.IsEmpty())
								{
									withError = true;
									instruction.Error = error;
								}
							}
							else
								withError = false;
						}
					}
					else
						withError = true;
					// Si no hay ningún error, añade el atributo y lee el siguiente token
					if (!withError)
					{ 
						// Añade el atributo
						instruction.Attributes.Add(attribute);
						// Lee el siguiente token
						token = GetToken();
					}
				}
				// Si hay algún error, lo añade
				if (withError)
					instruction.Error = "Error en la definición de parámetros";
		}

		/// <summary>
		///		Comprueba si un token es una instrucción diferente
		/// </summary>
		private bool IsNextInstruction(Token nextToken)
		{
			return nextToken.Type == Token.TokenType.StartComment || nextToken.Type == Token.TokenType.StartSentenceBlock ||
				   nextToken.Type == Token.TokenType.Sentence || nextToken.Type == Token.TokenType.TagHTML;
		}

		/// <summary>
		///		Comprueba si un token es una instrucción diferente que debe estar dentro de esta instrucción
		/// </summary>
		private bool IsNextInstructionInternal(InstructionBase instruction, Token nextToken)
		{
			return IsNextInstruction(nextToken) && nextToken.Indent > instruction.Token.Indent;
		}

		/// <summary>
		///		Comprueba si un token es un fin de comando
		/// </summary>
		private bool IsEndCommand(Token nextToken)
		{
			return (nextToken.Type == Token.TokenType.StartSentenceBlock &&
					GetNextTokensString(3).Equals("<%end%>", StringComparison.CurrentCultureIgnoreCase)) ||
				   (nextToken.Type == Token.TokenType.Sentence &&
				    nextToken.Content.Equals("end", StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		///		Comprueba si un token es un comando else
		/// </summary>
		private bool IsElseCommand(Token token)
		{
			return (token.Type == Token.TokenType.StartSentenceBlock &&
					GetNextTokensString(3).Equals("<%else%>", StringComparison.CurrentCultureIgnoreCase)) ||
				   (token.Type == Token.TokenType.Sentence &&
				    token.Content.Equals("else", StringComparison.CurrentCultureIgnoreCase));
		}

		/// <summary>
		///		Obtiene el siguiente token
		/// </summary>
		private Token GetToken(bool isSimulated = false)
		{
			if (Tokens == null || IndexToken > Tokens.Count - 1)
				return new Token { Type = Token.TokenType.EOF };
			else if (isSimulated)
				return Tokens[IndexToken];
			else
				return Tokens[IndexToken++];
		}

		/// <summary>
		///		Obtiene una cadena con los siguientes tokens
		/// </summary>
		private string GetNextTokensString(int count)
		{
			string content = "";

				// Obtiene las cadenas de los siguientes tokens
				for (int index = 0; index < count; index++)
					if (IndexToken + index < Tokens.Count)
						content += Tokens[IndexToken + index].Content;
				// Devuelve el contenido
				return content;
		}

		/// <summary>
		///		Obtiene una instrucción de error
		/// </summary>
		private InstructionBase GetInstructionError(Token token, string error)
		{
			InstructionBase instruction = new InstructionBase(token);

				// Indica que es un error
				instruction.Error = "Tipo de instrucción desconocida";
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		private void AddError(Token token, string message)
		{
			Compiler.LocalErrors.Add(token, message);
		}

		/// <summary>
		///		Se recupera del error (busca el primer token de Html)
		/// </summary>
		private void RecoverError()
		{
			Token token = GetToken(true);

				while (!IsEof(token) && token.Type != Token.TokenType.TagHTML)
					token = GetToken();
		}

		/// <summary>
		///		Lanza el evento de depuración de los tokens
		/// </summary>
		private void DebugTokens()
		{
			string result = "";

				// Obtiene los tokens de la cadena
				foreach (Token token in Tokens)
					result += Environment.NewLine + token.ToString();
				// Lanza el evento de depuración
				Compiler.RaiseEventDebug(EventArgs.DebugEventArgs.Mode.Tokenizer, "Tokens", result);
		}

		/// <summary>
		///		Lanza el evento de depuración de las instrucciones
		/// </summary>
		private void DebugInstructions()
		{
			Compiler.RaiseEventDebug(EventArgs.DebugEventArgs.Mode.Instructions, "Instructions", Instructions.GetDebugString());
		}

		/// <summary>
		///		Comprueba si es el final del archivo
		/// </summary>
		private bool IsEof(Token token)
		{
			return token.Type == Token.TokenType.EOF;
		}

		/// <summary>
		///		Compilador
		/// </summary>
		private Compiler Compiler { get; set; }

		/// <summary>
		///		Tokens
		/// </summary>
		internal TokensCollection Tokens { get; private set; }

		/// <summary>
		///		Instrucciones
		/// </summary>
		internal InstructionsBaseCollection Instructions { get; } = new InstructionsBaseCollection();

		/// <summary>
		///		Indice del token actual
		/// </summary>
		private int IndexToken { get; set; }
	}
}
