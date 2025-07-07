using System.Collections;
using System.Collections.Generic;
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

	[SerializeField]
	GameObject SelectCardPrefab;

	PromptSystem promptSystem;
	VisualPromptSystem visualPromptSystem;
	ConnectNodes connectNodes;
	bool isPrompt;
	[SerializeField]
	string nothingToEdit;
	[SerializeField]
	TMP_Text nothingToEditText;

	GameObject button;
	string promptLabel = "Prompt";
	string cardLabel = "Card";

	GameObject getText;
	[SerializeField]
	GameObject getSelectCards;
	[SerializeField]
	GameObject getSelectCardSpawnPoint;

	bool corutineIsRunning;

	public List<CardData> cards = new List<CardData>();
	GameObject currentlySelectedObject;
	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		startPosition = rectTransform.anchoredPosition;
		visualPromptSystem = FindFirstObjectByType<VisualPromptSystem>();
		promptSystem = FindFirstObjectByType<PromptSystem>();
		PromptButtonTag promptButtonTag = GetComponentInChildren<PromptButtonTag>();
		getText = FindFirstObjectByType<TMP_InputField>().gameObject;
		button = promptButtonTag.gameObject;
	}
	public void OnButtonClick()
	{
		if (!isOut)
		{
			if (visualPromptSystem.currenntlySelectedObject == null)
			{

				StartCoroutine(NothingToEditCor(2f));
				return;
			}
			if (corutineIsRunning)
			{
				return;
			}
			connectNodes = visualPromptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
			StartCoroutine(SlideOut(.5f));
			isOut = true;
		}
		else
		{
			if (corutineIsRunning)
			{
				return;
			}
			StartCoroutine(SlideIn(.5f));
			isOut = false;
		}
	}

	IEnumerator NothingToEditCor(float duration)
	{
		corutineIsRunning = true;
		nothingToEditText.text = nothingToEdit;
		nothingToEditText.gameObject.SetActive(true);
		float elapsedTime = 0f;

		while (elapsedTime < duration)
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
		if (connectNodes == null) return;
		if (currentlySelectedObject != visualPromptSystem.currenntlySelectedObject)
		{
			IsPrompt();
		}
	}

	void CheckSelectedObject()
	{
		if (visualPromptSystem.currenntlySelectedObject == null) return;
		if (connectNodes == null) return;
		if (connectNodes.gameObject != visualPromptSystem.currenntlySelectedObject)
		{
			connectNodes = visualPromptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
			isPrompt = connectNodes.isPrompt;
			UpdateButton();
		}

	}

	public void UpdateText(TMP_InputField text)
	{
		connectNodes = visualPromptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
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
		if(currentlySelectedObject != visualPromptSystem.currenntlySelectedObject || currentlySelectedObject == null)
		{
			isPrompt = visualPromptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>().isPrompt;
		
		}
		else
		{
			isPrompt = !isPrompt;
		}

		getText.SetActive(isPrompt);
		getSelectCards.SetActive(!isPrompt);
		if (getSelectCards.activeSelf == true)
		{
			if (cards.Count != promptSystem.cards.Count)
			{
				cards = promptSystem.cards;
				foreach (var card in cards)
				{
					GameObject selectCard = Instantiate(SelectCardPrefab, getSelectCardSpawnPoint.transform);
					selectCard.transform.parent = getSelectCardSpawnPoint.transform;
					selectCard.GetComponent<ChangeNameAndImage>().ChangeNameImage(card.cardName, card.cardImage);
					selectCard.GetComponentInChildren<GetCard>().cardData = card;
					selectCard.GetComponentInChildren<GetCard>().currentlySelected = visualPromptSystem.currenntlySelectedObject;
				}
			}
			
		}
		connectNodes = visualPromptSystem.currenntlySelectedObject.GetComponent<ConnectNodes>();
		connectNodes.isPrompt = isPrompt;
		UpdateButton();
		currentlySelectedObject = visualPromptSystem.currenntlySelectedObject;
	}
}
