using UnityEngine;

public class UnloadZone : MonoBehaviour
{
    //public float maxTime = 60f;
    //private float timeLeft;

    public string parcelTag;
    public int parcelCount;
    public int totalValue;

    private TruckCargo cargo;

    private bool isTruck;
    private bool isRightParcel;

    private void Update()
    {
        if (isTruck && isRightParcel)
        {
            Unload();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Truck"))
        {
            cargo = other.GetComponentInChildren<TruckCargo>();

            if (cargo == null) return;

            parcelCount = cargo.parcels.Count;

            totalValue = 0;

            foreach (Parcel p in cargo.parcels)
            {
                totalValue += p.value;
            }

            // Time bonus (simple version)
            //float timeMultiplier = Mathf.Clamp01(timeLeft / maxTime) + 1f;

            //int finalMoney = Mathf.RoundToInt(totalValue * timeMultiplier);

            //Debug.Log($"Delivered {parcelCount} parcels. Earned: {finalMoney}");

            // Destroy delivered parcels
            foreach (Parcel p in cargo.parcels)
            {
                Destroy(p.gameObject);
            }

            cargo.parcels.Clear();
        }*/

        if (other.CompareTag("Truck"))
        {
            isTruck = true;
            cargo = other.GetComponentInChildren<TruckCargo>();
        }

        if (other.CompareTag(parcelTag))
        {
            isRightParcel = true;
        }

        
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Truck"))
    //    {
    //        timeLeft -= Time.deltaTime;
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Truck"))
        {
            //timeleft = maxtime; // reset for next delivery
            SaveSystem.Instance.Save(other.gameObject, false);   
        }

        
    }

    private void Unload()
    {
        //Debug.Log(parcelTag + " is delivered!");
        isTruck = false;
        isRightParcel = false;

        parcelCount = cargo.parcels.Count;

        totalValue = 0;

        foreach (Parcel p in cargo.parcels)
        {
            totalValue += p.value;
        }

        // Time bonus (simple version)
        //float timeMultiplier = Mathf.Clamp01(timeLeft / maxTime) + 1f;

        //int finalMoney = Mathf.RoundToInt(totalValue * timeMultiplier);

        //Debug.Log($"Delivered {parcelCount} parcels. Earned: {finalMoney}");
        GameManager.Instance.AddMoney(totalValue);
        MissionManager.instance.missionIndex += 1;

        // Destroy delivered parcels
        foreach (Parcel p in cargo.parcels)
        {
            Destroy(p.gameObject);
        }

        cargo.parcels.Clear();
    }
}
