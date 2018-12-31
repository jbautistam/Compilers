using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibMarkupLanguage.Services.XML;

namespace Bau.Libraries.LibTokenizer.Lexical.Rules.Repository
{
	/// <summary>
	///		Repository de <see cref="RuleBase"/>
	/// </summary>
	internal class RuleRepository
	{ 
		// Constantes privadas
		private const string TagRoot = "RuleSet";
		private const string TagRuleDelimited = "RuleDelimited";
		private const string TagRulePattern = "RulePattern";
		private const string TagRuleWord = "RuleWord";
		private const string TagRuleWordFixed = "RuleWordFixed";
		private const string TagType = "Type";
		private const string TagSubType = "SubType";
		private const string TagKeyWord = "KeyWord";
		private const string TagPatternStart = "PatternStart";
		private const string TagPatternContent = "PatternContent";
		private const string TagToEndLine = "ToEndLine";
		private const string TagToFirstSpace = "ToFirstSpace";
		private const string TagIncludeStart = "IncludeStart";
		private const string TagIncludeEnd = "IncludeEnd";
		private const string TagMustTrim = "MustTrim";
		private const string TagStart = "Start";
		private const string TagEnd = "End";

		/// <summary>
		///		Lee un archivo de definiciones
		/// </summary>
		internal RuleCollection Load(string fileName)
		{
			RuleCollection rules = new RuleCollection();
			MLFile fileML = new XMLParser().Load(fileName);

				// Carga los datos
				if (fileML != null)
					foreach (MLNode nodeML in fileML.Nodes)
						if (nodeML.Name == TagRoot)
							foreach (MLNode childML in nodeML.Nodes)
								switch (childML.Name)
								{
									case TagRuleDelimited:
											rules.Add(GetRuleDelimited(childML));
										break;
									case TagRulePattern:
											rules.Add(GetRulePattern(childML));
										break;
									case TagRuleWord:
											rules.Add(GetRuleWord(childML));
										break;
									case TagRuleWordFixed:
											rules.Add(GetRuleWordFixed(childML));
										break;
								}
				// Devuelve la colección de reglas
				return rules;
		}

		/// <summary>
		///		Obtiene los datos de una regla fija
		/// </summary>
		private RuleBase GetRuleWordFixed(MLNode nodeML)
		{
			RuleWordFixed rule = new RuleWordFixed(Tokens.Token.TokenType.Unknown, null, null);

				// Obtiene los parámetros básicos
				AssignBasicParameters(nodeML, rule);
				// Obtiene las palabras claves y los separadores
				rule.Words = LoadSeparators(nodeML, TagKeyWord);
				// Devuelve la regla
				return rule;
		}

		/// <summary>
		///		Obtiene una regla de palabra clave
		/// </summary>
		private RuleBase GetRuleWord(MLNode nodeML)
		{
			RuleWord rule = new RuleWord(Tokens.Token.TokenType.Unknown, null, null, null, false);

				// Obtiene los parámetros básicos
				AssignBasicParameters(nodeML, rule);
				// Obtiene las palabras claves y los separadores
				rule.Words = LoadSeparators(nodeML, TagKeyWord);
				rule.Separators = LoadSeparators(nodeML, TagEnd);
				// Devuelve la regla
				return rule;
		}

		/// <summary>
		///		Obtiene una regla de patrón
		/// </summary>
		private RuleBase GetRulePattern(MLNode nodeML)
		{
			RulePattern rule = new RulePattern(Tokens.Token.TokenType.Unknown, null, null, null);

				// Obtiene los parámetros básicos
				AssignBasicParameters(nodeML, rule);
				// Obtiene los patrones
				rule.PatternStart = nodeML.Attributes[TagPatternStart].Value;
				rule.PatternContent = nodeML.Attributes[TagPatternContent].Value;
				// Devuelve la regla
				return rule;
		}

		/// <summary>
		///		Obtiene una regla delimitada
		/// </summary>
		private RuleBase GetRuleDelimited(MLNode nodeML)
		{
			RuleDelimited rule = new RuleDelimited(Tokens.Token.TokenType.Unknown, null, null, null, false, false, false, false);

				// Asigna los parámetros básicos
				AssignBasicParameters(nodeML, rule);
				// Asigna los parámetros especiales de este tipo de regla
				rule.ToEndLine = nodeML.Attributes[TagToEndLine].Value.GetBool();
				rule.Starts = LoadSeparators(nodeML, TagStart);
				rule.Ends = LoadSeparators(nodeML, TagEnd);
				rule.IncludeStart = nodeML.Attributes[TagIncludeStart].Value.GetBool(true);
				rule.IncludeEnd = nodeML.Attributes[TagIncludeEnd].Value.GetBool(true);
				// Devuelve la regla
				return rule;
		}

		/// <summary>
		///		Asigna los parámetros básicos a una regla
		/// </summary>
		private void AssignBasicParameters(MLNode nodeML, RuleBase rule)
		{
			rule.Type = GetType(nodeML.Attributes[TagType].Value);
			rule.SubType = nodeML.Attributes[TagSubType].Value.GetInt();
			rule.MustTrim = nodeML.Attributes[TagMustTrim].Value.GetBool(true);
			rule.ToFirstSpace = nodeML.Attributes[TagToFirstSpace].Value.GetBool();
		}

		/// <summary>
		///		Obtiene el valor del enumerado
		/// </summary>
		private Tokens.Token.TokenType GetType(string value)
		{
			return value.GetEnum(Tokens.Token.TokenType.Unknown);
		}

		/// <summary>
		///		Carga los separadores de inicio o fin
		/// </summary>
		private string[] LoadSeparators(MLNode nodeML, string tag)
		{
			List<string> separators = new List<string>();

				// Carga los separadores
				foreach (MLNode childML in nodeML.Nodes)
					if (childML.Name == tag)
						separators.Add(childML.Value);
				// Devuelve los separadores
				if (separators.Count == 0)
					return null;
				else
					return separators.ToArray();
		}

		/// <summary>
		///		Graba un archivo de definiciones
		/// </summary>
		internal void Save(RuleCollection rules, string fileName)
		{
			MLFile fileML = new MLFile();
			MLNode objMLRoot = fileML.Nodes.Add(TagRoot);

				// Crea los nodos
				foreach (RuleBase rule in rules)
					if (rule is RuleWord)
						objMLRoot.Nodes.Add(GetNode(rule as RuleWord));
					else if (rule is RuleDelimited)
						objMLRoot.Nodes.Add(GetNode(rule as RuleDelimited));
					else if (rule is RulePattern)
						objMLRoot.Nodes.Add(GetNode(rule as RulePattern));
					else if (rule is RuleWordFixed)
						objMLRoot.Nodes.Add(GetNode(rule as RuleWordFixed));
					else
						throw new NotImplementedException("Tipo de regla desconocida");
				// Graba el archivo
				new XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el nodo de una regla de palabra clave
		/// </summary>
		private MLNode GetNode(RuleWord rule)
		{
			MLNode nodeML = GetNodeBase(TagRuleWord, rule);

				// Asigna los atributos
				AddNodesSeparator(nodeML, rule.Words, TagKeyWord);
				AddNodesSeparator(nodeML, rule.Separators, TagEnd);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el nodo de una regla de palabra clave de longitud fija
		/// </summary>
		private MLNode GetNode(RuleWordFixed rule)
		{
			MLNode nodeML = GetNodeBase(TagRuleWordFixed, rule);

				// Asigna los atributos
				AddNodesSeparator(nodeML, rule.Words, TagKeyWord);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el nodo de una regla de delimitada
		/// </summary>
		private MLNode GetNode(RuleDelimited rule)
		{
			MLNode nodeML = GetNodeBase(TagRuleDelimited, rule);

				// Añade los atributos
				nodeML.Attributes.Add(TagToEndLine, rule.ToEndLine);
				nodeML.Attributes.Add(TagIncludeStart, rule.IncludeStart);
				nodeML.Attributes.Add(TagIncludeEnd, rule.IncludeEnd);
				// Añade los parámetros de inicio y fin
				AddNodesSeparator(nodeML, rule.Starts, TagStart);
				AddNodesSeparator(nodeML, rule.Ends, TagEnd);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el nodo de una regla de patrón
		/// </summary>
		private MLNode GetNode(RulePattern rule)
		{
			MLNode nodeML = GetNodeBase(TagRulePattern, rule);

				// Asigna los atributos
				nodeML.Attributes.Add(TagPatternStart, rule.PatternStart);
				nodeML.Attributes.Add(TagPatternContent, rule.PatternContent);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene un nodo con los datos básicos de una regla
		/// </summary>
		private MLNode GetNodeBase(string tag, RuleBase rule)
		{
			MLNode nodeML = new MLNode(tag);

				// Añade los atributos básicos
				nodeML.Attributes.Add(TagType, rule.Type.ToString());
				if (rule.SubType != null)
					nodeML.Attributes.Add(TagSubType, rule.SubType.ToString());
				nodeML.Attributes.Add(TagMustTrim, rule.MustTrim);
				nodeML.Attributes.Add(TagToFirstSpace, rule.ToFirstSpace);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene los nodos de cadenas de inicio y fin
		/// </summary>
		private void AddNodesSeparator(MLNode nodeML, string[] separators, string tag)
		{
			if (separators != null)
				foreach (string strSeparator in separators)
					nodeML.Nodes.Add(tag, strSeparator);
		}
	}
}
