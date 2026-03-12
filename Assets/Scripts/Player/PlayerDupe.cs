using UnityEngine;

public class PlayerDupe : MonoBehaviour
{
    public GameObject[] abilityModels;

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

    public void SetModel(int ability)
    {
        // Update Model
        for (int i = 0; i < abilityModels.Length; i++)
        {
            if (i == ability)
            {
                abilityModels[i].SetActive(true);
            }
            else
            {
                abilityModels[i].SetActive(false);
            }
        }
    }
}
