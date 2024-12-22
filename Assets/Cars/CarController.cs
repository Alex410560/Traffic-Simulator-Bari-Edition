using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Ruote e sterzo")]
    public float maxSteerAngle = 45f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public float turnSpeed = 5f;
    float targetSteerAngle = 0;

    [Header("Motore")]
    public float maxMotorTorque = 2500f;

    [Header("Freni")]
    public float maxBrakeTorque = 2400f;
    public bool isBraking = false;

    [Header("Velocità in km/h")]
    public float currentSpeed;
    public float maxSpeed;

    [Header("Sensori")]
    public float sensorLength = 2.3f;
    public Vector3 frontSensorPosition = new Vector3 (0, 0.5f, 2.4f);
    public float frontSideSensorPosition = 0.9f;
    public float frontSensorAngle = 30f;

    public bool restoreSpeed = false;

    [Header("Altro")]
    public Waypoint currentWaypoint;
    public Rigidbody rb;
    public CarCounter carCounter;

    RaycastHit hitCenter;
    RaycastHit hitRight;
    RaycastHit hitLeft;
    CarController carInFront = null;
    bool right = false;
    bool center = false;
    bool left = false;

    void Start()
    {
        maxSpeed = currentWaypoint.maxSpeed;
    }

    void FixedUpdate()
    {
        currentSpeed = rb.velocity.magnitude * 3.6f;

        Sensors();
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
        LerpToSteerAngle();
    }

    void ApplySteer()                                                                       // Da modificare se si vuole considerare l'altezza
    {
        Vector3 waypointPosition = currentWaypoint.GetPosition();
        waypointPosition.y = transform.position.y;

        Vector3 relativeVector = transform.InverseTransformPoint(waypointPosition);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
    } 

    void LerpToSteerAngle()
    {
        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    void Drive()
    {
        if (currentSpeed < maxSpeed)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
            rb.drag = 0;
            isBraking = false;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            rb.drag = 0.5f;
            isBraking = true;
        }
    }

    void Braking()
    {
        if (isBraking)
        {
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;
        }
        else
        {
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
        }
    }

    void CheckWaypointDistance()
    {
        Vector3 carPosition = transform.position;
        Vector3 waypointPosition = currentWaypoint.GetPosition();

        // Ignora la componente Y
        waypointPosition.y = carPosition.y;

        if (Vector3.Distance(carPosition, waypointPosition) < 2f) // if (Vector3.Distance(transform.position, currentWaypoint.GetPosition()) < 2f) -> considera l'altezza
        {
            bool shouldBranch = false;

            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)                             // Sceglie se seguire il branch o meno
            {
                if (Random.Range(0f, 1f) <= currentWaypoint.branchRatio)
                {
                    shouldBranch = true;
                }
            }

            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count)];        // Sceglie quale branch seguire
            }
            else
            {
                if (currentWaypoint.nextWaypoint == null)                                                           // Se i waypoint sono terminati...
                {
                    Destroy(gameObject);                                                                            // ...elimina il Game Object
                    carCounter.Decrease();
                }
                else
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;                                                 // ...altrimenti aggiorna il waypoint da seguire
                }
            }

            maxSpeed = currentWaypoint.maxSpeed;
        }
    }

    void Sensors()
    {
        Vector3 sensorStartingPosition = transform.position;
        sensorStartingPosition += transform.forward * frontSensorPosition.z;
        sensorStartingPosition += transform.up * frontSensorPosition.y;

        bool allClear = true;
        bool allClear2 = true;
        right = false;
        center = false;
        left = false;

        //center
        if (Physics.Raycast(sensorStartingPosition, transform.forward, out hitCenter, sensorLength))
        {
            if (hitCenter.collider.CompareTag("Car"))
            {
                allClear = false;
                center = true;

                GameObject hitObject = hitCenter.collider.gameObject;
                carInFront = hitObject.GetComponentInParent<CarController>();
            }

            if (hitCenter.collider.CompareTag("Stop") || hitCenter.collider.CompareTag("Semaforo1") || hitCenter.collider.CompareTag("Semaforo2"))
            {
                allClear2 = false;
                center = true;
            }

            Debug.DrawLine(sensorStartingPosition, hitCenter.point);
        }
        

        //right
        sensorStartingPosition += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartingPosition, transform.forward, out hitRight, sensorLength))
        {
            if (hitRight.collider.CompareTag("Car"))
            {
                allClear = false;
                right = true;

                GameObject hitObject = hitRight.collider.gameObject;
                carInFront = hitObject.GetComponentInParent<CarController>();
            }

            if (hitRight.collider.CompareTag("Stop") || hitRight.collider.CompareTag("Semaforo1") || hitRight.collider.CompareTag("Semaforo2"))
            {
                allClear2 = false;
                right = true;
            }

            Debug.DrawLine(sensorStartingPosition, hitRight.point);
        }

        
        /*
        //right angle
        if (Physics.Raycast(sensorStartingPosition, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Car"))
            {
                GameObject hitObject = hit.collider.gameObject;

                CarEngine c = hitObject.GetComponentInParent<CarEngine>();

                if (c.currentSpeed < currentWaypoint.maxSpeed)
                {
                    maxSpeed = c.currentSpeed;
                    allClear = false;
                    restoreSpeed = true;
                }

                Debug.DrawLine(sensorStartingPosition, hit.point);
            }
        }
        */
        

        //left
        sensorStartingPosition -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartingPosition, transform.forward, out hitLeft, sensorLength))
        {
            if (hitLeft.collider.CompareTag("Car"))
            {
                allClear = false;
                left = true;

                GameObject hitObject = hitLeft.collider.gameObject;
                carInFront = hitObject.GetComponentInParent<CarController>();
            }

            if (hitLeft.collider.CompareTag("Stop")||hitLeft.collider.CompareTag("Semaforo1")||hitLeft.collider.CompareTag("Semaforo2"))
            {
                allClear2 = false;
                left = true;
            }

            Debug.DrawLine(sensorStartingPosition, hitLeft.point);
        }

        
        /*
        //left angle
        if (Physics.Raycast(sensorStartingPosition, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Car"))
            {
                GameObject hitObject = hit.collider.gameObject;

                CarEngine c = hitObject.GetComponentInParent<CarEngine>();

                if (c.currentSpeed < currentWaypoint.maxSpeed)
                {
                    maxSpeed = c.currentSpeed;
                    allClear = false;
                    restoreSpeed = true;
                }

                Debug.DrawLine(sensorStartingPosition, hit.point);
            }
        }
        */

        if (allClear == false)
        {
            if (carInFront.currentSpeed < currentWaypoint.maxSpeed)
            {
                if (hitCenter.distance < 4 && center == true)
                {
                    maxSpeed = 0;
                }
                else if (hitRight.distance < 4 && right == true)
                {
                    maxSpeed = 0;
                }
                else if (hitLeft.distance < 4 && left == true)
                {
                    maxSpeed = 0;
                }
                else
                {
                    if (carInFront.currentSpeed < 5)
                    {
                        maxSpeed = 15;
                    }
                    else
                    {
                        maxSpeed = carInFront.currentSpeed;
                    }
                }

                restoreSpeed = true;
            }
        }

        if (allClear2 == false)
        {
            if (hitCenter.distance < 2 && center == true)
            {
                maxSpeed = 0;
            }
            else if (hitRight.distance < 2 && right == true)
            {
                maxSpeed = 0;
            }
            else if (hitLeft.distance < 2 && left == true)
            {
                maxSpeed = 0;
            }
            else
            {
                maxSpeed = 10;
            }

            restoreSpeed = true;
        }

        // Quando i sensori non rilevano più nulla
        if (allClear && allClear2 && restoreSpeed)
        {
            maxSpeed = currentWaypoint.maxSpeed;

            restoreSpeed = false;
        }
    }
}
