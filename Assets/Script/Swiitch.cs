using UnityEngine;

public class Swiitch : MonoBehaviour
{
    public GameObject[] background;
    int index = 0;

    void Start()
    {
        UpdateBackgroundVisibility();
    }

    void UpdateBackgroundVisibility()
    {
        for (int i = 0; i < background.Length; i++)
        {
            background[i].SetActive(i == index);
        }
    }

    public void Next()
    {
        int prevIndex = index; // Save the previous index
        index += 1;

        if (index >= background.Length)
        {
            index = 0; // Wrap around to the first background
        }

        if (index != prevIndex)
        {
            UpdateBackgroundVisibility();
            Debug.Log(index);
        }
    }

    public void Previous()
    {
        int prevIndex = index; // Save the previous index
        index -= 1;

        if (index < 0)
        {
            index = background.Length - 1; // Wrap around to the last background
        }

        if (index != prevIndex)
        {
            UpdateBackgroundVisibility();
            Debug.Log(index);
        }
    }
}
