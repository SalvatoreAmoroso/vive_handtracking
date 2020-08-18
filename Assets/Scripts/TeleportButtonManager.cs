using UnityEngine;

public class TeleportButtonManager : MonoBehaviour
{

    public float triggerOffset = 0f;

    private static Color activatedColor = new Color(0, 0.3f, 0, 1);
    private static Color deactivatedColor = new Color(0.3f, 0, 0, 1);
    private bool btnActivated = true;
    private Teleport_Handtracking teleport;
    private Renderer buttonRenderer;

    private void Start()
    {
        teleport = FindObjectOfType(typeof(Teleport_Handtracking)) as Teleport_Handtracking;
        if (teleport == null)
        {
            Debug.LogError("Teleport: No Teleport_Handtracking instance found in map.", this);
            Destroy(gameObject);
            return;
        }
        buttonRenderer = GetComponent<Renderer>();
        buttonRenderer.material.EnableKeyword("_EMISSION");

        UpdateButtonState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ViveHands")) return;
        btnActivated = !btnActivated;
        UpdateButtonState();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("ViveHands")) return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("ViveHands")) return;
    }

    private void UpdateButtonState()
    {
        if (btnActivated)
        {
            teleport.teleportEnabled = true;
            buttonRenderer.material.SetColor("_EmissionColor", activatedColor);
        }
        else
        {
            teleport.teleportEnabled = false;
            buttonRenderer.material.SetColor("_EmissionColor", deactivatedColor);
        }
    }
}