using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.Compiler.LibTokenizer.Variables
{
	/// <summary>
	///		Colección de <see cref="Variable"/>
	/// </summary>
	public class VariablesCollection : List<Variable>
	{
		/// <summary>
		///		Añade una variable
		/// </summary>
		public void Add(string name, string value, int index = 0)
		{
			Variable variable = new Variable(name, ValueBase.GetInstance(value), index);
			int indexFound = IndexOf(name, index);

				if (indexFound >= 0)
					this[indexFound] = variable;
				else
					Add(name, ValueBase.GetInstance(value), index);
		}

		/// <summary>
		///		Añade una variable
		/// </summary>
		public void Add(string name, object value, int index = 0, int scope = 0)
		{
			Variable variable;
			int indexFound = IndexOf(name, index);

				// Asigna la variable
				variable = new Variable(name, ValueBase.GetInstance(value), index, scope);
				// Asigna el valor a la variable en la tabla o lo añade
				if (indexFound >= 0)
					this[indexFound] = variable;
				else
					Add(variable);
		}

		/// <summary>
		///		Añade una variable
		/// </summary>
		public void Add(string name, ValueBase value, int index = 0, int scope = 0)
		{
			Add(new Variable(name, value, index, scope));
		}

		/// <summary>
		///		Obtiene el índice de una variable
		/// </summary>
		public int IndexOf(string name, int index = 0)
		{ 
			// Recorre la colección
			for (int indexCollection = 0; indexCollection < Count; indexCollection++)
				if (this[indexCollection].Name.EqualsIgnoreCase(name) && this[indexCollection].Index == index)
					return indexCollection;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return -1;
		}

		/// <summary>
		///		Comprueba si existe una variable
		/// </summary>
		public bool Exists(string name)
		{
			return IndexOf(name) < 0;
		}

		/// <summary>
		///		Busca una variable
		/// </summary>
		public Variable Search(string name, int index = 0)
		{
			Variable variable = this.FirstOrDefault(item => item.Name.EqualsIgnoreCase(name) &&
															item.Index == index);

				// Si no se ha encontrado ninguna variable, la crea
				if (variable == null)
				{ 
					// Crea la variable
					variable = new Variable(name, ValueBase.GetInstance("null"), index);
					// ... y la añade a la colección
					Add(variable);
				}
				// Devuelve la variable
				return variable;
		}

		/// <summary>
		///		Elimina las variables en determinado ámbito
		/// </summary>
		public void RemoveAtScope(string id, int scope)
		{
			for (int index = Count - 1; index >= 0; index--)
				if (this[index].Name.EqualsIgnoreCase(id) && this[index].Scope == scope)
					RemoveAt(index);
		}

		/// <summary>
		///		Clona una colección de variables
		/// </summary>
		public VariablesCollection Clone()
		{
			VariablesCollection variables = new VariablesCollection();

				// Clona la colección
				foreach (Variable variable in this)
					variables.Add(variable.Clone());
				// Devuelve la colección
				return variables;
		}

		/// <summary>
		///		Ordena las variables por su índice
		/// </summary>
		internal void SortByIndex()
		{
			Sort((first, second) => first.Index.CompareTo(second.Index));
		}
	}
}
