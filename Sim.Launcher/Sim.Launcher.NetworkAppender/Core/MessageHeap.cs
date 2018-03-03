using System.Collections;
using System.Collections.Generic;

namespace Sim.Launcher.NetworkAppender.Core
{
	internal sealed class MessageHeap<TChank> : IRepository<TChank>
	{
		private readonly List<TChank> _heap = new List<TChank>();
		private readonly List<HeapCursor<TChank>> _cursors = new List<HeapCursor<TChank>>();

		public void Enqueue(TChank buffer)
		{
			lock(_heap)
				_heap.Add(buffer);
		}

		internal void Maintain()
		{
			lock(_heap)
				lock(_cursors)
				{
					var bound = _heap.Count;
					_cursors.ForEach(
						cursor =>
							bound =
							bound < cursor.CurrentIndex
								? bound
								: cursor.CurrentIndex);
					if(bound >= 0)
					{
						_heap.RemoveRange(0, bound);
						_cursors.ForEach(cursor => cursor.Shift(bound));
					}
				}
		}

		internal void NotifyCursorRemove(HeapCursor<TChank> cursor)
		{
			lock(_cursors)
				_cursors.Remove(cursor);
		}

		// IEnumerable<TChank>
		public IEnumerator<TChank> GetEnumerator()
		{
			lock(_cursors)
			{
				var cursor = new HeapCursor<TChank>(this, _heap);
				_cursors.Add(cursor);
				return cursor;
			}
		}

		// IEnumerable<TChank>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
