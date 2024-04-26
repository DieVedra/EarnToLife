using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Linq;

public class SceneSetupLoader : MonoBehaviour {
	static SceneSetupData data;
	
	[InitializeOnLoadMethod]
	public static void Init() {
		data = (SceneSetupData)AssetDatabase.LoadAssetAtPath("Assets/Settings/Tools/scene_setup.asset", typeof(SceneSetupData));
	}

	[MenuItem("Scenes/Entry Scene &0")]
	static void SceneSetup1()
	{
		Load(0);
	}

	[MenuItem("Scenes/Level1 Part1 &1")]
	static void SceneSetup2() 
	{
		Load(1);
	}

	[MenuItem("Scenes/Level1 Part2 &2")]
	static void SceneSetup3()
	{
		Load(2);
	}

	[MenuItem("Scenes/Level1 Part3 &3")]
	static void SceneSetup4()
	{
		Load(3);
	}

	// [MenuItem("Scenes/Scene Setup 5 &4")]
	// static void SceneSetup5() 
	// {
	// 	Load(4);
	// }
	//
	// [MenuItem("Scenes/Scene Setup 6 &5")]
	// static void SceneSetup6() 
	// {
	// 	Load(5);
	// }

	static void Load(int index)
	{
		EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), EditorSceneManager.GetActiveScene().path);
		if (data == null)
		{
			Init();
		}

		var config = data.Setup.FirstOrDefault(d => d.Index == index);
		if(config.Setup == null){
			Debug.LogError(string.Format("Scene Setup with Index {0} not configured. Plase add it to Assets/data/scene_setup", index));
		} else {
			EditorUtility.DisplayProgressBar("Loading scene Setup", "Please wait a bit, work in progress", 0);
			EditorSceneManager.RestoreSceneManagerSetup(config.Setup);
			EditorUtility.DisplayProgressBar("Loading scene Setup", "Please wait a bit, work in progress", 1);
			EditorUtility.ClearProgressBar();
		}
	}
}
