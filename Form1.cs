using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static Calculator.W32;
using static Calculator.VcnMath;
using System.Collections.Generic;

namespace Calculator
{
    public partial class MainForm : Form
    {
        protected override void WndProc(ref Message m)
        {
            var val = 2;
            if (m.Msg == 0x0085)
            {
                DwmSetWindowAttribute(Handle, 2, ref val, 4);
                MARGINS dmwMargins = new MARGINS(0, 0, 0, 1);
                DwmExtendFrameIntoClientArea(Handle, ref dmwMargins);
            }
            if (m.Msg == 0x112)
            {
                if ((int)m.WParam == 2) {
                    AboutBox aboutBox = new AboutBox();
                    aboutBox.ShowDialog(this);
                }
            }
            base.WndProc(ref m);
        }

        public MainForm()
        {
            InitializeComponent();
            int Style = GetWindowLong(Handle, GWL.Style);
            SetWindowLong(Handle, GWL.Style, Style &~ WS_MAXIMIZEBOX| WS_SYSMENU |WS_MINIMIZEBOX);
            int sysmenu = (int)GetSystemMenu(Handle, false);
            RemoveMenu(sysmenu, 0xF000,(int) 0x0L);
            RemoveMenu(sysmenu, 0xF030, (int)0x0L);
            AppendMenu(sysmenu,(int) 0x800L, 1, "");
            AppendMenu(sysmenu, 0, 2, "About Calculator");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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

        int currentCharIndex;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FormulaBox.Text))
            {
                if (!WaterMarkVisible)
                {
                    Graphics graphics = FormulaBox.CreateGraphics();
                    graphics.DrawString("Enter Problem Here...", FormulaBox.Font, new SolidBrush(Color.Gray), new PointF(0, 0));
                    WaterMarkVisible = true;
                }
            }
            else WaterMarkVisible = false;
            if (!(FormulaBox.SelectionStart == FormulaBox.Text.Length))
            {
                //TextInfo textInfo = GetTextInfo(FormulaBox.Text, FormulaBox.SelectionStart, @"\<b\>", @"\<\/b\>");

                //Title_Label.Text = textInfo.Start + "-" + textInfo.StartLength + " \"" + textInfo.Value + "\" " + textInfo.End + "-" + textInfo.EndLength + ":=>" + textInfo.NeedCount;


                //Title_Label.Text = "";
                //List<TextInfo> list = WhereIsMe(FormulaBox.Text, FormulaBox.SelectionStart, @"\<", @"\>\>");
                //foreach (TextInfo info in list)
                //{
                //    Title_Label.Text += info.Start + " - " + info.End;
                //}
                Graphics graphics = FormulaBox.CreateGraphics();
                if (currentCharIndex != FormulaBox.SelectionStart && FormulaBox.Focused )
                {
                    FormulaBox.Refresh();
                    List<TextInfo> list = WhereIsMe(FormulaBox.Text, FormulaBox.SelectionStart);
                    TextInfo text1 = GetRootListTextInfo(list);
                    if (!(text1.Start == text1.End))
                    {
                        SizeF txtSize = graphics.MeasureString("(", FormulaBox.Font);
                        Point PosStart = FormulaBox.GetPositionFromCharIndex(text1.Start);
                        Point PosEnd = FormulaBox.GetPositionFromCharIndex(text1.End);
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosStart, new Size(8, (int)txtSize.Height)));
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosEnd, new Size(8, (int)txtSize.Height)));
                    }
                    else
                    {
                    }
                    currentCharIndex = FormulaBox.SelectionStart;
                }
            }
        }

        public string Answer= "0";
        bool WaterMarkVisible;
        private void FormulaBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FormulaBox.Text))
                {
                    ResultBox.Text = "= 0";
                    return;
                }
                else
                {

                    //Title_Label.Text = GetResult(FormulaBox.Text);
                    ResultBox.Text = "= " + FilterCalc(GetResult(FormulaBox.Text));
                    Answer = FilterCalc(Result(FormulaBox.Text));
                    if (ResultBox.Text.ToLower().Contains("error"))
                    {
                        FormulaBox.Select(Currentmatch.Index, Currentmatch.Value.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Answer = "0";
                if (ex.GetType() == typeof(OverflowException))
                {
                    ResultBox.Text = "= Error : Overflow.";
                    FormulaBox.Select(Currentmatch.Index, Currentmatch.Length);
                }
                else
                {
                    ResultBox.Text = "= " + GetLastError.Message;
                    if (Currentmatch != null) FormulaBox.Select(Currentmatch.Index, Currentmatch.Length);
                }
            }
            DetailFrm detailFrm = new DetailFrm();
            if (IsVisible(detailFrm, out Form form))
            {
                DetailFrm detail =(DetailFrm) form;
                string Step = "";
                if (stepBystep.Count>0)
                foreach (string Str in stepBystep)
                {
                    Step += Str + Environment.NewLine;
                }
                detail.textBox1.Text = Step;
            }
        }

        private void button48_Click(object sender, EventArgs e)
        {
            DetailFrm detailFrm = new DetailFrm();
            if (!IsVisible(detailFrm, out Form form))
            {
                detailFrm.Location = new Point(Location.X + Width + 2, Location.Y);
                detailFrm.Show(this);
                string Step = "";
                if (stepBystep.Count > 0)
                    foreach (string Str in stepBystep)
                    {
                        Step += Str + Environment.NewLine;
                    }
                detailFrm.textBox1.Text = Step;
            }
            else { form.Close(); }
        }

        private bool IsVisible(Form frm, out Form visibleform)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == frm.Name)
                {
                    visibleform = form;
                    return true;
                }
            }
            visibleform = null;
            return false;
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(Answer == "..." || Answer.ToLower().Contains("error")))
                    ResultBox.Text = "= " + getAngle(Convert.ToDouble(Answer)).ToString() + "°";
            }
            catch
            {
                ResultBox.Text = "= Error: Cannot convert to Degrees";
            }
        }

        #region ->Button
        private void button24_Click(object sender, EventArgs e)
        {
            ResultBox.Text = "= " + Answer;
        }

        private void button55_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "^2";
            FormulaBox.SelectionLength = 0;
        }

        private void button69_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "^3";
            FormulaBox.SelectionLength = 0;
        }

        private void button49_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "log(";
            FormulaBox.SelectionStart = FormulaBox.SelectionStart - 1;
            FormulaBox.SelectionLength = 0;
            FormulaBox.Focus();
        }

        private void button64_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "round(";
            FormulaBox.SelectionStart = FormulaBox.SelectionStart - 1;
            FormulaBox.SelectionLength = 0;
            FormulaBox.Focus();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "(√)";
            FormulaBox.SelectionStart = FormulaBox.SelectionStart - 1;
            FormulaBox.SelectionLength = 0;
            FormulaBox.Focus();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = ((Button)sender).Text;
            FormulaBox.SelectionLength = 0;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            FormulaBox.Text = string.Empty;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int Pos = FormulaBox.SelectionStart;
            if (!(FormulaBox.SelectionStart == 0))
            {
                FormulaBox.Text = FormulaBox.Text.Remove(FormulaBox.SelectionStart -1, 1);
                FormulaBox.SelectionStart = Pos -1;
            }
            FormulaBox.SelectionLength = 0;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = ((Button)sender).Text + "(";
            FormulaBox.SelectionLength = 0;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "!";
            FormulaBox.SelectionLength = 0;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            FormulaBox.SelectedText = "-";
            FormulaBox.SelectionLength = 0;
        }
        #endregion

        private void button76_Click(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
            FormulaBox.SelectionStart = FormulaBox.SelectionStart == 0 ? FormulaBox.TextLength : FormulaBox.SelectionStart - 1;
            FormulaBox.Refresh();
            Graphics graphics = FormulaBox.CreateGraphics();
            Pen pen = new Pen(Color.Orange);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            pen.Width = 2;
            String text = FormulaBox.Text.Substring(0, FormulaBox.SelectionStart);
            Point point = FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart);
            SizeF fntsizeB = graphics.MeasureString("S", FormulaBox.Font);
            List<TextInfo> list = WhereIsMe(FormulaBox.Text, FormulaBox.SelectionStart);
            TextInfo text1 = GetRootListTextInfo(list);
            if (!(text1.Start == text1.End))
            {
                SizeF txtSize = graphics.MeasureString("(", FormulaBox.Font);
                Point PosStart = FormulaBox.GetPositionFromCharIndex(text1.Start);
                Point PosEnd = FormulaBox.GetPositionFromCharIndex(text1.End);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosStart, new Size(8, (int)txtSize.Height)));
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosEnd, new Size(8, (int)txtSize.Height)));
            }
            if (FormulaBox.SelectionStart == FormulaBox.TextLength)
            {
                if (FormulaBox.TextLength >= 1)
                {
                    graphics.DrawLine(pen, FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart - 1).X + 10, point.Y, FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart - 1).X + 10, point.Y + fntsizeB.Height);
                }
            }
            else
                graphics.DrawLine(pen, point.X, point.Y, point.X,point.Y+fntsizeB.Height);
        }

        private void button75_Click(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
            FormulaBox.SelectionStart = FormulaBox.SelectionStart == FormulaBox.TextLength ? 0 : FormulaBox.SelectionStart + 1;
            FormulaBox.Refresh();
            Graphics graphics = FormulaBox.CreateGraphics();
            Pen pen = new Pen(Color.Orange);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            pen.Width = 2;
            String text = FormulaBox.Text.Substring(0, FormulaBox.SelectionStart);
            Point point = FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart);
            SizeF fntsize = graphics.MeasureString(text, FormulaBox.Font);
            SizeF fntsizeB = graphics.MeasureString("S", FormulaBox.Font);
            List<TextInfo> list = WhereIsMe(FormulaBox.Text, FormulaBox.SelectionStart);
            TextInfo text1 = GetRootListTextInfo(list);
            if (!(text1.Start == text1.End))
            {
                SizeF txtSize = graphics.MeasureString("(", FormulaBox.Font);
                Point PosStart = FormulaBox.GetPositionFromCharIndex(text1.Start);
                Point PosEnd = FormulaBox.GetPositionFromCharIndex(text1.End);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosStart, new Size(8, (int)txtSize.Height)));
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosEnd, new Size(8, (int)txtSize.Height)));
            }
            if (FormulaBox.SelectionStart == FormulaBox.TextLength)
            {
                if (FormulaBox.TextLength >= 1)
                {
                    graphics.DrawLine(pen, FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart - 1).X + 12, FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart - 1).Y, FormulaBox.GetPositionFromCharIndex(FormulaBox.SelectionStart - 1).X + 12, point.Y + fntsizeB.Height);
                }
            }
            else
                graphics.DrawLine(pen, point.X, point.Y, point.X, point.Y+ fntsizeB.Height);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            panel1.Visible = !panel1.Visible;
            panel2.Visible = !panel2.Visible;
        }

        private void FormulaBox_CausesValidationChanged(object sender, EventArgs e)
        {
            
        }

        private void FormulaBox_StyleChanged(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
        }

        private void FormulaBox_Leave(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
        }

        private void FormulaBox_Enter(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            WaterMarkVisible = false;
        }
    }
}


public class FormulaTextBox : TextBox
{
    public FormulaTextBox() : base()
    {
        
    }

    string _placeholder;
    public string PlaceHolder
    {
        set
        {
            _placeholder = value;
        }
        get
        {
            return _placeholder;
        }
    }

    //protected override void OnPaint(PaintEventArgs e)
    //{
    //    if (this.Text == "")
    //    e.Graphics.DrawString(_placeholder, this.Font, new SolidBrush(Color.Gray), new Point(1, 2));
    //    List<TextInfo> list = WhereIsMe(this.Text, this.SelectionStart);
    //    TextInfo text = GetRootListTextInfo(list);
    //    if (!(text.Start == text.End))
    //    {
    //        SizeF txtSize = e.Graphics.MeasureString("(", this.Font);
    //        Point PosStart = this.GetPositionFromCharIndex(text.Start);
    //        Point PosEnd = this.GetPositionFromCharIndex(text.End);
    //        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(30,Color.Blue)), new Rectangle(PosStart, new Size(8, (int)txtSize.Height)));
    //        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Blue)), new Rectangle(PosEnd, new Size(8, (int)txtSize.Height)));
    //    }
    //    base.OnPaint(e);
    //}
    public IntPtr GraphicsHandle;

    const int WM_PAINT = 0x000F;
    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);
        if (m.Msg == WM_PAINT)
        {
            //this.OnPaint(new PaintEventArgs(Graphics.FromHwnd(m.HWnd), this.ClientRectangle));
            GraphicsHandle = m.HWnd;
        }
    }
}