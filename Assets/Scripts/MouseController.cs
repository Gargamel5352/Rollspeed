using UnityEngine;

public class MouseController: MonoBehaviour {
    public CursorLockMode mode = CursorLockMode.None;
    public bool visible = true;

    void Awake() {
        Cursor.lockState = mode;
        Cursor.visible = visible;
    }
}
