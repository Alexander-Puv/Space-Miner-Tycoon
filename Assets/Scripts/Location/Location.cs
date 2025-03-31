using UnityEngine;

public class Location : MonoBehaviour {
    public void SetCanBeSelected(bool value) {
        GetComponentInChildren<LocationModel>().SetCanBeSelected(value);
    }
}
