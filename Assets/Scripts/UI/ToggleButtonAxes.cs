using UnityEngine;

public class ToggleButtonAxes : ToggleButton
{

    public override void Start()
    {
        base.Start();

        toggleState = PlayerPreferencesManager.Instance.useAxes;
    }

    public override void OnStateUpdated(bool newState)
    {
        base.OnStateUpdated(newState);
        PlayerPreferencesManager.Instance.SetUseAxes(newState);
    }
}
