// ---------------------------------------------
// ComplexLogger - by The Illusion
// ---------------------------------------------
// Reusage Rights ------------------------------
// You are free to use this script or portions of it in your own mods, provided you give me credit in your description and maintain this section of comments in any released source code
//
// Warning !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// Ensure you change the namespace to whatever namespace your mod uses, so it doesnt conflict with other mods
// ---------------------------------------------

using MelonLoader.Pastel;

using SaveManager.Utilities.Logger.Enums;
using SaveManager.Utilities.Exceptions;

namespace SaveManager.Utilities.Logger
{
	/// <summary>
	/// 
	/// </summary>
	public class ComplexLogger<T> : BaseLogger<T> where T : MelonBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="levels"></param>
		public ComplexLogger(FlaggedLoggingLevel[]? levels = null)
		{
			Instance = this;

			AddLevel(FlaggedLoggingLevel.None);
			AddLevel(FlaggedLoggingLevel.Exception);

			if (levels == null) return;

			foreach (var level in levels)
			{
				AddLevel(level);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ComplexLogger<T> Instance { get; set; }

		/// <summary>
		/// The current logging level. Levels are bitwise added or removed.
		/// </summary>
		public FlaggedLoggingLevel CurrentLevel { get; private set; } = new();

		/// <summary>
		/// Add a flag to the existing list
		/// </summary>
		/// <param name="level">The level to add</param>
		public bool AddLevel(FlaggedLoggingLevel level)
		{
			if (CurrentLevel.HasFlag(level))
			{
				Log($"Attempting to add already existing level: {level}", FlaggedLoggingLevel.Debug);
				return false;
			}

			CurrentLevel |= level;

			Log($"Added flag {level}", FlaggedLoggingLevel.Debug);
			return true;
		}

		/// <summary>
		/// Remove a flag from the list
		/// </summary>
		/// <param name="level">Level to remove</param>
		/// <remarks>Attempting to remove "<see cref="FlaggedLoggingLevel.None"/>" or "<see cref="FlaggedLoggingLevel.Exception"/>" is not supported</remarks>
		public bool RemoveLevel(FlaggedLoggingLevel level)
		{
			if (level == FlaggedLoggingLevel.None)
			{
				Log("Attempting to remove \"FlaggedLoggingLevel.None\" is not supported", FlaggedLoggingLevel.Debug);
				return false;
			}
			else if (level == FlaggedLoggingLevel.Exception)
			{
				Log("Attempting to remove \"FlaggedLoggingLevel.Exception\" is not supported", FlaggedLoggingLevel.Debug);
				return false;
			}

			CurrentLevel &= ~level;

			Log($"Removed flag {level}", FlaggedLoggingLevel.Debug);
			return true;
		}

		public bool AddOrRemoveLevel(FlaggedLoggingLevel level, bool add)
		{
			if (add)
			{
				return AddLevel(level);
			}
			else
			{
				return RemoveLevel(level);
			}
		}

		// All Log methods should use the following order:
		// message, level, extra**, parameters
		// message is the log contents
		// level is which level this log is to be displayed at
		// extra** are things like exceptions (think args, kwargs from python).
		// parameters must be last due to it being a params object[]

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="level"></param>
		/// <param name="parameters"></param>
		public void Log(string message, FlaggedLoggingLevel level, params object[] parameters)
		{
			Log(message,level, null, LoggingSubType.Normal, parameters);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		/// <param name="level"></param>
		/// <param name="parameters"></param>
		public void Log(string message, FlaggedLoggingLevel level, System.Exception exception, params object[] parameters)
		{
			Log(message, level, exception, LoggingSubType.Normal, parameters);
		}

		/// <summary>
		/// Print a log if the current level matches the level given.
		/// </summary>
		/// <param name="level">The level of this message (NOT the existing the level)</param>
		/// <param name="message">Formatted string to use in this log</param>
		/// <param name="LogSubType">Used to write separators only when the logging level matches the current flags</param>
		/// <param name="exception">The exception, if applicable, to display</param>
		/// <param name="parameters">Any additional params</param>
		/// <remarks>
		/// <para>Use <see cref="WriteSeperator(object[])"/> or <see cref="WriteIntraSeparator(string, object[])"/> for seperators without using the flagged level</para>
		/// <para>There is also <see cref="WriteStarter"/> if you require a prebuild startup message to display regardless of user settings (DONT DO THIS)</para>
		/// </remarks>
		public void Log(string message, FlaggedLoggingLevel level, System.Exception? exception, LoggingSubType LogSubType, params object[] parameters)
		{
			if (LogSubType != LoggingSubType.Normal)
			{
				if (LogSubType == LoggingSubType.Separator)
				{
					WriteSeperator(level);
					return;
				}
				else if (LogSubType == LoggingSubType.IntraSeparator)
				{
					WriteIntraSeparator(level, message);
					return;
				}
			}

			if (CurrentLevel.HasFlag(level))
			{
				switch (level)
				{
					case FlaggedLoggingLevel.Trace:
						Write($"[TRACE] {message}", parameters);
						break;
					case FlaggedLoggingLevel.Debug:
						Write($"[DEBUG] {message}", parameters);
						break;
					case FlaggedLoggingLevel.Verbose:
						Write($"[INFO] {message}", parameters);
						break;
					case FlaggedLoggingLevel.Warning:
						Write($"[WARNING] {message}", [Color.red, parameters]);
						break;
					case FlaggedLoggingLevel.Error:
						Write($"[ERROR] {message}", [Color.red, parameters]);
						break;
					case FlaggedLoggingLevel.Critical:
						Write($"[CRITICAL] {message}", [Color.red, FontStyle.Bold, parameters]);
						break;
					case FlaggedLoggingLevel.Exception:
						WriteException(message, exception);
						break;
					default:
						break;
				}
				return;
			}
			return;
		}

		/// <summary>
		/// Logs a prebuilt startup message
		/// </summary>
		/// <remarks>Requires use of a <see cref="BuildInfo"/> class with a property named <see cref="BuildInfo.Version"/></remarks>
		public void WriteStarter()
		{
			Write($"Mod loaded with v{BuildInfo.Version}");
		}

		/// <summary>
		/// Prints a seperator
		/// </summary>
		/// <param name="parameters">Any additional params</param>
		private void WriteSeperator(params object[] parameters)
		{
			Write("==============================================================================", parameters);
		}

		/// <summary>
		/// Prints a seperator
		/// </summary>
		/// <param name="parameters">Any additional params</param>
		/// <param name="level">The level of this message (NOT the existing the level)</param>
		public void WriteSeperator(FlaggedLoggingLevel level, params object[] parameters)
		{
			if (CurrentLevel.HasFlag(level)) WriteSeperator(parameters);
		}

		/// <summary>
		/// Logs an "Intra Seperator", allowing you to print headers
		/// </summary>
		/// <param name="message">The header name. Should be short</param>
		/// <param name="parameters">Any additional params</param>
		private void WriteIntraSeparator(string message, params object[] parameters)
		{
			Write($"=========================   {message}   =========================", parameters);
		}

		/// <summary>
		/// Logs an "Intra Seperator", allowing you to print headers
		/// </summary>
		/// <param name="message">The header name. Should be short</param>
		/// <param name="level">The level of this message (NOT the existing the level)</param>
		/// <param name="parameters">Any additional params</param>
		public void WriteIntraSeparator(FlaggedLoggingLevel level, string message, params object[] parameters)
		{
			if (CurrentLevel.HasFlag(level)) WriteIntraSeparator(message, parameters);
		}

		/// <summary>
		/// Prints a log with <c>[EXCEPTION]</c> at the start.
		/// </summary>
		/// <param name="message">The formated string to add as the message. Displayed before the exception</param>
		/// <param name="exception">The exception thrown</param>
		/// <remarks>
		/// <para>This is done as building the exception otherwise can be tedious</para>
		/// </remarks>
		private void WriteException(string message, System.Exception? exception)
		{
			System.Text.StringBuilder sb = new();

			sb.Append("[EXCEPTION]");
			sb.Append(message);

			if (exception != null) sb.AppendLine(exception.Message);
			else sb.AppendLine("Exception was null");

			Write(sb.ToString());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="FormatedMessage"></param>
		public void WriteLogBlock(string FormatedMessage)
		{
			Write(FormatedMessage);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lines"></param>
		public void WriteLogBlock(string[] lines)
		{
			System.Text.StringBuilder LogBlock = new();

			foreach (string line in lines)
			{
				LogBlock.AppendLine(line);
			}

			WriteLogBlock(LogBlock.ToString());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Title"></param>
		/// <param name="Lines"></param>
		/// <param name="color"></param>
		public void WriteLogBlock(string Title, string[] Lines, System.Drawing.Color color)
		{
			int lenght = Lines.Length;
			List<System.Drawing.Color> lineColors = new();

			for (int c = 0; c < lenght; c++)
			{
				lineColors.Add(color);
			}

			System.Text.StringBuilder LogBlock = new();

			LogBlock.AppendLine(Title);

			for (int i = 0; i < Lines.Length; i++)
			{
				LogBlock.AppendLine(Lines[i].Pastel(lineColors[i]));
			}

			WriteLogBlock(LogBlock.ToString());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Title"></param>
		/// <param name="lines"></param>
		/// <param name="lineColors"></param>
		/// <exception cref="ComplexLoggerException"></exception>
		public void WriteLogBlock(string Title, string[] lines, List<System.Drawing.Color> lineColors)
		{
			// if the lineColors array is not null, check the length to ensure that they are equal
			if (lineColors != null)
			{
				if (lines.Length != lineColors?.Count)
				{
					throw new ComplexLoggerException($"WriteLogBlock({Title}, {lines.Length}, {lineColors?.Count})::Attempting to build a LogBlock requires equal length arrays for both parameters \'lines\' and \'lineColors\'");
				}
			}

			System.Text.StringBuilder LogBlock = new();

			LogBlock.AppendLine(Title);

			// if the lineColors array is not null, append each line with the associated color
			if (lineColors != null)
			{
				for (int i = 0; i < lines.Length; i++)
				{
					LogBlock.AppendLine(lines[i].Pastel(lineColors[i]));
				}
			}
			// if the lineColors array is null, use the default color when appending each line
			else
			{
				for (int i = 0; i < lines.Length; i++)
				{
					LogBlock.AppendLine(lines[i]);
				}
			}

			WriteLogBlock(LogBlock.ToString());
		}
	}
}