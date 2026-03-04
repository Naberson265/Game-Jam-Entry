using UnityEngine;

public class PlayerDupe : MonoBehaviour
{

    [SerializeField] private int clipTime = 20;
    private int counter = 0;

    private void FixedUpdate()
    {
        counter++;
        if (counter > clipTime) {
            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            this.enabled = false;
        }
    }
}
