using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BlossomValley.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        static readonly string FILEPATH = Application.persistentDataPath + "/Save.save";

        public static void Save(GameSaveState save)
        {
            //Save as JSON
            //string json = JsonUtility.ToJson(save);
            //File.WriteAllText(FILEPATH, json); 

            //Save as Binary file
            using (FileStream file = File.Create(FILEPATH))
            {
                new BinaryFormatter().Serialize(file, save);
            }
        }

        public static GameSaveState Load()
        {
            GameSaveState loadedSave = null;

            //JSON 
            // if (File.Exists(FILEPATH))
            // {
            //     string json = File.ReadAllText(FILEPATH);
            //     loadedSave = JsonUtility.FromJson<GameSaveState>(json); 
            // }

            //Binary method
            if (File.Exists(FILEPATH))
            {
                using (FileStream file = File.Open(FILEPATH, FileMode.Open))
                {
                    object loadedData = new BinaryFormatter().Deserialize(file);
                    loadedSave = (GameSaveState)loadedData;
                }
            }
            return loadedSave;
        }

        public static bool HasSave() => File.Exists(FILEPATH);
    }
}