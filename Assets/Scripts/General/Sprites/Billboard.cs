using UnityEngine;

public class Billboard : MonoBehaviour
{
	private void Start()
	{
		playerCamera = Camera.main;
	}
	private void LateUpdate()
	{
		transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward);
	}
	private Camera playerCamera;
}