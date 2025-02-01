using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Language
{
    public class LanguageManager : MonoBehaviour
    {
        private static LanguageManager instance;
        private static Dictionary<string, TextAsset> langDictionary = new Dictionary<string, TextAsset>();
        private static JObject selectedLanguage;
        public static event EventHandler onLanguageChanged;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            LoadLanguageFiles();

            string currentLanguage = PlayerPrefs.GetString("lang");
            if (currentLanguage == "")
            {
                PlayerPrefs.SetString("lang", "lang_en");
                currentLanguage = "lang_en";
            }
            SetLanguage(currentLanguage);

            onLanguageChanged += (sender, args) =>
                Debug.Log("Language set to: " + GetLanguageName(selectedLanguage));
        }

        public static void SetLanguage(string id)
        {
            selectedLanguage = JObject.Parse(langDictionary[id].text);
            PlayerPrefs.SetString("lang", id);
            onLanguageChanged?.Invoke(instance, EventArgs.Empty);
        }

        public static string GetToken(string token)
        {
            if (selectedLanguage == null)
            {
                LoadLanguageFiles();
                SetLanguage("lang_en");
            }

            string[] tokens = token.Split('.');
            if (!selectedLanguage.ContainsKey(tokens[0])) return GetBackupToken(token);

            JToken returnValue = selectedLanguage.SelectToken(tokens[0]);
            for (int i = 1; i < tokens.Length; i++)
            {
                if (!JObject.Parse(returnValue.ToString()).ContainsKey(tokens[i])) return GetBackupToken(token);
                returnValue = returnValue?.SelectToken(tokens[i]);
            }

            return returnValue?.Value<string>();
        }

        private static string GetBackupToken(string token)
        {
            Debug.LogWarning("[LANG] Could not find word '" + token + "' in language '" + GetLanguageName(selectedLanguage) + "'");
            JObject englishLang = JObject.Parse(langDictionary["lang_en"].text);
            JToken returnValue;
            string[] tokens = token.Split('.');
            if (!englishLang.ContainsKey(tokens[0])) return "";

            returnValue = englishLang.SelectToken(tokens[0]);
            for (int i = 1; i < tokens.Length; i++)
            {
                if (!JObject.Parse(returnValue.ToString()).ContainsKey(tokens[i])) return "";
                returnValue = returnValue?.SelectToken(tokens[i]);
            }

            return returnValue?.Value<string>();
        }

        private static string GetLanguageName(JObject language)
        {
            return language.SelectToken("lang_name")?.Value<String>();
        }

        private static void LoadLanguageFiles()
        {
            // Find all JSON files in the directory
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Language");
            foreach (var json in jsonFiles)
            {
                langDictionary.Add(json.name, json);
            }
        }
    }
}
