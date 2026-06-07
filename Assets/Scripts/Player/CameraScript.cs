using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Start()
    {
        // Sets every pref to its default in case the settings were never opened until this point.
        if (PlayerPrefs.GetFloat("Sensitivity") == 0) PlayerPrefs.SetFloat("Sensitivity", 1);
        if (PlayerPrefs.GetFloat("RenderDist") == 0) PlayerPrefs.SetFloat("RenderDist", 1000);
        if (PlayerPrefs.GetInt("OcclusionCulling") == 0) PlayerPrefs.SetInt("OcclusionCulling", 2);
        if (PlayerPrefs.GetInt("TurnWithCamera") == 0) PlayerPrefs.SetInt("TurnWithCamera", 1);
        LockMouse();
        camComp = GetComponent<Camera>();
    }
    void LateUpdate()
    {
        SettingManagement();
        if (canMove && !paused) CamMove();
        if (Input.GetButtonDown("Pause") && canMove)
        {
            if (paused) UnpauseGame();
            else PauseGame();
        }
        if (paused)
        {
            Time.timeScale = 0f;
            if (freeCamMode) FreeCamMove();
        }
        else Time.timeScale = 1f;
    }
    public void SettingManagement()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
        camComp.farClipPlane = PlayerPrefs.GetFloat("RenderDist");
        if (PlayerPrefs.GetInt("OcclusionCulling") == 2) camComp.useOcclusionCulling = true;
        else camComp.useOcclusionCulling = false;
    }
	private void CamMove()
    {
        // Still follows the player if the player can't make inputs, but the camera can't turn.
        if (PlayerController.playerController.canMove) xRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        if (PlayerController.playerController.canMove) yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        if (PlayerController.playerController.canMove) distanceToPlayer -= Input.GetAxis("Mouse ScrollWheel") * 10f;
        if (distanceToPlayer > maxDistance) distanceToPlayer = maxDistance;
        if (distanceToPlayer < minDistance) distanceToPlayer = minDistance;
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        transform.localEulerAngles = new Vector3(xRot, yRot, 0f);
        camFixedDirTransform.localEulerAngles = new Vector3(0f, yRot, 0f);
        RaycastHit raycastHit;
        
        if (Physics.Raycast(playerTransform.position, -transform.forward, out raycastHit, distanceToPlayer, rayLayerMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = raycastHit.point + (transform.forward * 0.1f);
        }
        else
        {
            transform.position = playerTransform.position - (transform.forward * distanceToPlayer);
        }
    }
	private void FreeCamMove()
    {
        UnlockMouse();
        // Turning. Carried over when unpausing.
        xRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        yRot += Input.GetAxis("Mouse X") * mouseSensitivity;
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        transform.localEulerAngles = new Vector3(xRot, yRot, 0f);
        camFixedDirTransform.localEulerAngles = new Vector3(0f, yRot, 0f);
        Vector3 movementDir = Input.GetAxisRaw("Vertical") * transform.forward + Input.GetAxisRaw("Horizontal") * transform.right;
        camChar.Move(movementDir * Time.deltaTime);
    }
    public void PauseGame()
    {
        UnlockMouse();
        paused = true;
        pauseMenu.SetActive(true);
    }
    public void UnpauseGame()
    {
        freeCamMode = false;
        LockMouse();
        paused = false;
        pauseMenu.SetActive(false);
        bool first = true;
        foreach (Transform child in pauseMenu.transform)
        {
            if(first)
            {
                child.gameObject.SetActive(true);
                first = false;
            } else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    public void LockMouse()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }
    public void UnlockMouse()
    {
		Cursor.lockState = CursorLockMode.None;
    }
    public CharacterController camChar;
	public float distanceToPlayer = 12f, maxDistance = 30f, minDistance = 0.1f, mouseSensitivity = 1f, xRot = 0f, yRot = 0f;
    public bool canMove = true, paused = false, freeCamMode = false;
	public LayerMask rayLayerMask;
	public Transform playerTransform, camFixedDirTransform;
	public GameObject pauseMenu;
    private Camera camComp;
}
