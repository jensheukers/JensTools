using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TileMapToImage
{
    class Program //TODO: MAKE PROGRAM RUN FROM PARAMETERS
    {
        static void Main(string[] args)
        {
            float version = 0.5f;
            Console.WriteLine("Version " + version  + " - (Fixed memory leak, user can now write tiles up to *any size*)");
            Console.WriteLine("Version " + version + " - (Make sure that tilemap is to math 2 IE: (248x248,512x512))");
            Console.WriteLine("Version " + version + " - (If any help is required send a E-mail to: EMAIL)");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("Enter Path to TileMap.png:");
            string imgPath = Console.ReadLine();
            Console.WriteLine("Enter Save location:");
            string savePath = Console.ReadLine();
            Console.WriteLine("Enter tile size");
            string size = Console.ReadLine();
            
            Console.WriteLine("Would you like to save images with transparency too(To be fixed soon) - yes / no");
            string yesno = Console.ReadLine();

            if(yesno == "yes")
            {
                GenerateImages(imgPath, savePath, int.Parse(size), true);
            }
            else if(yesno == "no")
            {
                GenerateImages(imgPath, savePath, int.Parse(size), false);
            }
            else
            {
                Console.WriteLine("Wrong input given, saving images with transparency too");
                GenerateImages(imgPath, savePath, int.Parse(size), true);
            }
        }

        static void GenerateImages(string imgPath, string savePath, int size,bool transparency)
        {

            if(!File.Exists(imgPath))
            {
                Console.Write("No file found on " + imgPath + ", Press any key to terminate the program");
                Console.ReadKey();
                return;
            }

            if(!Directory.Exists(savePath))
            {
                Console.Write("No Directory found on " + savePath + ", Press any key to terminate the program");
                Console.ReadKey();
                return;
            }

            int x;
            int y;
            int xx;
            int yy;
            int tileNum = 0;
            Bitmap tileMap = new Bitmap(Image.FromFile(imgPath));

            for (x = 0; x < tileMap.Width; x += size)
            {
                for(y = 0; y < tileMap.Height; y += size)
                {
                    Console.WriteLine("\n" + "Converting tile" + tileNum);
                    Bitmap tile = new Bitmap(size,size);
                    Color col = new Color();

                    for (xx = 0; xx < size; xx++)
                    {
                        for (yy = 0; yy < size; yy++) {
                            //Console.Write(xx + "," + yy + "|");
                            col = tileMap.GetPixel(x + xx, y + yy);
                            tile.SetPixel(xx, yy, col);
                        }
                    }
                    if(col.A != 0) {
                        Console.WriteLine("Saving Tile : " + tileNum + " From : " + imgPath + " --> " + savePath);
                        tile.Save(savePath + "/" + "tile" + tileNum + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        tile = null;
                        tileNum += 1;
                    }
                    else
                    {
                        if(!transparency) {
                            Console.WriteLine("Tile is transparent, skipping....");
                            tile = null;
                        }
                        else
                        {
                            Console.WriteLine("Saving Tile : " + tileNum + " From : " + imgPath + " --> " + savePath);
                            tile.Save(savePath + "/" + "tile" + tileNum + ".png", System.Drawing.Imaging.ImageFormat.Png);
                            tile = null;
                            tileNum += 1;
                        }
                    }
                }
            }
            
            Console.WriteLine("\n" + "Operation completed, press any key to terminate program");
            Console.ReadKey();
        }
    }
}
