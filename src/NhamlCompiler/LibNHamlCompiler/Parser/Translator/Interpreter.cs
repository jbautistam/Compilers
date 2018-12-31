using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.NhamlCompiler.Parser.Evaluator;
using Bau.Libraries.NhamlCompiler.Parser.Instructions;
using Bau.Libraries.NhamlCompiler.Parser.Tokens;
using Bau.Libraries.NhamlCompiler.Variables;

namespace Bau.Libraries.NhamlCompiler.Parser.Translator
{
	/// <summary>
	///		Intérprete
	/// </summary>
	internal class Interpreter
	{ 
		// Constantes privadas
		private const string CharsNoSpace = ".,;:)]}"; // ... caracteres que no precisan que se añada un espacio anterior en la salida

		internal Interpreter(Compiler compiler, VariablesCollection variables, int maxInstructions = 0, bool isCompressed = false)
		{
			Compiler = compiler;
			MaxInstructions = maxInstructions;
			Builder = new Writer.StringBuilderHtml(isCompressed);
			ExpressionComputer = new ExpressionCompute(variables);
		}

		/// <summary>
		///		Interpreta un programa
		/// </summary>
		internal string Parse(string source)
		{
			Lexical.ParserManager parser = new Lexical.ParserManager(Compiler);

				// Interpreta las líneas de programa
				parser.Parse(source);
				// Ejecuta las instrucciones
				if (Compiler.LocalErrors.Count > 0)
					Builder.Add("Error en la interpretación");
				else if (MaxInstructions > 0)
					Execute(parser.Instructions.Select(MaxInstructions));
				else
					Execute(parser.Instructions);
				// Devuelve la cadena resultante
				return Builder.Builder.ToString();
		}

		/// <summary>
		///		Ejecuta el programa
		/// </summary>
		private void Execute(InstructionsBaseCollection instructions)
		{
			if (instructions != null)
				foreach (InstructionBase instruction in instructions)
					Execute(instruction);
		}

		/// <summary>
		///		Ejecuta una instrucción
		/// </summary>
		private void Execute(InstructionBase instruction)
		{
			if (instruction is InstructionNhaml)
				Execute(instruction as InstructionNhaml);
			else if (instruction is InstructionComment)
				Execute(instruction as InstructionComment);
			else if (instruction is InstructionCode)
				Execute(instruction as InstructionCode);
			else if (instruction is InstructionForEach)
				Execute(instruction as InstructionForEach);
			else if (instruction is InstructionFor)
				Execute(instruction as InstructionFor);
			else if (instruction is InstructionIf)
				Execute(instruction as InstructionIf);
			else if (instruction is InstructionWhile)
				Execute(instruction as InstructionWhile);
			else if (instruction is InstructionLet)
				Execute(instruction as InstructionLet);
			else if (instruction.Token.Type != Token.TokenType.EOF)
				Compiler.LocalErrors.Add(instruction.Token, "Error en la ejecución. Instrucción desconocida");
		}

		/// <summary>
		///		Ejecuta una instrucción Nhaml
		/// </summary>
		private void Execute(InstructionNhaml instruction)
		{
			int index = 0;

				// Asigna la indentación
				Builder.Indent = instruction.Token.Indent;
				// Añade la etiqueta de apertura
				Builder.AddTag(GetTagHtml(instruction.Token, true) + " " + GetAttributes(instruction), false, instruction.IsInner);
				// Añade los literales
				foreach (InstructionBase innerInstruction in instruction.Instructions)
					if (innerInstruction is InstructionComment)
						Execute(innerInstruction as InstructionComment);
					else if (innerInstruction is InstructionNhaml || innerInstruction.Token.Type == Token.TokenType.Sentence) // ... tiene que estar antes de comprobar si es una instrucción base
						Execute(innerInstruction);
					else if (innerInstruction is InstructionVariableIdentifier)
						Builder.Add(" " + GetVariableValue(innerInstruction as InstructionVariableIdentifier));
					else if (innerInstruction is InstructionBase || innerInstruction is InstructionLiteral)
					{
						bool addSpace = MustAddSpace(index, innerInstruction.Token.Content);
						string content = innerInstruction.Token.Content;

						// Añade el contenido
						Builder.Add((addSpace ? " " : "") + content);
						// Incrementa el índice
						index++;
					}
				// Añade la etiqueta de cierre
				Builder.Indent = instruction.Token.Indent;
				Builder.AddTag(GetTagHtml(instruction.Token, false), true, instruction.IsInner);
		}

		/// <summary>
		///		Obtiene la etiqueta de Html
		/// </summary>
		private string GetTagHtml(Token token, bool start)
		{
			string html;

				// Obtiene la etiqueta
				if (token.Content.StartsWith("&"))
				{
					if (start)
						html = $"div id=\"{token.Content.Substring(1)}\"";
					else
						html = "div";
				}
				else
					html = token.Content.Substring(1);
				// Devuelve la etiqueta
				return html;
		}

		/// <summary>
		///		Obtiene el valor de una variable
		/// </summary>
		private string GetVariableValue(InstructionVariableIdentifier instruction)
		{
			ValueBase value = GetVariable(instruction);

				if (value == null)
					return $"##Error al obtener la variable {instruction.Variable.Name}";
				else if (value.Content != null)
					return value.Content;
				else
					return "";
		}

		/// <summary>
		///		Obtiene una variable
		/// </summary>
		private ValueBase GetVariable(InstructionVariableIdentifier instruction)
		{
			Variable variable = ExpressionComputer.Search(instruction.Variable, out string error);

				if (!string.IsNullOrWhiteSpace(error))
				{
					Compiler.LocalErrors.Add(instruction.Token, error);
					return ValueBase.GetError($"## Error al obtener la variable: {error} ##");
				}
				else if (variable == null) // ... nunca se debería dar
				{
					Compiler.LocalErrors.Add(instruction.Token, $"No se encuentra el valor de la variable {instruction.Variable.Name}");
					return ValueBase.GetError($"## No se encuentra la variable: {instruction.Variable.Name} ##");
				}
				else
					return variable.Value;
		}

		/// <summary>
		///		Obtiene los atributos de una instrucción
		/// </summary>
		private string GetAttributes(InstructionNhaml instruction)
		{
			string attributes = "";

				// Añade los parámetros
				foreach (Parameter parameter in instruction.Attributes)
				{
					ValueBase result = ExpressionComputer.Evaluate(parameter.VariableRPN);

						// Añade el nombre
						attributes += $" {parameter.Name}=";
						// Añade el valor
						if (result.HasError)
							Compiler.LocalErrors.Add(instruction.Token, result.Error);
						else
							attributes += $"\"{result.Content}\"";
				}
				// Devuelve los atributos
				return attributes;
		}

		/// <summary>
		///		Ejecuta una instrucción de código
		/// </summary>
		private void Execute(InstructionCode instruction)
		{ 
			Builder.Add(Environment.NewLine + instruction.Content);
		}

		/// <summary>
		///		Ejecuta un comentario
		/// </summary>
		private void Execute(InstructionComment instruction)
		{
			if (!Builder.IsCompressed)
			{ 
				// Añade el inicio de comentario
				Builder.Indent = instruction.Token.Indent;
				Builder.AddIndent();
				Builder.Add("<!--");
				// Añade el texto
				Builder.Add(instruction.Content);
				// Añade el fin de comentario
				Builder.Add("-->");
			}
		}

		/// <summary>
		///		Ejecuta una instrucción if
		/// </summary>
		private void Execute(InstructionIf instruction)
		{
			ValueBase result = ExpressionComputer.Evaluate(instruction.ConditionRPN);

				if (result.HasError)
					Compiler.LocalErrors.Add(instruction.Token, result.Error);
				else if (!(result is ValueBool))
					Compiler.LocalErrors.Add(instruction.Token, "El resultado de calcular la expresión no es un valor lógico");
				else if ((result as ValueBool).Value)
					Execute(instruction.Instructions);
				else
					Execute(instruction.InstructionsElse);
		}

		/// <summary>
		///		Ejecuta una instrucción while
		/// </summary>
		private void Execute(InstructionWhile instruction)
		{
			bool end = false;
			int loopIndex = 0;

				// Ejecuta las instrucciones en un bucle
				do
				{
					ValueBase result = ExpressionComputer.Evaluate(instruction.ConditionRPN);

						if (result.HasError)
						{
							Compiler.LocalErrors.Add(instruction.Token, result.Error);
							end = true;
						}
						else if (!(result is ValueBool))
						{
							Compiler.LocalErrors.Add(instruction.Token, "El resultado de calcular la expresión no es un valor lógico");
							end = true;
						}
						else if (!(result as ValueBool).Value)
							end = true;
						else
							Execute(instruction.Instructions);
				}
				while (!end && ++loopIndex < Compiler.MaximumRepetitionsLoop);
		}

		/// <summary>
		///		Ejecuta una instrucción for
		/// </summary>
		private void Execute(InstructionFor instruction)
		{
			Variable variableIndex = ExpressionComputer.Search(instruction.IndexVariable, out string error);

				if (!error.IsEmpty())
					Compiler.LocalErrors.Add(instruction.Token, $"Error al obtener la variable índice: {error}");
				else
				{
					ValueBase valueStart = ExpressionComputer.Evaluate(instruction.StartValueRPN);

						if (valueStart.HasError)
							Compiler.LocalErrors.Add(instruction.Token, $"Error al obtener el valor de inicio del bucle for {valueStart.Error}");
						else if (!(valueStart is ValueNumeric))
							Compiler.LocalErrors.Add(instruction.Token, "El valor de inicio del bucle for no es un valor numérico");
						else
						{
							ValueBase valueEnd = ExpressionComputer.Evaluate(instruction.EndValueRPN);

								if (valueEnd.HasError)
									Compiler.LocalErrors.Add(instruction.Token, $"Error al obtener el valor de fin del bucle for {valueEnd.Error}");
								else if (!(valueEnd is ValueNumeric))
									Compiler.LocalErrors.Add(instruction.Token, "El valor de fin del bucle for no es un valor numérico");
								else
								{
									ValueBase valueStep;

										// Obtiene el valor del paso
										if (instruction.StepValueRPN == null || instruction.StepValueRPN.Count == 0)
											valueStep = ValueBase.GetInstance("1");
										else
											valueStep = ExpressionComputer.Evaluate(instruction.StepValueRPN);
										// Comprueba los errores antes de entrar en el bucle
										if (valueStep.HasError)
											Compiler.LocalErrors.Add(instruction.Token, $"Error al obtener el valor de paso del bucle for {valueEnd.Error}");
										else if (!(valueEnd is ValueNumeric))
											Compiler.LocalErrors.Add(instruction.Token, "El valor de paso del bucle for no es un valor numérico");
										else
										{
											int indexLoop = 0;
											int start = (int) (valueStart as ValueNumeric).Value;
											int end = (int) (valueEnd as ValueNumeric).Value;
											int intStep = (int) (valueStep as ValueNumeric).Value;
											int index = start;

												// Cambia el valor de la variable de índice
												variableIndex.Value = ValueBase.GetInstance(index.ToString());
												// Ejecuta las instrucciones del bucle
												while (index <= end && indexLoop < Compiler.MaximumRepetitionsLoop)
												{ 
													// Ejecuta las instrucciones del bucle
													Execute(instruction.Instructions);
													// Incrementa la variable índice y cambia el valor de la variable
													index += intStep;
													variableIndex.Value = ValueBase.GetInstance(index.ToString());
													// Incrementa el número de iteraciones
													indexLoop++;
												}
												// Comprueba el número de iteraciones
												if (indexLoop >= Compiler.MaximumRepetitionsLoop)
													Compiler.LocalErrors.Add(instruction.Token, "Se ha sobrepasado el número máximo de iteraciones del bucle for");
										}
								}
						}
				}
		}

		/// <summary>
		///		Ejecuta las instrucciones de un bucle foreach
		/// </summary>
		private void Execute(InstructionForEach instruction)
		{
			Variable variable = ExpressionComputer.Search(instruction.IndexVariable, out string error);

				if (!string.IsNullOrEmpty(error))
					Compiler.LocalErrors.Add(instruction.Token, error);
				else
				{
					VariablesCollection variables = ExpressionComputer.Search(instruction.ListVariable, out error).Members;

						if (!string.IsNullOrEmpty(error))
							Compiler.LocalErrors.Add(instruction.Token, error);
						else
						{ 
							// Ordena las variables por su índice
							variables.SortByIndex();
							// Recorre las variables ejecutando el código (en realidad puede que no fuera necesario comprobar el número de iteraciones porque
							// la colección de variables no se va a modificar por mucho que lo intente el código Nhaml)
							for (int index = 0; index < variables.Count && index < Compiler.MaximumRepetitionsLoop; index++)
							{ 
								// Asigna el contenido a la variable
								variable.Value = variables[index].Value;
								// Ejecuta las instrucciones
								Execute(instruction.Instructions);
							}
						}
				}
		}

		/// <summary>
		///		Ejecuta una instrucción de asignación
		/// </summary>
		private void Execute(InstructionLet instruction)
		{
			Variable variable = ExpressionComputer.Search(instruction.Variable, out string error);

				if (!string.IsNullOrWhiteSpace(error))
					Compiler.LocalErrors.Add(instruction.Token, error);
				else
				{
					ValueBase value = ExpressionComputer.Evaluate(instruction.ExpressionsRPN);

						if (value.HasError)
							Compiler.LocalErrors.Add(instruction.Token, value.Error);
						else
							variable.Value = value;
				}
		}

		/// <summary>
		///		Comprueba si se debe añadir un espacio
		/// </summary>
		private bool MustAddSpace(int index, string text)
		{
			bool addSpace = index != 0;

				// Comprueba si debe añadir el espacio
				if (addSpace)
					foreach (char chrChar in CharsNoSpace)
						if (text.StartsWith(chrChar.ToString()))
							addSpace = false;
				// Devuelve si se debe añadir un espacio
				return addSpace;
		}

		/// <summary>
		///		Generador de HTML
		/// </summary>
		private Writer.StringBuilderHtml Builder { get; }

		/// <summary>
		///		Compilador
		/// </summary>
		private Compiler Compiler { get; }

		/// <summary>
		///		Número máximo de instrucciones a compilar
		/// </summary>
		private int MaxInstructions { get; }

		/// <summary>
		///		Objeto de ejecución de expresiones
		/// </summary>
		private ExpressionCompute ExpressionComputer { get; }
	}
}
