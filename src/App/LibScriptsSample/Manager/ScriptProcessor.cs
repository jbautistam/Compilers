using System;
using System.Collections.Generic;

using Bau.Libraries.Compiler.LibInterpreter.Processor;
using Bau.Libraries.Compiler.LibInterpreter.Processor.Sentences;

namespace Bau.Libraries.LibScriptsSample.Manager
{
	/// <summary>
	///		Procesador del script
	/// </summary>
	internal class ScriptProcessor : ProgramProcessor
	{
		internal ScriptProcessor(JobContextModel jobContext)
		{
			JobContext = jobContext;
		}

		/// <summary>
		///		Procesa el programa
		/// </summary>
		internal void Process(string fileName, Dictionary<string, object> parameters)
		{
			Models.ProgramModel program = new Repository.ScriptRepository().LoadByFile(fileName);

				// Inicializa el generador base
				Initialize(CreateParser(), "{{", "}}");
				// Añade las variables iniciales
				foreach (KeyValuePair<string, object> parameter in parameters)
					Context.Actual.VariablesTable.Add(parameter.Key, parameter.Value);
				Context.Actual.VariablesTable.Add("Today", DateTime.Now);
				// Ejecuta el programa
				if (program.Sentences.Count == 0)
					AddError($"Can't find sentences at {fileName}");
				else
					Execute(program.Sentences);
		}

		/// <summary>
		///		Crea el analizador léxico
		/// </summary>
		private Parser CreateParser()
		{
			Parser parser = new Parser();

				// Inicializa el analizador léxico
				parser.Initialize();
				// Devuelve el analizador léxico
				return parser;
		}

		/// <summary>
		///		Ejecuta una serie de sentencias
		/// </summary>
		protected override void Execute(SentenceBase abstractSentence)
		{
			AddError($"Sentence unknown: {abstractSentence}");
		}

		/// <summary>
		///		Añade un mensaje de depuración
		/// </summary>
		protected override void AddDebug(string message)
		{
			JobContext.WriteDebug(message);
		}

		/// <summary>
		///		Añade una salida a la consola
		/// </summary>
		protected override void AddConsoleOutput(string message)
		{
			JobContext.WriteConsole(message);
		}

		/// <summary>
		///		Añade un mensaje informativo
		/// </summary>
		protected override void AddInfo(string message)
		{
			JobContext.WriteInfo(message);
		}

		/// <summary>
		///		Añade un error y detiene la compilación si es necesario
		/// </summary>
		protected override void AddError(string error)
		{
			// Añade el mensaje de error
			JobContext.WriteError(error);
			// Detiene la compilación
			Stopped = true;
		}

		/// <summary>
		///		Contexto de ejecución del trabajo
		/// </summary>
		private JobContextModel JobContext { get; }
	}
}
