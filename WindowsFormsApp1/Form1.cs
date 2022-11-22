using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Image = System.Drawing.Image;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections.Specialized;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static string value = "1";
        List<double> lista = new List<double>();
        double sum = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String imageLocation = "";

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "All Files(*.*)|*.*| jpg files(.*jpg)|*.jpg| BMP files(.*bmp)|*.bmp";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = dialog.FileName;

                    pictureBox1.ImageLocation = imageLocation;

                    Image img = Image.FromFile(imageLocation);
                    ImageFormat format = img.RawFormat;

                    var fileSize = new FileInfo(imageLocation).Length / 1024;

                    textBox1.Text = "Image Type : " + format.ToString() + "\r\n" + "Image dimensions: " + img.Width + " x " + img.Height + "\r\n" + "File Size: " + fileSize + "KB";

                    Console.WriteLine("Image Type : " + format.ToString());
                    Console.WriteLine("Image width : " + img.Width);
                    Console.WriteLine("Image height : " + img.Height);
                    Console.WriteLine("Image resolution : " + (img.VerticalResolution * img.HorizontalResolution));

                    Console.WriteLine("Image Pixel depth : " + Image.GetPixelFormatSize(img.PixelFormat));

                    pictureBox1.Visible = true;
                    pictureBox1.Update();

                }

            }
            catch
            {
                MessageBox.Show("Error", "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Bitmap copyBitmap = new Bitmap((Bitmap)pictureBox1.Image);
                double d1 = double.Parse(value, CultureInfo.InvariantCulture);

                /*pictureBox2.Image = SubtractAddFactorImage(copyBitmap, ConvolutionFilter(copyBitmap, GaussianBlur(5, d1)));*/
                pictureBox2.Image = UnsharpMaskingLibrary.UnsharpMaskingClass.SubtractAddFactorImage(copyBitmap,
                                    UnsharpMaskingLibrary.UnsharpMaskingClass.ConvolutionFilter(copyBitmap,
                                    UnsharpMaskingLibrary.UnsharpMaskingClass.GaussianBlur(5, d1)));

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                lista.Add(elapsedMs * 0.001);
                sum += elapsedMs * 0.001;
                textBox2.Text = "stopwatch value : " + elapsedMs * 0.001 + " s\r\nAverage: " + sum / lista.Count;

            }
            else
            {
                MessageBox.Show("Please select image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            value = trackBar1.Value.ToString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Number of threads: " + comboBox1.SelectedItem);
        }
    }
}
