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
            String imageLocation = "";
            

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
/*                dialog.Filter = "jpg files(.*jpg)|*.jpg| BMP files(.*bmp)|*.bmp| All Files(*.*)|*.*";*/
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
/*            ProcessImage(copyBitmap);*/
              sharp(copyBitmap);

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

        /*       public static Bitmap Sharpen(Bitmap image)
               {
                   Bitmap sharpenImage = (Bitmap)image.Clone();

                   const int filterWidth = 5;
                   const int filterHeight = 5;
                   int width = image.Width;
                   int height = image.Height;

                   // Create sharpening filter.
                   double[,] filter = new double[filterWidth, filterHeight] {
           { -1, -1, -1, -1, -1 },
           { -1,  2,  2,  2, -1 },
           { -1,  2,  100,  2, -1 },
           { -1,  2,  2,  2, -1 },
           { -1, -1, -1, -1, -1 }
       };

                   double factor = 1.0 / 100.0;
                   double bias = 0.0;

                   Color[,] result = new Color[image.Width, image.Height];

                   // Lock image bits for read/write.
                   BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                   // Declare an array to hold the bytes of the bitmap.
                   int bytes = pbits.Stride * height;
                   byte[] rgbValues = new byte[bytes];

                   // Copy the RGB values into the array.
                   System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

                   int rgb;
                   // Fill the color array with the new sharpened color values.
                   for (int x = 0; x < width; ++x)
                   {
                       for (int y = 0; y < height; ++y)
                       {
                           double red = 0.0, green = 0.0, blue = 0.0;

                           for (int filterX = 0; filterX < filterWidth; filterX++)
                           {
                               for (int filterY = 0; filterY < filterHeight; filterY++)
                               {
                                   int imageX = (x - filterWidth / 2 + filterX + width) % width;
                                   int imageY = (y - filterHeight / 2 + filterY + height) % height;

                                   rgb = imageY * pbits.Stride + 3 * imageX;

                                   red += rgbValues[rgb + 2] * filter[filterX, filterY];
                                   green += rgbValues[rgb + 1] * filter[filterX, filterY];
                                   blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                               }
                               int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                               int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                               int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                               result[x, y] = Color.FromArgb(r, g, b);
                           }
                       }
                   }

                   // Update the image with the sharpened pixels.
                   for (int x = 0; x < width; ++x)
                   {
                       for (int y = 0; y < height; ++y)
                       {
                           rgb = y * pbits.Stride + 3 * x;

                           rgbValues[rgb + 2] = result[x, y].R;
                           rgbValues[rgb + 1] = result[x, y].G;
                           rgbValues[rgb + 0] = result[x, y].B;
                       }
                   }

                   // Copy the RGB values back to the bitmap.
                   System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
                   // Release image bits.
                   sharpenImage.UnlockBits(pbits);

                   return sharpenImage;
               }*/
        public static Bitmap sharp(Bitmap img)
        {
            Bitmap sharpenImage = new Bitmap(img.Width, img.Height);

            int filterWidth = 3;
            int filterHeight = 3;
            int w = img.Width;
            int h = img.Height;

            double[,] filter = new double[filterWidth, filterHeight];

            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] =
           filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[img.Width, img.Height];

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;


                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + w) % w;
                            int imageY = (y - filterHeight / 2 + filterY + h) % h;

                            Color imageColor = img.GetPixel(imageX, imageY);
                            red += imageColor.R * filter[filterX, filterY];
                            green += imageColor.G * filter[filterX, filterY];
                            blue += imageColor.B * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }
            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    sharpenImage.SetPixel(i, j, result[i, j]);
                }
            }
            return sharpenImage;
        }



    }
}
