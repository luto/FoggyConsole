using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoggyConsole.Controls
{
    /// <summary>
    /// A very basic <code>ContainerControl</code>
    /// It has no appearance, controls within it are aligned using thier top and left values.
    /// </summary>
    public class Panel : ContainerControl
    {
        public Panel(PanelDrawer drawer = null)
            : base(drawer)
        {
            if (drawer == null)
                base.Drawer = new PanelDrawer(this);
        }
    }

    public class PanelDrawer : ControlDrawer
    {
        private Panel Panel { get { return Control as Panel; } }

        
        public PanelDrawer(Panel control)
            : base(control)
        {
        }


        public override void Draw(int leftOffset, int topOffset)
        {
            leftOffset += Panel.Left;
            topOffset  += Panel.Top;

            foreach (var control in Panel)
            {
                control.Drawer.Draw(leftOffset, topOffset);
            }
        }
    }
}
