using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace Convert.Extension
{
    public class LibreOfficeService : IOfficeService
    {
        public void ConvertToPdf(string filePath, string outDir)
        {
            var libreOfficePath = "/opt/libreoffice7.1/program/soffice";

#if DEBUG
            libreOfficePath = @"C:\Program Files\LibreOffice\program\soffice.exe";
#endif

            var procStartInfo = new ProcessStartInfo(libreOfficePath,
                $" --invisible --convert-to pdf  {filePath} --outdir {outDir}")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Environment.CurrentDirectory
            };

            //开启线程
            var process = new Process() {StartInfo = procStartInfo};
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception();
            }
        }

        public async Task PdfToImage(string fileName, string filePath, string outDir)
        {
            var settings = new MagickReadSettings();
            using (var images = new MagickImageCollection())
            {
                await images.ReadAsync(filePath, settings);
                int pageCount = images.Count;
                for (int i = 0; i < pageCount; i++)
                {
                    var image = images[i];
                    image.Format = MagickFormat.Png;
                    //image.Density = new Density(1000);
                    image.Quality = 100;
                    string imgPath = $"{fileName}_{i}.png"; //相对路径   
                    string filename = Path.Combine(outDir, imgPath);
                    await image.WriteAsync(filename);
                }
            }
        }
    }
}
