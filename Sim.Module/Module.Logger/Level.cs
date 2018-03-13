using System;

namespace Sim.Module.Logger
{
	public class Level
	{
		private readonly bool _isInherit;

		public string Name { get; }
		public int Value { get; }
		public string DisplayName { get; }

		public Level(string levelName) : this(int.MaxValue, levelName, levelName)
		{
			_isInherit = true;
		}

		public Level(int level, string levelName) : this(level, levelName, levelName) { }

		public Level(int level, string levelName, string displayName)
		{
			if(levelName == null)
			{
				throw new ArgumentException("levelName is null");
			}

			if(displayName == null)
			{
				throw new ArgumentException("displayName is null");
			}

			Value = level;
			Name = string.Intern(levelName);
			DisplayName = displayName;
			_isInherit = false;
		}

		public override string ToString()
		{
			return Name;
		}

		public override int GetHashCode()
		{
			return Value;
		}

		public static bool operator >(Level left, Level right)
		{
			return Compare(left, right) > 0;
		}

		public static bool operator <(Level left, Level right)
		{
			return Compare(left, right) < 0;
		}

		public static bool operator >=(Level left, Level right)
		{
			return Compare(left, right) >= 0;
		}

		public static bool operator <=(Level left, Level right)
		{
			return Compare(left, right) <= 0;
		}

		public static bool operator ==(Level left, Level right)
		{
			return Compare(left, right) == 0;
		}

		public static bool operator !=(Level left, Level right)
		{
			return Compare(left, right) != 0;
		}

		public bool Equals(Level other)
		{
			return Compare(this, other) == 0;
		}

		public override bool Equals(object @object)
		{
			return Equals(@object as Level);
		}

		public int CompareTo(object @object)
		{
			return Compare(this, @object as Level);
		}

		public static int Compare(Level left, Level right)
		{
			if(ReferenceEquals(left, right))
			{
				return 0;
			}

			if(ReferenceEquals(null, left) && ReferenceEquals(null, right))
			{
				return 0;
			}

			if(ReferenceEquals(null, left))
			{
				return -1;
			}

			// ReSharper disable once ConvertIfStatementToReturnStatement
			if(ReferenceEquals(null, right))
			{
				return 1;
			}

			return left.Value.CompareTo(right.Value);
		}

		public static string RenderLevelName(Level level)
		{
			return
				ReferenceEquals(level, null)
					? "UNDEFINED"
					: level.DisplayName;
		}

		/// <summary>
		///     The <see cref="Off" /> level designates a higher level than all the rest.
		/// </summary>
		public static readonly Level Off = new Level(int.MaxValue, "OFF");

		/// <summary>
		///     The <see cref="Emergency" /> level designates very severe error events.
		///     System unusable, emergencies.
		/// </summary>
		public static readonly Level Log5Debug = new Level(120000, "log5:DEBUG");

		/// <summary>
		///     The <see cref="Emergency" /> level designates very severe error events.
		///     System unusable, emergencies.
		/// </summary>
		public static readonly Level Emergency = new Level(120000, "EMERGENCY");

		/// <summary>
		///     The <see cref="Fatal" /> level designates very severe error events
		///     that will presumably lead the application to abort.
		/// </summary>
		public static readonly Level Fatal = new Level(110000, "FATAL");

		/// <summary>
		///     The <see cref="Alert" /> level designates very severe error events.
		///     Take immediate action, alerts.
		/// </summary>
		public static readonly Level Alert = new Level(100000, "ALERT");

		/// <summary>
		///     The <see cref="Critical" /> level designates very severe error events.
		///     Critical condition, critical.
		/// </summary>
		public static readonly Level Critical = new Level(90000, "CRITICAL");

		/// <summary>
		///     The <see cref="Severe" /> level designates very severe error events.
		/// </summary>
		public static readonly Level Severe = new Level(80000, "SEVERE");

		/// <summary>
		///     The <see cref="Error" /> level designates error events that might
		///     still allow the application to continue running.
		/// </summary>
		public static readonly Level Error = new Level(70000, "ERROR");

		/// <summary>
		///     The <see cref="Warn" /> level designates potentially harmful
		///     situations.
		/// </summary>
		public static readonly Level Warn = new Level(60000, "WARN");

		/// <summary>
		///     The <see cref="Notice" /> level designates informational messages
		///     that highlight the progress of the application at the highest level.
		/// </summary>
		public static readonly Level Notice = new Level(50000, "NOTICE");

		/// <summary>
		///     The <see cref="Info" /> level designates informational messages that
		///     highlight the progress of the application at coarse-grained level.
		/// </summary>
		public static readonly Level Info = new Level(40000, "INFO");

		/// <summary>
		///     The <see cref="Debug" /> level designates fine-grained informational
		///     events that are most useful to debug an application.
		/// </summary>
		public static readonly Level Debug = new Level(30000, "DEBUG");

		/// <summary>
		///     The <see cref="Fine" /> level designates fine-grained informational
		///     events that are most useful to debug an application.
		/// </summary>
		public static readonly Level Fine = new Level(30000, "FINE");

		/// <summary>
		///     The <see cref="Trace" /> level designates fine-grained informational
		///     events that are most useful to debug an application.
		/// </summary>
		public static readonly Level Trace = new Level(20000, "TRACE");

		/// <summary>
		///     The <see cref="Finer" /> level designates fine-grained informational
		///     events that are most useful to debug an application.
		/// </summary>
		public static readonly Level Finer = new Level(20000, "FINER");

		/// <summary>
		///     The <see cref="Verbose" /> level designates fine-grained informational
		///     events that are most useful to debug an application.
		/// </summary>
		public static readonly Level Verbose = new Level(10000, "VERBOSE");

		/// <summary>
		///     The <see cref="Finest" /> level designates fine-grained informational
		///     events that are most useful to debug an application.
		/// </summary>
		public static readonly Level Finest = new Level(10000, "FINEST");

		/// <summary>
		///     The <see cref="All" /> level designates the lowest level possible.
		/// </summary>
		public static readonly Level All = new Level(int.MinValue, "ALL");

		/// <summary>
		///     The <see cref="Inherit" /> level designates the lowest level possible.
		/// </summary>
		public static readonly Level Inherit = new Level("INHERIT");
	}
}