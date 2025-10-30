using UnityEngine;

public class ChanigngNeedType : MonoBehaviour
{
    public RoomManager roomManager;
    public bool isStand;
    public NeedType myType;

    private void OnEnable()
    {
        if (roomManager == null)
        {
            Debug.LogWarning($"{name}: RoomManager не назначен!");
            return;
        }

        if (isStand)
        {
            roomManager.standType = myType;
            Debug.Log($"{name}: standType изменён на {myType}");
        }
        else
        {
            roomManager.flagType = myType;
            Debug.Log($"{name}: flagType изменён на {myType}");
        }
    }
}
