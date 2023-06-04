using UnityEngine;

public class NoDestroy : MonoBehaviour {
    void Awake() {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (objects.Length > 1) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
