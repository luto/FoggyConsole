using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A <code>Control</code> which can contain other Controls.
    /// </summary>
    public abstract class ContainerControl : Control, IList<Control>
    {
        private List<Control> _controls;

        #region IList
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count { get { return _controls.Count; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the value to set is null</exception>
        public Control this[int index]
        {
            get { return _controls[index]; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");
                value.Container = this;
                OnControlRemoved(_controls[index]);
                _controls[index] = value;
                OnControlAdded(value);
            }
        }

        /// <summary>
        /// Gets the element with the specified name.
        /// </summary>
        /// <returns>
        /// The element with the specified name.
        /// </returns>
        /// <param name="name">The name of the element to get.</param>
        public Control this[string name]
        {
            get { return GetByName(name); }
        }
        #endregion

        /// <summary>
        /// Creates a new <code>ContainerControl</code>
        /// </summary>
        /// <param name="drawer"></param>
        /// <exception cref="ArgumentException">Thrown if the ControlDrawer which should be set already has an other Control assigned</exception>
        public ContainerControl(IControlDrawer drawer)
            : base(drawer)
        {
            _controls = new List<Control>();
        }

        #region IList
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        [DebuggerStepThrough]
        public IEnumerator<Control> GetEnumerator()
        {
            return _controls.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(Control item)
        {
            _controls.Add(item);
            OnControlAdded(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            var controls = _controls.ToArray();
            _controls.Clear();
            foreach (var c in controls)
                OnControlRemoved(c);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(Control item)
        {
            return _controls.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo(Control[] array, int arrayIndex)
        {
            _controls.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(Control item)
        {
            var retn = _controls.Remove(item);
            OnControlRemoved(item);
            return retn;
        }

        /// <summary>
        /// Removes the element with the name <paramref name="name"/>
        /// </summary>
        /// <param name="name">The name of the element to remove</param>
        /// <returns>true if the element has been removed, otherwise false</returns>
        public bool RemoveByName(string name)
        {
            var index = FindByName(name);
            if (index == -1)
                return false;
            var c = this[index];
            RemoveAt(index);
            OnControlRemoved(c);
            return true;
        }

        private Control GetByName(string name)
        {
            var index = FindByName(name);
            if (index == -1)
                return null;
            else
                return this[index];
        }

        private int FindByName(string name)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Name == name)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(Control item)
        {
            return _controls.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, Control item)
        {
            _controls.Insert(index, item);
            OnControlAdded(item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void RemoveAt(int index)
        {
            var c = this[index];
            _controls.RemoveAt(index);
            OnControlRemoved(c);
        }
        #endregion

        /// <summary>
        /// Fired if a control gets added to this container
        /// </summary>
        /// <seealso cref="ControlRemoved"/>
        public event EventHandler<ContainerControlEventArgs> ControlAdded;

        private void OnControlAdded(Control c)
        {
            c.Container = this;
            if (ControlAdded != null)
                ControlAdded(this, new ContainerControlEventArgs(c));
        }

        /// <summary>
        /// Fired if a control gets removed from this container
        /// </summary>
        /// <seealso cref="ControlAdded"/>
        public event EventHandler<ContainerControlEventArgs> ControlRemoved;

        private void OnControlRemoved(Control c)
        {
            c.Container = null;
            if(ControlRemoved != null)
                ControlRemoved(this, new ContainerControlEventArgs(c));
        }
    }

    /// <summary>
    /// Used by <code>ContainerControl</code> in ControlAdded and ControlRemoved events
    /// </summary>
    public class ContainerControlEventArgs : EventArgs
    {
        /// <summary>
        /// The control which has been added or removed
        /// </summary>
        public Control Control { get; private set; }

        /// <summary>
        /// Creates a new <code>ContainerControlEventArgs</code>
        /// </summary>
        /// <param name="control">The control which has been added or removed</param>
        public ContainerControlEventArgs(Control control)
        {
            this.Control = control;
        }
    }

    /// <summary>
    /// Base class for all ControlDrawers which are able to draw a <code>ContainerControl</code>
    /// </summary>
    public abstract class ContainerControlDrawer<T> : ControlDrawer<T> where T : ContainerControl
    {
        /// <summary>
        /// Creates a new ControlDrawer
        /// </summary>
        /// <param name="control">The Control to draw</param>
        public ContainerControlDrawer(T control = null)
            : base(control)
        {
        }


        /// <summary>
        /// Calculates the boundary of the Control given in the Control-Property and stores it in the Boundary-Property
        /// </summary>
        /// <param name="leftOffset">Offset for the left value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="topOffset">Offset for the top value (used to convert local coordinates within a container to global ones)</param>
        /// <param name="boundary">The boundary of the <code>ContainerControl</code> in which the <code>Control</code> is placed</param>
        public override void CalculateBoundary(int leftOffset, int topOffset, Rectangle boundary)
        {
            base.CalculateBoundary(leftOffset, topOffset, boundary);
            CalcuateChildBoundaries();
        }

        /// <summary>
        /// Calculates the boundaries of all children widthin the ContainerControl
        /// </summary>
        protected virtual void CalcuateChildBoundaries()
        {
            foreach (var control in _control)
            {
                control.Drawer.CalculateBoundary(Boundary.Left, Boundary.Top, Boundary);
            }
        }
    }
}
