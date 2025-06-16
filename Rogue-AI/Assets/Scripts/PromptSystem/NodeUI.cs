using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
	[SerializeField] private AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[SerializeField]
	Vector2 startPosition;
	[SerializeField] Vector2 targetPosition;
	RectTransform rectTransform;
	bool isOut;

	PromptSystem promptSystem;
	ConnectNodes connectNodes;
	bool isPrompt;
	[SerializeField]
	string nothingToEdit;
	[SerializeField]		
	TMP_Text nothingToEditText;

	GameObject button;
	string promptLabel = "Prompt";
	string cardLabel = "Card";

	bool corutineIsRunning;
	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		startPosition = rectTransform.anchoredPosition;
		promptSystem = FindFirstObjectByType<PromptSystem>();
		PromptButtonTag promptButtonTag = GetComponentInChildren<PromptButtonTag>();
		button = promptButtonTag.gameObject;
	}
	public void OnButtonClick()
	{
		if (!isOut)
		{
			if (promptSystem.currenntlySelectedObject == null)
			{
				
				StartCoroutine(NothingToEditCor(2f));
				return;
			}
			if(corutineIsRunning)
			{
				return;
			}
			connectNodes = promptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
			StartCoroutine(SlideOut(.5f));
			isOut = true;
		}
		else
		{
			if(corutineIsRunning)
			{
				return;
			}
			isOut = false;
		}		
	}

	IEnumerator NothingToEditCor(float duration)
	{
		corutineIsRunning = true;
		nothingToEditText.text = nothingToEdit;
		nothingToEditText.gameObject.SetActive(true);
		float elapsedTime = 0f;

		while(elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			nothingToEditText.color = Color.Lerp(Color.white, Color.clear, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		nothingToEditText.gameObject.SetActive(false);
		corutineIsRunning = false;
	}
	
	IEnumerator SlideOut(float duration)
	{
		corutineIsRunning = true;
		float elapsedTime = 0f;
		
		while (elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			float easedT = slideCurve.Evaluate(t);
			
			rectTransform.anchoredPosition = Vector2.LerpUnclamped(startPosition, targetPosition, easedT);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		rectTransform.anchoredPosition = targetPosition;
		corutineIsRunning = false;
	}
	IEnumerator SlideIn(float duration)
	{
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			float easedT = slideCurve.Evaluate(t);

			rectTransform.anchoredPosition = Vector2.LerpUnclamped(targetPosition, startPosition, easedT);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		rectTransform.anchoredPosition = startPosition; 
		isOut = false;
	}

	private void Update()
	{
		CheckSelectedObject();
	}

	void CheckSelectedObject()
	{
		if (promptSystem.currenntlySelectedObject == null) return;
		if(connectNodes == null) return;
		if (connectNodes.gameObject != promptSystem.currenntlySelectedObject)
		{
			connectNodes = promptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
			isPrompt = connectNodes.isPrompt;
			UpdateButton();
		}
		
	}

	public void UpdateText(TMP_InputField text)
	{
		connectNodes = promptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
		connectNodes.UpdatePromptText(text.text);
	}

	void UpdateButton()
	{
		if (isPrompt)
		{
			button.GetComponentInChildren<TMP_Text>().text = promptLabel;
		}
		else
		{
			button.GetComponentInChildren<TMP_Text>().text = cardLabel;
		}
	}
	public void IsPrompt()
	{
		isPrompt = !isPrompt;
		connectNodes = promptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
		connectNodes.isPrompt = isPrompt;
		UpdateButton();
	}
}
