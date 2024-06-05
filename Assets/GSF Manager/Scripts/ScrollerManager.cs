using UnityEngine;

public class ScrollerManager : MonoBehaviour
{
    private Vector3 initialPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }
    private void OnEnable()
    {
        if(initialPosition != Vector3.zero)
        {
            transform.position = initialPosition;
        }
    }
}
