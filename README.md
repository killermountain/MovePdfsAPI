# MovePdfsAPI

.Net Core WebAPI is doing all the work of fetching pdfs, converting to html, parsing, making JSON, and storing into the Database.


1. Add Database credentials to the mysqlDB.py file in the python-scripts folder.
2. Add Python Executable path to MovePdfsController.cs file in Controllers folder.

  #----------------------------------------------------------------------------------#
  
PDF Upload REST API.

POST REQUEST PATH: http://localhost:53787/api/movepdfs/upload

FORMDATA:
  FILENAME: file
  Body: <pdf file>
  
EXAMPLE REQUEST in JAVASCRIPT

  fetch('http://localhost:53787/api/movepdfs/upload', 
  {
        method: "POST",
        headers: {
          'Content-Type': 'application/pdf'
        },
        body: formData
  });

  #----------------------------------------------------------------------------------#
