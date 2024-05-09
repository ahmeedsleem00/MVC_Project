using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            // 1 - Get location Folder path
           
            
            // string folderPath = Directory.GetCurrentDirectory() + "wwwroot\\files\\" + folderName;
          
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2 - Get file name make it unique

            string fileName = $"{Guid.NewGuid()}{file.FileName}";


            // 3 - Get File Path -->Folder Path + File Name

            string filePath = Path.Combine(folderPath,fileName);


            // 4 - Save file as stream : Data per time

            using var fileStream = new FileStream(filePath,FileMode.Create);

            file.CopyTo(fileStream);
            return fileName;
        }
    

       public static void DeleteFile(string fileName,string folderName)
       {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName,fileName);
          
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
       }
    }
}
