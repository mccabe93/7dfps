using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rayfinder : MonoBehaviour
{
    public Transform Source;
    public Transform Destination;

    public float Speed { get; set; } = 1f;
    public bool CanFly { get; set; } = false;
    public bool CanFall { get; set; } = false;
    public Node Path { get; } = new Node(Vector3.zero, null, false, false);

    private PhysicsRaycaster _raycaster;
    private List<RaycastResult> _raycastHits = new List<RaycastResult>();
    private PointerEventData _pointerEventData = new PointerEventData(EventSystem.current);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _raycaster = gameObject.AddComponent<PhysicsRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        Source.position = Vector3.MoveTowards(
            Source.position,
            Destination.position,
            Speed * Time.deltaTime
        );
    }

    private void TryCreateVector(Transform destination)
    {
        _raycastHits.Clear();
        _pointerEventData.position = Source.position + Vector3.forward;
        _raycaster.Raycast(_pointerEventData, _raycastHits);
        if (_raycastHits.Count > 0)
        {
            /*
            Path.Clear();
            foreach (var hit in _raycastHits)
            {
                Path.Add(hit.worldPosition);
            }
            */
        }
    }

    private void CreateCollisionGrid(Transform destination) { }
}

public class Node
{
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
