using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using FinalTask;
using System.Text;

namespace Module8_FinalProject_4
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = string.Empty;
            if (args.Length > 0)
            {
                filePath = args[0];
            }
            else
            {
                Console.WriteLine("Аргумент прогроммы filePath не задан.");
                return;
            }


            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Путь не указан");
                return;
            }

            List<Student> studentList = null;
            FileInfo fileInfo = new FileInfo(filePath);
            try
            {
                studentList = GetStudents(fileInfo);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            DirectoryInfo directoryInfoDesktop = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            DirectoryInfo directoryInfoStudent = directoryInfoDesktop.CreateSubdirectory("Students");

            foreach (var group in studentList.GroupBy(s => s.Group))
            {
                string contentFile = GetContentFile(group.Cast<Student>().ToList());
                try
                {
                    CreateFile(directoryInfoStudent, group.Key, contentFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
        }


        private static List<Student> GetStudents(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
            {
                throw new Exception($"Файл ({fileInfo.FullName}) не существует");
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (var fs = new FileStream(fileInfo.FullName, FileMode.OpenOrCreate))
            {
                var students = (Student[])formatter.Deserialize(fs); 
;               return students.ToList();
            }
        }

        public static string GetContentFile(List<Student> sudentList)
        {
            StringBuilder result = new StringBuilder();
            int count = 0;
            foreach (Student studetnt in sudentList)
            {
                count += 1;
                result.AppendLine($"{count}. {studetnt.Name} {studetnt.DateOfBirth}");
            }

            return result.ToString();
        }

        public static void CreateFile(DirectoryInfo directoryTarget,string fileName, string contentFile)
        {
            FileInfo fi = new FileInfo(directoryTarget.FullName + "\\" + fileName + ".txt");
            
            using (StreamWriter sw = fi.CreateText())
            {
                sw.Write(contentFile);
                Console.WriteLine($"Файл {fi.FullName} создан");
            }
        }
    }
}
