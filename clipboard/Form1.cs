using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinBlur;

namespace clipboard
{
    public partial class Form1 : Form
    {
        [DllImport("CopyPasteRelease.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ClipBoardData();

        private bool dragging = false;
        private Point dragStartPoint;
        private bool keepRunning = true;

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            SetRoundedForm(15); // Set a 5-pixel radius for rounded corners
        }

        private void SetRoundedForm(int radius)
        {
            // Create a rounded rectangle for the form
            GraphicsPath formPath = new GraphicsPath();
            formPath.AddArc(0, 0, radius, radius, 180, 90); // Top-left corner
            formPath.AddArc(Width - radius, 0, radius, radius, 270, 90); // Top-right corner
            formPath.AddArc(Width - radius, Height - radius, radius, radius, 0, 90); // Bottom-right corner
            formPath.AddArc(0, Height - radius, radius, radius, 90, 90); // Bottom-left corner
            formPath.CloseFigure();

            Region = new Region(formPath);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.Size = new Size(410, 420);
            flowLayoutPanel1.AutoScroll = true;

            Controls.Add(flowLayoutPanel1);

            // Apply rounded corners to the panel
            GraphicsPath panelPath = new GraphicsPath();
            panelPath.AddArc(0, 0, 5, 5, 180, 90); // Top-left corner with 5-pixel radius
            panelPath.AddArc(flowLayoutPanel1.Width - 5, 0, 5, 5, 270, 90); // Top-right corner
            panelPath.AddArc(flowLayoutPanel1.Width - 5, flowLayoutPanel1.Height - 5, 5, 5, 0, 90); // Bottom-right corner
            panelPath.AddArc(0, flowLayoutPanel1.Height - 5, 5, 5, 90, 90); // Bottom-left corner
            panelPath.CloseFigure();

            flowLayoutPanel1.Region = new Region(panelPath);

            await LoadAsyncData();
        }

        private async Task LoadAsyncData()
        {
            while (keepRunning)
            {
                await Task.Delay(100);
                IntPtr result = await Task.Run(() => ClipBoardData());
                string message = Marshal.PtrToStringAnsi(result);

                if (message != previousData)
                {
                    CreateAndFillTextBox(message);
                    previousData = message;
                }
            }
        }

        private string previousData = string.Empty;
        private int textBoxCount = 0;
        private FlowLayoutPanel flowLayoutPanel1;


        private void CreateAndFillTextBox(string text)
        {


            Button newTextBox = new Button();
            newTextBox.Size = new Size(338, 70);
            newTextBox.FlatStyle = FlatStyle.Flat;
            newTextBox.FlatAppearance.BorderSize = 0;
            newTextBox.Location = new Point(50, 50 + textBoxCount * 90);
            newTextBox.TextAlign = ContentAlignment.MiddleLeft;

            newTextBox.Text = text;
            newTextBox.Font = new Font("Arial", 9, FontStyle.Regular);
            newTextBox.ForeColor = Color.FromArgb(220, 220, 220);
            newTextBox.BackColor = Color.FromArgb(60, 60, 60);

            newTextBox.Click += Button_Click;

            Controls.Add(newTextBox);
            textBoxCount++;
            flowLayoutPanel1.Controls.Add(newTextBox);



            GraphicsPath txtpath = new GraphicsPath();
            txtpath.AddArc(0, 0, 5, 5, 180, 90); // Top-left corner with 5-pixel radius
            txtpath.AddArc(newTextBox.Width - 5, 0, 5, 5, 270, 90); // Top-right corner
            txtpath.AddArc(newTextBox.Width - 5, newTextBox.Height - 5, 5, 5, 0, 90); // Bottom-right corner
            txtpath.AddArc(0, newTextBox.Height - 5, 5, 5, 90, 90); // Bottom-left corner
            txtpath.CloseFigure();

            newTextBox.Region = new Region(txtpath);









        }

        private void Button_Click(object sender, EventArgs e)
        {
            // TODO: Butona tıklama işlemleri
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragStartPoint = new Point(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dragDelta = new Point(e.X - dragStartPoint.X, e.Y - dragStartPoint.Y);
                Location = new Point(Location.X + dragDelta.X, Location.Y + dragDelta.Y);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            keepRunning = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Your pictureBox1 click event handling
        }
    }
}
