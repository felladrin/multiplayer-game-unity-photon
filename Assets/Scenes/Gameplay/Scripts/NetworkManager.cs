using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{
    private string version = "v0.0.1";

    public string playerPrefabName;
    public string ballPrefabName;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(version);
        PhotonNetwork.logLevel = PhotonLogLevel.ErrorsOnly;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Joined Random Room");
    }

    private void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4}, TypedLobby.Default);
        Debug.Log("Room Created");
    }

    private int FindEmptySlot()
    {
        var ocupiedPositions = new List<int>();

        foreach (var player in PhotonNetwork.playerList)
        {
            object playerPosition;
            player.customProperties.TryGetValue("PlayerPosition", out playerPosition);

            if (playerPosition != null)
                ocupiedPositions.Add((int) playerPosition);
        }

        for (var i = 1; i < 5; i++)
        {
            if (!ocupiedPositions.Contains(i))
            {
                return i;
            }
        }

        return 1;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.InstantiateSceneObject(ballPrefabName, Vector3.one, Quaternion.identity, 0, null);
        }

        var emptySlot = FindEmptySlot();
        var position = new Vector2(0, -4.5f);
        switch (emptySlot)
        {
            case 1:
                position = new Vector2(0, -4.5f);
                break;
            case 2:
                position = new Vector2(0, 4.5f);
                break;
            case 3:
                position = new Vector2(4.5f, 0);
                break;
            case 4:
                position = new Vector2(-4.5f, 0);
                break;
        }

        var playerName = GlobalStorage.Load<string>("PlayerName");

        var bar = PhotonNetwork.Instantiate(playerPrefabName + emptySlot, position, Quaternion.identity, 0);
        bar.GetComponent<BarBehaviour>().isInputEnabled = true;
        bar.GetComponent<SpriteRenderer>().color = Color.green;
        bar.GetComponentInChildren<TextMesh>().text = playerName;

        var hashtable = new Hashtable
        {
            {"PlayerName", playerName},
            {"PlayerPosition", emptySlot}
        };
        PhotonNetwork.player.SetCustomProperties(hashtable);
    }
}