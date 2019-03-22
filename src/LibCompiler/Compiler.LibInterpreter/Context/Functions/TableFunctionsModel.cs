using System;
using System.Collections.Generic;

namespace Bau.Libraries.Compiler.LibInterpreter.Context.Functions
{
	/// <summary>
	///		Tabla de funciones
	/// </summary>
	public class TableFunctionsModel
	{
		public TableFunctionsModel(ContextModel context)
		{
			Context = context;
		}

		/// <summary>
		///		Añade una función
		/// </summary>
		public void Add(string name, Processor.Sentences.SentenceFunction function)
		{
			// Normaliza el nombre
			name = Normalize(name);
			// Añade / modifica el valor
			if (Functions.ContainsKey(name))
				Functions[name] = function;
			else
				Functions.Add(name, function);
		}

		/// <summary>
		///		Obtiene una función
		/// </summary>
		public Processor.Sentences.SentenceFunction GetIfExists(string name)
		{
			// Normaliza el nombre
			name = Normalize(name);
			// Obtiene el valor
			if (Functions.ContainsKey(name))
				return Functions[name];
			else if (Context.Parent != null)
				return Context.Parent.FunctionsTable.GetIfExists(name);
			else
				return null;
		}

		/// <summary>
		///		Normaliza el nombre de la variable
		/// </summary>
		private string Normalize(string name)
		{
			return name.ToUpper();
		}

		/// <summary>
		///		Indizador
		/// </summary>
		public Processor.Sentences.SentenceFunction this[string name]
		{
			get { return GetIfExists(name); }
			set { Add(name, value); }
		}

		/// <summary>
		///		Contexto
		/// </summary>
		private ContextModel Context { get; }

		/// <summary>
		///		Diccionario de funciones
		/// </summary>
		private Dictionary<string, Processor.Sentences.SentenceFunction> Functions { get; } = new Dictionary<string, Processor.Sentences.SentenceFunction>();
	}
}
