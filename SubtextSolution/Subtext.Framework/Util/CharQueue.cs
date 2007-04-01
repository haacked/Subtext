using System;
using System.Collections.Generic;
using System.Text;

namespace Subtext.Framework.Util
{
	#region CharQueue
	internal class CharQueue
	{
		private char[] _list;
		private int _charCount = 0;

		protected CharQueue() { }
		public CharQueue(int length)
		{
			_list = new char[length];
		}

		public char this[int i]
		{
			get { return _list[i]; }
			set { _list[i] = value; }
		}

		public void Enqueue(char x)
		{
			for (int i = 0; i < _list.Length; i++)
			{
				if (i < _list.Length - 1)
					_list[i] = _list[i + 1];
				else
					_list[i] = x;
			}
		}

		public int Length
		{
			get { return _charCount; }
		}

		public char Dequeue()
		{
			char result = _list[0];

			char[] compacted = new char[_list.Length - 1];
			_list.CopyTo(compacted, 1);
			_list = compacted;

			return result;
		}

		public bool Holds(string value)
		{
			return Holds(value, StringComparison.InvariantCultureIgnoreCase);
		}

		public bool Holds(string value, StringComparison comparisonType)
		{
			return String.Equals(value, this.ToString(), comparisonType);
		}

		public override string ToString()
		{
			return new string(_list);
		}

		public string ToString(int length)
		{
			if (length != _list.Length)
			{
				char[] results = new char[length];
				_list.CopyTo(results, 0);
				return new string(results);
			}
			else
				return this.ToString();
		}


	#endregion

	}
}
