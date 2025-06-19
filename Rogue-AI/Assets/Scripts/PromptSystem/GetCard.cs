using UnityEngine;

public class GetCard : MonoBehaviour
{
	public CardData cardData;

	public GameObject currentlySelected;

	VisualPromptSystem visProm;

	private void Start()
	{
		visProm = FindFirstObjectByType<VisualPromptSystem>();
	}

	public void OnButtonClick()
	{
		currentlySelected = visProm.currenntlySelectedObject;
		currentlySelected.GetComponent<ConnectNodes>().AssignCardData(cardData);
	}
}
