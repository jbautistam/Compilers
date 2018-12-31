using System;

namespace Bau.Libraries.Compiler.LibInterpreter.Expressions
{
	/// <summary>
	///		Base para las expresiones de operación
	/// </summary>
    public abstract class ExpressionOperatorBase : ExpressionBase
    {
		/// <summary>
		///		Prioridad
		/// </summary>
		public abstract int Priority { get; }
    }
}
