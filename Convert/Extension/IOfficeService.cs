using System.Threading.Tasks;

namespace Convert.Extension
{
    public interface IOfficeService
    {
        void ConvertToPdf(string filePath, string outDir);

        Task PdfToImage(string fileName, string filePath, string outDir);
    }
}