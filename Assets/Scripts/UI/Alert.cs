using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alert : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI alertText;

    [SerializeField]
    Image progressBar;

    [SerializeField]
    float timeBeforeDisappear = 2.0f;

    float timer;

    void Start()
    {
        timer = 0.0f;
    }

    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            progressBar.fillAmount = timer / timeBeforeDisappear;

            if(timer <= 0)
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void StartTimer()
    {
        timer = timeBeforeDisappear;
        progressBar.fillAmount = 0.0f;
    }

    public void SetText(string text)
    {
        alertText.SetText(text);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
