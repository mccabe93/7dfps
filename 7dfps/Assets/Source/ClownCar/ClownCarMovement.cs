using UnityEngine;
using UnityEngine.AI;

public class ClownCarMovement : MonoBehaviour
{
    public GameObject[] Waypoints;
    public NavMeshAgent Agent;

    private int _currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Agent.SetDestination(Waypoints[_currentWaypointIndex].transform.position);
    }

    // Update is called once per frame
    void Update() { }
}
