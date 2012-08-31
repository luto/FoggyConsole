using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A checkbox-like control of which only one in a group can be checked at at the same time
    /// </summary>
    public class Combobox : CheckableBase
    {
        /// <summary>
        /// The group this Combobox belongs to
        /// </summary>
        public string ComboboxGroup { get; set; }

        /// <summary>
        /// Creates a new Combobox
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>ComboboxDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public Combobox(string text, ControlDrawer<Combobox> drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                base.Drawer = new ComboboxDrawer(this);
            this.Checked = CheckState.Unchecked;
            this.CheckedChanging += OnCheckedChanging;
        }

        private void OnCheckedChanging(object sender, CheckedChangingEventArgs checkedChangingEventArgs)
        {
            var groupBoxes = Container.OfType<Combobox>()
                                      .Where(cb => cb.ComboboxGroup == ComboboxGroup);
            
            foreach (var cb in groupBoxes)
            {
                if(cb != this && cb.Checked == CheckState.Checked)
                    cb.Checked = CheckState.Unchecked;
            }
        }
    }

    /// <summary>
    /// Draws a Combobox
    /// </summary>
    public class ComboboxDrawer : CheckableBaseDrawer<Combobox>
    {
        public ComboboxDrawer(Combobox control)
            : base(control, "({1}) {0}")
        {
        }
    }
}
