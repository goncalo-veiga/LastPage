using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image; 
    public GameObject warningObject;  
    public float fadeDuration = 1f; 
    public float visibleDuration = 4f;  

    public CanvasGroup canvasGroup;
    private bool isFading = false;

    private void Start()
    {
        canvasGroup = image.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f; 
    }

    private void Update()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(StartFade());
        }
    }

    private System.Collections.IEnumerator StartFade()
    {

        warningObject.SetActive(true);

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(visibleDuration);
        yield return StartCoroutine(FadeOut());

        warningObject.SetActive(false);

        isFading = false;
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f; 
    }
}