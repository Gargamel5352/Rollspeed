using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {
    [SerializeField] float anglePerSecond = 10f;

    void Update() {
        Vector3 angles = transform.eulerAngles;
        angles.z += anglePerSecond * Time.deltaTime;
        transform.eulerAngles = angles;
    }
}
