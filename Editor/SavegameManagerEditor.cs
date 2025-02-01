using SaveSystem;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomEditor(typeof(SavegameManager))]
    public class SavegameManagerEditor : UnityEditor.Editor
    {
        private int selectedSaveFile = 0;
        private string[] indexStrings = { "1", "2", "3" };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SavegameManager myTarget = (SavegameManager)target;
            selectedSaveFile = GUILayout.SelectionGrid(selectedSaveFile, indexStrings, 3);
            if (GUILayout.Button("Save"))
            {
                myTarget.SaveThisGame(selectedSaveFile+1);
            }
            if (GUILayout.Button("Load"))
            {
                myTarget.LoadThisGame(selectedSaveFile+1);
            }
        }
    }
}