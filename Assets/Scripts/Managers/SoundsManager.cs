using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    [Header("General")]

    [SerializeField]
    bool playAudio = true;

    [Header("Radial Audio")]
    [SerializeField]
    AudioSource radialOpenAudio;

    [SerializeField]
    AudioSource radialCloseAudio;

    [Header("Select Audio")]

    [SerializeField]
    AudioSource selectAudioSource;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private static void PlayAudioSource(AudioSource audioSource, Vector3 position)
    {
        if(!Instance.playAudio) return;

        if(!audioSource) return;

        audioSource.transform.position = position;
        audioSource.Play();
    }

    public static void PlayRadialOpen(Vector3 position)
    {
        PlayAudioSource(Instance.radialOpenAudio, position);
    }

    public static void PlayRadialClose(Vector3 position)
    {
        PlayAudioSource(Instance.radialCloseAudio, position);
    }

    public static void PlaySelect(Vector3 position)
    {
        PlayAudioSource(Instance.selectAudioSource, position);
    }
}
