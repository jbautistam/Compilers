using System;
using System.Text;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.NhamlCompiler.Parser.Writer
{
	/// <summary>
	///		Clase para generación de HTML
	/// </summary>
	internal class StringBuilderHtml
	{ 
		// Variables privadas
		private string[] tagsAutoClose = { "img", "br", "hr", "meta", "link" };
		private string[] tagsNoWrite = { "NoTag" };

		internal StringBuilderHtml(bool isCompressed = false)
		{
			IsCompressed = isCompressed;
			Indent = 0;
		}

		/// <summary>
		///		Añade un texto
		/// </summary>
		internal void Add(string text)
		{
			Builder.Append(text);
		}

		/// <summary>
		///		Añade una etiqueta
		/// </summary>
		internal void AddTag(string text, bool end, bool isInner)
		{
			bool isAutoEnd;

				// Normaliza la etiqueta
				text = (text ?? "").Trim();
				// Comprueba si es una etiqueta de autocierre
				isAutoEnd = CheckIsAutoEnd(text);
				// Añade la indentación
				if (!isInner && (!end || (end && !isAutoEnd)))
					AddIndent();
				else if (isInner && !end)
					Add(" ");
				// Añade la etiqueta de apertura o cierre
				if (!CheckIsTagNoWritable(text))
				{
					if (end)
					{
						if (!isAutoEnd)
							Add($"</{text}> ");
					}
					else
						Add($"<{text}>");
				}
		}

		/// <summary>
		///		Añade la indentación
		/// </summary>
		internal void AddIndent(bool isfixed = false)
		{
			if (!IsCompressed || isfixed)
			{ 
				// Añade el salto de línea
				Add(Environment.NewLine);
				// Añade la tabulación
				Add(new string('\t', Indent));
			}
		}

		/// <summary>
		///		Comprueba si es una etiqueta de autocierre
		/// </summary>
		private bool CheckIsAutoEnd(string text)
		{ 
			// Comprueba si es una etiqueta autocerrada
			foreach (string tag in tagsAutoClose)
				if (text.Equals(tag, StringComparison.CurrentCultureIgnoreCase) ||
						text.StartsWith(tag + " ", StringComparison.CurrentCultureIgnoreCase))
					return true;
			// Si ha llegado hasta aquí es porque no es una etiqueta autocerrada
			return false;
		}

		/// <summary>
		///		Comprueba si es una etiqueta que no se debe imprimir
		/// </summary>
		private bool CheckIsTagNoWritable(string text)
		{ 
			// Comprueba si está entre las etiquetas no imprimibles
			foreach (string tag in tagsNoWrite)
				if (text.EqualsIgnoreCase(tag))
					return true;
			// Si ha llegado hasta aquí es porque no es una etiqueta que o se debe imprimir
			return false;
		}

		/// <summary>
		///		Cadena generada
		/// </summary>
		internal StringBuilder Builder { get; } = new StringBuilder();

		/// <summary>
		///		Indica si la generación de datos se comprime
		/// </summary>
		internal bool IsCompressed { get; }

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; set; }
	}
}
