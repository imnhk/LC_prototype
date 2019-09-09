using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : PumpFramework.Common.Singleton<PlayerControl>
{
    [SerializeField]
    private GameObject playerPrefab = null;
    [SerializeField]
    private AudioClip weaponSwapClip = null;

    private Transform playerSpawnSpot;
    private FloatingJoystick joystick;
    
    public GameObject Player { get { return playerObject; } }
    private GameObject playerObject;
    private Player player;

    public Vector3 MovementInput { get { return movement; } }
    private Vector3 movement;

    void Awake()
    {
        // 플레이어 생성 위치 확인
        this.playerSpawnSpot = GameObject.Find("PlayerStartSpot").GetComponent<Transform>();
        this.joystick = GameObject.Find("FloatingJoystick").GetComponent<FloatingJoystick>();
        
    }

    void Start()
    {
        // 플레이어 생성
        this.playerObject = Instantiate<GameObject>(this.playerPrefab, this.playerSpawnSpot.position, this.playerSpawnSpot.rotation, this.transform);
        this.player = this.playerObject.GetComponent<Player>();
    }

    private void Update()
    {
        // 플레이어 이동 입력을 받는다.
        movement = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        //movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }

    public void ChangeAttackStyle()
    {
        PumpFramework.Common.SoundManager.Instance.PlaySound(weaponSwapClip, this.transform.position, false);

        switch (player.info.weaponType)
        {
            case WeaponType.Burst:
                player.info.weaponType = WeaponType.Loop;
                break;

            case WeaponType.Loop:
                player.info.weaponType = WeaponType.Once;
                break;

            case WeaponType.Once:
                player.info.weaponType = WeaponType.Burst;
                break;

            default:
                player.info.weaponType = WeaponType.Once;
                break;
        }
    }
}
