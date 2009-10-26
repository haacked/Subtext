#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Subtext.Scripting
{
    /// <summary>
    /// A collection of <see cref="TemplateParameter"/> instances.
    /// </summary>
    public class TemplateParameterCollection : ICollection<TemplateParameter>
    {
        readonly List<TemplateParameter> _list = new List<TemplateParameter>();

        /// <summary>
        /// Gets the <see cref="TemplateParameter"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TemplateParameter this[int index]
        {
            get { return _list[index]; }
        }

        /// <summary>
        /// Gets the <see cref="TemplateParameter"/> with the specified name.
        /// </summary>
        /// <value></value>
        public TemplateParameter this[string name]
        {
            get
            {
                foreach(TemplateParameter parameter in _list)
                {
                    if(String.Equals(parameter.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        return parameter;
                    }
                }
                return null;
            }
        }

        #region ICollection<TemplateParameter> Members

        void ICollection<TemplateParameter>.Add(TemplateParameter item)
        {
            Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Gets a value indicating whether the collection contains the specified 
        /// <see cref="TemplateParameter">Script</see>.
        /// </summary>
        public bool Contains(TemplateParameter item)
        {
            if(item == null)
            {
                throw new ArgumentNullException("item");
            }

            return Contains(item.Name);
        }

        public void CopyTo(TemplateParameter[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        public bool Remove(TemplateParameter item)
        {
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<TemplateParameter> Members

        IEnumerator<TemplateParameter> IEnumerable<TemplateParameter>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Determines whether [contains] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified name]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string name)
        {
            return this[name] != null;
        }

        /// <summary>
        /// Creates a template parameter from a match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public TemplateParameter Add(Match match)
        {
            if(match == null)
            {
                throw new ArgumentNullException("match");
            }

            if(this[match.Groups["name"].Value] != null)
            {
                return this[match.Groups["name"].Value];
            }

            var parameter = new TemplateParameter(match.Groups["name"].Value, match.Groups["type"].Value,
                                                  match.Groups["default"].Value);
            Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds the specified value. If it already exists, returns 
        /// the existing one, otherwise just returns the one you added.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public TemplateParameter Add(TemplateParameter value)
        {
            if(value == null)
            {
                throw new ArgumentNullException("value");
            }

            if(Contains(value))
            {
                return this[value.Name];
            }
            _list.Add(value);
            value.ValueChanged += OnValueChanged;
            return value;
        }

        /// <summary>
        /// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
        /// to the end of the collection.
        /// </summary>
        /// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="TemplateParameter"/>s to add to the collection. </param>
        public void AddRange(IEnumerable<TemplateParameter> value)
        {
            foreach(TemplateParameter parameter in value)
            {
                Add(parameter);
            }
        }

        /// <summary>
        /// Gets the index in the collection of the specified 
        /// <see cref="TemplateParameter">Script</see>, if it exists in the collection.
        /// </summary>
        /// <param name="value">The <see cref="TemplateParameter">Script</see> 
        /// to locate in the collection.</param>
        /// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
        public int IndexOf(TemplateParameter value)
        {
            return _list.IndexOf(value);
        }

        /// <summary>
        /// Provides a shortcut to set a value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string name, string value)
        {
            if(this[name] != null)
            {
                this[name].Value = value;
            }
        }

        private void OnValueChanged(object sender, ParameterValueChangedEventArgs args)
        {
            OnValueChanged(args);
        }

        protected void OnValueChanged(ParameterValueChangedEventArgs args)
        {
            EventHandler<ParameterValueChangedEventArgs> changeEvent = ValueChanged;
            if(changeEvent != null)
            {
                changeEvent(this, args);
            }
        }

        /// <summary>
        /// Event raised when any parameter within this collection changes 
        /// its values.
        /// </summary>
        public event EventHandler<ParameterValueChangedEventArgs> ValueChanged;
    }
}