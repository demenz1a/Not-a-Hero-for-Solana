using UnityEngine;
using TMPro;
using System.Collections;
using IDosGames;
using System.Collections.Generic;
using Unity.Cinemachine;
using NBitcoin.Protocol.Behaviors;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class NPCVisitor : MonoBehaviour
{
    public AudioSource auidoSource;
    public AudioClip coin;
    public AudioClip yes;
    public static NPCVisitor instance;
    public GameObject perehodPrefab;
    [Header("Параметры движения")]

    public float moveSpeed = 2f;
    private Vector2 moveDirection = Vector2.right;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private int returnCount = 0;
    private float moveMultiplayer = 1f;
    public float moveMultiplayerAdd = 0.2f;
    public float speechDuration = 2f;


    [Header("Потребности")]
    public NeedType currentNeed = NeedType.Weapon; 
    private NeedType nextHintNeed;
    private bool isHintGiven = false;

    [Header("Ссылки")]
    public RoomManager currentRoom;
    public TextMeshPro speechText;

    [Header("Сцена босса")]
    public string bossSceneName = "BossFight";
    private int MoneyPlayer;

    [Header("🎥 Камера (Cinemachine)")]
    public CinemachineCamera mainCamera; 
    public CinemachineCamera mainheroCamera;

    public bool isInRoom = false;
    public bool isEvaluating = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextHintNeed = GetNextNeed(currentNeed);
        Debug.Log($"{name} стартует с нуждой: {currentNeed}");
    }

    private void FixedUpdate()
    {
        if (!isEvaluating)
            rb.MovePosition(rb.position + moveDirection * moveSpeed * moveMultiplayer * Time.fixedDeltaTime);

        Flip();
    }

    private void Flip()
    {
        if ((isFacingRight && moveDirection.x < 0f) || (!isFacingRight && moveDirection.x > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Повороты
        if (collision.CompareTag("TurnRight"))
        {
            moveDirection = Vector2.right;
            moveMultiplayer += moveMultiplayerAdd;
            returnCount++;
        }

        // Выход из комнаты
        else if (collision.CompareTag("ExitRoom"))
        {
            isEvaluating = false;
            currentRoom = null;
            isInRoom = false;
            ShowSpeech("");
            CamToPlayer();
        }

        // Вход в комнату (EnterRoom)
        else if (collision.CompareTag("EnterRoom"))
        {
            currentRoom = collision.GetComponent<RoomManager>();
            CamToHero();
            if (currentRoom != null && !isEvaluating)
            {
                isInRoom = true;
                if (returnCount >= 11)
                    StartCoroutine(StartBossEvent());
                else
                    StartCoroutine(EvaluateAndTurnBack());
            }
        }
    }

    private IEnumerator EvaluateAndTurnBack()
    {
        isEvaluating = true;
        moveDirection = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        int score = currentRoom.Evaluate(currentNeed);
        Debug.Log($"{name} оценивает комнату. Нужда: {currentNeed}, результат: {score}");

        if (score <= 0)
        {
            ShowSpeech("Your loot is kinda shit...");
            GiveMoney(0);
        }
        else if (score == 1)
        {
            ShowSpeech("It's okay");
            GiveMoney(2);
            PlayCoin();
            ClaimRewardSystem.ClaimCoinReward(2, 1);
        }
        else if (score >= 2)
        {
            ShowSpeech("NICE!");
            PlayYes();
            yield return new WaitForSeconds(0.3f);
            GiveMoney(4);
            PlayCoin();
            ClaimRewardSystem.ClaimCoinReward(4, 1);
        }

        yield return new WaitForSeconds(speechDuration + 0.5f);

        ShowSpeech(GetHintMessage(nextHintNeed));

        yield return new WaitForSeconds(speechDuration + 0.5f);

        currentNeed = nextHintNeed;
        nextHintNeed = GetNextNeed(currentNeed);
        isHintGiven = false;

        moveDirection = Vector2.left;
        isEvaluating = false;
    }


    private NeedType GetNextNeed(NeedType current)
    {
        switch (current)
        {
            case NeedType.Weapon: return NeedType.Medical;
            case NeedType.Medical: return NeedType.Magic;
            case NeedType.Magic: return NeedType.Weapon;
            default: return NeedType.Weapon;
        }
    }

    private string GetHintMessage(NeedType need)
    {
        switch (need)
        {
            case NeedType.Weapon:
                return "I'm going to beat some boss, next time i'll need a [shake]new weapon[/shake].";
            case NeedType.Medical:
                return "He kicked my ass...i'll need [shake]heal[/shake] next time";
            case NeedType.Magic:
                return "I'm want to warm mobs, i'll need [shake]potions[/shake] next time.";
            default:
                return "uhh idk...";
        }
    }

    private void GiveMoney(int amount)
    {
        if (amount > 0)
        {
            RoomManager.instance.coins += amount;
            Debug.Log($"{name} дал торговцу {amount} монет!");
        }
        else
        {
            Debug.Log($"{name} не дал ничего...");
        }
    }


    private IEnumerator StartBossEvent()
    {
        isEvaluating = true;
        moveDirection = Vector2.zero;

        ShowSpeech("Umm sorry man");
        yield return new WaitForSeconds(2f);

        ShowSpeech("But im cleared location, so now i'm need to kill you...");
        yield return new WaitForSeconds(3f);

        Debug.Log("Загрузка сцены босса...");
        Instantiate(perehodPrefab);
    }

    private Coroutine typingCoroutine;
    private List<(int start, int end)> shakeRanges = new List<(int, int)>();

    public void ShowSpeech(string message)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(ParseShakeTags(message)));
    }

    private (string cleanText, List<(int start, int end)> ranges) ParseShakeTags(string message)
    {
        shakeRanges.Clear();
        string clean = "";
        int offset = 0;

        while (true)
        {
            int start = message.IndexOf("[shake]", offset);
            if (start == -1) break;
            int end = message.IndexOf("[/shake]", start);
            if (end == -1) break;

            int cleanStart = start - CountRemovedTagsBefore(message, start);
            int cleanEnd = end - CountRemovedTagsBefore(message, end) - "[shake]".Length;
            shakeRanges.Add((cleanStart, cleanEnd));

            offset = end + "[/shake]".Length;
        }

        clean = message.Replace("[shake]", "").Replace("[/shake]", "");
        return (clean, shakeRanges);
    }

    private int CountRemovedTagsBefore(string text, int index)
    {
        int count = 0;
        int pos = 0;
        while (pos < index)
        {
            int nextStart = text.IndexOf("[shake]", pos);
            int nextEnd = text.IndexOf("[/shake]", pos);
            if (nextStart != -1 && nextStart < index)
            {
                count += "[shake]".Length;
                pos = nextStart + "[shake]".Length;
            }
            else if (nextEnd != -1 && nextEnd < index)
            {
                count += "[/shake]".Length;
                pos = nextEnd + "[/shake]".Length;
            }
            else break;
        }
        return count;
    }

    private IEnumerator TypeText((string cleanText, List<(int start, int end)> ranges) parsed)
    {
        speechText.text = "";
        string message = parsed.cleanText;
        float delay = 0.03f;

        for (int i = 0; i < message.Length; i++)
        {
            speechText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(delay);
        }

        StartCoroutine(AnimateShake(parsed.ranges));
        StartCoroutine(ClearSpeech());
    }

    private IEnumerator ClearSpeech()
    {
        yield return new WaitForSeconds(speechDuration);
        speechText.text = "";
    }


    private IEnumerator AnimateShake(List<(int start, int end)> ranges)
    {
        TMP_TextInfo textInfo = speechText.textInfo;
        float shakeTime = 1.5f; 
        float timer = 0f;
        float amplitude = 0.1f;

        while (timer < shakeTime)
        {
            timer += Time.deltaTime;
            float t = 1f - (timer / shakeTime);
            float currentAmp = amplitude * t;

            speechText.ForceMeshUpdate();
            textInfo = speechText.textInfo;

            foreach (var range in ranges)
            {
                for (int i = range.start; i < range.end && i < textInfo.characterCount; i++)
                {
                    var charInfo = textInfo.characterInfo[i];
                    if (!charInfo.isVisible) continue;

                    int vIndex = charInfo.vertexIndex;
                    Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                    Vector2 offset = new Vector2(Random.Range(-currentAmp, currentAmp), Random.Range(-currentAmp, currentAmp));

                    for (int j = 0; j < 4; j++)
                        verts[vIndex + j] += (Vector3)offset;
                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                speechText.UpdateGeometry(meshInfo.mesh, i);
            }

            yield return new WaitForSeconds(0.03f);
        }
    }

    private void CamToHero()
    {
        mainCamera.Priority = 1;
        mainheroCamera.Priority = 2;
    }

    private void CamToPlayer()
    {
        mainCamera.Priority = 2;
        mainheroCamera.Priority = 1;
    }

    public void PlayYes()
    {
        auidoSource.PlayOneShot(yes);
    }

    public void PlayCoin()
    {
        auidoSource.PlayOneShot(coin);
    }
}