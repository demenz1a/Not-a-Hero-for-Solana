using UnityEngine;

public class ActivateThisShit : MonoBehaviour
{
    [SerializeField] private GameObject shit;
    public void ActivateThisShitt()
    {
        shit.SetActive(true);
    }
}
