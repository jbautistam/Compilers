using System;

namespace Bau.Applications.CompilerTestConsole
{
	/// <summary>
	///		Clase para manejo de log
	/// </summary>
	internal class Logger : Libraries.LibScriptsSample.Interfaces.ILogger
	{
		/// <summary>
		///		Tipo de log
		/// </summary>
		internal enum LogItemType
		{
			/// <summary>Mensaje de depuración</summary>
			Debug,
			/// <summary>Mensaje informativo</summary>
			Info,
			/// <summary>Mensaje de advertencia</summary>
			Warning,
			/// <summary>Mensaje de error</summary>
			Error
		}

		internal Logger(LogItemType minimumLog)
		{
			MinimumLog = minimumLog;
		}

		/// <summary>
		///		Escribe un mensaje informativo
		/// </summary>
		public void WriteInfo(string message)
		{
			if (MustShow(LogItemType.Info))
				Write(ConsoleColor.White, "INFO: ", message);
		}

		/// <summary>
		///		Escribe un mensaje de depuración
		/// </summary>
		public void WriteDebug(string message)
		{
			if (MustShow(LogItemType.Debug))
				Write(ConsoleColor.Blue, "DEBUG: ", message);
		}

		/// <summary>
		///		Escribe un mensaje de advertencia
		/// </summary>
		public void WriteWarning(string message)
		{
			if (MustShow(LogItemType.Warning))
				Write(ConsoleColor.Gray, "WARNING: ", message);
		}

		/// <summary>
		///		Escribe un mensaje de error
		/// </summary>
		public void WriteError(string message, Exception exception = null)
		{
			if (MustShow(LogItemType.Error))
				Write(ConsoleColor.Red, "ERROR: ", message + Environment.NewLine + exception?.Message);
		}

		/// <summary>
		///		Escribe un mensaje de error
		/// </summary>
		public void WriteError(Exception exception)
		{
			if (MustShow(LogItemType.Error))
				Write(ConsoleColor.Red, "ERROR: ", exception.Message);
		}

		/// <summary>
		///		Escribe un mensaje
		/// </summary>
		private void Write(ConsoleColor color, string header, string message)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(header);
			Console.WriteLine(new string(' ', 5) + message);
		}

		/// <summary>
		///		Indica si se debe mostrar el elemento de log
		/// </summary>
		private bool MustShow(LogItemType itemType)
		{
			return (int) itemType >= (int) MinimumLog;
		}

		/// <summary>
		///		Log mínimo para impresión
		/// </summary>
		internal LogItemType MinimumLog { get; }
	}
}
