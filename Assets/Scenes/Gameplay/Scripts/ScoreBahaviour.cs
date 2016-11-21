using UnityEngine;
using UnityEngine.UI;

public class ScoreBahaviour : MonoBehaviour
{
    private Text textComponent;
    public int pointsToWinTheRound = 50;
    public RoundOverPanelBehaviour RoundOverPanel;

    private void Start()
    {
        textComponent = GetComponent<Text>();
        InvokeRepeating("UpdateScores", 1, 1);
    }

    private void UpdateScores()
    {
        var someoneWonTheRound = false;

        var text = "Score Board\n\n";

        foreach (var player in PhotonNetwork.playerList)
        {
            object playerName;
            player.customProperties.TryGetValue("PlayerName", out playerName);

            if (playerName == null) continue;

            var score = player.GetScore();

            text += (playerName + ":").PadRight(12, ' ') + (score + " pts").PadLeft(8, ' ') + "\n";

            if (!PhotonNetwork.isMasterClient || score < pointsToWinTheRound) continue;

            RoundOverPanel.SetWinnerName((string) playerName);
            someoneWonTheRound = true;
        }

        textComponent.text = text;

        if (!someoneWonTheRound)
            return;

        foreach (var player in PhotonNetwork.playerList)
        {
            player.SetScore(0);
        }
    }
}