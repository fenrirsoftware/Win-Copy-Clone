using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clipboard
{
    public partial class Form1 : Form
    {
        // DLL'yi çağırmak için P/Invoke kullanılır.
        [DllImport("CopyPasteRelease.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ClipBoardData();
      


       




        private bool dragging = false;

        private Point dragStartPoint;


        private bool keepRunning = true;

        public Form1()
        {
            InitializeComponent();

            // Form kapatıldığında olayları dinle
            FormClosing += Form1_FormClosing;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadAsyncData();
           
            AddScrollBar();
        }
        //hello world helmsys kruvazör
        private async Task LoadAsyncData()
        {
            while (keepRunning)
            {
                IntPtr result = await Task.Run(() => ClipBoardData());
                string message = Marshal.PtrToStringAnsi(result);

                // Eğer gelen veri bir önceki veri ile eşit değilse
                if (message != previousData)
                {
                    CreateAndFillTextBox(message);
                    previousData = message;
                }

                await Task.Delay(1000);
            }
        }
        private string previousData = string.Empty;
        private int textBoxCount = 0;
        private void CreateAndFillTextBox(string text)
        {
            TextBox newTextBox = new TextBox();
            newTextBox.Size = new Size(290, 53);
            newTextBox.Location = new Point(5, 5 + textBoxCount * 60);
            newTextBox.Multiline = true;
            newTextBox.ScrollBars = ScrollBars.Vertical;
            newTextBox.Text = text;
            Controls.Add(newTextBox);
            textBoxCount++;
        }

        private void AddScrollBar()
        {
            VScrollBar vScrollBar = new VScrollBar();
            vScrollBar.Dock = DockStyle.Right;
            vScrollBar.Scroll += VScrollBar_Scroll;
            Controls.Add(vScrollBar);
        }

        private void VScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox)
                {
                    control.Top -= e.NewValue - e.OldValue;
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form kapatıldığında döngüyü sonlandır
            keepRunning = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;    
                dragStartPoint = new Point(e.X, e.Y);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dragDelta = new Point(e.X - dragStartPoint.X, e.Y - dragStartPoint.Y);
                Location = new Point(Location.X + dragDelta.X, Location.Y + dragDelta.Y);
            }
        }
    }
}
