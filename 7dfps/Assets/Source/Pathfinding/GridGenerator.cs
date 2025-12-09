using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public BoxCollider GridCollider;
    public Vector3 Position;
    public Collider Collider;
    private int[,,] _positionMatrix;
    private BoxCollider[,,] _colliderMatrix;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update() { }

    public void CreateGrid()
    {
        int widthCells =
            Collider.bounds.size.x > 0
                ? Mathf.CeilToInt(Collider.bounds.size.x / GridCollider.bounds.size.x + 2)
                : 1;
        int heightCells =
            Collider.bounds.size.y > 0
                ? Mathf.CeilToInt(Collider.bounds.size.y / GridCollider.bounds.size.y + 2)
                : 1;
        int depthCells =
            Collider.bounds.size.z > 0
                ? Mathf.CeilToInt(Collider.bounds.size.z / GridCollider.bounds.size.z + 2)
                : 1;
        _positionMatrix = new int[widthCells, heightCells, depthCells];
        _colliderMatrix = new BoxCollider[widthCells, heightCells, depthCells];

        for (int x = 0; x < widthCells; x++)
        {
            for (int y = 0; y < heightCells; y++)
            {
                for (int z = 0; z < depthCells; z++)
                {
                    Vector3 position = new Vector3(
                        Collider.bounds.center.x
                            + ((x - widthCells / 2f) * GridCollider.bounds.size.x),
                        Collider.bounds.center.y
                            + ((y - heightCells / 2f) * GridCollider.bounds.size.y),
                        Collider.bounds.center.z
                            + ((z - depthCells / 2f) * GridCollider.bounds.size.z)
                    );
                    _colliderMatrix[x, y, z] = GameObject.Instantiate(
                        GridCollider,
                        position,
                        Quaternion.identity,
                        this.transform
                    );
                }
            }
        }
    }
}
