using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Context
{
	/// <summary>
	///		Clase con los datos de contexto
	/// </summary>
	public class ContextModel
	{
		public ContextModel(ContextModel parent)
		{
			Parent = parent;
			if (Parent == null)
				ScopeIndex = 0;
			else
				ScopeIndex = Parent.ScopeIndex + 1;
			Variables = new Variables.TableVariableModel(this);
		}

		/// <summary>
		///		Contexto padre
		/// </summary>
		public ContextModel Parent { get; }

		/// <summary>
		///		Variables
		/// </summary>
		public Variables.TableVariableModel Variables { get; }

		/// <summary>
		///		Indice del ámbito
		/// </summary>
		public int ScopeIndex { get; }
	}
}
