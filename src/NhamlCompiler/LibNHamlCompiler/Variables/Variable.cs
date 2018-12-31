using System;

namespace Bau.Libraries.NhamlCompiler.Variables
{
	/// <summary>
	///		Clase base para las variables
	/// </summary>
	public class Variable
	{ 
		// Variables privadas
		private string _name;

		public Variable(string name, ValueBase value = null, int index = 0, int intScope = 0)
		{
			Name = name;
			Value = value;
			Index = index;
			Scope = intScope;
		}

		/// <summary>
		///		Normaliza el nombre de una variable
		/// </summary>
		internal static string Normalize(string name)
		{ 
			// Asigna el valor
			name = (name ?? "").Trim();
			// Normaliza el valor
			if (!name.StartsWith("$"))
				return $"${name}";
			else
				return name;
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
		public string Name
		{
			get { return _name; }
			set { _name = Normalize(value); }
		}

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
