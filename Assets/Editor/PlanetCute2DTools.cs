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

    [MenuItem("PlanetCute2D/Remove Shadows %&r")]
    public static void RemoveShadows() {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Shadow")) {
            DestroyImmediate(obj);
        }
    }

    [MenuItem("PlanetCute2D/Make Shadows %&m")]
    public static void MakeShadows() {
        GameObject obj = GameObject.Find("ShadowMakerHelper");
        if(obj == null) {
            Debug.LogError("No ShadowMakerHelper in the scene, please drag in the prefab");
            return;
        }
        ShadowMakerHelper helper = obj.GetComponent<ShadowMakerHelper>();

        RemoveShadows();
        foreach(GameObject ground in GameObject.FindGameObjectsWithTag("Ground")) {
            MakeShadow(helper, ground);
        }
    }

    private static void MakeShadow(ShadowMakerHelper helper, GameObject ground) {
        // if there's a block above us, don't bother
        if(IsGroundPresent(ground, new Vector2(0, 1))) {
            return;
        }
        // if there's a block above to the left, make a west shadow
        if(IsGroundPresent(ground, new Vector2(-1, 1))) {
            Debug.Log("Making west shadow for " + ground.name);
            GameObject obj = Instantiate(helper.shadowWest) as GameObject;
            obj.transform.parent = ground.transform;
            obj.transform.position = ground.transform.position;
        }
        // if there's a block above to the right, make an east shadow
        if(IsGroundPresent(ground, new Vector2(1, 1))) {
            GameObject obj = Instantiate(helper.shadowEast) as GameObject;
            obj.transform.parent = ground.transform;
            obj.transform.position = ground.transform.position;
        }

    }

    private static bool IsGroundPresent(GameObject ground, Vector2 offset) {
        offset.y -= .5f;
        offset.y *= VERTICAL_DIST;
        Vector3 checkPos = ground.transform.position + (Vector3)offset;
        //Debug.Log(ground.name + " checking at " + checkPos);
        return Physics2D.OverlapCircle(checkPos, .05f,
            1 << LayerMask.NameToLayer("Ground")) != null;
    }
}
