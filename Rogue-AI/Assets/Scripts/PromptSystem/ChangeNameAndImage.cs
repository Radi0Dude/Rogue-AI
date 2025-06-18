using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNameAndImage : MonoBehaviour
{

	[SerializeField] private TMP_Text nameText;
	[SerializeField] private Image imageComponent;
	public void ChangeNameImage(string name, Sprite image)
	{
		
		if (nameText != null)
		{
			nameText.text = name;
		}

		if (imageComponent != null && image != null)
		{
			imageComponent.sprite = image;
		}
	}
}
