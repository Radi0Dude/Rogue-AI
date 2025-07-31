using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNameAndImage : MonoBehaviour
{

	[SerializeField] private TMP_Text nameText;
	[SerializeField] private Image imageComponent;
	public void ChangeNameImage(string name)
	{
		
		if (nameText != null)
		{
			nameText.text = name;
		}

		
	}
}
