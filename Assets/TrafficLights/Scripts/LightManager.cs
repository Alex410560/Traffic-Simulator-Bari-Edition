using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class LightManager : MonoBehaviour {
    // setup for lights timer
    [Header("Durata delle luci")]
    public float YellowTime;
    public float GreenTime;

    //test
    public bool f1;
    public bool f2;
    public bool f3;
    public bool f4;

    [HideInInspector] public GameObject trafficLight1;
    [HideInInspector] public GameObject trafficLight2;
    [HideInInspector] public GameObject trafficLight3;
    [HideInInspector] public GameObject trafficLight4;

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(LightManager))]
    public class LightManagerEditor : Editor {
        
        SerializedProperty f1;
        SerializedProperty trafficLight1;

        SerializedProperty f2;
        SerializedProperty trafficLight2;

        SerializedProperty f3;
        SerializedProperty trafficLight3;

        SerializedProperty f4;
        SerializedProperty trafficLight4;

        private void OnEnable() {
            f1 = serializedObject.FindProperty(nameof(LightManager.f1));
            trafficLight1 = serializedObject.FindProperty(nameof(LightManager.trafficLight1));

            f2 = serializedObject.FindProperty(nameof(LightManager.f2));
            trafficLight2 = serializedObject.FindProperty(nameof(LightManager.trafficLight2));

            f3 = serializedObject.FindProperty(nameof(LightManager.f3));
            trafficLight3 = serializedObject.FindProperty(nameof(LightManager.trafficLight3));

            f4 = serializedObject.FindProperty(nameof(LightManager.f4));
            trafficLight4 = serializedObject.FindProperty(nameof(LightManager.trafficLight4));
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            serializedObject.Update();

            if (f1.boolValue) { 
                EditorGUILayout.PropertyField(trafficLight1);
            }

            if (f2.boolValue) {
                EditorGUILayout.PropertyField(trafficLight2);
            }

            if (f3.boolValue) {
                EditorGUILayout.PropertyField(trafficLight3);
            }

            if (f4.boolValue) {
                EditorGUILayout.PropertyField(trafficLight4);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
    #endregion

    void Start() {
        if (f1 && f2 && !f3 && !f4) {
            StartCoroutine(TrafficLightCycle(trafficLight1));
            StartCoroutine(TrafficLightCycle(trafficLight2));
        } else if (f1 && f2 && f3 && !f4) {
            StartCoroutine(TrafficLightCycle3());
        } else if (f1 && f2 && f3 && f4) {
            StartCoroutine(TrafficLightCycle(trafficLight1));
            StartCoroutine(TrafficLightCycle(trafficLight2));
            StartCoroutine(TrafficLightCycle(trafficLight3));
            StartCoroutine(TrafficLightCycle(trafficLight4));
        }
    }


    private IEnumerator TrafficLightCycle(GameObject t) {
        while (true) // Infinite Loop
        {
            if (t.CompareTag("Semaforo1")) {
                // Rosso
                SetLightState(true, false, false, t);
                SetLightStateP(false, false, true, t);
                yield return new WaitForSeconds(GreenTime);

                SetLightState(true, false, false, t);
                SetLightStateP(false, true, false, t);
                yield return new WaitForSeconds(YellowTime);

                // Verde
                SetLightState(false, false, true, t);
                SetLightStateP(true, false, false, t);
                yield return new WaitForSeconds(GreenTime);

                // Giallo
                SetLightState(false, true, false, t);
                SetLightStateP(true, false, false, t);
                yield return new WaitForSeconds(YellowTime);

            } else if (t.CompareTag("Semaforo2")) {
                // Verde
                SetLightState(false, false, true, t);
                SetLightStateP(true, false, false, t);
                yield return new WaitForSeconds(GreenTime);

                // Giallo
                SetLightState(false, true, false, t);
                SetLightStateP(true, false, false, t);
                yield return new WaitForSeconds(YellowTime);

                // Rosso
                SetLightState(true, false, false, t);
                SetLightStateP(false, false, true, t);
                yield return new WaitForSeconds(GreenTime);

                SetLightState(true, false, false, t);
                SetLightStateP(false, true, false, t);
                yield return new WaitForSeconds(YellowTime);
            }
        }
    }

    IEnumerator TrafficLightCycle3()
    {
        while (true)
        {
            SetLightStateP(true, false, false, trafficLight1);
            SetLightStateP(true, false, false, trafficLight2);
            SetLightStateP(true, false, false, trafficLight3);

            SetLightState(true, false, false, trafficLight1);
            SetLightState(true, false, false, trafficLight2);
            SetLightState(false, false, true, trafficLight3);
            yield return new WaitForSeconds(GreenTime);

            SetLightState(false, true, false, trafficLight3);
            yield return new WaitForSeconds(YellowTime);



            SetLightState(true, false, false, trafficLight1);
            SetLightState(false, false, true, trafficLight2);
            SetLightState(true, false, false, trafficLight3);
            yield return new WaitForSeconds(GreenTime);

            SetLightState(false, true, false, trafficLight2);
            yield return new WaitForSeconds(YellowTime);



            SetLightState(false, false, true, trafficLight1);
            SetLightState(true, false, false, trafficLight2);
            SetLightState(true, false, false, trafficLight3);
            yield return new WaitForSeconds(GreenTime);

            SetLightState(false, true, false, trafficLight1);
            yield return new WaitForSeconds(YellowTime);



            SetLightState(true, false, false, trafficLight1);

            SetLightStateP(false, false, true, trafficLight1);
            SetLightStateP(false, false, true, trafficLight2);
            SetLightStateP(false, false, true, trafficLight3);
            yield return new WaitForSeconds(GreenTime);

            SetLightStateP(false, true, false, trafficLight1);
            SetLightStateP(false, true, false, trafficLight2);
            SetLightStateP(false, true, false, trafficLight3);
            yield return new WaitForSeconds(YellowTime);
        }
    }

    //function for car's lights
    private void SetLightState(bool red, bool yellow, bool green, GameObject s) {
        //Object ON
        s.transform.Find("RedOn").gameObject.SetActive(red);
        s.transform.Find("YellowOn").gameObject.SetActive(yellow);
        s.transform.Find("GreenOn").gameObject.SetActive(green);
        //Object OFF
        s.transform.Find("RedOff").gameObject.SetActive(!red);
        s.transform.Find("YellowOff").gameObject.SetActive(!yellow);
        s.transform.Find("GreenOff").gameObject.SetActive(!green);

        if (!green)
            s.GetComponent<BoxCollider>().enabled = true;
        else
            s.GetComponent<BoxCollider>().enabled = false;
    }

    //function for pedestrian's lights
    private void SetLightStateP(bool red, bool yellow, bool green, GameObject s) {
        //Object ON
        s.transform.Find("RedOnP").gameObject.SetActive(red);
        s.transform.Find("YellowOnP").gameObject.SetActive(yellow);
        s.transform.Find("GreenOnP").gameObject.SetActive(green);
        //Object OFF
        s.transform.Find("RedOffP").gameObject.SetActive(!red);
        s.transform.Find("YellowOffP").gameObject.SetActive(!yellow);
        s.transform.Find("GreenOffP").gameObject.SetActive(!green);
    }
}


