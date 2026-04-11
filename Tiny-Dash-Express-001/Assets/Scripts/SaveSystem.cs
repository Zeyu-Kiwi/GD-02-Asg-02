using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    public GameObject truck;

    public GameObject parcels;
    public GameObject spawnParcelPos;
    public bool isLoaded;

    private void Awake()
    {
        Instance = this;
        PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (truck != null)
            {
                Debug.Log("Reload btn pressed!");
                Load(truck);
            }
        }
    }

    //get the data of parcel prefab, is it a load or unload zone, and parcel spawn position
    public void GetData(GameObject p, GameObject pos)
    {
        parcels = p;
        spawnParcelPos = pos;
    }

    //SAVE
    public void Save(GameObject truck, bool zone)
    {
        //check zone
        isLoaded = zone;

        // Money
        PlayerPrefs.SetInt("Money", GameManager.Instance.totalMoney);

        // Timer
        PlayerPrefs.SetFloat("Time", GameManager.Instance.elapsedTime);

        // Position
        Vector3 pos = truck.transform.position;
        PlayerPrefs.SetFloat("PosX", pos.x);
        PlayerPrefs.SetFloat("PosY", pos.y);
        PlayerPrefs.SetFloat("PosZ", pos.z);

        // Rotation
        Vector3 rot = truck.transform.eulerAngles;
        PlayerPrefs.SetFloat("RotY", rot.y);

        PlayerPrefs.Save();

        Debug.Log("Game Saved!");
    }

    //LOAD
    public void Load(GameObject truck)
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            Debug.Log("No save found!");
            return;
        }

        // Restore money
        GameManager.Instance.totalMoney = PlayerPrefs.GetInt("Money");

        // Restore timer
        GameManager.Instance.elapsedTime = PlayerPrefs.GetFloat("Time");

        // Get saved position
        Vector3 pos = new Vector3(
            PlayerPrefs.GetFloat("PosX"),
            PlayerPrefs.GetFloat("PosY"),
            PlayerPrefs.GetFloat("PosZ")
        );

        float rotY = PlayerPrefs.GetFloat("RotY");

        // PHYSICS FIX (VERY IMPORTANT)
        Rigidbody rb = truck.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Apply position AFTER stopping physics
        truck.transform.position = pos;
        truck.transform.rotation = Quaternion.Euler(0, rotY, 0);

        //destroy the old parcels then spawn new ones according to data stored
        TruckCargo cargo = truck.GetComponentInChildren<TruckCargo>();

        if (isLoaded && cargo != null)
        {
            foreach (Parcel p in cargo.parcels)
            {
                Destroy(p.gameObject);
            }
            cargo.parcels.Clear();

            GameObject parcel = Instantiate(parcels, spawnParcelPos.transform);
        }
        else
        {
            Debug.Log("No parcels in delivery.");
        }

        Debug.Log("Game Loaded!");
    }
}