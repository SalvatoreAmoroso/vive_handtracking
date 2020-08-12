using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Teleport : MonoBehaviour
{
    private int state = 0;

    public float teleportFadeTime = 0.3f;
    //public ArcRenderer ArcRenderer = null;
    //public GameObject Arc = null;

    public TeleportArc_Handtracking teleportArc = null;

    public float floorFixupMaximumTraceDistance = 1.0f;
    public LayerMask floorFixupTraceLayerMask;
    public AudioSource headAudioSource;
    public AudioClip teleportSound;

    private float currentFadeTime = 0.0f;
    private bool teleporting = false;
    private Vector3 pointedAtPosition;
    private Player player = null;

    // Start is called before the first frame update
    private void Start()
    {
        player = Player.instance;
        //Arc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        Quaternion rotation = Camera.main.transform.rotation;
        rotation.x = 0;
        rotation.z = 0;
        rotation.y = rotation.y + 0.82f;
        //Arc.transform.position = player.feetPositionGuess;
        //Arc.transform.rotation = rotation;
        if (state == 1)
        {
            //Rotate Arc with player
        }
        else if (state == 2)
        {
            if (!teleporting /*&& ArcRenderer.endPosition != null*/)
            {
                state = 0;
                InitiateTeleportFade();
            }
        }

    }


    public void OnStateChanged(int state)
    {
        this.state = state;
        if (state == 1)
            teleportArc.Show();
        else
            teleportArc.Hide();
        //Arc.SetActive(state == 1); //Active arc
    }


    private void InitiateTeleportFade()
    {
        teleporting = true;

        currentFadeTime = teleportFadeTime;

        SteamVR_Fade.Start(Color.clear, 0);
        SteamVR_Fade.Start(Color.black, currentFadeTime);

        //headAudioSource.transform.SetParent(player.hmdTransform);
        //headAudioSource.transform.localPosition = Vector3.zero;
        //PlayAudioClip(headAudioSource, teleportSound);

        Invoke("TeleportPlayer", currentFadeTime);
    }

    private void PlayAudioClip(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void TeleportPlayer()
    {
        teleporting = false;

        SteamVR_Fade.Start(Color.clear, currentFadeTime);

        Vector3 teleportPosition = new Vector3();//ArcRenderer.endPosition;


        // Find the actual floor position below the navigation mesh
        if (floorFixupMaximumTraceDistance > 0.0f)
        {
            if (Physics.Raycast(teleportPosition + 0.05f * Vector3.down, Vector3.down, out RaycastHit raycastHit, floorFixupMaximumTraceDistance, floorFixupTraceLayerMask))
            {
                teleportPosition = raycastHit.point;
            }
        }

        Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
        player.trackingOriginTransform.position = teleportPosition + playerFeetOffset;

        if (player.leftHand.currentAttachedObjectInfo.HasValue)
            player.leftHand.ResetAttachedTransform(player.leftHand.currentAttachedObjectInfo.Value);
        if (player.rightHand.currentAttachedObjectInfo.HasValue)
            player.rightHand.ResetAttachedTransform(player.rightHand.currentAttachedObjectInfo.Value);

    }
}
