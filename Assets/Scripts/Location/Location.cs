using UnityEngine;

public class Location : MonoBehaviour {
    private bool canBeSelected = false;

    public void SetCanBeSelected(bool value) {
        canBeSelected = value;
    }

    private void OnMouseDown() {
        if (canBeSelected) {
            TravelMenu.Instance.SelectLocation(this);
        }
    }
}
