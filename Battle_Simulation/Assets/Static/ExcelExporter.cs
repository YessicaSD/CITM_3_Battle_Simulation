using UnityEngine;
using System.IO;

public static class ExcelExporter
{
    private static string folderName = "Report";
    private static string reportFileName = "BattleInfo.csv";
    private static string reportSeparator = ","; // CSV separator for columns

    private static string[] reportHeaders = new string[8]
   {
        "Character name",
        "Hp",
        "Strenght",
        "Speed",
        "Num Wins",
        "Num Defeats",
        "Num simulations",
        "TimeStamp"
   };

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + folderName;
    }

    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }
    static void CheckDirectory()
    {
        string dir = GetDirectoryPath();

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void CheckFile()
    {
        string file = GetFilePath();

        if (!File.Exists(file))
        {
            CreateReport();
        }
    }

    public static void AppendToReport(string[] strings)
    {
        CheckDirectory();
        CheckFile();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }
            finalString += reportSeparator + GetTimeStamp();
            sw.WriteLine(finalString);
        }
    }

    public static string GetTimeStamp()
    {
        return System.DateTime.UtcNow.ToString();
    }

    public static void CreateReport()
    {
        CheckDirectory();

        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < reportHeaders.Length; ++i)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }

            sw.WriteLine(finalString);
        }
    } 
     

}
