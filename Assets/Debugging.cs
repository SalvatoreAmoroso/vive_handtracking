using UnityEngine;

public class Debugging : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        transform.position = new Vector3(x, y + 5, z);
    }
}
