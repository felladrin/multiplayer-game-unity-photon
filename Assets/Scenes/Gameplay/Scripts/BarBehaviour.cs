using UnityEngine;

public class BarBehaviour : MonoBehaviour
{
    public bool isVertical;
    public bool isInputEnabled;
    private AudioSource audioSource;

    private void Start()
    {
        PhotonView.Get(this).RPC("UpdatePlayerNames", PhotonTargets.Others);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isInputEnabled)
        {
            FollowMouse();
        }
    }

    private void FollowMouse()
    {
        var mousePos = Input.mousePosition;

        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
            return;

        var wantedPos = Camera.main.ScreenToWorldPoint(mousePos);

        if (isVertical)
        {
            wantedPos.x = transform.position.x;
        }
        else
        {
            wantedPos.y = transform.position.y;
        }

        wantedPos.z = transform.position.z;
        transform.position = wantedPos;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isInputEnabled && other.gameObject.tag == "Ball")
        {
            audioSource.Play();
        }
    }

    [PunRPC]
    private void UpdatePlayerNames()
    {
        foreach (var player in PhotonNetwork.playerList)
        {
            object playerName, playerPosition;
            player.customProperties.TryGetValue("PlayerName", out playerName);
            player.customProperties.TryGetValue("PlayerPosition", out playerPosition);

            if (playerName == null) continue;

            var go = GameObject.Find("Player" + playerPosition + "(Clone)");
            if (go != null)
            {
                go.GetComponentInChildren<TextMesh>().text = (string) playerName;
            }
        }
    }
}