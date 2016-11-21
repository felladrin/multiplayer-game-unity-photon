using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public int ForceToApplyOnStart = 350;

    public int MaxDistanceFromCenter = 15;

    public int MinimumVelocity = 15;

    public int MaximumVelocity = 30;

    private AudioSource audioSource;

    private Rigidbody2D body;

    private Transform thisTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        thisTransform = transform;
    }

    private void Update()
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        if (thisTransform.position.x > MaxDistanceFromCenter
            || thisTransform.position.x < -MaxDistanceFromCenter
            || thisTransform.position.y > MaxDistanceFromCenter
            || thisTransform.position.y < -MaxDistanceFromCenter)
        {
            thisTransform.position = Vector3.zero;
        }
        else if (body.velocity.magnitude < MinimumVelocity)
        {
            ApplyRandomForce();
        }
        else if (body.velocity.magnitude > MaximumVelocity)
        {
            body.velocity /= 2;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        audioSource.Play();

        if (!PhotonNetwork.isMasterClient)
            return;

        var target = 0;

        if (other.gameObject.tag == "BorderCollider")
        {
            switch (other.gameObject.name)
            {
                case "Player1Collider":
                    target = 1;
                    break;
                case "Player2Collider":
                    target = 2;
                    break;
                case "Player3Collider":
                    target = 3;
                    break;
                case "Player4Collider":
                    target = 4;
                    break;
            }

            foreach (var player in PhotonNetwork.playerList)
            {
                object playerPosition;
                player.customProperties.TryGetValue("PlayerPosition", out playerPosition);

                if (playerPosition != null && target == (int) playerPosition)
                {
                    player.AddScore(-1);
                }
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            switch (other.gameObject.name)
            {
                case "Player1(Clone)":
                    target = 1;
                    break;
                case "Player2(Clone)":
                    target = 2;
                    break;
                case "Player3(Clone)":
                    target = 3;
                    break;
                case "Player4(Clone)":
                    target = 4;
                    break;
            }

            foreach (var player in PhotonNetwork.playerList)
            {
                object playerPosition;
                player.customProperties.TryGetValue("PlayerPosition", out playerPosition);

                if (playerPosition != null && target == (int) playerPosition)
                {
                    player.AddScore(1);
                }
            }
        }
    }

    private void ApplyRandomForce()
    {
        body.AddForce(new Vector2(ForceToApplyOnStart * RandomMultiplier(), ForceToApplyOnStart * RandomMultiplier()));
    }

    private int RandomMultiplier()
    {
        if (Random.value > 0.5)
        {
            return -1;
        }

        return 1;
    }
}