using TMPro;
using UnityEngine;

public class KeyboardVR : MonoBehaviour
{
    [Header("Spawn Settings")]

    [SerializeField]
    float spawnDistanceFromPlayer = 5.0f;

    [SerializeField]
    float spawnHeight = 1.2f;

    [Header("Keyboard Settings")]
    [SerializeField]
    public bool caps = false;

    string outputBuffer = "";

    TMP_InputField targetInputField;

    [Header("Keyboard Components")]
    [SerializeField]
    TextMeshProUGUI capsText;

    public event System.Action<bool> CapsChanged;

    public void InitKeyboard(TMP_InputField newTargetInputField)
    {

        outputBuffer = newTargetInputField.text;
        targetInputField = newTargetInputField;

        if (PlayerRig.Instance)
        {
            Vector3 playerPosition = PlayerRig.Instance.gameObject.transform.position;
            Vector3 playerLookDirection = PlayerRig.Instance.GetPlayerHead().forward;

            playerLookDirection.y = spawnHeight;

            Vector3 spawnPosition = playerPosition + playerLookDirection * spawnDistanceFromPlayer;
            transform.position = spawnPosition;
        }


        CapsChanged.Invoke(caps);
        UpdateCapsText();
        UpdateOutputBufferText();
    }

    public void KeyPressed(string keyValue)
    {
        outputBuffer += keyValue;
        UpdateOutputBufferText();
    }

    public void BackspacePressed()
    {
        if (outputBuffer.Length == 0) return;

        outputBuffer = outputBuffer.Substring(0, outputBuffer.Length - 1);
        UpdateOutputBufferText();
    }

    public void CapsPressed()
    {
        caps = !caps;
        CapsChanged.Invoke(caps);
        UpdateCapsText();
    }

    public void SpacePressed()
    {
        outputBuffer += " ";
        UpdateOutputBufferText();
    }

    public void EnterPressed()
    {
        Destroy(gameObject);
    }

    void UpdateCapsText()
    {
        capsText.text = "Caps (" + (caps ? "ON" : "OFF") + ")"; 
    }

    void UpdateOutputBufferText()
    {
        targetInputField.text = outputBuffer;
    }

    public void SetTargetInputfield(TMP_InputField newTargetInputfield)
    {
        targetInputField = newTargetInputfield;
    }
}
