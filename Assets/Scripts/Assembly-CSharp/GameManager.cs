using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Maximum bus price - the most expensive bus costs this amount.
    /// Uses the same constant as BusPriceConfig for consistency.
    /// </summary>
    public const int MAX_BUS_PRICE = 25000;

    [Header("Bus Prices - Most expensive is " + "25,000")]
    [SerializeField]
    private int[] busPrices = new int[] {
        0,              // Bus 1 - Free (starter bus)
        5000,           // Bus 2
        10000,          // Bus 3
        15000,          // Bus 4
        18000,          // Bus 5
        22000,          // Bus 6
        MAX_BUS_PRICE   // Bus 7 - Most expensive (25,000)
    };

    [Header("Multiplayer Settings")]
    [SerializeField]
    private bool isMultiplayerEnabled = true;

    [Header("Game State")]
    [SerializeField]
    private int playerMoney = 5000;

    [SerializeField]
    private int selectedBusIndex = 0;

    [SerializeField]
    private bool[] unlockedBuses = new bool[] { true, false, false, false, false, false, false };

    private const string PLAYER_MONEY_KEY = "PlayerMoney";
    private const string SELECTED_BUS_KEY = "SelectedBus";
    private const string UNLOCKED_BUSES_KEY = "UnlockedBuses_";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int GetBusPrice(int busIndex)
    {
        if (busIndex >= 0 && busIndex < busPrices.Length)
        {
            return busPrices[busIndex];
        }
        return 0;
    }

    public bool IsBusUnlocked(int busIndex)
    {
        if (busIndex >= 0 && busIndex < unlockedBuses.Length)
        {
            return unlockedBuses[busIndex];
        }
        return false;
    }

    public bool TryUnlockBus(int busIndex)
    {
        if (busIndex < 0 || busIndex >= busPrices.Length)
        {
            return false;
        }

        if (unlockedBuses[busIndex])
        {
            return true; // Already unlocked
        }

        int price = busPrices[busIndex];
        if (playerMoney >= price)
        {
            playerMoney -= price;
            unlockedBuses[busIndex] = true;
            SaveGameData();
            return true;
        }

        return false;
    }

    public int GetPlayerMoney()
    {
        return playerMoney;
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        SaveGameData();
    }

    public void SetSelectedBus(int busIndex)
    {
        if (busIndex >= 0 && busIndex < unlockedBuses.Length && unlockedBuses[busIndex])
        {
            selectedBusIndex = busIndex;
            SaveGameData();
        }
    }

    public int GetSelectedBus()
    {
        return selectedBusIndex;
    }

    public bool IsMultiplayerEnabled()
    {
        return isMultiplayerEnabled;
    }

    public void SetMultiplayerEnabled(bool enabled)
    {
        isMultiplayerEnabled = enabled;
    }

    private void LoadGameData()
    {
        playerMoney = PlayerPrefs.GetInt(PLAYER_MONEY_KEY, 5000);
        selectedBusIndex = PlayerPrefs.GetInt(SELECTED_BUS_KEY, 0);

        for (int i = 0; i < unlockedBuses.Length; i++)
        {
            unlockedBuses[i] = PlayerPrefs.GetInt(UNLOCKED_BUSES_KEY + i, i == 0 ? 1 : 0) == 1;
        }
    }

    private void SaveGameData()
    {
        PlayerPrefs.SetInt(PLAYER_MONEY_KEY, playerMoney);
        PlayerPrefs.SetInt(SELECTED_BUS_KEY, selectedBusIndex);

        for (int i = 0; i < unlockedBuses.Length; i++)
        {
            PlayerPrefs.SetInt(UNLOCKED_BUSES_KEY + i, unlockedBuses[i] ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public int GetTotalBusCount()
    {
        return busPrices.Length;
    }
}