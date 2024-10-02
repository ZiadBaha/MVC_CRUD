using Microsoft.AspNetCore.Http;
using System;
using System.IO; // for files

namespace Demo.PL.Helper
{
    public class DocumentSettings
    {
        //Return path of file at server
        public static string UploadFile(IFormFile file, string folderName)
        {
            
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath=Path.Combine(folderPath, fileName);
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName; 
             

        }

        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
           if (File.Exists(filePath))
                File.Delete(filePath);
            
        
        }
    }
}
