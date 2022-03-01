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
        string py_script = Path.Combine(python_scripts, "HtmlToDB.py");
        static string output_html_folder = Path.Combine(python_scripts, "output-html");
        static string output_json = Path.Combine(python_scripts, "output-json");
        //--------------------------------- Enter Python Path here ------------------------------------
        static string python_executable = @"""C:\Program Files\Python38\python.exe""";
        //---------------------------------------------------------------------------------------------
        static string hospital_name = "MSE";


        public IActionResult GetPdf() 
        {
            return Ok("Pdf API running...");
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0 && file.FileName.ToLower().EndsWith(".pdf"))
            {
                if (!Directory.Exists(received_pdfs)) 
                { Directory.CreateDirectory(received_pdfs); }

                string filename = file.FileName.Replace(".pdf","");
                string filePath = Path.Combine(received_pdfs, file.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                string output_folder = worker_obj.Convert(filePath, output_html_folder);
                if (output_folder == "-1")
                { BadRequest("Unable to convert PDF to HTML."); }

                //      RunPython.execPython(); (python_exec_path, script, filepath, json_out, hospital)
                string output = RunPython.execPython(python_executable, py_script, Path.Combine(output_html_folder, filename, filename + ".html"), output_json, hospital_name);
                if (output.Contains("done!")) 
                {return Ok("File received, processed and saved into the database.");}
                else { return BadRequest("Error while parsing html."); }
                
            }
            else
            {
                return BadRequest("Not a valid pdf file.");
            }
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
