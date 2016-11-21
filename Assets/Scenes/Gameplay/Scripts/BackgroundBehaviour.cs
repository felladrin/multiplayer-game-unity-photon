using UnityEngine;
using UnityEngine.UI;

public class BackgroundBehaviour : MonoBehaviour
{
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f), Random.Range(0.6f, 0.9f));
        InvokeRepeating("ChangeColor", 0.5f, 0.5f);
    }

    void ChangeColor()
    {
        var newR = image.color.r + 0.003f * (RandomBool() ? 1 : -1);
        var newG = image.color.g + 0.003f * (RandomBool() ? 1 : -1);
        var newB = image.color.b + 0.003f * (RandomBool() ? 1 : -1);

        if (newR < 0 || newR > 1)
            newR = image.color.r;

        if (newG < 0 || newG > 1)
            newG = image.color.g;

        if (newB < 0 || newB > 1)
            newB = image.color.b;

        image.color = new Color(newR, newG, newB);
    }

    static bool RandomBool()
    {
        return Random.value > 0.5;
    }
}