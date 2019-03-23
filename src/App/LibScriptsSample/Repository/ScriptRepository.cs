using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.Compiler.LibInterpreter.Context.Variables;
using Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences;
using Bau.Libraries.LibScriptsSample.Models;

namespace Bau.Libraries.LibScriptsSample.Repository
{
	/// <summary>
	///		Clase de lectura de los repositorios para los scripts
	/// </summary>
	internal class ScriptRepository
	{
		// Constantes privadas
		private const string TagRoot = "Script";
		private const string TagImport = "Import";
		private const string TagFileName = "FileName";
		private const string TagSentenceException = "Exception";
		private const string TagType = "Type";
		private const string TagName = "Name";
		private const string TagValue = "Value";
		private const string TagSentenceIf = "If";
		private const string TagCondition = "Condition";
		private const string TagThen = "Then";
		private const string TagElse = "Else";
		private const string TagSentenceString = "String";
		private const string TagSentenceNumeric = "Numeric";
		private const string TagSentenceBoolean = "Boolean";
		private const string TagSentenceDate = "Date";
		private const string TagDateNow = "Now";
		private const string TagSentenceLet = "Let";
		private const string TagVariable = "Variable";
		private const string TagSentenceFor = "For";
		private const string TagSentenceWhile = "While";
		private const string TagStart = "Start";
		private const string TagEnd = "End";
		private const string TagStep = "Step";
		private const string TagSentencePrint = "Print";
		private const string TagSentenceFunction = "Function";
		private const string TagArguments = "Arguments";
		private const string TagSentenceReturn = "Return";
		private const string TagSentenceCall = "Call";
		private const string TagResult = "Result";
		private const string TagParameter = "Parameter";

		/// <summary>
		///		Carga el programa de un archivo
		/// </summary>
		internal ProgramModel LoadByFile(string fileName)
		{
			return Load(new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName), System.IO.Path.GetDirectoryName(fileName));
		}

		/// <summary>
		///		Carga el programa de un texto
		/// </summary>
		internal ProgramModel LoadByText(string xml, string pathBase)
		{
			return Load(new LibMarkupLanguage.Services.XML.XMLParser().ParseText(xml), pathBase);
		}

		/// <summary>
		///		Carga el programa
		/// </summary>
		private ProgramModel Load(MLFile fileML, string pathBase)
		{
			ProgramModel program = new ProgramModel();

				// Carga las sentencias del programa
				if (fileML != null)
					foreach (MLNode rootML in fileML.Nodes)
						if (rootML.Name == TagRoot)
							program.Sentences.AddRange(LoadSentences(rootML.Nodes, pathBase));
				// Devuelve el programa cargado
				return program;
		}

		/// <summary>
		///		Carga las instrucciones de una serie de nodos
		/// </summary>
		private SentenceCollection LoadSentences(MLNodesCollection nodesML, string pathBase)
		{
			SentenceCollection sentences = new SentenceCollection();

				// Lee las instrucciones
				foreach (MLNode rootML in nodesML)
					switch (rootML.Name)
					{
						case TagImport:
								sentences.AddRange(LoadByFile(System.IO.Path.Combine(pathBase, rootML.Attributes[TagFileName].Value)).Sentences);
							break;
						case TagSentenceException:
								sentences.Add(LoadSentenceException(rootML));
							break;
						case TagSentenceIf:
								sentences.Add(LoadSentenceIf(rootML, pathBase));
							break;
						case TagSentenceString:
								sentences.Add(LoadSentenceDeclare(rootML, VariableModel.VariableType.String));
							break;
						case TagSentenceNumeric:
								sentences.Add(LoadSentenceDeclare(rootML, VariableModel.VariableType.Numeric));
							break;
						case TagSentenceBoolean:
								sentences.Add(LoadSentenceDeclare(rootML, VariableModel.VariableType.Boolean));
							break;
						case TagSentenceDate:
								sentences.Add(LoadSentenceDeclare(rootML, VariableModel.VariableType.Date));
							break;
						case TagSentenceLet:
								sentences.Add(LoadSentenceLet(rootML));
							break;
						case TagSentenceFor:
								sentences.Add(LoadSentenceFor(rootML, pathBase));
							break;
						case TagSentenceWhile:
								sentences.Add(LoadSentenceWhile(rootML, pathBase));
							break;
						case TagSentencePrint:
								sentences.Add(LoadSentencePrint(rootML));
							break;
						case TagSentenceFunction:
								sentences.Add(LoadSentenceFunction(rootML, pathBase));
							break;
						case TagSentenceReturn:
								sentences.Add(LoadSentenceReturn(rootML));
							break;
						case TagSentenceCall:
								sentences.Add(LoadSentenceCall(rootML));
							break;
					}
				// Devuelve la colección
				return sentences;
		}

		/// <summary>
		///		Carga una sentencia de impresión
		/// </summary>
		private SentenceBase LoadSentencePrint(MLNode rootML)
		{
			return new SentencePrint
							{
								Message = rootML.Value
							};
		}

		/// <summary>
		///		Carga una sentencia de declaración de variables
		/// </summary>
		private SentenceBase LoadSentenceDeclare(MLNode rootML, VariableModel.VariableType type)
		{
			SentenceDeclare sentence = new SentenceDeclare();

				// Asigna las propiedades
				sentence.Type = type;
				sentence.Name = rootML.Attributes[TagName].Value;
				if (!string.IsNullOrWhiteSpace(rootML.Attributes[TagValue].Value))
					sentence.Value = ConvertStringValue(type, rootML.Attributes[TagValue].Value);
				else
					sentence.Value = ConvertStringValue(type, rootML.Value);
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Carga una sentencia de asignación
		/// </summary>
		private SentenceBase LoadSentenceLet(MLNode rootML)
		{
			SentenceLet sentence = new SentenceLet();

				// Asigna las propiedades
				sentence.Variable = rootML.Attributes[TagVariable].Value;
				sentence.Type = rootML.Attributes[TagType].Value.GetEnum(VariableModel.VariableType.Unknown);
				sentence.Expression = rootML.Value;
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Carga una sentencia for
		/// </summary>
		private SentenceBase LoadSentenceFor(MLNode rootML, string pathBase)
		{
			SentenceFor sentence = new SentenceFor();

				// Asigna las propiedades
				sentence.Variable = rootML.Attributes[TagVariable].Value;
				sentence.StartExpression = rootML.Attributes[TagStart].Value;
				sentence.EndExpression = rootML.Attributes[TagEnd].Value;
				sentence.StepExpression = rootML.Attributes[TagStep].Value;
				// Carga las sentencias
				sentence.Sentences.AddRange(LoadSentences(rootML.Nodes, pathBase));
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Carga los datos de una sentencia de excepción
		/// </summary>
		private SentenceBase LoadSentenceException(MLNode rootML)
		{
			return new SentenceException
							{
								Message = rootML.Value
							};
		}

		/// <summary>
		///		Carga una sentencia If
		/// </summary>
		private SentenceBase LoadSentenceIf(MLNode rootML, string pathBase)
		{
			SentenceIf sentence = new SentenceIf();

				// Carga la condición
				sentence.Condition = rootML.Attributes[TagCondition].Value;
				// Carga las sentencias de la parte then y else
				foreach (MLNode nodeML in rootML.Nodes)
					switch (nodeML.Name)
					{
						case TagThen:
								sentence.SentencesThen.AddRange(LoadSentences(nodeML.Nodes, pathBase));
							break;
						case TagElse:
								sentence.SentencesElse.AddRange(LoadSentences(nodeML.Nodes, pathBase));
							break;
					}
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Carga una sentencia While
		/// </summary>
		private SentenceBase LoadSentenceWhile(MLNode rootML, string pathBase)
		{
			SentenceWhile sentence = new SentenceWhile();

				// Carga la condición y las sentencias
				sentence.Condition = rootML.Attributes[TagCondition].Value;
				sentence.Sentences.AddRange(LoadSentences(rootML.Nodes, pathBase));
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Carga una sentencia de declaración de una función
		/// </summary>
		private SentenceBase LoadSentenceFunction(MLNode rootML, string pathBase)
		{
			SentenceFunction sentence = new SentenceFunction();

				// Carga la condición y las sentencias
				sentence.Name = rootML.Attributes[TagName].Value;
				sentence.Type = rootML.Attributes[TagType].Value.GetEnum(VariableModel.VariableType.Unknown);
				// Añade los argumentos y las sentencias
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagArguments)
						foreach (SentenceBase sentenceBase in LoadSentences(nodeML.Nodes, pathBase))
							if (sentenceBase is SentenceDeclare sentenceDeclare)
							{
								if (sentenceDeclare.Value != null)
									sentence.Arguments.Add(new VariableModel(sentenceDeclare.Name, sentenceDeclare.Value));
								else
									sentence.Arguments.Add(new VariableModel(sentenceDeclare.Name, sentenceDeclare.Type));
							}
				// Añade las sentencias
				sentence.Sentences.AddRange(LoadSentences(rootML.Nodes, pathBase));
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Carga el contenido de la sentencia Return
		/// </summary>
		private SentenceBase LoadSentenceReturn(MLNode rootML)
		{
			return new SentenceReturn
							{
								Expression = rootML.Value
							};
		}

		/// <summary>
		///		Carga una sentencia de llamada a una función
		/// </summary>
		private SentenceBase LoadSentenceCall(MLNode rootML)
		{
			SentenceCallFunction sentence = new SentenceCallFunction();

				// Asigna las propiedades
				sentence.Function = rootML.Attributes[TagName].Value;
				sentence.ResultVariable = rootML.Attributes[TagResult].Value;
				// Asigna los argumentos
				foreach (MLNode nodeML in rootML.Nodes)
					if (nodeML.Name == TagParameter)
						sentence.ParameterExpressions.Add(nodeML.Value);
				// Devuelve la sentencia
				return sentence;
		}

		/// <summary>
		///		Convierte una cadena con un valor
		/// </summary>
		private object ConvertStringValue(VariableModel.VariableType type, string value)
		{
			if (string.IsNullOrEmpty(value))
				return null;
			else
				switch (type)
				{ 
					case VariableModel.VariableType.Boolean:
						return value.GetBool();
					case VariableModel.VariableType.Date:
						if (value.EqualsIgnoreCase(TagDateNow))
							return DateTime.Now;
						else
							return value.GetDateTime();
					case VariableModel.VariableType.Numeric:
						return value.GetDouble();
					default:
						return value;
				}
		}
	}
}
