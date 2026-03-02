using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Start()
    {
        LockMouse();
    }
    void Update()
    {
        if (canMove && !paused) CamMove();
        if (Input.GetButtonDown("Pause"))
        {
            if (paused) UnpauseGame();
            else PauseGame();
        }
        if (paused) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
	private void CamMove()
    {
        xRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        transform.localEulerAngles = new Vector3(xRot, yRot, 0f);
        camFixedDirTransform.localEulerAngles = new Vector3(0f, yRot, 0f);
        RaycastHit raycastHit;
        if (Physics.Raycast(playerTransform.position, -transform.forward, out raycastHit, 10f, rayLayerMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = raycastHit.point + (transform.forward * 0.2f);
        }
        else
        {
            transform.position = playerTransform.position - (transform.forward * 9.9f);
        }
    }
    public void PauseGame()
    {
        UnlockMouse();
        paused = true;
        pauseMenu.SetActive(true);
    }
    public void UnpauseGame()
    {
        LockMouse();
        paused = false;
        pauseMenu.SetActive(false);
    }
    public void LockMouse()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }
    public void UnlockMouse()
    {
		Cursor.lockState = CursorLockMode.None;
    }
	public float mouseSensitivity = 1f;
	public float xRot = 0f;
	public float yRot = 0f;
    public bool canMove = true;
    public bool paused = false;
	public LayerMask rayLayerMask;
	public Transform playerTransform;
	public Transform camFixedDirTransform;
	public GameObject pauseMenu;
}
