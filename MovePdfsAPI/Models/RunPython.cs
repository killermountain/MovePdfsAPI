using System.Diagnostics;

namespace MovePdfsAPI.Models
{
    public static class RunPython
    {

        public static string execPython(string python_exec_path, string script, string filepath, string json_out, string hospital) 
        {
            ProcessStartInfo start = new ProcessStartInfo();


            start.FileName = python_exec_path;  // "C:\\Program Files\\Python38\\python.exe";
            //var script = "D:\\VS.NetApps\\MovePdfs\\MovePdfsAPI\\bin\\Debug\\net5.0\\python-scripts\\HtmlToDB.py";
            //var filepath = "D:\\VS.NetApps\\MovePdfs\\MovePdfsAPI\\bin\\Debug\\net5.0\\python-scripts\\output-html\\admission\\admission.html";
            //var json_out = "D:\\VS.NetApps\\MovePdfs\\MovePdfsAPI\\bin\\Debug\\net5.0\\python-scripts\\output-json\\";
            //var hospital = "MSE";


            start.Arguments = string.Format("{0} {1} {2} {3}", script, filepath, hospital, json_out);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;

            string output = "";
            string error = "";

            using (Process process = Process.Start(start))
            {
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }

            if (output.Contains("done!"))
                return output;
            else
                return error;
        }
        
    }
}
