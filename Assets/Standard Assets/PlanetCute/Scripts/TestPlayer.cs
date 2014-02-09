using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour {
    public void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            rigidbody2D.velocity = new Vector3(0, 5, 0);
        }
    }
}
