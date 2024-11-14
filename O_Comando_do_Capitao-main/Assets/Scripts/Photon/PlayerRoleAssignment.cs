using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerRoleAssignment : MonoBehaviourPunCallbacks
{
    public TMP_Text roleText;
    private string playerRole;

    void Start()
    {
        AssignRole();
        DisplayRole();
    }

    private void AssignRole()
    {
        // Ordena os jogadores pela ordem de entrada na sala
        int playerIndex = PhotonNetwork.PlayerList.Length;

        if (playerIndex == 1)
        {
            playerRole = "Host";
        }
        else if (playerIndex == 2)
        {
            playerRole = "Capitão";
        }
        else if (playerIndex == 3)
        {
            playerRole = "Marujo";
        }
        else
        {
            playerRole = "Espectador";
        }
    }

    private void DisplayRole()
    {
        roleText.text = playerRole;
    }

    public string GetPlayerRole()
    {
        return playerRole;
    }

    // Para atualizar o papel de jogadores, caso alguém saia e a lista mude
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AssignRole();
        DisplayRole();
    }
}
