using System;
using Aspose.Words;
using System.IO;

namespace MovePdfsAPI.Models
{
    public class PdfToHtml
    {
        public PdfToHtml() 
        {
            initializeLicense();
        }

        private bool initializeLicense()
        {
            License license = new License();
            
            try
            {
                license.SetLicense("Aspose.Words.NET.lic");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string Convert(string file, string output_html_dir) 
        {
            if (!Directory.Exists(output_html_dir))
                Directory.CreateDirectory(output_html_dir);

            string filename = Path.GetFileName(file);
            string output_dir = Path.Combine(output_html_dir, filename.Replace(".pdf", "")) ;

            if (!Directory.Exists(output_dir))
                Directory.CreateDirectory(output_dir);

            string output_file = output_dir + "\\" + filename.Replace(".pdf", ".html");

            try
            {
                var doc = new Document(file);
                doc.Save(output_file);
                File.Delete(file);
            }
            catch (Exception ex)
            {
                return "-1";
            }
            return output_dir;
        }
    }
}
