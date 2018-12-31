using System;
using System.Collections.Generic;
using System.Linq;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.NhamlCompiler.Variables
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
		public void Add(string name, ValueBase value, int index = 0)
		{
			Add(new Variable(name, value, index));
		}

		/// <summary>
		///		Obtiene el índice de una variable
		/// </summary>
		public int IndexOf(string name, int index = 0)
		{ 
			// Normaliza el nombre
			name = Variable.Normalize(name);
			// Recorre la colección
			for (int indexCollection = 0; indexCollection < Count; indexCollection++)
				if (this[indexCollection].Name.EqualsIgnoreCase(name) && this[indexCollection].Index == index)
					return indexCollection;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return -1;
		}

		/// <summary>
		///		Busca una variable
		/// </summary>
		public Variable Search(string name, int index = 0)
		{
			Variable variable = null;

				// Normaliza el nombre
				name = Variable.Normalize(name);
				// Obtiene el primer elemento
				variable = this.FirstOrDefault(objSearchVariable => objSearchVariable.Name.EqualsIgnoreCase(name) &&
																	objSearchVariable.Index == index);
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
		///		Clona una colección de variables
		/// </summary>
		internal VariablesCollection Clone()
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

		/// <summary>
		///		Depura el contenido de las variables
		/// </summary>
		public void Debug()
		{
			foreach (Variable variable in this)
				System.Diagnostics.Debug.WriteLine($"{variable.Name}[{variable.Index}] = {variable.Value.Content}");
		}

		/// <summary>
		///		Elimina una variable por su nombre
		/// </summary>
		public void Remove(string name)
		{
			for (int index = Count - 1; index >= 0; index--)
				if (name.EqualsIgnoreCase(this[index].Name) ||
						("$" + name).EqualsIgnoreCase(this[index].Name))
					RemoveAt(index);
		}
	}
}
