using System;

namespace Bau.Libraries.NhamlCompiler.EventArgs
{
	/// <summary>
	///		Argumentos de los eventos de depuración
	/// </summary>
	public class DebugEventArgs : System.EventArgs
	{
		/// <summary>
		///		Modo desde el que se envían los eventos de depuración
		/// </summary>
		public enum Mode
		{
			Unknown,
			Tokenizer,
			Instructions
		}

		public DebugEventArgs(Mode mode, string title, string message)
		{
			DebugMode = mode;
			Title = title;
			Message = message;
		}

		/// <summary>
		///		Modo
		/// </summary>
		public Mode DebugMode { get; }

		/// <summary>
		///		Título para la depuración
		/// </summary>
		public string Title { get; }

		/// <summary>
		///		Mensaje de depuración
		/// </summary>
		public string Message { get; }
	}
}
