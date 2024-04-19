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
        
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), EditorSceneManager.GetActiveScene().path);
        EditorSceneManager.OpenScene("Assets/Scenes/EntryScene.unity");
    }
    [MenuItem("Tools/ Level1Scene")]
    public static void LoadLevel1()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), EditorSceneManager.GetActiveScene().path);
        EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level_1_Parts/Part1.unity");
    }
}
