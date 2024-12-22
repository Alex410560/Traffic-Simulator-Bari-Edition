using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        EditorGUILayout.BeginVertical("box");
        DrawButtons();
        EditorGUILayout.EndVertical();


        obj.ApplyModifiedProperties();
    }

    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if (Selection.activeGameObject != null)      // Se è stato selezionato un Game Object e il Game Object ha lo script "Waypoint"...
        {
            if (Selection.activeGameObject.GetComponent<Waypoint>() && Selection.activeGameObject.name.StartsWith("Waypoint"))
            {
                if (GUILayout.Button("Add Branch Waypoint"))                                                    // ...fai comparire questi bottoni
                {
                    CreateBranch();
                }

                if (GUILayout.Button("Create Waypoint Before"))
                {
                    CreateWaypointBefore();
                }

                if (GUILayout.Button("Create Waypoint After"))
                {
                    CreateWaypointAfter();
                }

                if (GUILayout.Button("Remove Waypoint"))
                {
                    RemoveWaypoint();
                }
            }
        }
    }

    private void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)                                 // Se è stato selezionato un Game Object...
        {
            if (Selection.activeGameObject.transform.parent != null)            // Se il Game Object ha un genitore...
            {
                waypointRoot = Selection.activeGameObject.transform.parent;     // ...allora il Waypoint Root è il genitore del Game Object
            }
            else
            {
                waypointRoot = Selection.activeGameObject.transform;            // ...altrimenti il Waypoint Root è il Game Object stesso
            }
        }
    }

    void CreateWaypoint()
    {
        int counter = 0;

        for (int i = 0; i < waypointRoot.childCount; i++)                                        // Rinomina i waypoint già esistenti (se necessario)
        {
            if (waypointRoot.GetChild(i).gameObject.name.StartsWith("Waypoint"))
            {
                counter++;
            }
        }

        GameObject waypointObject = new GameObject("Waypoint " + counter, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);
        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        if (waypointRoot.childCount > 1)
        {
            waypointRoot.GetChild(counter - 1).GetComponent<Waypoint>().nextWaypoint = waypoint;
            
            waypoint.transform.position = waypointRoot.GetChild(counter - 1).GetComponent<Waypoint>().transform.position;
        }

        waypoint.transform.SetSiblingIndex(counter);

        Selection.activeGameObject = waypoint.gameObject;
    }

    void CreateWaypointBefore()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();                                                    // Riferimento allo script Waypoint del waypoint selezionato

        for (int i = selectedWaypoint.transform.GetSiblingIndex(); i < waypointRoot.childCount; i++)                                        // Rinomina i waypoint già esistenti (se necessario)
        {
            if (waypointRoot.GetChild(i).gameObject.name.StartsWith("Waypoint"))
            {
                waypointRoot.GetChild(i).gameObject.name = "Waypoint " + (i + 1);
            }
        }

        GameObject waypointObject = new GameObject ("Waypoint " + selectedWaypoint.transform.GetSiblingIndex(), typeof (Waypoint));         // Crea un waypoint di nome "Waypoint + indice" e aggiunge lo script Waypoint
        waypointObject.transform.SetParent(waypointRoot, false);                                                                            // Trasforma il nuovo waypoint nel figlio di Waypoint Root
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();                                                                     // Riferimento allo script Waypoint del nuovo waypoint
        waypointObject.transform.position = selectedWaypoint.transform.position;                                                            // Posiziona il nuovo waypoint nella stessa posizione del waypoint selezionato

        GameObject h = GameObject.Find(waypointRoot.name + "/" + "Waypoint " + (selectedWaypoint.transform.GetSiblingIndex() - 1));         // Riferimento al waypoint precedente al waypoint selezionato

        if (h != null)                                                                                                                      // Se esiste un waypoint precedente a quello selezionato...
        {
            Waypoint c = h.GetComponent<Waypoint>();
            c.nextWaypoint = newWaypoint;                                                                                                   // ...il prossimo waypoint del waypoint precedente è il nuovo waypoint
        }

        newWaypoint.nextWaypoint = selectedWaypoint;                                                                                        // Il prossimo waypoint del nuovo waypoint è il waypoint selezionato

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());                                                // Posiziona il nuovo oggetto nella giusta posizione dell'elenco

        Selection.activeGameObject = newWaypoint.gameObject;                                                                                // Seleziona il nuovo waypoint
    }

    void CreateWaypointAfter()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();                                                    // Riferimento allo script Waypoint del waypoint selezionato

        for (int i = selectedWaypoint.transform.GetSiblingIndex()+1; i < waypointRoot.childCount; i++)                                      // Rinomina i waypoint già esistenti (se necessario)
        {
            if (waypointRoot.GetChild(i).gameObject.name.StartsWith("Waypoint"))
            {
                waypointRoot.GetChild(i).gameObject.name = "Waypoint " + (i + 1);
            }
        }

        GameObject waypointObject = new GameObject("Waypoint " + (selectedWaypoint.transform.GetSiblingIndex() + 1), typeof(Waypoint));     // Crea un waypoint di nome "Waypoint + indice" e aggiunge lo script Waypoint
        waypointObject.transform.SetParent(waypointRoot, false);                                                                            // Trasforma il nuovo waypoint nel figlio di Waypoint Root
        Waypoint newWaypoint = waypointObject.GetComponent<Waypoint>();                                                                     // Riferimento allo script Waypoint del nuovo waypoint
        waypointObject.transform.position = selectedWaypoint.transform.position;                                                            // Posiziona il nuovo waypoint nella stessa posizione del waypoint selezionato

        if (selectedWaypoint.nextWaypoint != null)                                                                                          // Se dopo il nuovo waypoint ci sono altri waypoint...
        {
            newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;                                                                       // ...allora il prossimo waypoint del nuovo waypoint è il prossimo Waypoint del Waypoint selezionato 
        }

        selectedWaypoint.nextWaypoint = newWaypoint;                                                                                        // Il prossimo waypoint del waypoint selezionato è il nuovo Waypoint

        newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex()+1);                                              // Posiziona il nuovo waypoint nella giusta posizione dell'elenco                                              

        Selection.activeGameObject = newWaypoint.gameObject;                                                                                // Selezione il nuovo waypoint
    }

    void RemoveWaypoint()
    {
        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();                                                    // Riferimento al waypoint selezionato

        for (int i = selectedWaypoint.transform.GetSiblingIndex() + 1; i < waypointRoot.childCount; i++)                                    // Rinomina i waypoint già esistenti (se necessario)
        {
            if (waypointRoot.GetChild(i).gameObject.name.StartsWith("Waypoint"))
            {
                waypointRoot.GetChild(i).gameObject.name = "Waypoint " + (i - 1);
            }
        }

        GameObject h = GameObject.Find(waypointRoot.name + "/" + "Waypoint " + (selectedWaypoint.transform.GetSiblingIndex() - 1));         // Riferimento al waypoint precedente al waypoint selezionato

        if (h != null)                                                                                                                      // Se esiste un waypoint precedente a quello selezionato...
        {
            Waypoint c = h.GetComponent<Waypoint>();
            c.nextWaypoint = selectedWaypoint.nextWaypoint;                                                                                 // ...il prossimo waypoint del waypoint precedente è il nuovo waypoint
        }

        DestroyImmediate(selectedWaypoint.gameObject);                                                                                      // Rimuovi il waypoint selezionato
    }

    void CreateBranch()
    {
        Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();

        int counter = 0;
        
        for (int i = branchedFrom.transform.GetSiblingIndex() + 1; i < waypointRoot.childCount; i++)                                    // Rinomina i waypoint già esistenti (se necessario)
        {
            if (waypointRoot.GetChild(i).gameObject.name.StartsWith("Branch"))
            {
                counter++;
            }
        }

        GameObject waypointObject = new GameObject("Branch " + counter, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        
        branchedFrom.branches.Add(waypoint);

        waypoint.transform.position = branchedFrom.transform.position;

        Selection.activeGameObject = waypoint.gameObject;
    }
}
