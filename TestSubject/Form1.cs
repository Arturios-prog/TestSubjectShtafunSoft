using System.Diagnostics;

namespace TestSubject
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        //Simply returns the image
        public Image Img(string url)
        {
            string path = Directory.GetCurrentDirectory();
            path = path.Replace(@"\", @"/");
            path = path.Replace(@"bin/Debug/net6.0-windows", @"images/") + url;
            return new Bitmap(path);
        }

        //Displays Images
        public void DisplayImages(Row row)
        {
            List<Bitmap> Rowbitmaps = new List<Bitmap>();

            List<Bitmap> bitmaps = new List<Bitmap>();

            List<int> yPosPerRow = new List<int>();

            yPosPerRow.Add(0);

            Bitmap? finalImage = null;

            foreach (var o in row.objects)
            {
                if (o.GetType() == typeof(Bitmap))
                {
                    Rowbitmaps.Add((Bitmap)o);
                }

            }

            try
            {
                int minHeight = Rowbitmaps.Min(b => b.Height);

                if (Rowbitmaps == null) throw new Exception("There are no images");


                int width = 0;
                int height = 0;
                int rowHeight = 0;
                int rowWidth = 0;

                int index = 0;

                // Loop for each bitmap
                for (int x = 0; x < Rowbitmaps.Count; x++)
                {
                    // Should stop the loop if the bitmaps count is not
                    // exactly divisible for the colCount requested
                    if (index >= Rowbitmaps.Count)
                        break;

                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(Rowbitmaps[index]);

                    // recalculate the width for the current row

                    rowWidth += bitmap.Width;

                    rowHeight = bitmap.Height > rowHeight ? bitmap.Height : rowHeight;
                    bitmaps.Add(bitmap);
                    index++;


                    // Where to put in the Y axis the next row when merging
                    yPosPerRow.Add(height);

                }
                height = rowHeight;
                width = (rowWidth > width ? rowWidth : width);

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                index = 0;
                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    // Again loop over images

                    int offsetX = 0;
                    for (int y = 0; y < bitmaps.Count; y++)
                    {

                        // Exit if not exactly divisible
                        if (index >= bitmaps.Count)
                            break;

                        using (Bitmap image = bitmaps[index])
                        {

                            g.DrawImage(image,
                        new Rectangle(offsetX, yPosPerRow[y], image.Width, image.Height));
                            offsetX += image.Width;
                        }
                        index++;
                    }

                }

                pictureBox1.Image = finalImage;

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();
                throw;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap bitmap in bitmaps)
                {
                    bitmap.Dispose();
                }
            }
        }

        //Main Function
        public void DrawStoryBoard(Row row, int totalWidth)
        {
            List<Bitmap> Rowbitmaps = new List<Bitmap>();

            List<Bitmap> bitmaps = new List<Bitmap>();

            List<int> yPosPerRow = new List<int>();

            yPosPerRow.Add(0);

            Bitmap? finalImage = null;

            foreach (var o in row.objects)
            {
                if (o.GetType() == typeof(Bitmap))
                {
                    Rowbitmaps.Add((Bitmap)o);
                }

            }

            try
            {
                int minHeight = Rowbitmaps.Min(b => b.Height);

                if (Rowbitmaps == null) throw new Exception("There are no images");

                Bitmap imageWithMinHeight = Rowbitmaps.FirstOrDefault(b => b.Height == minHeight);

                if (imageWithMinHeight == null)
                {
                    throw new Exception("Image with min height is null");
                }

                int width = 0;
                int height = 0;

                int rowWidth = 0;

                int index = 0;

                // Loop for each bitmap
                for (int x = 0; x < Rowbitmaps.Count; x++)
                {
                    // Should stop the loop if the bitmaps count is not
                    // exactly divisible for the colCount requested
                    if (index >= Rowbitmaps.Count)
                        break;

                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(Rowbitmaps[index]);

                    // recalculate the width for the current row
                    if (bitmap.Height != minHeight)
                    {
                        double bitmapRatio = (double)minHeight / (double)bitmap.Height;

                        rowWidth += (int)Math.Round(bitmap.Width * bitmapRatio);
                    }
                    else
                    {
                        rowWidth += bitmap.Width;
                    }

                    bitmaps.Add(bitmap);
                    index++;


                    // Where to put in the Y axis the next row when merging
                    yPosPerRow.Add(height);

                }
                height = minHeight;
                width = (rowWidth > width ? rowWidth : width);

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                index = 0;
                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    // Again loop over images

                    int offsetX = 0;
                    for (int y = 0; y < bitmaps.Count; y++)
                    {

                        // Exit if not exactly divisible
                        if (index >= bitmaps.Count)
                            break;

                        using (Bitmap image = bitmaps[index])
                        {
                            if (image.Height != minHeight)
                            {
                                double bitmapRatio = (double)minHeight / (double)image.Height;
                                g.DrawImage(image,
                                new Rectangle(offsetX, yPosPerRow[y],
                                (int)Math.Round(image.Width * bitmapRatio), (int)Math.Round(image.Height * bitmapRatio)));
                                offsetX += (int)Math.Round(image.Width * bitmapRatio);
                            }
                            else
                            {
                                g.DrawImage(image,
                            new Rectangle(offsetX, yPosPerRow[y], image.Width, image.Height));
                                offsetX += image.Width;
                            }

                        }
                        index++;
                    }

                }

                pictureBox2.Image = finalImage;

                double finalImageRatio = (double)finalImage.Width / (double)totalWidth;

                if (finalImageRatio >= 1)
                {
                    pictureBox2.Size = new Size((int)Math.Round(finalImage.Width / finalImageRatio), (int)Math.Round(finalImage.Height / finalImageRatio));
                    Console.WriteLine("> 1");
                }
                else
                {
                    pictureBox2.Size = new Size((int)Math.Round(finalImage.Width * finalImageRatio), (int)Math.Round(finalImage.Height * finalImageRatio));
                }
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();
                throw;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap bitmap in bitmaps)
                {
                    bitmap.Dispose();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Row r1 = new Row();

            /* If for some reason "relative" path for images is not working please use the actual path to the images */
            r1.Add(Img("img1.jpg")).Add(Img("img2.jpg")).Add(Img("img3.jpg")).Add(Img("img4.jpg"));
            
            DisplayImages(r1);

            DrawStoryBoard(r1, 800);

        }
    }
}