using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinBlur;
using static WinBlur.UI;

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
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;

            SetBlurStyle(cntrl: this, blurType: BlurType.Acrylic, designMode: Mode.DarkMode);

            // Apply rounded corners to the form
         
          

            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.Size = new Size(410, 420);
            flowLayoutPanel1.AutoScroll = true;

            Controls.Add(flowLayoutPanel1);
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
    newTextBox.Size = new Size(389, 70);
    newTextBox.FlatStyle = FlatStyle.Flat;
    newTextBox.FlatAppearance.BorderSize = 0;
    
    // Add a margin of 10 pixels from the top
    newTextBox.Location = new Point(5, 5 + textBoxCount * 90); // Adjusted the margin and vertical spacing
    
    newTextBox.TextAlign = ContentAlignment.MiddleLeft;

    newTextBox.Text = text;
    newTextBox.Font = new Font("Arial", 9, FontStyle.Regular);
    newTextBox.ForeColor = Color.FromArgb(220, 220, 220);
    newTextBox.BackColor = Color.FromArgb(60, 60, 60);

    // Apply rounded corners to the button
    newTextBox.Click += Button_Click;

    Controls.Add(newTextBox);
    textBoxCount++;
    flowLayoutPanel1.Controls.Add(newTextBox);
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

        }

        // Button kenarlarını yuvarlamak için özel metot
    


     
    }
}
