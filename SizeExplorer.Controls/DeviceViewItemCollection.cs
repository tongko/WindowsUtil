using System.Collections.Generic;
using IEnumerable = System.Collections.IEnumerable;
using IEnumerator = System.Collections.IEnumerator;

namespace SizeExplorer.Controls
{
	public class DeviceViewItemCollection : IList<DeviceViewItem>
	{
		private const int IncrementBase = 10;
		private DeviceViewItem[] _items;
		private int _versions;

		public DeviceViewItemCollection()
		{
			_items = new DeviceViewItem[IncrementBase];
		}

		private int FindItem(DeviceViewItem item)
		{
			for (var i = 0; i < _items.Length; i++)
			{
				if (item == _items[i])
					return i;
			}

			return -1;
		}



		#region IList Implementation

		public int IndexOf(DeviceViewItem item)
		{
			return FindItem(item);
		}

		public void Insert(int index, DeviceViewItem item)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new System.NotImplementedException();
		}

		public DeviceViewItem this[int index]
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		public void Add(DeviceViewItem item)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(DeviceViewItem item)
		{
			throw new System.NotImplementedException();
		}

		public void CopyTo(DeviceViewItem[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		public int Count
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool IsReadOnly
		{
			get { throw new System.NotImplementedException(); }
		}

		public bool Remove(DeviceViewItem item)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerator<DeviceViewItem> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		#endregion


		#region Sub Types

		public class DeviceViewItemEnumerator : IEnumerator<DeviceViewItem>
		{
			#region IEnumerator<DeviceViewItem> Implementations

			public DeviceViewItem Current
			{
				get { throw new System.NotImplementedException(); }
			}

			public void Dispose()
			{
				throw new System.NotImplementedException();
			}

			object IEnumerator.Current
			{
				get { throw new System.NotImplementedException(); }
			}

			public bool MoveNext()
			{
				throw new System.NotImplementedException();
			}

			public void Reset()
			{
				throw new System.NotImplementedException();
			}

			#endregion
		}

		#endregion
	}
}
