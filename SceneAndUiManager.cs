using System.Collections;
﻿using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using FairyGUI;

public class SceneAndUiManager : MonoBehaviour
{

	public SceneAndUiManager()
	{
	}
	static SceneAndUiManager _instance;
	
	public static SceneAndUiManager inst
	{
		get
		{
			if (_instance == null)
			{
				GameObject go = new GameObject("SceneAndUiManager");
				DontDestroyOnLoad(go);
				_instance = go.AddComponent<SceneAndUiManager>();
			}
			return _instance;
		}
	}
	
    Dictionary <string, string> levelScenesInterfacesIndex;
    Dictionary <string, int> _levelDictionary;
    Dictionary <string, int> _uiDictionary;
    List<GComponent> _sceneLevels;
	GComponent _sceneLoaderView;

	public void Init()
	{
		_sceneLoaderView = UIPackage.CreateObject("CutScene", "CutScene").asCom;
		_sceneLoaderView.SetSize(GRoot.inst.width, GRoot.inst.height);
		_sceneLoaderView.AddRelation(GRoot.inst, RelationType.Size);
		_sceneLevels = new List<GComponent>();
		_levelDictionary = new Dictionary<string,int>();
		_uiDictionary = new Dictionary<string,int>();
	}
    
    public void SetlevelScenesInterfacesIndex(Dictionary <string, string> _levelScenesInterfacesIndex)
    {
        levelScenesInterfacesIndex = _levelScenesInterfacesIndex;
        foreach(KeyValuePair<string, string> entry in levelScenesInterfacesIndex)
        {
            SetSceneForUse(entry.Key, entry.Value);
        }
    }
	
	void SetSceneForUse(string sceneName, string interfaceName)
	{
	    
	    if (!_uiDictionary.ContainsKey(interfaceName)) {
    	    int c = _uiDictionary.Count;
	        _uiDictionary[interfaceName] = c;
            _sceneLevels.Add(UIPackage.CreateObject("CutScene", interfaceName).asCom);
            _sceneLevels[c].SetSize(GRoot.inst.width, GRoot.inst.height);
            _sceneLevels[c].AddRelation(GRoot.inst, RelationType.Size);
        } else {    
    	    _levelDictionary[sceneName] = _uiDictionary[interfaceName];
        }
        
        Debug.Log(_uiDictionary);
        Debug.Log(_levelDictionary);
        Debug.Log(_sceneLevels);
        _sceneLevels[_uiDictionary[interfaceName]].GetChild(sceneName).onClick.Add(() =>
        {
	        LoadLevel(sceneName);
        });
	}

	public void LoadLevel(string levelName)
	{
		StartCoroutine(DoLoad(levelName));
		GRoot.inst.AddChild(_sceneLoaderView);
	}

	IEnumerator DoLoad(string sceneName)
	{
		GRoot.inst.AddChild(_sceneLoaderView);
		GProgressBar pb = _sceneLoaderView.GetChild("pb").asProgress;
		pb.value = 0;
#if UNITY_5_3_OR_NEWER
		AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
#else
		AsyncOperation op = Application.LoadLevelAsync(sceneName);
#endif
		float startTime = Time.time;
		while (!op.isDone || pb.value != 100)
		{
			int value = (int)((Time.time - startTime) * 100f / 3f);
			if (value > 100)
				value = 100;
			pb.value = value;
			yield return null;
		}

		GRoot.inst.RemoveChild(_sceneLoaderView);
		GRoot.inst.AddChild(_sceneLevels[_uiDictionary[levelScenesInterfacesIndex[sceneName]]]);
	}
}
