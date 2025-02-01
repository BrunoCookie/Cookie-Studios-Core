using UnityEngine;

namespace SaveSystem
{
    public class SavegameManager : MonoBehaviour
    {
        public static SavegameManager instance;

        public SaveGame savegame;
    
        [HideInInspector]
        public int saveID = 1;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        
        }

        public static void SaveGame()
        {
            global::SaveSystem.SaveSystem.SaveTheGame(instance.savegame, instance.saveID);
        }

        public void SaveThisGame(int id)
        {
            global::SaveSystem.SaveSystem.SaveTheGame(savegame, id);
        }

        public void LoadThisGame(int ID)
        {
            SaveGame sg = global::SaveSystem.SaveSystem.LoadTheGame(ID);
            saveID = ID;
            savegame = sg;
        }
    }
}
