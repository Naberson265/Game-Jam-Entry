using UnityEngine;

public class PlayerDupe : MonoBehaviour
{

    [SerializeField] private float clipTime = 0.4f;
    private float counter = 0;

    private Collider col;
    private Rigidbody rb;

    private void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (counter > clipTime)
        {
            col.enabled = true;
            rb.useGravity = true;
            this.enabled = false;
        } else
        {
            counter += Time.deltaTime;
        }
    }
}
