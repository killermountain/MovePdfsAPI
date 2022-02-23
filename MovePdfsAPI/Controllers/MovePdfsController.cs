using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovePdfsAPI.Models;
using System;
using System.IO;

namespace MovePdfsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovePdfsController : ControllerBase
    {
        PdfToHtml worker_obj = new PdfToHtml();
        string CWD = AppContext.BaseDirectory;

        public IActionResult GetPdf() 
        {
            return Ok("Pdf API running...");
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (file.Length > 0 && file.FileName.ToLower().EndsWith(".pdf"))
            {
                string filePath = Path.Combine(CWD, "received-pdfs", file.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                string output_folder = worker_obj.Convert(filePath, Path.Combine(CWD, "output-html"));
                
            }
            else
            {
                BadRequest();
            }
            return Ok("File ---> " + file.FileName + " <--- received.");
        }

        [HttpPost]
        [Route("uploadmultiple")]
        public IActionResult Upload(IFormFileCollection files) 
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string CWD = AppContext.BaseDirectory;
                    string filePath = Path.Combine(CWD, "received-pdfs", file.FileName);

                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                else
                {
                    BadRequest();
                }
            }
            return Ok("Total of ---> "+ files.Count + " <--- Files received.");
        }

        
    }
}
