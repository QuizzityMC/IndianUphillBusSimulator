using UnityEngine;
using System.Collections.Generic;

public class VehicleManager : MonoBehaviour
{
    public static VehicleManager Instance { get; private set; }

    [Header("Vehicle Prefabs")]
    [SerializeField]
    private GameObject[] vehiclePrefabs;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] spawnPoints;

    private List<GameObject> activeVehicles = new List<GameObject>();
    private int currentVehicleIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Get selected bus from GameManager
        if (GameManager.Instance != null)
        {
            currentVehicleIndex = GameManager.Instance.GetSelectedBus();
        }
    }

    public GameObject SpawnVehicle(int vehicleIndex, Transform spawnPoint)
    {
        if (vehiclePrefabs == null || vehicleIndex < 0 || vehicleIndex >= vehiclePrefabs.Length)
        {
            Debug.LogWarning("Invalid vehicle index: " + vehicleIndex);
            return null;
        }

        if (vehiclePrefabs[vehicleIndex] == null)
        {
            Debug.LogWarning("Vehicle prefab at index " + vehicleIndex + " is null");
            return null;
        }

        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject vehicle = Instantiate(vehiclePrefabs[vehicleIndex], position, rotation);
        activeVehicles.Add(vehicle);

        return vehicle;
    }

    public GameObject SpawnPlayerVehicle()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        return SpawnVehicle(currentVehicleIndex, spawnPoint);
    }

    public Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return null;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    public void DespawnVehicle(GameObject vehicle)
    {
        if (vehicle != null)
        {
            activeVehicles.Remove(vehicle);
            Destroy(vehicle);
        }
    }

    public void DespawnAllVehicles()
    {
        foreach (var vehicle in activeVehicles)
        {
            if (vehicle != null)
            {
                Destroy(vehicle);
            }
        }
        activeVehicles.Clear();
    }

    public int GetActiveVehicleCount()
    {
        return activeVehicles.Count;
    }

    public List<GameObject> GetActiveVehicles()
    {
        return new List<GameObject>(activeVehicles);
    }

    public void SetCurrentVehicleIndex(int index)
    {
        currentVehicleIndex = Mathf.Clamp(index, 0, vehiclePrefabs != null ? vehiclePrefabs.Length - 1 : 0);
    }

    public int GetCurrentVehicleIndex()
    {
        return currentVehicleIndex;
    }

    public int GetTotalVehicleCount()
    {
        return vehiclePrefabs != null ? vehiclePrefabs.Length : 0;
    }
}