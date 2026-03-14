using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Modal : MonoBehaviour
{
    [Header("Text")]

    [SerializeField]
    public TextMeshProUGUI text;

    [Header("Negative Button")]

    [SerializeField]
    public Button negativeButton;

    [SerializeField]
    public TextMeshProUGUI negativeButtonText;

    [Header("Positive Button")]

    [SerializeField]
    public Button positiveButton;

    [SerializeField]
    public TextMeshProUGUI positiveButtonText;
}