using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "scene_setup", menuName = "Tools/SceneSetup")]
public class SceneSetupData : ScriptableObject {
	
	[System.Serializable]
	public struct SetupData {
		public int Index;
		public string Name;
		public SceneSetup[] Setup;
	}

	public SetupData[] Setup;
}

#endif
