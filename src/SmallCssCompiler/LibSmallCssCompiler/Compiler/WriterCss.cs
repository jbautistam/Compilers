using System;
using System.Collections.Generic;
using System.Text;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibTokenizer.Interpreter;
using Bau.Libraries.LibTokenizer.Interpreter.Instructions;
using Bau.Libraries.LibSmallCssCompiler.Compiler.Instructions;

namespace Bau.Libraries.LibSmallCssCompiler.Compiler
{
	/// <summary>
	///		Escribe un archivo Css
	/// </summary>
	internal class WriterCss
	{   
		// Variables privadas
		private StringBuilder _generator = new StringBuilder();

		internal WriterCss(Program program, bool minimize)
		{
			Program = program;
			Minimize = minimize;
		}

		/// <summary>
		///		Genera el CSS de un programa
		/// </summary>
		internal string Generate()
		{
			string result;

				// Escribe el programa
				result = GetCss(Program, "");
				// Minimiza el resultado
				if (Minimize && !result.IsEmpty())
					result = MinimizeCss(result);
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Minimiza una cadena CSS
		/// </summary>
		private string MinimizeCss(string css)
		{ 
			// Minimiza la cadena
			css = css.Replace(Environment.NewLine, " ");
			css = css.Replace("\n", " ");
			css = css.Replace("\r", " ");
			css = css.Replace("{ ", "{");
			css = css.Replace(" { ", "{");
			css = css.Replace(" }", "}");
			css = css.Replace("} ", "}");
			css = css.Replace("; ", ";");
			css = css.Replace(": ", ":");
			css = css.Replace(", ", ",");
			// Quita los espaccios dobles
			while (css.IndexOf("  ") >= 0)
				css = css.Replace("  ", " ");
			// Devuelve la cadena
			return css.TrimIgnoreNull();
		}

		/// <summary>
		///		Escribe el CSS de un programa
		/// </summary>
		/// <remarks>
		///		CssParent
		///			{ text: xxx
		///			}
		///			
		///			CssParent h1
		///				{ text:xxx
		///				}
		/// </remarks>
		private string GetCss(Program program, string tagParent)
		{
			string result = "";

				// Obtiene el CSS de las instrucciones
				foreach (InstructionBase instruction in program.Sentences)
					result += GetCss(program, instruction, tagParent);
				// Devuelve el CSS	
				return result;
		}

		/// <summary>
		///		Obtiene el CSS de una línea
		/// </summary>
		private string GetCss(Program program, InstructionBase instruction, string tagParent)
		{
			string result = "";

				// Obtiene el CSS de la instrucción
				if (instruction is InstructionLineCss)
					result = GetLine(program, tagParent, instruction as InstructionLineCss) + Environment.NewLine;
				else if (instruction is InstructionVariableIdentifier)
					AddVariable(program, instruction as InstructionVariableIdentifier);
				else if (instruction is InstructionIfDefined)
					result += CallCheckDefined(instruction as InstructionIfDefined, tagParent);
				else if (instruction is InstructionMedia)
					result += GetMediaLine(program, tagParent, instruction as InstructionMedia) + Environment.NewLine;
				else if (instruction is InstructionComment)
					result += GetComment(instruction.Token.Value) + Environment.NewLine;
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Añade una variable a la tabla de símbolos de un programa / función
		/// </summary>
		private void AddVariable(Program program, InstructionVariableIdentifier instruction)
		{
			if (instruction != null && instruction.Value.Count > 0)
				program.SymbolsTable.Variables.Add(instruction.Variable.Name, instruction.Value[0].Token.Value);
		}

		/// <summary>
		///		Escribe una línea @media
		/// </summary>
		private string GetMediaLine(Program program, string tagParent, InstructionMedia instruction)
		{
			string line = instruction.Token.Value;

				// Añade los parámetros a la línea
				line += " " + instruction.Parameters + Environment.NewLine + "{";
				// Añade el contenido de la línea
				if (instruction.Line != null)
					line += GetLine(program, null, instruction.Line);
				// Devuelve a línea creada
				return line + Environment.NewLine + "}";
		}

		/// <summary>
		///		Escribe las líneas CSS
		/// </summary>
		private string GetLine(Program program, string tagParent, InstructionLineCss lineCss)
		{
			string tag = CombineTags(tagParent, GetTagsCss(program, lineCss));
			string line = "";
			List<string> lineChilds = new List<string>();

				// Recorre las líneas hija
				line = GetContentChildSentences(program, lineCss.Sentences, tag, lineChilds);
				// Devuelve la línea compactada
				return CompactCss(lineCss.Token.Indent, tag, line, lineChilds);
		}

		/// <summary>
		///		Combina las etiquetas padre con las hija
		/// </summary>
		private string CombineTags(string tagsParent, string tagsChild)
		{
			string target = "";

				// Combina las etiquetas
				if (tagsParent.IsEmpty())
					target = tagsChild;
				else if (tagsChild.IsEmpty())
					target = tagsParent;
				else
				{
					string[] tagsParentParts = tagsParent.Split(',');
					string[] tagsChildParts = tagsChild.Split(',');

					foreach (string parent in tagsParentParts)
						for (int index = 0; index < tagsChildParts.Length; index++)
						{
							string result = parent.TrimIgnoreNull();

								// Quita los espacios
								tagsChildParts[index] = tagsChildParts[index].TrimIgnoreNull();
								// Añade la cadena al resultado
								if (!tagsChildParts[index].IsEmpty())
								{	
									// Si no comienza por dos puntos, añade un espacio
									if (!tagsChildParts[index].StartsWith(":"))
										result += " ";
									// Añade la cadena hija
									result += tagsChildParts[index];
								}
								// Añade el resultado a la cadena destino
								target = target.AddWithSeparator(result, ",");
						}
				}
				// Devuelve el resultado
				return target;
		}

		/// <summary>
		///		Obtiene el contenido de una serie de líneas
		/// </summary>
		private string GetContentChildSentences(Program program, InstructionsBaseCollection instructions, string tag, List<string> lineChilds)
		{
			string line = "";

				// Recorre las instrucciones
				foreach (InstructionBase instruction in instructions)
					if (instruction is InstructionComment)
						line += GetComment(instruction.Token.Value) + Environment.NewLine;
					else if (instruction is InstructionMixinCall)
						line += CallFunction(instruction as InstructionMixinCall, tag, lineChilds) + Environment.NewLine;
					else if (instruction is InstructionIfDefined)
						line += CallCheckDefined(instruction as InstructionIfDefined, tag) + Environment.NewLine;
					else if (instruction is InstructionLineCss)
					{
						InstructionLineCss child = instruction as InstructionLineCss;

							if (child != null)
							{
								if (child.Sentences != null && child.Sentences.Count > 0)
									lineChilds.Add(GetLine(program, tag, child));
								else
								{
									string tagChild = GetTagsCss(program, child).TrimIgnoreNull();

										// Añade un punto y coma si es necesario
										if (!tagChild.EndsWith(";"))
											tagChild += ";";
										// Añade las etiquetas de la línea
										line += GetIndent(child.Token.Indent + 1) + tagChild + Environment.NewLine;
								}
							}
					}
				// Devuelve la línea
				return line;
		}

		/// <summary>
		///		Compacta una cadena con líneas CSS
		/// </summary>
		private string CompactCss(int indent, string tag, string line, List<string> lineChilds)
		{
			string result = "";

				// Quita los espacios
				line = line.TrimIgnoreNull();
				// Añade los datos de la línea
				if (!line.IsEmpty())
				{
					result = GetIndent(indent) + tag + Environment.NewLine;
					result += GetIndent(indent + 1) + "{ ";
					result += line + Environment.NewLine;
					result += GetIndent(indent + 1) + "}" + Environment.NewLine;
				}
				// Añade las líneas internas
				foreach (string lineChild in lineChilds)
					if (!lineChild.TrimIgnoreNull().IsEmpty())
						result += lineChild + Environment.NewLine;
				// Devuelve la cadena
				return result;
		}

		/// <summary>
		///		Ejecuta una llamada a una función
		/// </summary>
		private string CallFunction(InstructionMixinCall function, string tagParent, List<string> lines)
		{
			string result = "";

				// Obtiene la cadena de la función
				if (function != null)
				{
					Program programFunction = Program.Functions.Search(function.Name);

						if (programFunction != null)
						{
							List<string> lineChilds = new List<string>();

								// Borra la tabla de símbolos
								programFunction.SymbolsTable.Variables.Clear();
								// Añade las variables
								for (int index = 0; index < programFunction.Arguments.Count; index++)
									if (function.Parameters.Count > index)
									{
										string value = function.Parameters[index].Value;

											// Si es una variable recoge el valor
											if (value.StartsWithIgnoreNull("$"))
												value = GetVariableValue(programFunction, value);
											// Añade la variable
											programFunction.SymbolsTable.Variables.Add(programFunction.Arguments[index], value);
									}
								// Obtiene el contenido de las líneas hija
								result = GetContentChildSentences(programFunction, programFunction.Sentences, tagParent, lineChilds);
								// Añade las líneas hija
								lines.AddRange(lineChilds);
						}
				}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Ejecuta las sentencias IfDefined
		/// </summary>
		private string CallCheckDefined(InstructionIfDefined instruction, string tagParent)
		{
			string result = "";

				// Comprueba si está definida la variable
				if (instruction != null)
				{
					string value = Program.SymbolsTable.Variables.Search(instruction.Identifier).Value.Content;

						if (!value.IsEmpty() && !value.EqualsIgnoreCase("null") && value != "")
						{ 
							// Obtiene el contenido de las líneas hija
							foreach (InstructionBase objLine in instruction.SentencesIf)
								if (objLine is InstructionLineCss)
									result += GetTagsCss(Program, objLine as InstructionLineCss) + ";";
								else
									result += GetCss(Program, objLine, tagParent);
						}
				}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Escribe la línea
		/// </summary>
		private void WriteLine(Program program, InstructionLineCss line)
		{
			_generator.AppendLine(GetIndent(line.Token.Indent) + GetTagsCss(program, line));
		}

		/// <summary>
		///		Obtiene las etiquetas Css de una línea
		/// </summary>
		private string GetTagsCss(Program program, InstructionLineCss line)
		{
			string tag = "";

				// Recoge las etiquetas
				foreach (TokenSmallCss token in line.Tokens)
					if (token.TypeCss == TokenSmallCss.TokenCssType.Comment)
						tag += GetComment(token.Value);
					else if (token.TypeCss == TokenSmallCss.TokenCssType.Literal)
					{
						if (token.Value.IsEmpty()) // ... por el problema de los tokens con "" que aparecen vacíos
							tag += "\"\"";
						else
							tag += token.Value + " ";
					}
					else if (token.TypeCss == TokenSmallCss.TokenCssType.Variable)
						tag += GetVariableValue(program, token) + " ";
				// Devuelve la cadena
				return tag;
		}

		/// <summary>
		///		Obtiene una cadena de comentario
		/// </summary>
		private string GetComment(string comment)
		{
			if (Minimize)
				return " ";
			else
				return "/*" + comment + "*/ ";
		}

		/// <summary>
		///		Obtiene el valor de una variable
		/// </summary>
		private string GetVariableValue(Program program, TokenSmallCss token)
		{
			return GetVariableValue(program, token.Value);
		}

		/// <summary>
		///		Obtiene el valor de una variable
		/// </summary>
		private string GetVariableValue(Program program, string variable)
		{
			string value = program.SymbolsTable.Variables.Search(variable).Value.Content;

				// Si no ha encontrado el valor en la tabla de símbolos busca en el padre
				if ((value.IsEmpty() || value.EqualsIgnoreCase("null")) && !program.ID.EqualsIgnoreCase(Program.ID))
					value = GetVariableValue(Program, variable);
				// Devuelve el valor de la variable
				return value;
		}

		/// <summary>
		///		Escribe la indentación
		/// </summary>
		private string GetIndent(int indent)
		{
			string line = "";

				// Escribe la indentación
				if (!Minimize)
					for (int index = 0; index < indent; index++)
						line += '\t';
				else
					line = " ";
				// Devuelve la línea
				return line;
		}

		/// <summary>
		///		Programa que se está interpretando
		/// </summary>
		internal Program Program { get; }

		/// <summary>
		///		Indica si se debe minimizar la salida
		/// </summary>
		internal bool Minimize { get; }
	}
}
