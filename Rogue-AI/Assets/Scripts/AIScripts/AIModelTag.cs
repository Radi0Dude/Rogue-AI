using UnityEngine;

public class AIModelTag : MonoBehaviour
{
    [SerializeField] private AIModelType aiModelType;
    
    public AIModelType AIModelType => aiModelType;
}
