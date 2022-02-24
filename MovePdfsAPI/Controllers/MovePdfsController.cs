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
        static string CWD = AppContext.BaseDirectory;
        static string received_pdfs = Path.Combine(CWD, "received-pdfs");
        static string python_scripts = Path.Combine(CWD, "python-scripts");
        string py_script = Path.Combine(python_executable, "HtmlToDB.py");
        static string output_html = Path.Combine(python_scripts, "output-html");
        static string output_json = Path.Combine(python_scripts, "output-json");
        static string python_executable = "C:\\Program Files\\Python38\\python.exe";
        
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
                string filename = file.FileName.Replace(".pdf","");
                string filePath = Path.Combine(received_pdfs, file.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                string output_folder = worker_obj.Convert(filePath, output_html);

                //      RunPython.execPython(); (python_exec_path, script, filepath, json_out, hospital)
                return Ok(RunPython.execPython(python_executable, py_script, Path.Combine(output_html,filename,filename+".html"), output_json, hospital_name));
            }
            else
            {
                BadRequest("Not a valid pdf file.");
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
