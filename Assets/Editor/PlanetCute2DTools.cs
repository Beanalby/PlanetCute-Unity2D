using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlanetCute2DTools : Editor {

    private const float VERTICAL_DIST = .42f;

    [MenuItem("PlanetCute2D/Fix Order %&f")]
    public static void FixOrder() {
        // all the ground exists at half intervals of y, so set the order as
        // double its position
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Ground")) {
            SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
            rend.sortingOrder = Mathf.RoundToInt(obj.transform.position.y / VERTICAL_DIST);
        }
    }
}
