using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    private int starCount;
    private int displayedStars;
    private float timeStep = 0.5f;
    private float lastTimeStep;
    private RectTransform image;

    public void DisplayStars(int stars)
    {
        displayedStars = 0;
        starCount = stars;
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RectTransform>();
        image.sizeDelta = new Vector2(0f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (displayedStars < starCount && Time.time - lastTimeStep > timeStep)
        {
            displayedStars++;
            image.sizeDelta = new Vector2(displayedStars * 100f, 100f);
            lastTimeStep = Time.time;
        }
    }
}
