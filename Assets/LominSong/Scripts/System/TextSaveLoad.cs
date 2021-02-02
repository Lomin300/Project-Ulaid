using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextSaveLoad : MonoBehaviour
{
 
    public string source; //읽어낸 텍스트 할당받는 변수

    /*
    public void WriteData(string strData)
    {
        // FileMode.Create는 덮어쓰기.
        FileStream f = new FileStream(Application.dataPath + "/SpaceX" + "/" + "text.txt", FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
        writer.WriteLine(strData);
        writer.Close();
    }

    public void ReadData()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/SpaceX" + "/" + "text.txt");
        source = sr.ReadLine();
        sr.Close();
    }*/

    public void Write(string writeDatas)
    {
        //File.Exists(Application.dataPath + "/SpaceX" + "/" + "text.txt");

        FileStream file = File.Create(Application.dataPath + "/InventorySystem/SpaceX" + "/" + "text.txt");

        file.Close();

        File.WriteAllText(Application.dataPath + "/InventorySystem/SpaceX" + "/" + "text.txt", writeDatas);
    }

    public void Read()
    {
        source = File.ReadAllText(Application.dataPath + "/InventorySystem/SpaceX" + "/" + "text.txt");
    }
}
