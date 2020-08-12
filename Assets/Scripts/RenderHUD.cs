using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ViveHandTracking;

public class RenderHUD : MonoBehaviour
{
    public Text text;
    private StringBuilder textBuilder = new StringBuilder();

    // Update is called once per frame
    private void Update()
    {
        if (GestureProvider.Status == GestureStatus.Running)
        {
            textBuilder.Clear();
            if (GestureProvider.LeftHand != null)
            {
                textBuilder.Append("Left Hand: ");
                textBuilder.Append(GestureProvider.LeftHand.gesture.ToString("g"));
            }
            if (GestureProvider.RightHand != null)
            {
                textBuilder.Append("\nRight Hand: ");
                textBuilder.Append(GestureProvider.RightHand.gesture.ToString("g"));
            }
            text.text = textBuilder.ToString();
        }
    }
}
