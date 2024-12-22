using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PedestrianMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isCrossing = false;
    public float stopTime = 2f;

    [Header("Waypoints Configuration")]
    public GameObject waypointContainer;
    private List<Transform> waypoints = new List<Transform>();
    private Transform currentWaypoint;

    private GameObject redLight;
    private GameObject yellowLight;
    private GameObject greenLight;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

       
        if (waypointContainer != null)
        {
            foreach (Transform waypoint in waypointContainer.transform)
            {
                waypoints.Add(waypoint);
            }
        }
        else
        {
            Debug.LogError("Waypoints List not found");
        }

       
        if (waypoints.Count > 0)
        {
            SelectRandomWaypoint();
        }
    }

    void Update()
    {
        bool isWalking = agent.velocity.magnitude > 0.1f && !isCrossing;
        animator.SetBool("IsWalking", isWalking);

        if (!isCrossing && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SelectRandomWaypoint();
        }
    }

    void SelectRandomWaypoint()
    {
        int randomIndex = Random.Range(0, waypoints.Count);
        currentWaypoint = waypoints[randomIndex];
        agent.SetDestination(currentWaypoint.position);
    }

    void OnTriggerEnter(Collider other)
    {
        // controlla se il pedone entra in un'area SENZA semaforo
        if (other.CompareTag("Crosswalk") && !isCrossing)
        {
            StartCoroutine(CrosswalkBehavior());
        }

        // controlla se il pedone entra in un'area CON semaforo
        if (other.CompareTag("Semaforo1") || other.CompareTag("Semaforo2") && !isCrossing)
        {
            redLight = other.transform.Find("RedOnP").gameObject;
            yellowLight = other.transform.Find("YellowOnP").gameObject;
            greenLight = other.transform.Find("GreenOnP").gameObject;

            HandleTrafficLight();
        }
    }

    void HandleTrafficLight()
    {
        // controlla quale luce è accesa
        if (greenLight.activeSelf)
        {
            StartCoroutine(Go());
        }
        else if (yellowLight.activeSelf)
        {
            StartCoroutine(StopAndWait());
        }
        else if (redLight.activeSelf)
        {
            StartCoroutine(StopAndWait());
        }
    }

    IEnumerator StopAndWait()
    {
        // quando il semaforo è rosso o giallo, il pedone si ferma
        isCrossing = true;

        agent.isStopped = true;
        animator.SetBool("IsWalking", false);

        // il pedone aspetta solo se il semaforo è rosso o giallo
        while (redLight.activeSelf || yellowLight.activeSelf)
        {
            yield return null;
        }

        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        isCrossing = true;

        agent.isStopped = false;
        animator.SetBool("IsWalking", true);

        yield return new WaitForSeconds(0);

        agent.isStopped = false;
        animator.SetBool("IsWalking", true);

        isCrossing = false;
    }

    IEnumerator CrosswalkBehavior()
    {
        isCrossing = true;

        agent.isStopped = true;    // si ferma prima di attraversare

        animator.SetBool("IsWalking", false);  // animazione (fermo)

        yield return new WaitForSeconds(stopTime);   // aspetta

        agent.isStopped = false; // riprendi il movimento

        animator.SetBool("IsWalking", true);  // animazione (camminata)

        isCrossing = false;
    }
}

