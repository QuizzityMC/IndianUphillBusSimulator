using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Text moneyText;

    [SerializeField]
    private Text busNameText;

    [SerializeField]
    private Text busPriceText;

    [SerializeField]
    private Button buyButton;

    [SerializeField]
    private Button selectButton;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button nextBusButton;

    [SerializeField]
    private Button prevBusButton;

    [SerializeField]
    private Button multiplayerButton;

    [Header("Bus Display")]
    [SerializeField]
    private GameObject[] busModels;

    [SerializeField]
    private string[] busNames = new string[] {
        "Standard Bus",
        "City Bus",
        "Tourist Bus",
        "Luxury Bus",
        "Double Decker",
        "Mountain Express",
        "Super Deluxe"
    };

    private int currentBusIndex = 0;

    private void Start()
    {
        currentBusIndex = GameManager.Instance != null ? GameManager.Instance.GetSelectedBus() : 0;
        UpdateUI();
        SetupButtons();
    }

    private void SetupButtons()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        if (selectButton != null)
        {
            selectButton.onClick.AddListener(OnSelectButtonClicked);
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        if (nextBusButton != null)
        {
            nextBusButton.onClick.AddListener(OnNextBusClicked);
        }

        if (prevBusButton != null)
        {
            prevBusButton.onClick.AddListener(OnPrevBusClicked);
        }

        if (multiplayerButton != null)
        {
            multiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
        }
    }

    private void UpdateUI()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        // Update money display
        if (moneyText != null)
        {
            moneyText.text = GameManager.Instance.GetPlayerMoney().ToString("N0");
        }

        // Update bus name
        if (busNameText != null && currentBusIndex < busNames.Length)
        {
            busNameText.text = busNames[currentBusIndex];
        }

        // Update bus price display
        bool isUnlocked = GameManager.Instance.IsBusUnlocked(currentBusIndex);
        int price = GameManager.Instance.GetBusPrice(currentBusIndex);

        if (busPriceText != null)
        {
            busPriceText.text = isUnlocked ? "OWNED" : price.ToString("N0");
        }

        // Update button visibility
        if (buyButton != null)
        {
            buyButton.gameObject.SetActive(!isUnlocked);
            buyButton.interactable = GameManager.Instance.GetPlayerMoney() >= price;
        }

        if (selectButton != null)
        {
            selectButton.gameObject.SetActive(isUnlocked);
            bool isSelected = GameManager.Instance.GetSelectedBus() == currentBusIndex;
            selectButton.interactable = !isSelected;
        }

        // Update bus models
        UpdateBusDisplay();
    }

    private void UpdateBusDisplay()
    {
        if (busModels == null)
        {
            return;
        }

        for (int i = 0; i < busModels.Length; i++)
        {
            if (busModels[i] != null)
            {
                busModels[i].SetActive(i == currentBusIndex);
            }
        }
    }

    public void OnNextBusClicked()
    {
        int totalBuses = GameManager.Instance != null ? GameManager.Instance.GetTotalBusCount() : busNames.Length;
        currentBusIndex = (currentBusIndex + 1) % totalBuses;
        UpdateUI();
    }

    public void OnPrevBusClicked()
    {
        int totalBuses = GameManager.Instance != null ? GameManager.Instance.GetTotalBusCount() : busNames.Length;
        currentBusIndex = (currentBusIndex - 1 + totalBuses) % totalBuses;
        UpdateUI();
    }

    public void OnBuyButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.TryUnlockBus(currentBusIndex))
            {
                UpdateUI();
            }
        }
    }

    public void OnSelectButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetSelectedBus(currentBusIndex);
            UpdateUI();
        }
    }

    public void OnPlayButtonClicked()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsMultiplayerEnabled())
        {
            SceneManager.LoadScene("MultiplayerLobby");
        }
        else
        {
            SceneManager.LoadScene("Play(RCC)");
        }
    }

    public void OnMultiplayerButtonClicked()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public void OnSettingsButtonClicked()
    {
        // Settings functionality
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}