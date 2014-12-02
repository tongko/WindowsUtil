using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace SizeExplorer.Model
{
	public class SizeNodeCollection : IList<SizeNode>, IList
	{
		private static readonly SizeNode[] _emptyArray = new SizeNode[0];
		private const int DefCapacity = 4;
		private static readonly object SyncObject = new object();
		private int _version = 0;
		private SizeNode[] _items;
		private int _size;

		public SizeNodeCollection()
		{
			_items = new SizeNode[DefCapacity];
		}

		public SizeNodeCollection(int capacity)
		{
			_items = new SizeNode[capacity];
		}

		public SizeNodeCollection(IEnumerable<SizeNode> collection)
		{
			_items = collection.ToArray();
		}

		public SizeNode Parent { get; set; }

		public int Capacity
		{
			get
			{
				lock (SyncObject)
					return _items.Length;
			}
			set
			{
				lock (SyncObject)
				{
					if (value < _size)
						throw new ArgumentOutOfRangeException("value");
					if (value == _items.Length)
						return;
					if (value > 0)
					{
						var array = new SizeNode[value];
						if (_size > 0)
							Array.Copy(_items, 0, array, 0, _size);
						_items = array;
					}
					else
						_items = _emptyArray;
				}
			}
		}

		public object SyncRoot
		{
			get { return SyncObject; }
		}

		public int IndexOf(SizeNode item)
		{
			throw new System.NotImplementedException();
		}

		public void Insert(int index, SizeNode item)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new System.NotImplementedException();
		}

		public SizeNode this[int index]
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

		public void Add(SizeNode item)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(SizeNode item)
		{
			throw new System.NotImplementedException();
		}

		public void CopyTo(SizeNode[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		public int Count
		{
			get
			{
				lock (SyncObject)
					return _size;
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(SizeNode item)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerator<SizeNode> GetEnumerator()
		{
			IEnumerator<SizeNode> result;
			lock (SyncObject)
			{
				result = new SizeNodeEnumerator(this);
			}

			return result;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return  GetEnumerator();
		}

		public class SizeNodeEnumerator : IEnumerator<SizeNode>
		{
			private readonly SizeNodeCollection _collection;
			private readonly int _version;
			private int _currentIndex;
			private SizeNode _current;

			public SizeNodeEnumerator(SizeNodeCollection collection)
			{
				_currentIndex = -1;
				_version = collection._version;
				_collection = collection;
				_current = null;
			}

			public SizeNode Current
			{
				get { return _current; }
			}

			public void Dispose()
			{
			}

			object IEnumerator.Current
			{
				get
				{
					if (_currentIndex == -1 || _currentIndex == _collection._size)
						throw new InvalidOperationException();
					return _current;
				}
			}

			public bool MoveNext()
			{
				var coll = _collection;
				if (_version != coll._version || (uint) _currentIndex >= coll._size)
					return MoveNextRare();

				_current = coll._items[++_currentIndex];
				return true;
			}

			public bool MoveNextRare()
			{
				if (_version != _collection._version)
					throw new InvalidOperationException();

				_currentIndex = _collection._size + 1;
				_current = null;
				return false;
			}

			public void Reset()
			{
				if (_version != _collection._version)
					throw new InvalidOperationException();
				_currentIndex = -1;
				_current = null;
			}
		}

		int IList.Add(object value)
		{
			var item = value as SizeNode;
			if (item == null)
				throw new ArgumentException("value");

			int index;
			lock (SyncObject)
			{
				Add(item);
				index = _size - 1;
			}

			return index;
		}

		void IList.Clear()
		{
			Clear();
		}

		bool IList.Contains(object value)
		{
			return Contains(value as SizeNode);
		}

		int IList.IndexOf(object value)
		{
			return IndexOf(value as SizeNode);
		}

		void IList.Insert(int index, object value)
		{
			Insert(index, value as SizeNode);
		}

		bool IList.IsFixedSize
		{
			get { return false; }
		}

		bool IList.IsReadOnly
		{
			get { return IsReadOnly; }
		}

		void IList.Remove(object value)
		{
			Remove(value as SizeNode);
		}

		void IList.RemoveAt(int index)
		{
			RemoveAt(index);
		}

		object IList.this[int index]
		{
			get
			{
				lock (SyncObject)
					return _items[index];
			}
			set
			{
				lock (SyncObject)
					this[index] = value as SizeNode;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(_items, 0, array, index, _size);
		}

		int ICollection.Count
		{
			get
			{
				lock (SyncObject)
					return _size;
			}
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return SyncObject; }
		}
	}
}
