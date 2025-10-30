using UnityEngine;

public class SwipeChangeWithCollider : MonoBehaviour
{
    public GameObject[] objects1;
    private int currentIndex = 0;

    private Vector2 startPos;
    private bool isSwiping = false;
    public float swipeThreshold = 100f;

    public Collider2D swipeZone1;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (swipeZone1 != null && swipeZone1.OverlapPoint(mouseWorldPos))
            {
                startPos = mouseWorldPos;
                isSwiping = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float deltaX = endPos.x - startPos.x;

            if (Mathf.Abs(deltaX) > swipeThreshold * 0.01f)
            {
                if (deltaX > 0)
                    PreviousObject(); 
                else
                    NextObject();
            }

            isSwiping = false;
        }
    }

    void NextObject()
    {
        objects1[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % objects1.Length;
        objects1[currentIndex].SetActive(true);
    }

    void PreviousObject()
    {
        objects1[currentIndex].SetActive(false);
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = objects1.Length - 1;
        objects1[currentIndex].SetActive(true);
    }
}
