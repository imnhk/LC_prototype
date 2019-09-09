using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : PumpFramework.Common.Singleton<MapGenerator>
{
    private GameObject currentMap;

    private void Awake()
    {
        
        try
        {
            currentMap = Resources.Load<GameObject>("Map/" + LobbyManager.Instance.SelectedMap);
        }
        catch(System.NullReferenceException e)
        {
            Debug.LogError("Map resource can't be loaded. loading default map(MapType00)...");
            currentMap = Resources.Load<GameObject>("Map/MapType00");
        }
        
        Instantiate(currentMap, this.transform);
    }
}
