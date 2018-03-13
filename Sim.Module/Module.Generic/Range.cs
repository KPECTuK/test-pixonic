using System;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace Sim.Module.Generic
{
	public struct Range<T> : IRange<T> where T : IEquatable<T>, IComparable<T>
	{
		private static readonly Regex _parser = new Regex("(?'min'[\\S]+)\\s*-\\s*(?'max'[\\S]+)");

		public T Min { get; private set; }
		public T Max { get; private set; }

		public Range(T value) : this(value, value) { }

		public Range(T min, T max)
		{
			Min = min.CompareTo(max) < 0
				? min
				: max;
			Max = min.CompareTo(max) > 0
				? min
				: max;
		}

		public T Clamp(T value)
		{
			var result = Min.CompareTo(value) > 0
				? Min
				: value;
			result = Max.CompareTo(result) < 0
				? Max
				: result;
			return result;
		}

		public bool IsInside(T value, bool strictly = false)
		{
			return
				strictly
					? Max.CompareTo(value) > 0 && Min.CompareTo(value) < 0
					: Max.CompareTo(value) >= 0 && Min.CompareTo(value) <= 0;
		}

		public bool IsAbove(T value, bool strictly = false)
		{
			return
				strictly
					? Max.CompareTo(value) < 0
					: Max.CompareTo(value) <= 0;
		}

		public bool IsBelow(T value, bool strictly = false)
		{
			return
				strictly
					? Min.CompareTo(value) > 0
					: Min.CompareTo(value) >= 0;
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			var match = _parser.Match(reader.ReadElementContentAsString());
			try
			{
				Min = (T)Convert.ChangeType(match.Groups["min"].Value, typeof(T));
			}
			catch
			{
				Min = default(T);
			}
			try
			{
				Max = (T)Convert.ChangeType(match.Groups["max"].Value, typeof(T));
			}
			catch
			{
				Max = default(T);
			}
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteString($"{Min}-{Max}");
		}

		public override string ToString()
		{
			return $"{Min}-{Max}";
		}
	}
}