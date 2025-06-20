using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class VisualPromptSystem : MonoBehaviour
{
	public List<PromptNode> storyNodes = new List<PromptNode>();

	public GameObject currenntlySelectedObject;

	[SerializeField] public List<GameObject> nodes = new List<GameObject>();
	[SerializeField] GameObject nodePrefab;
	[SerializeField] Camera mainCamera;

	[SerializeField, Layer] string nodeOnHover;
	int currentNodeOnHoverLayer;
	[SerializeField, Layer] string nodeExtendor;
	int currentNodeExtendorLayer;
	[SerializeField, Layer] string nodeOnPickup;
	int currentNodeOnPickupLayer;

	private GameObject currentNodePickup;

	private GameObject currentNodeHover;
	private GameObject lastSplineShown;

	private Vector2 camPosTemp;

	GameObject hoveredObject;
	GameObject hoveredObjectForLine;

	private GameObject currentNodeExtendor;

	[SerializeField] LineRenderer lineRenderer;

	float camMoveSpeed = 1f;

	bool onHover;
	bool startHover;
	private enum EditorState
	{
		Idle,
		DraggingNode,
		HoverState,
		ConnectingNodes,
		MoveingCamera
	}

	private EditorState currentState = EditorState.Idle;

	private void Awake()
	{
		mainCamera = FindFirstObjectByType<Camera>();
		currentNodeExtendorLayer = 1 << LayerMask.NameToLayer(nodeExtendor);
		currentNodeOnHoverLayer = 1 << LayerMask.NameToLayer(nodeOnHover);
		currentNodeOnPickupLayer = 1 << LayerMask.NameToLayer(nodeOnPickup);

	}

	public bool IsPointerOverUI()
	{
		if (EventSystem.current == null)
			return false;

		PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
		{
			position = Input.mousePosition
		};

		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, results);

		foreach (RaycastResult result in results)
		{
			Canvas canvas = result.gameObject.GetComponentInParent<Canvas>();
			if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
				return true; 
		}

		return false; 
	}
	public void GetClick(InputAction.CallbackContext context)
	{

		if (context.performed)
		{
			currentNodeHover = null;
			
			if (!IsPointerOverUI())
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hit, 100, currentNodeOnPickupLayer))
				{
					PickupNode();
					currenntlySelectedObject = hit.collider.transform.root.gameObject;

					currentState = EditorState.DraggingNode;
				}
				else if (Physics.Raycast(ray, out RaycastHit extendorHit, 100, currentNodeExtendorLayer))
				{
					GetExtendor();
					currentState = EditorState.ConnectingNodes;
				}

				else
				{
					AddNode();
					currentState = EditorState.DraggingNode;
				}
			}
			
		}

		if (context.canceled)
		{
			
			if(currentState == EditorState.DraggingNode)
			{
				PlaceNode();
			}
			else if (currentState == EditorState.ConnectingNodes)
			{
				ConnectNodes(hoveredObjectForLine);
				currentNodeExtendor = null;
			}
			currentState = EditorState.Idle;

		}
	}

	public void GetMiddleMouse(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			camPosTemp = Mouse.current.position.ReadValue();
			currentState = EditorState.MoveingCamera;
		}

		if (context.canceled)
		{
			currentState = EditorState.Idle;
		}
	}

	private void Update()
	{
		switch (currentState)
		{
			case EditorState.DraggingNode:
				MoveNode();
				break;

			case EditorState.MoveingCamera:
				MoveCam();
				break;

			case EditorState.HoverState:

				break;

			case EditorState.ConnectingNodes:
				ConnectingNodes();
				break;

			case EditorState.Idle:
			default:
				break;
		}

		ScrollData();
		OnHover();
	}

	private void ConnectingNodes()
	{
		if (currentNodeExtendor == null) return;
		if(startHover == false)
		{
			hoveredObjectForLine = hoveredObject;
			startHover = true;
		}
		
		if (lineRenderer == null)
		{
			lineRenderer = new GameObject("ExtendorLine").AddComponent<LineRenderer>();
			lineRenderer.positionCount = 2;
			lineRenderer.startWidth = 0.1f;
			lineRenderer.endWidth = 0.1f;
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			lineRenderer.startColor = Color.black;
			lineRenderer.endColor = Color.black;
		}

		lineRenderer.SetPosition(0, currentNodeExtendor.transform.position);
		lineRenderer.SetPosition(1, mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue() + new Vector2(0, 10f)));
	}

	private void ConnectNodes(GameObject startObject)
	{
		if (onHover)
		{
			lineRenderer.SetPosition(1, hoveredObject.transform.position);
			hoveredObject.GetComponent<ConnectNodes>().connectedLines.Add(lineRenderer);
			hoveredObject.GetComponent<ConnectNodes>().connectedFrom.Add(hoveredObjectForLine);
			hoveredObjectForLine.GetComponent<ConnectNodes>().connectorLines.Add(lineRenderer);
			hoveredObjectForLine.GetComponent<ConnectNodes>().connectedTo.Add(hoveredObject);
			lineRenderer = null;
			hoveredObjectForLine = null;
			startHover = false;

		}
		else
		{
			Debug.LogError("No node hovered to connect to.");
			Destroy(lineRenderer);
		}
	}

	public void AddNode()
	{
		GameObject newNode = Instantiate(nodePrefab);
		currentNodePickup = newNode;
		currenntlySelectedObject = newNode;
		nodes.Add(newNode);
	}

	public void MoveNode()
	{
		if (currentNodePickup == null) return;

		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10f;
		currentNodePickup.transform.position = mainCamera.ScreenToWorldPoint(mousePosition);
	}

	public void PickupNode()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, currentNodeOnPickupLayer))
		{
			currentNodePickup = hit.collider.transform.root.gameObject;
		}
	}

	public void PlaceNode()
	{
		currentNodePickup = null;
	}

	public void GetExtendor()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, currentNodeExtendorLayer))
		{
			currentNodeExtendor = hit.collider.transform.gameObject;

		}
	}
	private void ScrollData()
	{
		if (Mouse.current != null && !IsPointerOverUI())
		{
			Vector2 scrollDelta = Mouse.current.scroll.ReadValue();
			if (scrollDelta.y != 0)
			{
				mainCamera.orthographicSize -= scrollDelta.y;
				mainCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2f, 20f);
				camMoveSpeed = mainCamera.orthographicSize / 20f;
			}
		}
	}

	private void MoveCam()
	{
		if (camPosTemp != Mouse.current.position.ReadValue())
		{
			Vector2 mouseDif = camPosTemp - Mouse.current.position.ReadValue();
			camPosTemp = Mouse.current.position.ReadValue();

			Vector3 newPos = new Vector3(mouseDif.x, mouseDif.y, 0);
			mainCamera.transform.position += newPos * Time.deltaTime * camMoveSpeed;
		}
	}

	private void OnHover()
	{
		if (IsPointerOverUI()) return;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 100, currentNodeOnHoverLayer))
		{
			hoveredObject = hit.collider.transform.root.gameObject;
			onHover = true;
			if (hoveredObject != currentNodeHover)
			{
				if (lastSplineShown != null)
				{
					lastSplineShown.SetActive(false);
					lastSplineShown = null;
				}

				currentNodeHover = hoveredObject;

				foreach (Transform child in currentNodeHover.transform)
				{
					child.gameObject.SetActive(true);
				}

				NodeConnector connect = currentNodeHover.GetComponentInChildren<NodeConnector>();
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
			if(hoveredObject != null)
			{
				hoveredObject = null;
			}
			onHover = false;
			currentNodeHover = null;
		}
	}

	public void RemoveNode()
	{
		// Placeholder for node deletion
	}

	public void GetScriptableObject()
	{
		// Placeholder for scriptable object loading
	}


}
