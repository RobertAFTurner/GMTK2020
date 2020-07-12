using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject StarEffectPrefab;

    private ParticleSystem starEffect;
    private int starCount;
    private int displayedStars;
    private float timeStep = 0.3f;
    private float lastTimeStep;
    private RectTransform image;
    private Vector2 originalSize;

    public void DisplayStars(int stars)
    {
        displayedStars = 0;
        starCount = stars;
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RectTransform>();
        originalSize = image.sizeDelta;
        //image.sizeDelta = new Vector2(0f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (displayedStars < starCount && Time.time - lastTimeStep > timeStep)
        {            
            displayedStars++;
            image.sizeDelta = new Vector2((displayedStars-1)*100f, originalSize.y);
            var pitchIncrease = displayedStars / 5f;
            AudioManagerController.Instance.PlaySound("Star", 0f, 1f + pitchIncrease);

            lastTimeStep = Time.time;

            if (displayedStars == 5)
            {
                var cam = Camera.main;
                var v = cam.ViewportToWorldPoint(new Vector3(0.3f, 0.5f, cam.nearClipPlane+10f));

                starEffect = Instantiate(StarEffectPrefab, v, Quaternion.identity).GetComponent<ParticleSystem>();
                starEffect.gameObject.SetActive(true);
                starEffect.Play();
                AudioManagerController.Instance.PlaySound("FiveStars");
            }
        }

        if (starEffect != null)
        {
            if (starEffect.isStopped)
            {
                Destroy(starEffect.gameObject);
                starEffect = null;
            }
        }
    }
}
