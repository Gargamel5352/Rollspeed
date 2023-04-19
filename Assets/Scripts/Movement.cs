using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour {
    [SerializeField] float sped = 12f;
    [SerializeField] float health = 100f;
    public Slider hpbar;
    [HideInInspector] Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        health = 100f;
    }

    void Update() {
        float spedmod = 100f / health;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(new Vector3(-sped * spedmod * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(new Vector3(sped * spedmod * Time.deltaTime, 0));
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.Translate(new Vector2(0f, 2f));
            // rb.AddForce(Vector2.up * 10f);
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            health -= 5f;
        }

        hpbar.value = health / 100f;
        if (health <= 0)
        {
            Scenes.death();
        }
    }
}
