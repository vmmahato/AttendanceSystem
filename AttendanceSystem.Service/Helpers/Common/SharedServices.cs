using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace AttendanceSystem.Helpers
{
   public static class SharedServices
    {

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }


        public static DateTime ConvertDate(string Date)
        {
            return Convert.ToDateTime(Date);
        }

        public static string AgeGroup(int Age)
        {
            if (Age > 0 && Age <= 15)
            {
                return "0-15";
            }
            else if (Age >= 16 && Age <= 30)
            {
                return "16-30";
            }
            else if (Age >= 31 && Age <= 45)
            {
                return "13-45";
            }
            else if (Age >= 46 && Age <= 60)
            {
                return "46-60";
            }
            else if (Age > 60)
            {
                return "60+";
            }
            else
            {
                return "Not Sp.";
            }
        }
        public static void DeleteFiles(string DirectoryPath)
        {
            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                if (Directory.Exists(DirectoryPath))
                {
                    foreach (string fileToDelete in System.IO.Directory.GetFiles(DirectoryPath))
                    {
                        System.IO.File.Delete(fileToDelete);
                    }
                    System.IO.Directory.Delete(DirectoryPath, true);
                }
            }
        }
        public static bool CheckFileLength(IFormFile File)
        {
            if ((File.Length) / 1024 <= 1024) { return true; }
            else
            {
                return false;
            }
        }

        public static TimeSpan? ConvertStringToTimeSpan(string Time)
        {
            if (!string.IsNullOrEmpty(Time))
            {
                return  TimeSpan.Parse(Time);
            }
            else
            {
                return null;
            }
        }

        public static string ConvertTimeSpanToString(TimeSpan? Time)
        {
            if (Time != null)
            {
                var NewDate = new DateTime();
                NewDate = (DateTime.Today + Time.Value);
                return NewDate.ToString("HH:mm", CultureInfo.InvariantCulture);
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
