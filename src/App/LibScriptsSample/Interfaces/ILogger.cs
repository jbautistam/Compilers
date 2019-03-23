using System;

namespace Bau.Libraries.LibScriptsSample.Interfaces
{
	/// <summary>
	///		Interface para tratamiento de log
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		///		Escribe un mensaje informativo
		/// </summary>
		void WriteInfo(string message);

		/// <summary>
		///		Escribe un mensaje de depuración
		/// </summary>
		void WriteDebug(string message);

		/// <summary>
		///		Escribe un mensaje de advertencia
		/// </summary>
		void WriteWarning(string message);

		/// <summary>
		///		Escribe un mensaje de error
		/// </summary>
		void WriteError(string message, Exception exception = null);

		/// <summary>
		///		Escribe un error
		/// </summary>
		void WriteError(Exception exception);
	}
}
