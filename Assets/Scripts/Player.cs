using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;
using System.Text;

public class IpInfoResponse {
    public string ip;
    public string city;
    public string region;
    public string country;
    public string loc;
    public string org;
    public string postal;
    public string timezone;
}

public class Player : MonoBehaviour {
    public GameObject wheel;
    public GameObject blud;
    public ParticleSystem gas1;
    public ParticleSystem gas2;
    public Text ipText;
    public AudioSource movementSound;
    public AudioSource damageSound;

    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float jumpHeight = 6f;
    [SerializeField] float health = 100f;
    [SerializeField] float wheelSpeed = 10f;
    [SerializeField] float ipUpdate = 3f; // seconds
    [SerializeField] public bool isGrounded = false;
    [SerializeField] float damageTimer = 0f; // seconds
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Slider hpbar;
    [SerializeField] Text timer;
    [HideInInspector] static float dmg;
    [HideInInspector] static bool canDamage = false;
    [HideInInspector] int groundLayer = 3;
    [HideInInspector] System.Random random = new System.Random();

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        hpbar = GameObject.FindWithTag("health").GetComponent<Slider>();
        timer = GameObject.FindWithTag("timer").GetComponent<Text>();
        health = 100f;
        if (PlayerPrefs.GetInt("HardMode", 0) == 1) {
            StartCoroutine(GetIP());
        }
    }

    IEnumerator GetIP() {
        UnityWebRequest www = UnityWebRequest.Get("https://ipinfo.io/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.LogError($"Error: www.error");
        } else {
            IpInfoResponse resp = JsonUtility.FromJson<IpInfoResponse>(www.downloadHandler.text);

            Manager.Instance.userIp = resp.ip;
        }
    }

    public string RandomString(int length) {
        const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~ ";
        return new string(
            Enumerable
                .Repeat(chars, length)
                .Select(
                    s => s[random.Next(s.Length)]
                )
                .ToArray()
        );
    }

    void Update() {
        /* Movement */
        float speedmod = 100f / health;
        float _maxSpeed = maxSpeed * speedmod;
        float speed = maxSpeed * speedmod * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            Vector3 angles = wheel.transform.eulerAngles;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                transform.Translate(new Vector3(-speed, 0));
                angles.z += speed * wheelSpeed;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                transform.Translate(new Vector3(speed, 0));
                angles.z -= speed * wheelSpeed;
                transform.localScale = new Vector3(1, 1, 1);
            }

            wheel.transform.eulerAngles = angles;

            if (!movementSound.isPlaying) {
                movementSound.Play(); // Play movement sound (no way)
            }
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))) {
            Vector2 velo = rb.velocity;
            velo.y = jumpHeight;
            rb.velocity = velo;
            gas1.Play();
            gas2.Play();
        }

        /* Health & Health Bar */
        damageTimer -= Time.deltaTime;
        canDamage = damageTimer <= 0;
        if (dmg > 0) {
            Instantiate(blud, transform.position + new Vector3(0, -2.9f, -10), Quaternion.identity);
            damageSound.Play();

            health -= dmg;
            dmg = 0;
            damageTimer = 0.5f;
        }

        hpbar.value = health / 100f;
        if (health <= 0) {
            Scenes.Death();
        }
        
        /* Timer */
        Manager.Instance.playTime += Time.deltaTime;
        float playTime = Manager.Instance.playTime;
        timer.text = $"{playTime:F2}s";

        /* Nice IP bozo */
        if (Manager.Instance.userIp != null) {
            string userIp = Manager.Instance.userIp;
            int shown = (int) Math.Floor(playTime / ipUpdate);

            if (shown > userIp.Length) {
                ipText.color = ((int) playTime) % 2 == 0 ? new Color(1, 0, 0) : new Color(1, 1, 1);
                if (shown - userIp.Length >= (int) Math.Ceiling(10f / ipUpdate)) {
                    Scenes.Death();
                }
                shown = userIp.Length;
            }
            StringBuilder builder = new();

            for (int i = 0; i < shown; i++) {
                builder.Append(userIp.ElementAt(i));
            }

            builder.Append(RandomString(userIp.Length - shown));

            ipText.text = builder.ToString();
        }

        /* Y level check */
        if (gameObject.transform.position.y <= -10) {
            Scenes.Death();
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D) {
        if (collision2D.gameObject.layer == groundLayer) {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision2D) {
        if (collision2D.gameObject.layer == groundLayer) {
            isGrounded = false;
        }
    }

    public static void damage(float damage) {
        if (!canDamage) return;
        if (damage > dmg) {
            dmg = damage;
        }
    }
}
