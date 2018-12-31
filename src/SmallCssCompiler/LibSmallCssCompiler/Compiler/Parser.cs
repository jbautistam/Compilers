using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibTokenizer.Interpreter;
using Bau.Libraries.LibTokenizer.Interpreter.Instructions;
using Bau.Libraries.LibTokenizer.Lexical.Tokens;
using Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler
{
	/// <summary>
	///		Compilador. Genera las instrucciones a partir de los tokens
	/// </summary>
	internal class Parser
	{
		/// <summary>
		///		Compila una serie de tokens
		/// </summary>
		internal Program Parse(TokenSmallCssCollection tokens)
		{
			Program program = new Program("Main", null);

				// Guarda los tokens
				IndexActual = 0;
				Source = tokens;
				// Compila el token actual
				while (IndexActual < Source.Count)
					program.Sentences.Add(CreateInstruction());
				// Devuelve la instrucciones
				return program;
		}

		/// <summary>
		///		Crea una instrucción a partir de los tokens
		/// </summary>
		private InstructionBase CreateInstruction()
		{
			switch (ActualToken.TypeCss)
			{
				case TokenSmallCss.TokenCssType.Comment:
					return CreateInstructionComment();
				case TokenSmallCss.TokenCssType.Error:
					return CreateInstructionError();
				case TokenSmallCss.TokenCssType.Literal:
					return CreateInstructionLiteral();
				case TokenSmallCss.TokenCssType.ReservedWord:
					return CreateInstructionReservedWord();
				case TokenSmallCss.TokenCssType.Variable:
					return CreateInstructionVariable();
				default:
					return null;
			}
		}

		/// <summary>
		///		Obtiene un bloque de instrucciones
		/// </summary>
		private InstructionsBaseCollection GetBlock(int indentBase)
		{
			InstructionsBaseCollection instructions = new InstructionsBaseCollection();
			bool end = false;

				// Obtiene las instrucciones que estén en un nivel de indentación superior
				while (!IsEof && !end)
				{
					Token token = ActualToken;

					if (token != null && token.Indent > indentBase)
						instructions.Add(CreateInstruction());
					else
						end = true;
				}
				// Devuelve la colección de instrucciones
				return instructions;
		}

		/// <summary>
		///		Crea una instrucción de comentario
		/// </summary>
		private InstructionBase CreateInstructionComment()
		{
			return new InstructionComment(GetToken());
		}

		/// <summary>
		///		Crea una instrucción de error
		/// </summary>
		private InstructionBase CreateInstructionError()
		{
			return new InstructionError(GetToken());
		}

		/// <summary>
		///		Crea una instrucción de palabra reservada
		/// </summary>
		private InstructionBase CreateInstructionReservedWord()
		{
			if (ActualToken.Value.EqualsIgnoreCase(CompilerConstants.Import))
				return CreateInstructionIncludeFile();
			else if (ActualToken.Value.EqualsIgnoreCase(CompilerConstants.MixinDefinition))
				return CreateInstructionMixinDefinition();
			else if (ActualToken.Value.EqualsIgnoreCase(CompilerConstants.MixinInclude))
				return CreateInstructionMininCall();
			else if (ActualToken.Value.EqualsIgnoreCase(CompilerConstants.IfDefined))
				return CreateInstructionIfDefined();
			else if (ActualToken.Value.EqualsIgnoreCase(CompilerConstants.Media))
				return CreateInstructionMedia();
			else
				return new InstructionLineCss(GetToken());
		}

		/// <summary>
		///		Crea la instrucción de crear archivo
		/// </summary>
		private InstructionIncludeFile CreateInstructionIncludeFile()
		{
			InstructionIncludeFile instruction = new InstructionIncludeFile(GetToken());
			TokenSmallCss token = GetToken();

				// Captura el nombre de archivo
				if (token.Row == instruction.Token.Row && token.TypeCss == TokenSmallCss.TokenCssType.Literal)
					instruction.FileName = token.Value;
				else if (token.Row == instruction.Token.Row && token.TypeCss == TokenSmallCss.TokenCssType.Comment)
					instruction.FileName = "_" + token.Value;
				else
					instruction.Error = ParseError("No se reconoce el nombre de archivo a incluir");
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Interpreta un error
		/// </summary>
		private InstructionError ParseError(string error)
		{
			return new InstructionError(ActualToken, error);
		}

		/// <summary>
		///		Crea la instrucción de definición de un Mixin
		/// </summary>
		private InstructionBase CreateInstructionMixinDefinition()
		{
			InstructionFunction instruction = new InstructionFunction(GetToken());
			TokenSmallCss token = GetToken();

				// Obtiene la función
				if (token.Row == instruction.Token.Row && token.TypeCss == TokenSmallCss.TokenCssType.Literal)
				{ 
					// Asigna el nombre de la función
					instruction.Name = token.Value;
					// Obtiene los argumentos
					if (ActualToken.Row == instruction.Token.Row)
					{
						bool end = false;

						while (!end && !IsEof)
						{ 
							// Comprueba si es un argumento
							if (ActualToken.Row == instruction.Token.Row)
							{
								if (ActualToken.TypeCss == TokenSmallCss.TokenCssType.Literal ||
										  ActualToken.TypeCss == TokenSmallCss.TokenCssType.Variable)
								{
									string argument = ActualToken.Value;

										// Normaliza el argumento
										if (!argument.StartsWith("$"))
											argument = "$" + argument;
										// Añade el nombre del argumento
										instruction.Arguments.Add(argument);
										// Obtiene el siguiente token
										GetToken();
								}
								else if (ActualToken.TypeCss == TokenSmallCss.TokenCssType.Comment)
									instruction.Sentences.Add(CreateInstructionComment());
								else
									instruction.Error = ParseError($"No se reconoce el token entre los argumentos de la función '{instruction.Name}'");
							}
							else
								end = true;
						}
					}
					if (!instruction.IsError)
					{
						if (CheckIsBlock(instruction.Token))
							instruction.Sentences.AddRange(GetBlock(instruction.Token.Indent));
						else
							instruction.Error = ParseError("No se han definido las instrucciones de la función");
					}
				}
				else
					instruction.Error = ParseError("No se encuentra el nombre de la función");
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Crea la instrucción de llamada a un Mixin
		/// </summary>
		private InstructionBase CreateInstructionMininCall()
		{
			InstructionMixinCall instruction = new InstructionMixinCall(GetToken());
			TokenSmallCss token = GetToken();

				// Obtiene los datos
				if (token.Row == instruction.Token.Row && token.TypeCss == TokenSmallCss.TokenCssType.Literal)
				{ 
					// Asigna el nombre de la función
					instruction.Name = token.Value;
					// Obtiene los argumentos
					if (ActualToken != null && ActualToken.Row == instruction.Token.Row)
					{
						bool end = false;

							while (!end && !IsEof)
							{ 
								// Comprueba si es un parámetro o un comentario
								if (ActualToken.Row == instruction.Token.Row)
								{
									if (ActualToken.TypeCss == TokenSmallCss.TokenCssType.Literal ||
											  ActualToken.TypeCss == TokenSmallCss.TokenCssType.Variable)
										instruction.Parameters.Add(GetToken());
									else if (ActualToken.TypeCss == TokenSmallCss.TokenCssType.Comment)
										instruction.Sentences.Add(CreateInstructionComment());
									else
										instruction.Error = ParseError($"No se reconoce el token entre los parámetros de llamada a la función '{instruction.Name}'");
								}
								else
									end = true;
							}
					}
				}
				else
					instruction.Error = ParseError("No se encuentra el nombre del mixin al que se está llamando");
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Crea una instrucción IfDefined
		/// </summary>
		private InstructionBase CreateInstructionIfDefined()
		{
			InstructionIfDefined instruction = new InstructionIfDefined(GetToken());
			TokenSmallCss token = GetToken();

				// Obtiene la variable
				if (token.Row == instruction.Token.Row &&
							(token.TypeCss == TokenSmallCss.TokenCssType.Literal ||
							 token.TypeCss == TokenSmallCss.TokenCssType.Variable))
				{ 
					// Asigna el nombre de la variable
					instruction.Identifier = token.Value;
					// Obtiene las instrucciones
					if (CheckIsBlock(instruction.Token))
						instruction.SentencesIf.AddRange(GetBlock(instruction.Token.Indent));
					else
						instruction.Error = ParseError("No se han definido las instrucciones de la función");
				}
				else
					instruction.Error = ParseError("No se encuentra la definición de variable");
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Crea una instrucción Media
		/// </summary>
		private InstructionBase CreateInstructionMedia()
		{
			InstructionMedia instruction = new InstructionMedia(GetToken());
			TokenSmallCss token = GetToken();

				// Rellena los parámetros
				while (!IsEof && token.Row == instruction.Token.Row)
				{ 
					// Añade el valor
					instruction.Parameters += $"{token.Value} ";
					// Obtiene el siguiente token
					token = GetToken();
				}
				// Si el siguiente contenido es de una línea CSS, lo guarda
				if (token.TypeCss == TokenSmallCss.TokenCssType.Literal)
					instruction.Line = CreateInstructionLiteral(token);
				else
					instruction.Error = ParseError("No se encuentra el contenido de la línea @media");
				// Dvuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Crea una instrucción de literal
		/// </summary>
		private InstructionLineCss CreateInstructionLiteral(TokenSmallCss token = null)
		{
			InstructionLineCss instruction;
			bool end = false;

				// Crea la instrucción
				if (token != null)
					instruction = new InstructionLineCss(token);
				else
					instruction = new InstructionLineCss(GetToken());
				// Recorre los tokens
				while (!IsEof && !end)
				{ 
					// Si cambiamos de línea a una de mayor indentación, obtenemos el bloque
					if (CheckIsBlock(instruction.Token) || CheckIsEndLine(instruction.Token))
						end = true;
					else if (ActualToken.Row == instruction.Token.Row) // ... si seguimos en la misma línea
					{
						if (ActualToken.TypeCss == TokenSmallCss.TokenCssType.Literal ||
								  ActualToken.TypeCss == TokenSmallCss.TokenCssType.Variable)
							instruction.Tokens.Add(GetToken());
						else if (ActualToken.TypeCss == TokenSmallCss.TokenCssType.Comment)
							instruction.Sentences.Add(CreateInstructionComment());
						else
							instruction.Tokens.Add(GetTokenError());
					}
				}
				// Si lo siguiente es un bloque
				if (CheckIsBlock(instruction.Token))
					instruction.Sentences.AddRange(GetBlock(instruction.Token.Indent));
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Obtiene una instrucción de asignación de variables
		/// </summary>
		private InstructionBase CreateInstructionVariable()
		{
			InstructionVariableIdentifier instruction = new InstructionVariableIdentifier(GetToken());
			TokenSmallCss tokenValue = GetToken();

				if (tokenValue.Row == instruction.Token.Row &&
					tokenValue.TypeCss == TokenSmallCss.TokenCssType.Literal)
				{
					if (tokenValue.Value.TrimIgnoreNull() == ":")
					{
						tokenValue = GetToken();
						if (tokenValue.Row == instruction.Token.Row &&
								tokenValue.TypeCss == TokenSmallCss.TokenCssType.Literal)
							instruction.Value.Add(new ExpressionBase(tokenValue));
						else
							instruction.Error = ParseError("No se reconoce el valor asignado a la variable");
					}
					else
						instruction.Value.Add(new ExpressionBase(tokenValue));
				}
				else
					instruction.Error = ParseError("No se reconoce el valor asignado a la variable");
				// Devuelve la instrucción
				return instruction;
		}

		/// <summary>
		///		Comprueba si lo siguiente es un bloque
		/// </summary>
		private bool CheckIsBlock(Token objPreviousToken)
		{
			return !IsEof && ActualToken.Row > objPreviousToken.Row && ActualToken.Indent > objPreviousToken.Indent;
		}

		/// <summary>
		///		Comprueba si lo siguiente es un bloque
		/// </summary>
		private bool CheckIsEndLine(Token objPreviousToken)
		{
			return IsEof || ActualToken.Row > objPreviousToken.Row;
		}

		/// <summary>
		///		Obtiene el token actual incrementando 
		/// </summary>
		private TokenSmallCss GetToken()
		{
			TokenSmallCss token = ActualToken;

				// Incrementa el índice
				IndexActual++;
				// Devuelve el token
				return token;
		}

		/// <summary>
		///		Obtiene un token de error
		/// </summary>
		private TokenSmallCss GetTokenError()
		{
			TokenSmallCss token = new TokenSmallCss(GetToken());

				// Indica que es un token erróneo
				token.TypeCss = TokenSmallCss.TokenCssType.Error;
				// Devuelve el token
				return token;
		}

		/// <summary>
		///		Indice de token actual
		/// </summary>
		private int IndexActual { get; set; }

		/// <summary>
		///		Indica si estamos al final del código
		/// </summary>
		private bool IsEof
		{
			get { return IndexActual >= Source.Count || ActualToken.TypeCss == TokenSmallCss.TokenCssType.EOF; }
		}

		/// <summary>
		///		Token anterior
		/// </summary>
		private TokenSmallCss PreviousToken
		{
			get
			{
				if (IndexActual > 0)
					return Source[IndexActual - 1];
				else
					return null;
			}
		}

		/// <summary>
		///		Obtiene el token actual
		/// </summary>
		private TokenSmallCss ActualToken
		{
			get
			{
				if (IndexActual >= Source.Count)
					return null;
				else
					return Source[IndexActual];
			}
		}

		/// <summary>
		///		Tokens origen
		/// </summary>
		private TokenSmallCssCollection Source { get; set; }
	}
}
