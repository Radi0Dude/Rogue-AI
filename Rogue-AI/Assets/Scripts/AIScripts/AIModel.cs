using UnityEngine;


public enum AIModelType
{
    Circle,
    Triangle,
    Square,
    Boss,
}
public class AIModel : MonoBehaviour
{
    [SerializeField] private AIModelTag[] ArtModels;

    public void Init(AIData data)
    {
        foreach (var model in ArtModels) 
        {
            if (data.AIModelType == model.AIModelType)
            {
                model.gameObject.SetActive(true);
                return;
            }
        }
    }
}
