using System.Collections;
using System.Collections.Generic;

namespace Sim.Launcher.NetworkAppender.Core
{
	internal class HeapCursor<TChank> : IEnumerator<TChank>
	{
		private readonly MessageHeap<TChank> _parent;
		private readonly IList<TChank> _heap;

		// TODO: solve offsets
		public int CurrentIndex { get; private set; }

		public void Shift(int offset)
		{
			CurrentIndex -= offset;
		}

		public HeapCursor(MessageHeap<TChank> parent, IList<TChank> heap)
		{
			_parent = parent;
			_heap = heap;
			CurrentIndex = -1;
		}

		// IDisposable
		public void Dispose()
		{
			_parent.NotifyCursorRemove(this);
		}

		// IEnumerator<byte[]>
		public bool MoveNext()
		{
			var result = CurrentIndex + 1 < _heap.Count;
			CurrentIndex += result ? 1 : 0;
			return result;
		}

		// IEnumerator<byte[]>
		public void Reset()
		{
			CurrentIndex = -1;
		}

		// IEnumerator<byte[]>
		public TChank Current
		{
			get
			{
				lock(_heap)
					return _heap[CurrentIndex];
			}
		}

		// IEnumerator<byte[]>
		object IEnumerator.Current { get { return Current; } }
	}
}
