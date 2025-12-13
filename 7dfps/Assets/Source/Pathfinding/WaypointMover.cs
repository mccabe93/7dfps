using System.Linq;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public bool AutoFill = true;
    public bool AutoFillRandomize = true;
    public string AutoFillTag = "AI_Waypoint";
    public Transform[] Path;
    public Rayfinder Rayfinder;

    private int _currentWaypointIndex = 0;

    // Helper transform used as a proxy destination (ignores Y and rotation of waypoints)
    private Transform _targetProxy;

    void Awake()
    {
        if (AutoFill)
        {
            Transform[] transforms = GameObject
                .FindGameObjectsWithTag(AutoFillTag)
                .Select(t => t.transform)
                .ToArray();
            System.Random rng = new System.Random();
            rng.Shuffle(transforms);
            Path = transforms;
        }
        // Create a proxy transform to hold the adjusted destination
        var proxyGO = new GameObject($"{gameObject.name}_WaypointProxy");
        _targetProxy = proxyGO.transform;
    }

    void Start()
    {
        Transform closestWaypoint = GetClosestWaypoint();
        if (closestWaypoint != null)
        {
            // Snap position but keep current Y
            transform.position = new Vector3(
                closestWaypoint.position.x,
                transform.position.y,
                closestWaypoint.position.z
            );
        }

        Rayfinder.Source = transform;
        _currentWaypointIndex = System.Array.IndexOf(Path, closestWaypoint);
        SetProxyDestination(closestWaypoint);
    }

    public void Move()
    {
        if (Rayfinder.Move(true) == MovementResult.ReachedDestination)
        {
            _currentWaypointIndex++;
            if (_currentWaypointIndex > Path.Length - 1)
            {
                _currentWaypointIndex = 0; // Loop back to start
            }

            Transform nextWaypoint = Path[_currentWaypointIndex];
            if (nextWaypoint != null)
            {
                SetProxyDestination(nextWaypoint);
            }
        }
    }

    private void SetProxyDestination(Transform waypoint)
    {
        if (waypoint == null || _targetProxy == null)
            return;

        // Use waypoint's X and Z, but keep current object's Y (ignore waypoint height)
        _targetProxy.position = new Vector3(
            waypoint.position.x,
            transform.position.y,
            waypoint.position.z
        );

        // Keep proxy rotation identity (Rayfinder calculates look direction from position delta)
        _targetProxy.rotation = Quaternion.identity;

        Rayfinder.Destination = _targetProxy;
    }

    private Transform GetClosestWaypoint()
    {
        Transform closest = null;
        float closestDistance = float.MaxValue;
        foreach (var waypoint in Path)
        {
            // Compare distance on XZ plane only
            float distance = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(waypoint.position.x, waypoint.position.z)
            );
            if (distance < closestDistance)
            {
                closest = waypoint;
                closestDistance = distance;
            }
        }
        return closest;
    }

    void OnDestroy()
    {
        if (_targetProxy != null)
        {
            Destroy(_targetProxy.gameObject);
        }
    }
}
