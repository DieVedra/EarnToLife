using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinarySave : ISaveMetod
{
    private BinaryFormatter _binaryFormatter = new BinaryFormatter();
    public object Load(string path)
    {
        if (File.Exists(path) == true)
        {
            using (FileStream file = new FileStream(path, FileMode.Open))
            {
                Debug.Log("Load File");

                return _binaryFormatter.Deserialize(file);
            }
        }
        else
        {
            Debug.Log("File is null");
            return null;
        }
    }

    public void Save(string path, object data)
    {
        using (FileStream file = new FileStream(path, FileMode.Create))
        {
            Debug.Log("Save File");

            _binaryFormatter.Serialize(file, data);
        }
    }
}
