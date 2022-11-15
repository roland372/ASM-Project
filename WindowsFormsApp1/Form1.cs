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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            /*Bitmap bitmap = new Bitmap("C:\\Users\\Damian\\Desktop\\image.png", true);*/

            /*Image img = new Bitmap(imgPath);*/

            /*pictureBox1.Image = img;*/


        }

        private void button1_Click(object sender, EventArgs e)
        {
            // <----- Bitmap ----->
            /* OpenFileDialog openFile = new OpenFileDialog();
             if (openFile.ShowDialog() == DialogResult.OK)
             {
                 pictureBox1.Image = new Bitmap(openFile.FileName);
             }*/
            // <----- Bitmap ----->


            String imageLocation = "";


            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(.*jpg)|*.jpg| BMP files(.*bmp)|*.bmp| All Files(*.*)|*.*";

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
    }
}
