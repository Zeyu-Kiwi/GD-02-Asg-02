using System.Collections.Generic;
using UnityEngine;

public class TruckCargo : MonoBehaviour
{
    public List<Parcel> parcels = new List<Parcel>();

    private void OnTriggerEnter(Collider other)
    {
        Parcel p = other.GetComponent<Parcel>();
        if (p != null && !parcels.Contains(p))
        {
            parcels.Add(p);
            //Debug.Log("parcel added");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Parcel p = other.GetComponent<Parcel>();
        if (p != null)
        {
            parcels.Remove(p);
            //Debug.Log("parcel removed");
        }
    }
}
