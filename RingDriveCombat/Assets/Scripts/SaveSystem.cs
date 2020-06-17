
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveSettings(GameSettings gameSettings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        GameSave gameSave = new GameSave(gameSettings);
        formatter.Serialize(stream, gameSave);
        stream.Close();
    }

    public static GameSave LoadGameSave()
    {
        string path = Application.persistentDataPath + "/game.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameSave gameSave =  formatter.Deserialize(stream) as GameSave;
            stream.Close();
            return gameSave;
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }
}
