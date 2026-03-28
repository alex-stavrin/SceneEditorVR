using UnityEngine;

public class AlertsManager : MonoBehaviour
{
    private static AlertsManager _instance;
    public static AlertsManager Instance { get { return _instance; } }

    [SerializeField]
    Alert alert;

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

    public static void SpawnAlert(string alertText)
    {
        if (!Instance.alert) return;

        Alert alert = Instance.alert;
        alert.gameObject.SetActive(true);
        alert.StartTimer();
        alert.SetText(alertText);
    }
}