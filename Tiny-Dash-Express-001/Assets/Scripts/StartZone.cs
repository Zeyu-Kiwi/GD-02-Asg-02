using UnityEngine;

public class StartZone : MonoBehaviour
{
    public GameObject parcelToDeliver;
    public GameObject spawnParcel;
    //public GameObject tempCollider;

    [SerializeField] private bool isLoaded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Truck") && !isLoaded)
        {
            //Debug.Log("Truck in. Load Parcel.");

            isLoaded = true;
            //tempCollider.SetActive(true); // active the cage before loading parcel so parcel won't fall off
            GameObject parcel = Instantiate(parcelToDeliver, spawnParcel.transform);
            AudioManager.instance.Play("Loading");


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Truck"))
        {
            //tempCollider.SetActive(false); // disable the cage when left the zone so parcel will fall off due to physics

            SaveSystem.Instance.GetData(parcelToDeliver, spawnParcel);
            SaveSystem.Instance.Save(other.gameObject, true);
        } 
    }
}
