using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldSceneName : MonoBehaviour
{
    private TMP_InputField inputField;

    [SerializeField]
    Button createSceneButton;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();

        if(inputField)
        {
            inputField.onValueChanged.AddListener(OnInputChanged);
        }
    }
    void OnEnable()
    {
        if(createSceneButton)
        {
            createSceneButton.interactable = false;
        }

        if(inputField)
        {
            inputField.text = "";
        }
    }

    void OnInputChanged(string newText)
    {
        if(newText.Length > 0)
        {
            if (createSceneButton) createSceneButton.interactable = true;
        }
        else
        {
            if(createSceneButton) createSceneButton.interactable = false;
        }
    }

}
