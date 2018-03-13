using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sim.Module.Data.Ids;
using Sim.Module.Data.State;
using Sim.Module.Generic;

namespace Sim.Module.Extensions
{
	public static class UtilityExtensions
	{
		public static RequestId Increase(this RequestId source)
		{
			throw new NotImplementedException();
		}

		public static void TryInject(this IContext context, object instance)
		{
			if(instance is IContextInjector)
			{
				(instance as IContextInjector).Context = context;
			}
		}

		public static string ToText<T>(this IEnumerable<T> source, string header = null, Func<T, string> renderer = null)
		{
			var rows = 0;
			var builder = new StringBuilder();
			renderer = renderer ?? (@object => @object.ToString());
			if(source == null)
			{
				return
					(header != null
						? $"{header} [rows: {rows}]\n{builder}"
						: builder.ToString())
					.TrimEnd(Environment.NewLine.ToCharArray());
			}

			foreach(var item in source)
			{
				builder.AppendLine(item == null
					? "null"
					: $"{++rows}: {renderer(item)}");
			}

			return
				(header != null
					? $"{header} [rows: {rows}]\n{builder}"
					: builder.ToString())
				.TrimEnd(Environment.NewLine.ToCharArray());
		}

		public static string ToText(this IEnumerable source, string header = null, Func<object, string> renderer = null)
		{
			return ToText(source.Cast<object>(), header, renderer);
		}

		public static string ToText(this Exception source)
		{
			if(source == null)
			{
				return "exception is null";
			}

			var exception = source;
			var builder = new StringBuilder();
			var counter = 0;
			while(exception != null)
			{
				builder.AppendLine("-- ." + ++counter);
				builder.Append("Exception: ");
				builder.AppendLine(exception.GetType().Name);
				builder.Append("Message: ");
				builder.AppendLine(exception.Message.TrimEnd('\n'));
				builder.AppendLine("Trace: ");
				builder.AppendLine(exception.StackTrace);
				exception = exception.InnerException;
			}

			builder.Append("-- .end exceptions trace");
			return $"Roll out totals: {counter}\n" + builder;
		}

		private static readonly Regex _typeTemplate = new Regex("(?'name'[^`]*)");

		private static string WriteArgs(Type type)
		{
			return
				type.IsGenericType
					? type.GetGenericArguments()
						.Aggregate(new StringBuilder(), (builder, _) => builder.Append(_.NameNice() + ", "))
						.ToString()
						.TrimEnd(", ".ToArray())
					: string.Empty;
		}

		public static string NameNice(this Type source)
		{
			if(source == null)
			{
				return "null";
			}

			return
				source.IsGenericType
					? $"{_typeTemplate.Match(source.Name).Groups["name"].Value}<{WriteArgs(source)}>"
					: source.Name;
		}

		public static int HashLy(this string source)
		{
			uint hash = 0;
			var sequence = Encoding.Unicode.GetBytes(source);
			sequence = sequence.Length > 0
				? sequence
				: Encoding.Unicode.GetBytes("undefined_".MakeUnique(24));
			unchecked
			{
				// ReSharper disable once unknown
				for(var ctr = 0; ctr < sequence.Length; ctr++)
				{
					hash = hash * 1664525 + sequence[ctr] + 1013904223;
				}
			}

			return (int)hash;
		}

		// transformations

		[ThreadStatic] private static Random _random;
		private static readonly char[] _base62Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

		public static string MakeUnique(this string source, int length = 6, int @base = 62)
		{
			_random = _random ?? new Random();
			var builder = new StringBuilder(source, source.Length + length);
			for(var ctr = 0; ctr < length; ctr++)
			{
				builder.Append(_base62Chars[_random.Next(@base)]);
			}

			return builder.ToString();
		}
	}
}