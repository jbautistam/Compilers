using System;

namespace Bau.Libraries.Compiler.LibTokenizer.Variables
{
	/// <summary>
	///		Clase base para las variables
	/// </summary>
	public class Variable
	{
		public Variable(string name, ValueBase value = null, int index = 0, int scope = 0)
		{
			Name = name;
			Value = value;
			Index = index;
			Scope = scope;
		}

		/// <summary>
		///		Clona el contenido de una variable
		/// </summary>
		internal Variable Clone()
		{
			Variable variable = new Variable(Name, Value, Index, Scope);

				// Clona los miembros
				variable.Members.AddRange(Members.Clone());
				// Devuelve la variable
				return variable;
		}

		/// <summary>
		///		Nombre de la variable
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Valor de la variable
		/// </summary>
		public ValueBase Value { get; set; }

		/// <summary>
		///		Indice de la variable
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		///		Miembros de la variable
		/// </summary>
		public VariablesCollection Members { get; } = new VariablesCollection();

		/// <summary>
		///		Nivel de ámbito
		/// </summary>
		public int Scope { get; set; }
	}
}
