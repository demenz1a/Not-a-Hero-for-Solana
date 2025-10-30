using UnityEngine;
using UnityEngine.EventSystems;

public class DisableEnterForButtons : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
