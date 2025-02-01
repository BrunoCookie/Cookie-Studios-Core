using System.Collections;
using Audio;
using Menu;
using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelLoader
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader instance;
        public bool playStartAnimation;
        public bool saveOnLevelExit;
        public Animator transition;
        public float transitionTime = 2f;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            if (playStartAnimation)
            {
                transition.SetTrigger("Beginning");
                AudioManager.instance.Play("LevelEntry");
            }
        }

        public void LoadLevelIndex(int index)
        {
            if (MenuManager.instance != null) MenuManager.instance.SetCanPause(false);
            if (index == -1)
            {
                StartCoroutine( CloseGame() );
                return;
            }
        
            if (saveOnLevelExit) SavegameManager.SaveGame();
            AudioManager.instance.ClearAudioAdditions();
            StartCoroutine( LoadLevel(index) );
        }

        public void ReloadLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        IEnumerator LoadLevel(int index)
        {
            transition.SetTrigger("Start");
            AudioManager.instance.Play("LevelExit");

            yield return new WaitForSecondsRealtime(transitionTime);
            Time.timeScale = 1f;
            SceneManager.LoadScene(index);
        }

        IEnumerator CloseGame()
        {
            transition.SetTrigger("Start");
            AudioManager.instance.Play("LevelExit");
            yield return new WaitForSecondsRealtime(transitionTime);
            Debug.Log("Exiting Game...");
            Application.Quit();
        }
    }
}
