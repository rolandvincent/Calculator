using System;
using System.Drawing;
using System.Windows.Forms;
using static Calculator.W32;

namespace Calculator
{
    public partial class DetailFrm : Form
    {
        public DetailFrm()
        {
            InitializeComponent();
            int Style = GetWindowLong(Handle, GWL.Style);
            SetWindowLong(Handle, GWL.Style, Style & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX | WS_SYSMENU);
            int sysmenu =(int) GetSystemMenu(Handle, false);
            //RemoveMenu(sysmenu, 0xF000,(int) 0x0L);
            //RemoveMenu(sysmenu, 0xF010,(int) 0x0L);
            RemoveMenu(sysmenu, 0xF020,(int) 0x0L);
            RemoveMenu(sysmenu, 0xF030,(int) 0x0L);
            RemoveMenu(sysmenu, 0xF120,(int) 0x0L);
            //EnableMenuItem((IntPtr)sysmenu,0xF000, 0);
            IntPtr ehWnd = FindWindowEx(base.Handle, IntPtr.Zero, "Edit", "");
            SubClassHwnd sub = new SubClassHwnd();
            sub.AssignHandle(ehWnd);
        }

        protected override void WndProc(ref Message m)
        {
            var val = 2;
            if (m.Msg == 0x0085)
            {
                DwmSetWindowAttribute(Handle, 2, ref val, 4);
                MARGINS dmwMargins = new MARGINS(0, 0, 0, 1);
                DwmExtendFrameIntoClientArea(Handle, ref dmwMargins);
            }
            base.WndProc(ref m);
        }
        MouseButtons MousePress = MouseButtons.None;
        private void titlebar_MouseDown(object sender, MouseEventArgs e)
        {
            MousePress = e.Button;
            ReleaseCapture();
            SendMessage((int)Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void Title_Bar_MouseUp(object sender, MouseEventArgs e)
        {
            if (MousePress == MouseButtons.Right)
            {
                IntPtr wMenu = GetSystemMenu(Handle, false);
                uint command = TrackPopupMenuEx(wMenu, TPM_LEFTBUTTON | TPM_RETURNCMD, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);
                if (command == 0) return;
                PostMessage(Handle, WM_SYSCOMMAND, new IntPtr(command), IntPtr.Zero);
            }
        }
        private void DetailFrm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        Point Loc;
        Size lstSize;
        MouseButtons mouseButtons;
        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            Loc = new Point(MousePosition.X-Location.X , MousePosition.Y-Location.Y);
            lstSize = Size;
            mouseButtons = e.Button;
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            mouseButtons = MouseButtons.None;
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseButtons== MouseButtons.Left)
            {
                Size = new Size(MousePosition.X - Location.X + lstSize.Width - Loc.X , MousePosition.Y  - Location.Y + lstSize.Height  - Loc.Y);
            }
        }
    }
}
public class SubClassHwnd : NativeWindow
{
    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x7b)
            return;
        base.WndProc(ref m);
    }
}
