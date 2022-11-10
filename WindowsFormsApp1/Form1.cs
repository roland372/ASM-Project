﻿using System;
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

                    /*pictureBox1.Visible = true;*/

                    /* pictureBox1.Update();*/



                }

            }
            catch
            {
                MessageBox.Show("Error", "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /*            pictureBox1.Image = Image.FromFile(@"C:\Users\Damian\Desktop\image.png");
            */

            /*            pictureBox1.Image = Image.FromFile(@"C:\\Users\\Damian\\Desktop\\image.png");
            */
            /*pictureBox1.Image = Properties.Resources.image;*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap copyBitmap = new Bitmap((Bitmap)pictureBox1.Image);
            ProcessImage(copyBitmap);
            pictureBox2.Image = copyBitmap;

        }

        public bool ProcessImage(Bitmap bmp)
        {
            for (int i =0; i < bmp.Width; ++i)
                {
                for (int j =0; j < bmp.Height; ++j)
                {
                    Color bmpColor = bmp.GetPixel(i, j);
                    int red = bmpColor.R; 
                    int green = bmpColor.G;
                    int blue = bmpColor.B;
                    int gray = (byte)(.299 * red + .587 * green + .114 * blue);
                    red = gray;
                    green = gray;
                    blue = gray;

                    bmp.SetPixel(i, j, Color.FromArgb(red, green, blue));

                  
                }
                
            }
            return true;
        }
    }
}
