using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PromptSystem : MonoBehaviour
{
	public List<PromptNode> storyNodes = new List<PromptNode>();
	[SerializeField]
	public List<GameObject> nodes = new List<GameObject>();
	[SerializeField]
	GameObject nodePrefab;
	bool currentlyNode;
	bool moveCam;


	GameObject currentNodePickup;
	GameObject currentNodeHover;
	GameObject lastSplineShown;
	[SerializeField]
	Camera mainCamera;

	Vector2 camPosTemp;

	private void Awake()
	{
		mainCamera = FindFirstObjectByType<Camera>();
	}
	public void GetClick(InputAction.CallbackContext context)
	{
		if (context.performed)
		{

			currentlyNode = true;
			currentNodeHover = null;
			CheckWhatYouHitSomething();
		}
		if (context.canceled)
		{
			currentlyNode = false;
		}
	}

	public void GetMiddleMouse(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			camPosTemp = Mouse.current.position.ReadValue();
			moveCam = true;
		}
		if (context.canceled)
		{
			moveCam = false;
			camPosTemp = Mouse.current.position.ReadValue();
		}
	}
	private void CheckWhatYouHitSomething()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			PickupNode();
		}
		else
		{
			AddNode();
		}
	}

	public void AddNode()
	{
		GameObject newNode = Instantiate(nodePrefab);
		currentNodePickup = newNode;
		nodes.Add(newNode);
	}
	public void MoveNode()
	{
		//Bool
		if (currentNodePickup == null)
		{
			return;
		}
		if (currentlyNode)
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = 10f;
			currentNodePickup.transform.position = mainCamera.ScreenToWorldPoint(mousePosition);
		}
	}
	private void Update()
	{
		if (currentlyNode)
		{
			MoveNode();
		}
		ScrollData();
		if (moveCam)
		{
			MoveCam();
		}
		OnHover();
	}

	void ScrollData()
	{
		if (Mouse.current != null)
		{
			Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
			if (scrollDelta.y != 0)
			{

				//Zoom in or out
				mainCamera.orthographicSize -= scrollDelta.y;
				mainCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2f, 20f);

			}

		}
	}

	void MoveCam()
	{
		if (camPosTemp != Mouse.current.position.ReadValue())
		{
			Vector2 mouseDif = camPosTemp - Mouse.current.position.ReadValue();

			camPosTemp = Mouse.current.position.ReadValue();

			Vector3 newPos = new Vector3(-mouseDif.x, -mouseDif.y, 0);

			mainCamera.transform.position += newPos * Time.deltaTime;
		}

	}
	public void PickupNode()
	{
		//bool
		currentlyNode = true;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100))
		{
			//Temp gameobject
			currentNodePickup = hit.collider.transform.root.gameObject;
		}
	}

	void OnHover()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100))
		{
			GameObject hoveredObject = hit.collider.transform.root.gameObject;


			if (hoveredObject != currentNodeHover)
			{

				if (lastSplineShown != null)
				{
					lastSplineShown.SetActive(false);
					lastSplineShown = null;
				}


				currentNodeHover = hoveredObject;

				foreach(Transform child in currentNodeHover.transform)
				{
					child.gameObject.SetActive(true);
				}

				ConnectNodes connect = currentNodeHover.GetComponentInChildren<ConnectNodes>();
				Debug.Log("Hovering over: " + currentNodeHover.name);
				Debug.Log("Connect: " + connect + " " + connect.gameObject.name);
				if (connect != null)
				{
					connect.gameObject.SetActive(true);
					lastSplineShown = connect.gameObject;
				}
			}
			
		}
		else
		{
		
			if (lastSplineShown != null)
			{
				lastSplineShown.SetActive(false);
				lastSplineShown = null;
			}
			currentNodeHover = null;
		}
	}

	public void PlaceNode()
	{
		currentNodePickup = null;
	}

	public void RemoveNode()
	{

	}

	public void GetScriptableObject()
	{

	}




}
