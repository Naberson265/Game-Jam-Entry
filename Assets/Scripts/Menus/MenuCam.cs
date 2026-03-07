using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuCamera : MonoBehaviour
{
    public float speed = 0.1f;
    public Transform currentDest;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentDest.position, speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentDest.rotation, speed);
    }
    public void NewCamDestination(Transform newDestination)
    {
        currentDest = newDestination;
    }
}
