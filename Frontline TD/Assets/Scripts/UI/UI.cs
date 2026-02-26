using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI waveText2;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject ShopMenu;

    void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameOver.SetActive(false);
        ShopMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        moneyText.text = BuildManager.Instance.GetCurrentMoney().ToString();
        livesText.text = PlayerManager.Instance.GetCurrentLives().ToString();
        waveText.text = EnemySpawner.Instance.GetCurrentWave().ToString();
        waveText2.text = EnemySpawner.Instance.GetCurrentWave().ToString();
    }

    public void Muriste()
    {
        GameOver.SetActive(true);
        ShopMenu.SetActive(false);
    }
}
