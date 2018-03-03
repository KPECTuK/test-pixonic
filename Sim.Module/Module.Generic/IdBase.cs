using Sim.Module.Extensions;

namespace Sim.Module.Generic
{
	public abstract class IdBase<TId> where TId : IdBase<TId>
	{
		// TODO: implememt Create()

		private readonly string _id;
		private readonly int _hash;

		//public static readonly TId EmptyId = typeof(TId).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null).Invoke(null) as TId;

		//public static TId Create<TId>(string id)
		//{

		//}

		protected IdBase(string id)
		{
			//if(string.IsNullOrWhiteSpace(id))
			//{
			//	throw new Exception("empty source");
			//}

			_id = string.IsNullOrWhiteSpace(id) ? "empty-id" : id;
			_hash = id?.HashLy() ?? -1;
		}

		public bool Equals(TId other)
		{
			return
				!ReferenceEquals(null, other) &&
				(ReferenceEquals(this, other) || string.Equals(other._id, _id));
		}

		public override bool Equals(object @object)
		{
			return Equals(@object as TId);
		}

		public static bool operator ==(TId first, IdBase<TId> second)
		{
			return
				!ReferenceEquals(null, first) &&
				(second?.Equals(first) ?? false);
		}

		public static bool operator !=(TId first, IdBase<TId> second)
		{
			return
				ReferenceEquals(null, first) ||
				!(second?.Equals(first) ?? false);
		}

		public string Render()
		{
			return _id;
		}

		public override int GetHashCode()
		{
			return _hash;
		}

		public override string ToString()
		{
			return $"{_id ?? string.Empty}(h:{_hash})";
		}
	}
}