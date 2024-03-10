using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : Editor
{
    [MenuItem("Tools/ EntryScene")]
    public static void LoadEntryScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/EntryScene.unity");
    }
    [MenuItem("Tools/ Level1Scene")]
    public static void LoadLevel1()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level_1_Parts/Part1.unity");
    }
}
