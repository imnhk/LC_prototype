using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PumpFramework.Common;
using TMPro;


public class PlayUIManager : Singleton<PlayUIManager>
{
    private Camera mainCam;
    private Player player;

    // 일시정지 메뉴
    private GameObject pauseMenu;
    
    private GameObject joystick;

    // 인게임 UI
    private TextMeshProUGUI playerHpText;
    private TextMeshProUGUI playerExpText;
    private Image playerHpGuage;
    private GameObject targetIndicator;
    private TextMeshProUGUI targetHpText;
    [SerializeField]
    private GameObject damagePopup;

    [SerializeField]
    private AudioClip buttonPressAudio;

    private void Awake()
    {
        // UI를 표시할 카메라
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        // 플레이어 체력 표시 UI
        playerHpText = GameObject.Find("PlayerHpText").GetComponent<TextMeshProUGUI>();
        playerExpText = GameObject.Find("PlayerExpText").GetComponent <TextMeshProUGUI>();
        playerHpGuage = GameObject.Find("PlayerHpGuage").GetComponent<Image>();
        // 조준한 적 표시 UI
        targetIndicator = GameObject.Find("TargetIndicator");
        targetHpText = GameObject.Find("TargetHp").GetComponent<TextMeshProUGUI>();
        // 일시정지 메뉴
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);

        // 컨트롤러
        joystick = GameObject.Find("FloatingJoystick");
    }

    private void Start()
    {
        this.player = PlayerControl.Instance.Player.GetComponent<Player>();
    }

    void Update()
    {
        // HP 표시
        playerHpText.text = player.info.HP.ToString();
        playerHpGuage.fillAmount = player.HpPercent;
        playerHpGuage.color = new Color32((byte)(255 - player.HpPercent * 255), (byte)(player.HpPercent * 255), (byte)(player.HpPercent * 255), 255);
        // EXP 표시
        playerExpText.text = "Exp: " + player.info.Exp.ToString();

        // 현재 Target과 그 Hp 표시
        if (player.HasTarget)
        {
            targetIndicator.transform.position = player.Target.transform.position + new Vector3(0, 3f, 0);
            targetIndicator.SetActive(true);
            targetHpText.text = player.Target.GetComponent<EnemyBase>().CurrentHp.ToString();
            targetHpText.gameObject.transform.position = mainCam.WorldToScreenPoint(player.Target.transform.position + new Vector3(0, 4f, 0));
            targetHpText.gameObject.SetActive(true);
        }
        else
        {
            targetIndicator.SetActive(false);
            targetHpText.gameObject.SetActive(false);
        }
    }

    public void CreateDamagePopup(int damage, Vector3 position)
    {
        GameObject popup = Instantiate(damagePopup, mainCam.WorldToScreenPoint(position), Quaternion.identity ,this.transform);
        TextMeshProUGUI popupText = popup.GetComponent<TextMeshProUGUI>();
        popupText.text = damage.ToString();
        if (damage >= 80)
        {
            popupText.color = new Color32(255, 0, 0, 255);
            popupText.fontSize *= 1.4f;
        }
        else
            popupText.color = new Color32(255, (byte)(255 - damage * 3), 0, 255);
    }

    public void SaveGame()
    {
        SoundManager.Instance.PlaySound(buttonPressAudio, this.transform.position, false);
        // Player 정보를 저장한다
        PlayerInfo playerInfo = PlayerControl.instance.Player.GetComponent<Player>().info;
        string playerInfoJson = JsonUtility.ToJson(playerInfo);
        PlayerPrefs.SetString("PlayerInfo", playerInfoJson);
    }
    public void RemoveSaves()
    {
        SoundManager.Instance.PlaySound(buttonPressAudio, this.transform.position, false);

        PlayerPrefs.DeleteAll();
    }

    public void PauseToggle()
    {
        SoundManager.Instance.PlaySound(buttonPressAudio, this.transform.position, false);

        if (pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            joystick.SetActive(true);
        }
        else
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            joystick.SetActive(false);
        }
    }

    public void SfxOptionChanged(bool selected)
    {
        if (selected)
            SoundManager.Instance.SfxOn();
        else
            SoundManager.Instance.SfxOff();
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
