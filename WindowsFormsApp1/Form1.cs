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
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Image = System.Drawing.Image;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Collections.Specialized;
using System.Drawing.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Collections;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        [DllImport("Dll1.dll")]
        //private unsafe static extern int suma(IntPtr array, int numElements, int value);
        /*        private unsafe static extern byte SubtractAddFactorImage(byte[] bitmap);
        */

        private unsafe static extern void SubtractAddFactorImage(byte* bmpOriginal, byte* bmpBlur, byte* bmpResult, int imageSizeInBytes);

        static string value = "1";
        List<double> lista = new List<double>();
        double sum = 0;

        int workerThreads;
        int completionPortThreads;

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedItem = "64";
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
                    string fileName = Path.GetFileName(imageLocation);


                    textBox1.Text = "File name: " + fileName + "\r\n" + "Image dimensions: " + img.Width + " x " + img.Height + "\r\n" + "File Size: " + fileSize + "KB";

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

                ThreadPool.SetMaxThreads(int.Parse(comboBox1.SelectedItem.ToString()), int.Parse(comboBox1.SelectedItem.ToString()));
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Bitmap copyBitmap = new Bitmap((Bitmap)pictureBox1.Image);
                double d1 = double.Parse(value, CultureInfo.InvariantCulture);

                pictureBox2.Image = UnsharpMaskingLibrary.UnsharpMaskingClass.SubtractAddFactorImage(copyBitmap,
                                    UnsharpMaskingLibrary.UnsharpMaskingClass.ConvolutionFilter(copyBitmap,
                                    UnsharpMaskingLibrary.UnsharpMaskingClass.GaussianBlur(5, d1)));

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                lista.Add(elapsedMs * 0.001);
                sum += elapsedMs * 0.001;
                textBox2.Text = "Stopwatch value : " + elapsedMs * 0.001 + " s\r\nAverage: " + sum / lista.Count + "\r\n" + "Number of threads: " + comboBox1.SelectedItem + "\r\n" + "Environment.ProcessorCount: " + Environment.ProcessorCount + "\r\n" + "Threads count" + Process.GetCurrentProcess().Threads.Count + "\r\n" + "Worker threads:" + workerThreads + "\r\n" + "Completion threads: " + completionPortThreads;

                int[] myArray = new int[10];
                GCHandle handle = GCHandle.Alloc(myArray, GCHandleType.Pinned);

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

        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private static readonly ImageConverter _imageConverter = new ImageConverter();

        public static byte[] CopyImageToByteArray(Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream ms = new MemoryStream(blob, true);
            ms.Position = 0;
            Bitmap img = new Bitmap(ms);

            return img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {

                ThreadPool.SetMaxThreads(int.Parse(comboBox1.SelectedItem.ToString()), int.Parse(comboBox1.SelectedItem.ToString()));
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Bitmap copyBitmap = new Bitmap((Bitmap)pictureBox1.Image);
                double d1 = double.Parse(value, CultureInfo.InvariantCulture);
                Console.WriteLine("1" + copyBitmap.GetType());

                byte[] bitmap1 = CopyImageToByteArray(copyBitmap);
                Console.WriteLine("2" + bitmap1.GetType());
                byte[] bitmap2 = CopyImageToByteArray(UnsharpMaskingLibrary.UnsharpMaskingClass.ConvolutionFilter(copyBitmap, UnsharpMaskingLibrary.UnsharpMaskingClass.GaussianBlur(5, 5000)));
                byte[] bitmap3 = new byte[copyBitmap.Width * copyBitmap.Height];
                int imageSizeInBytes = (copyBitmap.Width * copyBitmap.Height * 3) / 8;

                int[] intArray = new int[bitmap3.Length / sizeof(int)];
                byte[] byteArray = new byte[intArray.Length * sizeof(int)];
                Buffer.BlockCopy(intArray, 0, byteArray, 0, byteArray.Length);

                unsafe
                {
                    fixed (byte* FirstResult = &bitmap1[0])
                    fixed (byte* SecondResult = &bitmap2[0])
                    fixed (byte* ThirdResult = &bitmap3[0])
                    fixed (int* intArrayPtr = &intArray[0])
                    fixed (byte* byteArr = &byteArray[0])
                    {
                        SubtractAddFactorImage(FirstResult, SecondResult, byteArr, imageSizeInBytes);

                        try
                        {
                            if (byteArray == null || byteArray.Length == 0)
                            {
                                throw new ArgumentException("Byte array is empty or null");
                            }

                            using (MemoryStream mStream = new MemoryStream())
                            {
                                mStream.Write(byteArray, 0, byteArray.Length);
                                mStream.Position = 0;
                                Image img = Image.FromStream(mStream);
                                pictureBox2.Image = img;

                            }

                        }
                        catch (ArgumentException ex)
                        {
                            // Handle the exception, for example by logging it
                            //Console.WriteLine(ex.Message);
                            textBox2.Text = "ERROR : " + ex.Message;
                        }

                    }
                }


                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                lista.Add(elapsedMs * 0.001);
                sum += elapsedMs * 0.001;
                textBox2.Text = "Stopwatch value : " + elapsedMs * 0.001 + " s\r\nAverage: " + sum / lista.Count + "\r\n" + "Number of threads: " + comboBox1.SelectedItem + "\r\n" + "Environment.ProcessorCount: " + Environment.ProcessorCount + "\r\n" + "Threads count" + Process.GetCurrentProcess().Threads.Count + "\r\n" + "Worker threads:" + workerThreads + "\r\n" + "Completion threads: " + completionPortThreads;

                int[] myArray = new int[10];
                GCHandle handle = GCHandle.Alloc(myArray, GCHandleType.Pinned);

            }
            else
            {
                MessageBox.Show("Please select image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}