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

	PromptSystem promptSystem;

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

	Vector3 prevPos;

	NodeUI nodeUI;
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
		promptSystem = FindFirstObjectByType<PromptSystem>();
		nodeUI = FindFirstObjectByType<NodeUI>();
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
					Vector3 mousPos = Input.mousePosition;
					mousPos.z = 10f;
					Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousPos);
					AddNode(worldPos);
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
				if(hoveredObject == null)
				{
					Vector3 mousPos = Input.mousePosition;
					mousPos.z = 10f;
					Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousPos);
					AddNode(worldPos);
					OnHover();
					ConnectNodes(hoveredObjectForLine);
				}
				else
				{
					ConnectNodes(hoveredObjectForLine);
					currentNodeExtendor = null;
				}
				
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

	public void AddNode(Vector3 posToSpawnAt)
	{
		GameObject newNode = Instantiate(nodePrefab, posToSpawnAt, Quaternion.identity);
		hoveredObject = newNode;
		ConnectNodes conNode = newNode.GetComponentInChildren<ConnectNodes>();
		currentNodePickup = newNode;
		currenntlySelectedObject = newNode;
		nodes.Add(newNode);
		
		Debug.Log(nodeUI.cards.Count);
		float radius = 3.5f;
		float arcDegrees = 170f; 
		float startAngle = -arcDegrees / 2f; 
		float endAngle = arcDegrees / 2f;    
		int cardCount = promptSystem.cards.Count;
		int howManyCards = 0;
		for (int i = 0; i < cardCount; i++)
		{
			if (promptSystem.cards[i].cardType == CardType.Prompt && promptSystem.cards[i].cardRarity != CardRarity.Rare)
			{
				howManyCards++;
				
			}
		}
		float promptIndex = 0f;
		for (int i = 0; i < cardCount; i++)
		{
			if (promptSystem.cards[i].cardType != CardType.Prompt && promptSystem.cards[i].cardRarity != CardRarity.Rare)
				continue;

			float t = (howManyCards == 1) ? 0.5f : promptIndex / (howManyCards - 1);
			float angle = Mathf.Lerp(startAngle, endAngle, t);
			float angleRad = angle * Mathf.Deg2Rad;

			Vector3 offset = new Vector3(
				Mathf.Cos(angleRad) * radius,
				Mathf.Sin(angleRad) * radius,
				0f
			);

			Vector3 cardPos = newNode.transform.position + offset;

			GameObject card = Instantiate(nodePrefab, cardPos, Quaternion.identity);

			conNode.connectedTo.Add(card);
			ConnectNodes con = card.GetComponentInChildren<ConnectNodes>();
			con.isPrompt = false;
			con.connectedFrom.Add(newNode);

			con.AssignCardData(promptSystem.cards[i]);

			GameObject lineObj = new GameObject("ConnectionLine");
			LineRenderer lr = lineObj.AddComponent<LineRenderer>();
			lr.positionCount = 2;
			lr.startWidth = 0.1f;
			lr.endWidth = 0.1f;
			lr.material = new Material(Shader.Find("Sprites/Default"));
			lr.startColor = Color.black;
			lr.endColor = Color.black;

			lr.SetPosition(0, newNode.transform.position);
			lr.SetPosition(1, card.transform.position);

			conNode.connectorLines.Add(lr);
			con.connectedLines.Add(lr);

			promptIndex++;
		}
	}

	public void MoveNode()
	{
		
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 10f;
		currentNodePickup.transform.position = mainCamera.ScreenToWorldPoint(mousePosition);
		ConnectNodes currentNodeConnect = currentNodePickup.GetComponent<ConnectNodes>();
		
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
		ConnectNodes conNode = currenntlySelectedObject.GetComponent<ConnectNodes>();
		if (!conNode.isPrompt) return;

		foreach (GameObject obj in new List<GameObject>(conNode.connectedTo))
		{
			if (obj == null) continue;
			ConnectNodes con = obj.GetComponent<ConnectNodes>();
			if (con != null)
			{
				con.connectedFrom.Remove(currenntlySelectedObject);

				// Remove references from this node's connectedTo
				foreach (GameObject toObj in new List<GameObject>(con.connectedTo))
				{
					if (toObj != null)
					{
						ConnectNodes toCon = toObj.GetComponent<ConnectNodes>();
						if (toCon != null)
							toCon.connectedFrom.Remove(obj);
					}
				}

				foreach (LineRenderer line in new List<LineRenderer>(con.connectorLines))
				{
					if (line != null)
						Destroy(line.gameObject);
				}
			}
			Destroy(obj);
		}

		foreach (GameObject obj in new List<GameObject>(conNode.connectedFrom))
		{
			if (obj == null) continue;
			ConnectNodes con = obj.GetComponent<ConnectNodes>();
			if (con != null)
			{
				con.connectedTo.Remove(currenntlySelectedObject);

				foreach (LineRenderer line in new List<LineRenderer>(con.connectorLines))
				{
					if (line != null && line.gameObject == conNode.gameObject)
					{
						con.connectorLines.Remove(line);
						Destroy(line.gameObject);
					}
				}
			}
		}

		foreach (LineRenderer line in new List<LineRenderer>(conNode.connectedLines))
		{
			if (line != null)
				Destroy(line.gameObject);
		}
		foreach (LineRenderer line in new List<LineRenderer>(conNode.connectorLines))
		{
			if (line != null)
				Destroy(line.gameObject);
		}

		nodes.Remove(currenntlySelectedObject);
		conNode.connectedTo.Clear();
		conNode.connectedFrom.Clear();
		conNode.connectedLines.Clear();
		conNode.connectorLines.Clear();

		Destroy(currenntlySelectedObject);
	}

	public void GetScriptableObject()
	{
		// Placeholder for scriptable object loading
	}


}
