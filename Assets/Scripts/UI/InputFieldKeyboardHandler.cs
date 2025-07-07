using TMPro;
using UnityEngine;

[RequireComponent (typeof(TMP_InputField))]
public class InputFieldKeyboardHandler : MonoBehaviour
{
    TMP_InputField inputField;

    [SerializeField]
    GameObject keyboardPrefab;

    KeyboardVR keyboard = null;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onFocusSelectAll = false;
        inputField.onSelect.AddListener(OnSelect);
        inputField.selectionColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        inputField.customCaretColor = true;
        inputField.caretColor = new Color(0, 0, 0, 0);
        inputField.caretWidth = 0;
    }

    void OnSelect(string value)
    {
        if(!keyboard)
        {
            GameObject keyboardGameobject = Instantiate(keyboardPrefab);
            if(keyboardGameobject)
            {
                keyboard = keyboardGameobject.GetComponent<KeyboardVR>();
                keyboard.InitKeyboard(inputField);
            }
        }
    }

    private void OnDisable()
    {
        if(keyboard)
        {
            Destroy(keyboard.gameObject);
        }
    }
}
