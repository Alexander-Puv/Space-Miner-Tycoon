using UnityEngine;

public class Resource : MonoBehaviour {
    //[SerializeField] private Image image;
    [SerializeField] private string resourceName;
    [SerializeField] private float basePrice;
    [SerializeField] private float weight;

    //[SerializeField] private Vector3 rotationFrom;
    //[SerializeField] private Vector3 rotationTo;
    //[SerializeField] private Vector3 positionFrom;
    //[SerializeField] private Vector3 positionTo;
    //[SerializeField] private Vector3 ScaleFrom;
    //[SerializeField] private Vector3 ScaleTo;

    public float getBasePrice() {
        return basePrice;
    }
}
