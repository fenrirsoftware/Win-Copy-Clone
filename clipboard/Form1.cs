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
        }
        //hello world helmsys kruvazör
        private async Task LoadAsyncData()
        {
            while (keepRunning)
            {
                // Asenkron olarak testFonksiyon'u çağır
                IntPtr result = await Task.Run(() => ClipBoardData());

                // IntPtr'i C# stringine dönüştür
                string message = Marshal.PtrToStringAnsi(result);

                // Sonucu ekrana yazdır
                textBox1.Text = message;

                // Bir süre bekleyebilirsiniz, eğer gerekiyorsa
                await Task.Delay(1000); // Örneğin, 1 saniye bekleyin
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
