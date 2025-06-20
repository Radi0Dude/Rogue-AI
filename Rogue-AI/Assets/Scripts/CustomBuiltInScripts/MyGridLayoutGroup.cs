using UnityEngine.UI;

public class MyGridLayoutGroup : GridLayoutGroup
{
    public float GetHeightOfElements()
    {
        return GetTotalPreferredSize(0);
    }
}
