using UnityEngine;
using UnityEngine.Audio;
using PumpFramework.Common;



public class GameManager : Singleton<GameManager>
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemDatabase.Instance.ShowInventory();
        }
    }
}
