using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ImageToTileMap
{
    class Program
    {
        static void Main(string[] args)
        {
            float version = 0.4f;
            Console.WriteLine("Version " + version + " - (Convert images to a TileMap with max size of 8192x8192)");
            Console.WriteLine("Version " + version + " - (Program still has some bugs, try to make size as high as possible)");
            Console.WriteLine("Version " + version + " - (If any help is required send a E-mail to: EMAIL)");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("Enter Path to image folder that contains .png's");
            string imgPath = Console.ReadLine();
            Console.WriteLine("Enter Save location:");
            string savePath = Console.ReadLine();
            Console.WriteLine("Enter Size I.E (512)");
            int size = int.Parse(Console.ReadLine());

            if(size == 1 || size == 2 || size == 4 || size == 8 || size == 16 || size == 32 || size == 64 || size == 128 || size == 256 || size == 512 || size == 1024 || size == 2048 || size == 4096 || size == 8192)
            {
                GenerateTileMap(imgPath, savePath, size);
            }
            else
            {
                Console.WriteLine("Size is not to math 2, setting size to 4096");
                size = 4096;
                GenerateTileMap(imgPath, savePath, size);
            }
           
        }

        static void GenerateTileMap(string imgPath, string savePath, int size)
        {
            List<Image> images = new List<Image>();

            DirectoryInfo d = new DirectoryInfo(imgPath);
            foreach (var file in d.GetFiles("*.png"))
            {
                images.Add(Image.FromFile(imgPath + "/" + file));
                Console.WriteLine("File Found : " + file);
            }

            Console.WriteLine("Checking Image sizes........");

            bool failed = false;
            foreach(Image i in images)
            {
                Console.WriteLine(i);
                if(i.Height > images[0].Height || i.Height < images[0].Height)
                {
                    Console.WriteLine("image size differs from other images");
                }

                if(i.Width == i.Height)
                {
                    Console.WriteLine("Width: " + i.Width + " Is Equal to Height: " +i.Height);
                }
                else
                {
                    Console.WriteLine("Width: " + i.Width + " Is not Equal to Height: " + i.Height);
                }
            }

            int imageTotalSize = images[0].Height * images.Count;

            Console.WriteLine("Total requested image size (" + images[0].Height + " * " + images.Count + ") = " + imageTotalSize);
            Console.WriteLine("Total image size (" + size + " * " + size + ") = " + size * size);

            if (imageTotalSize > size * size) 
            {
                Console.WriteLine("Image size is not OK");
                failed = true;
            } 
            else
            {
                Console.WriteLine("Image size is OK");
            }

            if (failed)
            {
                Console.WriteLine("Image size check failed, press any key to terminate program");
                Console.ReadKey();
                return;
            }

            //Create Image
            Console.WriteLine("Creating Image");

            Bitmap image = new Bitmap(size,size);
            int x = 0;
            int y = 0;
      
            for(int i = 0; i < images.Count; i++)
            {
                Console.WriteLine("Adding Image : " + i + " x: " + x + " y: " + y);

                Bitmap iImage = new Bitmap(images[i], images[i].Height, images[i].Width);
                Color pixCol = new Color();

                for (int xx = 0; xx < iImage.Width; xx++)
                {
                    for (int yy = 0; yy < iImage.Height; yy++)
                    {
                        pixCol = iImage.GetPixel(xx,yy);
                        image.SetPixel(x + xx, y + yy,pixCol);
                    }
                }

                x += images[0].Width;

                if (x >= size)
                {
                    x = 0;
                    y += images[0].Height;

                    if(y >= size)
                    {
                        y = 0;
                        Console.WriteLine("Max amount of images added to tilemap, File has been saved but some images are not added(try higher scale), press any key to terminate program");
                        image.Save(savePath + "/" + "TileSheet.png", System.Drawing.Imaging.ImageFormat.Png);
                        Console.ReadKey();
                        return;
                    }
                }
            }
            image.Save(savePath + "/" + "TileSheet.png", System.Drawing.Imaging.ImageFormat.Png);

            Console.WriteLine("\n" + "Operation completed, press any key to terminate program");
            Console.ReadKey();

        }
    }
}
