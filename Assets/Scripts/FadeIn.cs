using System.Collections;
using UnityEngine;

public class FadeIn : MonoBehaviour {
    public float duration;
    [SerializeField] TextMesh text;
    void Awake() {
        text = gameObject.GetComponent<TextMesh>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    void Update() {
        StartCoroutine(FadeTextToFullAlpha(text, duration));
    }

    public IEnumerator FadeTextToFullAlpha(TextMesh text, float duration) {
        while (text.color.a < 1.0f) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / duration));
            yield return null;
        }
    }
}
