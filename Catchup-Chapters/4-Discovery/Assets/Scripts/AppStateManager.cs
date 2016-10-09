using HoloToolkit.Unity;
using UnityEngine;

/// <summary>
/// Keeps track of the current state of the experience.
/// </summary>
public class AppStateManager : Singleton<AppStateManager>
{
    /// <summary>
    /// Enum to track progress through the experience.
    /// </summary>
    public enum AppState
    {
        Starting = 0,
        PickingAvatar,
        WaitingForAnchor,
        WaitingForStageTransform,
        Ready
    }

    /// <summary>
    /// Tracks the current state in the experience.
    /// </summary>
    public AppState CurrentAppState { get; private set; }

    void Start()
    {
        
#if UNITY_EDITOR
        //OVERRIDE FOR UNITY
        PlayerAvatarStore.Instance.DismissAvatarPicker();
        LocalPlayerManager.Instance.SetUserAvatar(0);
        ImportExportAnchorManager.Instance.CurrentState = ImportExportAnchorManager.ImportExportState.Ready;

        CurrentAppState = AppState.WaitingForAnchor;

#else
        // We start in the 'picking avatar' mode.
        CurrentAppState = AppState.PickingAvatar;

        // We start by showing the avatar picker.
        PlayerAvatarStore.Instance.SpawnAvatarPicker();

#endif

    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), "state: " + CurrentAppState);
    }

    void Update()
    {
#if UNITY_EDITOR
        ImportExportAnchorManager.Instance.CurrentState = ImportExportAnchorManager.ImportExportState.Ready;
#endif

        switch (CurrentAppState)
        {
            case AppState.PickingAvatar:
                // Avatar picking is done when the avatar picker has been dismissed.
                if (PlayerAvatarStore.Instance.PickerActive == false)
                {
                    CurrentAppState = AppState.WaitingForAnchor;
                    Debug.Log("state: " + CurrentAppState);
                }
                break;
            case AppState.WaitingForAnchor:
                if (ImportExportAnchorManager.Instance.AnchorEstablished)
                {
                    CurrentAppState = AppState.WaitingForStageTransform;
                    GestureManager.Instance.OverrideFocusedObject = HologramPlacement.Instance.gameObject;
                    Debug.Log("state: " + CurrentAppState);
                }
                break;
            case AppState.WaitingForStageTransform:
                // Now if we have the stage transform we are ready to go.
                if (HologramPlacement.Instance.GotTransform)
                {
                    CurrentAppState = AppState.Ready;
                    Debug.Log("state: " + CurrentAppState);
                    GestureManager.Instance.OverrideFocusedObject = null;
                }
                break;
        }
    }
}