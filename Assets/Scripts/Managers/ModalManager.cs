using UnityEngine;
using UnityEngine.Events;

public class ModalManager : MonoBehaviour
{
    private static ModalManager _instance;
    public static ModalManager Instance { get { return _instance; } }

    [SerializeField]
    Modal modal;

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
        modal.gameObject.SetActive(false);
    }

    public static void OpenModal(string modalText, string negativeButtonText, string positiveButtonText, 
        UnityAction onPositiveButtonClicked, bool closeOnConfirm)
    {
        Instance.modal.gameObject.SetActive(true);

        Instance.modal.text.text = modalText;

        Instance.modal.negativeButtonText.text = negativeButtonText;
        Instance.modal.positiveButtonText.text = positiveButtonText;

        Instance.modal.negativeButton.onClick.RemoveAllListeners();
        Instance.modal.positiveButton.onClick.RemoveAllListeners();

        Instance.modal.negativeButton.onClick.AddListener(CloseModal);
        Instance.modal.positiveButton.onClick.AddListener(onPositiveButtonClicked);

        if(closeOnConfirm)
        {
            Instance.modal.positiveButton.onClick.AddListener(CloseModal);
        }
    }

    public static void CloseModal()
    {
        Instance.modal.gameObject.SetActive(false);

        Instance.modal.negativeButton.onClick.RemoveAllListeners();
        Instance.modal.positiveButton.onClick.RemoveAllListeners();
    }
}