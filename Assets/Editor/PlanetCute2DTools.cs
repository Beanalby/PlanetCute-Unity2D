using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlanetCute2DTools : Editor {

    private const float VERTICAL_DIST = .42f;

    [MenuItem("PlanetCute2D/Reset Shadows and Overlap %&r")]
    public static void GroundMoved() {
        FixOrder();
        RemoveShadows();
        MakeShadows();
    }

    public static void FixOrder() {
        // all the ground exists at half intervals of y, so set the order as
        // double its position
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Ground")) {
            int newOrder = Mathf.RoundToInt(obj.transform.position.y / VERTICAL_DIST);
            SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
            rend.sortingOrder = newOrder;
            EditorUtility.SetDirty(rend);
        }
    }

    public static void RemoveShadows() {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Shadow")) {
            DestroyImmediate(obj);
        }
    }

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
            CreateShadow(ground, helper.shadowWest);
        }
        // if there's a block above to the right, make an east shadow
        if(IsGroundPresent(ground, new Vector2(1, 1))) {
            CreateShadow(ground, helper.shadowEast);
        }

    }

    private static bool IsGroundPresent(GameObject ground, Vector2 offset) {
        BoxCollider2D box = ground.GetComponent<BoxCollider2D>();

        Vector3 checkPos = ground.transform.position + (Vector3)box.center;
        checkPos.y += box.size.y / 2;
        offset.y -= .5f;
        offset.y *= VERTICAL_DIST;

        checkPos += (Vector3)offset;
        return Physics2D.OverlapCircle(checkPos, .01f,
            1 << LayerMask.NameToLayer("Ground")) != null;
    }

    private static void CreateShadow(GameObject ground, GameObject prefab) {
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = ground.transform;
        BoxCollider2D box = ground.GetComponent<BoxCollider2D>();
        Vector3 pos = ground.transform.position + (Vector3)box.center;
        pos.y += box.size.y / 2;
        obj.transform.position = pos;
    }
}
