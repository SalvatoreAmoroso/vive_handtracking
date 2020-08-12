using UnityEngine;
using Valve.VR.InteractionSystem;

public class ArcRenderer : MonoBehaviour
{
    public LineRenderer lr;

    public float velocity = 15;
    public float angle = 35;
    public int resolution = 25;

    public Vector3 endPosition;

    private float gravity;
    private float radianAngle;
    private GameObject cylinder = null;
    private Player player = null;

    private void Awake()
    {
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    private void OnValidate()
    {
        if (lr != null && Application.isPlaying)
        {
            RenderArc();
        }
    }

    private void Start()
    {
        player = Player.instance;
        RenderArc();
    }

    public void RenderArc()
    {
        lr.useWorldSpace = false;
        lr.startWidth = 0.3f;
        lr.endWidth = 0.3f;
        lr.positionCount = resolution + 1;
        Vector3[] vectors = CalcArcArray();
        lr.SetPositions(vectors);

        Vector3 lastVector = vectors[vectors.Length - 1];
        endPosition = lastVector;

        //if (cylinder == null)
        //{
        //    cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //    cylinder.transform.localScale = new Vector3(1, 1, 1);
        //}
        //cylinder.transform.position = GetTerrainPos(endPosition);

    }

    private Vector3[] CalcArcArray()
    {
        Vector3[] arcArr = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            arcArr[i] = CalcArcPoint(t, maxDistance);
        }

        return arcArr;
    }

    private Vector3 CalcArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - (gravity * x * x / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        //float z = player.feetPositionGuess.z;
        float z = 0;
        //x += player.feetPositionGuess.x;
        //y += player.feetPositionGuess.y;

        return new Vector3(x, y, z);
    }

    private static Vector3 GetTerrainPos(Vector3 startVector)
    {
        Vector3 origin = new Vector3(startVector.x, 100, startVector.z);
        Physics.Raycast(origin, Vector3.down, out RaycastHit hit, Mathf.Infinity);
        return hit.point;
    }
}