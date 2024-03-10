using System;
public interface ISaveMetod
{
    object Load(string path);
    void Save(string path, object data);
    
    //void Save(string key, object data, Action<bool> callBack = null);
    //void Load<T>(string key, Action<T> callBack);
}
