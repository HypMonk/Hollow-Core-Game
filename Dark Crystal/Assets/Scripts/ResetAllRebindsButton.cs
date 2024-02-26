using UnityEngine;
using UnityEngine.InputSystem;

public class ResetAllRebindsButton : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;

    public void RestAllBindings()
    {
        foreach (InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        PlayerPrefs.DeleteKey("rebinds");
    }
}
