using System.Collections.Generic;
﻿using System.Collections;
using UnityEngine;
using FairyGUI;

/// <summary>
/// Demonstrated the simple flow of a game.
/// </summary>
public class SceneLoaderMain : MonoBehaviour
{

    public string UiPackage = "CutScene";
    public string FirstSceneToLoad = "scene1";
    public LevelSceneAndInterface[] levelsScenesAndInterfaces;
    Dictionary <string, string> convertedDictionary;
    
	void Start()
	{
		Application.targetFrameRate = 60;
		Stage.inst.onKeyDown.Add(OnKeyDown);

		UIPackage.AddPackage("UI/" + UiPackage);

		SceneAndUiManager.inst.Init();
		convertedDictionary = new Dictionary<string, string>();
        foreach(LevelSceneAndInterface asThis in levelsScenesAndInterfaces)
        {
            convertedDictionary[asThis.levelName] = asThis.interfaceName;
            Debug.Log(asThis.levelName);
            Debug.Log(asThis.interfaceName);
        }
		SceneAndUiManager.inst.SetlevelScenesInterfacesIndex(convertedDictionary);
		SceneAndUiManager.inst.LoadLevel(FirstSceneToLoad);
	}

	void OnKeyDown(EventContext context)
	{
		if (context.inputEvent.keyCode == KeyCode.Escape)
		{
			Application.Quit();
		}
	}
}
