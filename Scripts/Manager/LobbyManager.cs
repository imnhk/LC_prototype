using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : PumpFramework.Common.Singleton<LobbyManager>
{
    public string SelectedMap { get { return this.selectedMap; } }

    private string selectedMap;
    private Object[] maps;

    private TMP_Dropdown mapList;
    private Button startButton;

    private void Awake()    
    {
        if (instance != null)
            Destroy(instance.gameObject);

        DontDestroyOnLoad(this.gameObject);


        mapList = GameObject.Find("MapList").GetComponent<TMP_Dropdown>();
        startButton = GameObject.Find("StartButton").GetComponent<Button>();

        maps = Resources.LoadAll("Map", typeof(GameObject));
        foreach(var map in maps)
        {
            mapList.options.Add(new TMP_Dropdown.OptionData(map.name));
        }

        startButton.onClick.AddListener(LoadMap);
    }

    public void LoadMap()
    {
        selectedMap = mapList.options[mapList.value].text;
        SceneManager.LoadSceneAsync("Play", LoadSceneMode.Single);
    }
}
