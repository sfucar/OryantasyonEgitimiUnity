using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public void SetActiveGameObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void SetInactiveGameObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }


    public void SetActivateBoolValueGameObject(GameObject gameObject,bool boolValue)
    {
        gameObject.SetActive(boolValue);
    }
}
