using System.Collections;
using UnityEngine;

public class CarGenerator : MonoBehaviour
{
    public float delay = 3.0f;                          // Tempo di attesa in secondi tra uno spawn e il prossimo
    public int counter = 0;                             // Veicoli da spawnare
    public float initialSpeed = 50f;                    // Velocità iniziale dell'auto
    public Waypoint firstWaypoint;                      // Primo waypoint da seguire
    GameObject[] carPrefabs;                            // Prefab del veicolo
    public CarCounter carCounter;
    [SerializeField] bool infinite = false;

    void Start()
    {
        carPrefabs = Resources.LoadAll<GameObject>("");
    }

    void OnEnable()
    {
        if (infinite)
            StartCoroutine(InfiniteSpawn(delay));
        else
            StartCoroutine(Spawn(delay));
    }

    IEnumerator Spawn(float time)
    {
        for (int i=0; i<counter; i++)
        {
            yield return new WaitForSeconds(time);                                                              // Aspetta il numero di secondi specificato

            int randomIndex = Random.Range(0, carPrefabs.Length);

            GameObject newCar = Instantiate(carPrefabs[randomIndex], transform.position, transform.rotation);   // Genera veicolo nella posizione e orientamento dell'oggetto a cui è applicato lo script

            CarController carController = newCar.GetComponent<CarController>();

            carController.currentWaypoint = firstWaypoint;
            carController.carCounter = carCounter;
            newCar.GetComponent<Rigidbody>().velocity = transform.forward * (initialSpeed / 3.6f);

            carCounter.Increase();
        }
    }

    IEnumerator InfiniteSpawn(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);                                                              // Aspetta il numero di secondi specificato

            int randomIndex = Random.Range(0, carPrefabs.Length);

            GameObject newCar = Instantiate(carPrefabs[randomIndex], transform.position, transform.rotation);   // Genera veicolo nella posizione e orientamento dell'oggetto a cui è applicato lo script

            CarController carController = newCar.GetComponent<CarController>();

            carController.currentWaypoint = firstWaypoint;
            carController.carCounter = carCounter;
            newCar.GetComponent<Rigidbody>().velocity = transform.forward * (initialSpeed / 3.6f);

            carCounter.Increase();
        }
    }
}
