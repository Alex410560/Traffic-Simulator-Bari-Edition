using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public List<GameObject> walls1;
    public bool flag1 = false;

    //public Priority precedenza1;
    //public Priority precedenza2;

    void Start() {
        for (int i = 0; i < walls1.Count; i++) {
            walls1[i].GetComponent<BoxCollider>().enabled = false;
        }
    }

    //private void OnTriggerStay(Collider other) {

        //if (other.CompareTag("Car")) {
        //    if (precedenza1.flag && flag1|| precedenza2.flag && flag1) {
        //        //Debug.Log(precedenza1.flag);
        //        flag1 = true;
        //        for (int i = 0; i < walls1.Count; i++) {
        //            walls1[i].GetComponent<BoxCollider>().enabled = false;
        //        }
        //    } else if (precedenza1.flag && !flag1 || precedenza2.flag && !flag1) {
        //        //Debug.Log(precedenza1.flag);
        //        flag1 = false;
        //        for (int i = 0; i < walls1.Count; i++) {
        //            walls1[i].GetComponent<BoxCollider>().enabled = false;
        //        }
        //    } else {
        //        //Debug.Log(precedenza1.flag);
        //        flag1 = true;
        //        for (int i = 0; i < walls1.Count; i++) {
        //            walls1[i].GetComponent<BoxCollider>().enabled = true;
        //        }
        //    }
        //}       
    //}

    //private void OnTriggerExit(Collider other) {
    //    if (other.CompareTag("Car")) {
    //        flag1 = false;
    //        for (int i = 0; i < walls1.Count; i++) {
    //            walls1[i].GetComponent<BoxCollider>().enabled = false;
    //        }
    //    }
    //}
}
