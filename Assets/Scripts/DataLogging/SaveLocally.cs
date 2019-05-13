using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveLocally : MonoBehaviour
{
    private static string path = Application.persistentDataPath + "/";

    public static void SaveScoreData(QuestionHazardData data, string email){
        string m_localDataPath;

        DirectoryInfo dI = new DirectoryInfo(path);
        FileInfo[] files = dI.GetFiles("*.xml");

        int count = 0;
        foreach(FileInfo file in files)
            count++;

        m_localDataPath = (count + 1).ToString("D4") + "_" + email+".xml";

        //Serialize 
        XmlSerializer xs = new XmlSerializer(typeof(QuestionHazardData));
        TextWriter tw = new StreamWriter(path+m_localDataPath);
        xs.Serialize(tw, data);
        tw.Close();
    }

    public static QuestionHazardData LoadScoreData(string fileName){
        var result = new QuestionHazardData();
        XmlSerializer xs = new XmlSerializer(typeof(QuestionHazardData));

        using(var sr = new StreamReader(path + fileName))
        {
            result = (QuestionHazardData)xs.Deserialize(sr);
            sr.Close();
        }

        return result;
    }

    public static void DeleteFile(string fileName){
        File.Delete(fileName);
    }
}
