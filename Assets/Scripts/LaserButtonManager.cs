using UnityEngine;

public class LaserButtonManager : MonoBehaviour
{
    public float triggerOffset = 0f;

    private static Color activatedColor = new Color(0, 0.3f, 0, 1);
    private static Color deactivatedColor = new Color(0.3f, 0, 0, 1);
    private bool btnActivated = true;
    private Renderer buttonRenderer;

    private void Start()
    {
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
            buttonRenderer.material.SetColor("_EmissionColor", activatedColor);
        }
        else
        {
            buttonRenderer.material.SetColor("_EmissionColor", deactivatedColor);
        }
    }
}
