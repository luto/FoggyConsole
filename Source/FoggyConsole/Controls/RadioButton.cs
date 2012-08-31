using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A checkbox-like control of which only one in a group can be checked at at the same time
    /// </summary>
    public class RadioButton : CheckableBase
    {
        /// <summary>
        /// The group this RadioButton belongs to
        /// </summary>
        public string ComboboxGroup { get; set; }

        /// <summary>
        /// Creates a new RadioButton
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="drawer">The <code>ControlDrawer</code> to use. If null a new instance of <code>RadioButtonDrawer</code> will be used.</param>
        /// <exception cref="ArgumentException">Thrown if the <code>ControlDrawer</code> which should be set already has an other Control assigned</exception>
        public RadioButton(string text, ControlDrawer<RadioButton> drawer = null)
            : base(text, drawer)
        {
            if(drawer == null)
                base.Drawer = new RadioButtonDrawer(this);
            this.Checked = CheckState.Unchecked;
            this.CheckedChanging += OnCheckedChanging;
        }

        private void OnCheckedChanging(object sender, CheckedChangingEventArgs checkedChangingEventArgs)
        {
            var groupBoxes = Container.OfType<RadioButton>()
                                      .Where(cb => cb.ComboboxGroup == ComboboxGroup);
            
            foreach (var cb in groupBoxes)
            {
                if(cb != this && cb.Checked == CheckState.Checked)
                    cb.Checked = CheckState.Unchecked;
            }
        }
    }

    /// <summary>
    /// Draws a RadioButton
    /// </summary>
    public class RadioButtonDrawer : CheckableBaseDrawer<RadioButton>
    {
        public RadioButtonDrawer(RadioButton control)
            : base(control, "({1}) {0}")
        {
        }
    }
}
