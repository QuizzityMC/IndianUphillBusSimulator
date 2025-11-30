using UnityEngine;

/// <summary>
/// ScriptableObject to configure bus prices.
/// All prices are set so that the most expensive bus costs 25,000.
/// </summary>
[CreateAssetMenu(fileName = "BusPriceConfig", menuName = "Indian Uphill Bus/Bus Price Configuration")]
public class BusPriceConfig : ScriptableObject
{
    [System.Serializable]
    public class BusInfo
    {
        public string busName;
        public int price;
        public string description;
        public Sprite thumbnail;
        public bool isUnlockedByDefault;
    }

    [Header("Bus Configurations - Most expensive bus costs 25,000")]
    [SerializeField]
    private BusInfo[] buses = new BusInfo[]
    {
        new BusInfo { busName = "Standard Bus", price = 0, description = "Your starter bus. Basic but reliable.", isUnlockedByDefault = true },
        new BusInfo { busName = "City Bus", price = 5000, description = "A modern city bus with better handling.", isUnlockedByDefault = false },
        new BusInfo { busName = "Tourist Bus", price = 10000, description = "Comfortable tourist bus with panoramic windows.", isUnlockedByDefault = false },
        new BusInfo { busName = "Luxury Bus", price = 15000, description = "Luxury bus with premium features.", isUnlockedByDefault = false },
        new BusInfo { busName = "Double Decker", price = 18000, description = "Classic double decker with extra capacity.", isUnlockedByDefault = false },
        new BusInfo { busName = "Mountain Express", price = 22000, description = "Built for mountain terrain with powerful engine.", isUnlockedByDefault = false },
        new BusInfo { busName = "Super Deluxe", price = 25000, description = "The ultimate bus with top-tier performance.", isUnlockedByDefault = false }
    };

    public int GetBusCount()
    {
        return buses != null ? buses.Length : 0;
    }

    public BusInfo GetBusInfo(int index)
    {
        if (buses != null && index >= 0 && index < buses.Length)
        {
            return buses[index];
        }
        return null;
    }

    public int GetBusPrice(int index)
    {
        BusInfo info = GetBusInfo(index);
        return info != null ? info.price : 0;
    }

    public string GetBusName(int index)
    {
        BusInfo info = GetBusInfo(index);
        return info != null ? info.busName : "Unknown Bus";
    }

    public bool IsBusUnlockedByDefault(int index)
    {
        BusInfo info = GetBusInfo(index);
        return info != null ? info.isUnlockedByDefault : false;
    }

    public int GetMostExpensiveBusPrice()
    {
        int maxPrice = 0;
        if (buses != null)
        {
            foreach (var bus in buses)
            {
                if (bus.price > maxPrice)
                {
                    maxPrice = bus.price;
                }
            }
        }
        return maxPrice;
    }

    // Validation method to ensure most expensive bus is 25,000
    private void OnValidate()
    {
        if (buses != null)
        {
            int maxPrice = GetMostExpensiveBusPrice();
            if (maxPrice != 25000)
            {
                Debug.LogWarning("BusPriceConfig: Most expensive bus should cost 25,000. Current max: " + maxPrice);
            }
        }
    }
}
