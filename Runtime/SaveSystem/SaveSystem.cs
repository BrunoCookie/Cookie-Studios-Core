using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveSystem
    {
        public static void SaveTheGame (SaveGame savegame, int saveID)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = GetSaveFilePath(saveID);
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, savegame);
            stream.Close();
        }

        public static SaveGame LoadTheGame(int saveID)
        {
            string path = GetSaveFilePath(saveID);
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                SaveGame game = (SaveGame)formatter.Deserialize(stream);
                stream.Close();

                return game;
            }
        
            Debug.Log("Save file " + path + " created!");
            SaveTheGame(new SaveGame(), saveID);
            return LoadTheGame(saveID);
        }

        public static bool SaveFileExists(int saveID)
        {
            string path = GetSaveFilePath(saveID);
            return File.Exists(path);
        }

        public static void DeleteSaveFile(int saveID)
        {
            if (!SaveFileExists(saveID))
            {
                Debug.LogWarning("File you wanted to delete does not exist.");
                return;
            }

            string path = GetSaveFilePath(saveID);
            File.Delete(path);
        }

        private static string GetSaveFilePath(int saveID)
        {
            return Application.persistentDataPath + "/save" + saveID + ".save";
        }
    }
}
