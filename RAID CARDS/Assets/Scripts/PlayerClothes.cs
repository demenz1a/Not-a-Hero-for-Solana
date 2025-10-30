using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerClothes : MonoBehaviour
{
    [Header("Основные компоненты")]
    [SerializeField] private Animator characterAnimator;        
    [SerializeField] private RuntimeAnimatorController[] skinAnimators; 

    [Header("Управление UI")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;

    [Header("Дополнительные объекты")]
    [SerializeField] private Animator curtainsAnimator;         
    [SerializeField] private RoomManager roomManager;           

    private int currentIndex = 0;

    private void Start()
    {
        leftButton.onClick.AddListener(PreviousSkin);
        rightButton.onClick.AddListener(NextSkin);
        UpdateSkin();
    }

    public void NextSkin()
    {
        currentIndex++;
        if (currentIndex >= skinAnimators.Length)
            currentIndex = 0;
        StartCoroutine(ChangeSkinSequence());
    }

    public void PreviousSkin()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = skinAnimators.Length - 1;
        StartCoroutine(ChangeSkinSequence());
    }

    private IEnumerator ChangeSkinSequence()
    {
        if (curtainsAnimator != null)
        {
            curtainsAnimator.SetTrigger("Activate");
        }

        yield return new WaitForSeconds(0.2f); 

        UpdateSkin();

        curtainsAnimator.SetTrigger("Deactivate");
    }

    private void UpdateSkin()
    {
        if (characterAnimator != null && currentIndex < skinAnimators.Length)
        {
            characterAnimator.runtimeAnimatorController = skinAnimators[currentIndex];
        }
        else
        {
            Debug.LogWarning("Аниматор персонажа или список контроллеров не назначен!");
        }

        if (roomManager != null)
        {
            roomManager.playerSkinType = (NeedType)currentIndex;
            Debug.Log($"PlayerSkinType изменён на {roomManager.playerSkinType}");
        }
        else
        {
            Debug.LogWarning("RoomManager не назначен в PlayerClothes!");
        }
    }
}
