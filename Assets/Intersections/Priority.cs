using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR

using UnityEditor;

#endif

public class Priority : MonoBehaviour {
    public List<GameObject> walls;
    public bool flag = false;

    //public Test precedenza1;
    //public Test precedenza2;

    void Start() {
        for (int i = 0; i < walls.Count; i++) {
            walls[i].GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Car")) {
            flag = true;
            for (int i = 0; i < walls.Count; i++) {
                walls[i].GetComponent<BoxCollider>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Car")) {
            flag = false;
            for (int i = 0; i < walls.Count; i++) {
                walls[i].GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
