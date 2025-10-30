using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [Header("Текущие объекты в комнате")]
    public NeedType playerSkinType;
    public NeedType standType;
    public NeedType flagType;
    public int coins;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public int Evaluate(NeedType visitorNeed)
    {
        int score = 0;

        if (playerSkinType == visitorNeed) score++;
        if (standType == visitorNeed) score++;
        if (flagType == visitorNeed) score++;

        return score;
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"Добавлено {amount} монет. Всего: {coins}");
    }

    public bool SpendCoin()
    {
        if (coins > 0)
        {
            coins--;
            return true;
        }
        return false;
    }
}

