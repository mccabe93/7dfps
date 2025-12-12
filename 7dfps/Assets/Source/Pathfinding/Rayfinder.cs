using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MovementResult
{
    Unknown,
    Moved,
    Rotating,
    ReachedDestination,
}

public class Rayfinder : MonoBehaviour
{
    public Transform Source;
    public Transform Destination;

    public float StopDistance = 0.1f;
    public float Speed = 1f;
    public float RotationSpeed = 10f;
    public bool CanFly = false;
    public bool CanRotateAndMove = true;
    public List<Node> Path { get; } = new List<Node>();

    private readonly RaycastHit[] _raycastHits = new RaycastHit[32];

    public MovementResult Move(bool grounded)
    {
        if (Vector3.Distance(Source.position, Destination.position) < StopDistance)
        {
            return MovementResult.ReachedDestination;
        }

        if (CanRotateAndMove)
        {
            Source.position = Vector3.MoveTowards(
                Source.position,
                Destination.position,
                Speed * Time.deltaTime
            );
            Source.rotation = Quaternion.RotateTowards(
                Source.rotation,
                Quaternion.LookRotation(Destination.position - Source.position),
                Speed * Time.deltaTime * RotationSpeed
            );
        }
        else
        {
            if (Source.rotation == Quaternion.LookRotation(Destination.position - Source.position))
            {
                Source.position = Vector3.MoveTowards(
                    Source.position,
                    Destination.position,
                    Speed * Time.deltaTime
                );
                return MovementResult.Rotating;
            }
            else
            {
                Source.rotation = Quaternion.RotateTowards(
                    Source.rotation,
                    Quaternion.LookRotation(Destination.position - Source.position),
                    RotationSpeed * Time.deltaTime
                );
            }
        }
        return MovementResult.Moved;
    }

    private void TryCreateVector(Transform destination)
    {
        Ray ray = new Ray(Source.position, Source.position - Destination.position);
        Physics.RaycastNonAlloc(ray, _raycastHits);
        Debug.DrawRay(ray.origin, ray.direction);
        var notSelfHit = GetFirstHit();
        if (notSelfHit != null)
        {
            Path.Add(
                new Node(
                    Source.position,
                    notSelfHit.Value.collider,
                    isEndpoint: true,
                    createGrid: true
                )
            );
        }
    }

    private RaycastHit? GetFirstHit()
    {
        for (int i = 0; i < _raycastHits.Length; i++)
        {
            if (_raycastHits[i].transform == null)
            {
                break;
            }
            else if (
                _raycastHits[i].transform.gameObject == this
                || _raycastHits[i].transform.gameObject.tag == "Ground"
            )
            {
                continue;
            }
            return _raycastHits[i];
        }
        return null;
    }
}

public record Node
{
    public static Node DefaultNode = new Node(Vector3.zero, null, false, false);

    public readonly Vector3 Position;
    public readonly Collider Collider;
    public readonly bool IsEndpoint = false;
    public readonly bool CreateGrid = false;

    public Node(
        Vector3 position,
        Collider collider,
        bool isEndpoint = false,
        bool createGrid = false
    )
    {
        Position = position;
        Collider = collider;
        IsEndpoint = isEndpoint;
        CreateGrid = createGrid;
    }
}
