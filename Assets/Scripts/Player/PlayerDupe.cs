using System.Collections;
using UnityEngine;

public class PlayerDupe : MonoBehaviour
{
    public static PlayerDupe mostRecentDupe;

    public GameObject[] abilityModels;

    [SerializeField] private float clipTime = 0.4f;
    private float counter = 0;

    private Collider col;
    private Rigidbody rb;

    private void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        mostRecentDupe = this;
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

    public void DestroyDupe()
    {
        StartCoroutine(DestroyDupeRoutine());
    }

    private IEnumerator DestroyDupeRoutine()
    {
        Vector3 decreaseAmount = transform.localScale * (1f/100);
        print(decreaseAmount);
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = transform.localScale - decreaseAmount;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }
}
