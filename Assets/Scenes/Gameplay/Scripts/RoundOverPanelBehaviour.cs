using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundOverPanelBehaviour : MonoBehaviour
{
    public GameObject hidablePanel;
    public GameObject winnerText;

    public void SetWinnerName(string winnerName)
    {
        PhotonView.Get(this).RPC("DisplayEndRoundMessageToAllPlayers", PhotonTargets.All, winnerName);
    }

    [PunRPC]
    public void DisplayEndRoundMessageToAllPlayers(string winnerName)
    {
        winnerText.GetComponent<Text>().text = winnerName + " wins the round!";
        hidablePanel.SetActive(true);
    }

    public void Dismiss()
    {
        hidablePanel.SetActive(false);
    }

    public void JoinAnotherRoom()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}