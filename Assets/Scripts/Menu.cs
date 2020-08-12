using UnityEngine;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void OnStateChange(int state)
    {
        Debug.Log($"State changed to {state}");
    }
}
