using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager _instance;
    public static SceneTransitionManager Instance { get { return _instance; } }

    public Volume transitionVolume;
    public float fadeDuration = 1.0f;
    
    public CanvasGroup[] canvasGroupsToFade; 

    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        transitionVolume.profile.TryGet(out colorAdjustments);
        FadeIn();
    }

    public static void FadeOutAndLoad(string sceneName)
    {
        Instance.StartCoroutine(Instance.FadeOutRoutine(sceneName));
    }

    public static void FadeIn()
    {
        Instance.StartCoroutine(Instance.FadeRoutine(-20f, 0f, 0f, 1f));
    }

    private IEnumerator FadeOutRoutine(string sceneName)
    {
        yield return StartCoroutine(FadeRoutine(0f, -20f, 1f, 0f));
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeRoutine(float startExposure, float endExposure, float startAlpha, float endAlpha)
    {
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            if (colorAdjustments != null)
            {
                colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, endExposure, progress);
            }

            if (canvasGroupsToFade != null)
            {
                foreach (CanvasGroup cg in canvasGroupsToFade)
                {
                    if (cg != null) cg.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
                }
            }

            yield return null;
        }

        if (colorAdjustments != null) colorAdjustments.postExposure.value = endExposure;
        
        if (canvasGroupsToFade != null)
        {
            foreach (CanvasGroup cg in canvasGroupsToFade)
            {
                if (cg != null) cg.alpha = endAlpha;
            }
        }
    }
}