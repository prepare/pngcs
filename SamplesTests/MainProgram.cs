using SampleTests;
using System;
using System.Collections.Generic;
using System.Text;
using Hjg.Pngcs;


namespace SamplesTests
{

    class MainProgram
    {
        
        static void Main(string[] args)
        {



            Test1();

            long t0 = Environment.TickCount;
            //testX();
            myTestSuite();
            //testTextChunks();
            long t1 = Environment.TickCount;

            Console.Out.WriteLine("Done. (" + (t1 - t0) + " msecs) " + "Net version: " + Environment.Version + " Press ENTER to close");
            Console.In.ReadLine();
        }

        static void testX()
        { // placeholder method for misc tests
            PngReader png = FileHelper.CreatePngReader("C:/temp/map.png");
            Console.Out.WriteLine(png);
        }

        static void myTestSuite()
        {
            testSuite(new string[] { "d:/devel/repositories/pnjgs/pnjg/resources/testsuite1/", "D:/temp/testcs" });
        }

        /// <summary>
        /// textual chunks
        /// </summary>
        static void testTextChunks()
        {
            TestTextChunks.test();
        }

        static void Test1()
        {
            using (System.IO.FileStream fs = new System.IO.FileStream("d:\\WImageTest\\a01_4_1.png", System.IO.FileMode.Open))
            {
                PngReader reader1 = new PngReader(fs);
                var imgInfo = reader1.ImgInfo;
                int j = imgInfo.Rows;
                using (System.Drawing.Bitmap newBMP = new System.Drawing.Bitmap(imgInfo.Cols, imgInfo.Rows, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    System.Drawing.Imaging.BitmapData data = newBMP.LockBits(
                        new System.Drawing.Rectangle(0, 0, imgInfo.Cols, imgInfo.Rows),
                         System.Drawing.Imaging.ImageLockMode.ReadWrite,
                          newBMP.PixelFormat);

                    byte[] buffer = new byte[imgInfo.Cols * 4 * imgInfo.Rows];

                    int rowLen = data.Stride;
                    int targetpos = 0;
                    for (int i = 0; i < j; ++i)
                    {
                        ImageLine imgline = reader1.ReadRowByte(i);

                        byte[] scanlineBytes = imgline.ScanlineB;
                        //copy
                        System.Buffer.BlockCopy(
                            scanlineBytes, 0,
                            buffer, targetpos, scanlineBytes.Length);
                        targetpos += rowLen;
                    }
                    System.Runtime.InteropServices.Marshal.Copy(
                        buffer,
                        0,
                        data.Scan0, buffer.Length);

                    newBMP.UnlockBits(data);
                    newBMP.Save("d:\\WImageTest\\pngcs1.png");
                }
            }
        }
        static void sampleShowChunks(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("expected [inputfile]");
                return;
            }
            SampleShowChunks.showChunks(args[0]);
        }

        static void sampleConvertTrueColor(string file)
        {
            SampleConvertToTrueCol.doit(file);
        }


        static void sampleMirror(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("expected [inputfile] [outputfile]");
                return;
            }
            SampleMirrorImage.mirror(args[0], args[1]);
            Console.Out.WriteLine("sampleMirror done " + args[0] + " ->" + args[1]);
        }

        static void decreaseRed(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("expected [inputfile] [outputfile]");
                return;
            }
            SampleDecreaseRed.DecreaseRed(args[0], args[1]);
            Console.Out.WriteLine("decreaseRed done " + args[0] + " ->" + args[1]);
        }

        static void customChunk(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("expected [inputfile] [outputfile]");
                return;
            }
            Console.Out.WriteLine("custom chunk write : " + args[0] + " ->" + args[1]);
            SampleCustomChunk.testWrite(args[0], args[1]);
            Console.Out.WriteLine("custom chunk read: " + args[1]);
            SampleCustomChunk.testRead(args[1]);
        }

        static void testSingle(string file)
        {
            TestPngSuite.testSingle(file, null, null);
        }


        static void testSuite(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("expected [origdir] [destdir] [maxfiles]");
                return;
            }
            int maxfiles = args.Length < 3 ? 0 : int.Parse(args[2]);
            TestPngSuite.testAllSuite(args[0], args[1], maxfiles);
        }

    }
}
