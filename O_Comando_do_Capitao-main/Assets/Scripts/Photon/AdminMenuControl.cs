using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class AdminMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject adminMenu;
    public GameObject targetObject;
    public GameObject WaitingForStart;
    public TMP_Text CaptainNameText;
    public TMP_InputField playerNameInput;
    public TMP_Dropdown playerGenderDropdown;
    public TMP_Dropdown playersListDropdown;
    public TMP_Text captainText;
    public GameObject playButton;
    public GameObject waitingForStart;

    private bool isObjectVisible = false;
    private bool isWaitingForStartVisible = false;

    private List<string> playerNames = new List<string>();
    private List<string> remainingPlayers = new List<string>();
    private System.Random random;
    void Start()
    {
        random = new System.Random();
        SetUI();
        playerNameInput.onSelect.AddListener((text) =>
        {
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        });

        playerGenderDropdown.value = 0;
        ShowAdminMenuForHost();
    }

    private void SetUI()
    {
        if (IsHost())
        {
            adminMenu.SetActive(true);
        }
        else
        {
            adminMenu.SetActive(false);
        }
    }

    private void ShowAdminMenuForHost()
    {
        adminMenu.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void ToggleObjectVisibility()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isObjectVisible = !isObjectVisible;
            photonView.RPC("SetObjectVisibility", RpcTarget.Others, isObjectVisible);
        }
    }

    public void ToggleWaitingForStartVisibility()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isWaitingForStartVisible = !isWaitingForStartVisible;
            photonView.RPC("SetWaitingForStartVisibility", RpcTarget.Others, isWaitingForStartVisible);
        }
    }

    [PunRPC]
    private void SetObjectVisibility(bool visibility)
    {
        targetObject.SetActive(visibility);
    }

    [PunRPC]
    private void SetWaitingForStartVisibility(bool visibility)
    {
        WaitingForStart.SetActive(visibility);
        Debug.Log($"WaitingForStart visível: {visibility}");
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ShowAdminMenuForHost();
    }

    public void SetPlayerInfo()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string playerName = playerNameInput.text;

            if (!string.IsNullOrEmpty(playerName) && !playerNames.Contains(playerName))
            {
                playerNames.Add(playerName);
                remainingPlayers.Add(playerName);
                UpdatePlayersDropdown();
                photonView.RPC("UpdatePlayerInfo", RpcTarget.All, playerName);

                Debug.Log("Player adicionado: " + playerName);
            }
            else
            {
                Debug.LogWarning("Nome de jogador vazio ou já adicionado.");
            }
        }
    }

    [PunRPC]
    private void UpdatePlayerInfo(string name)
    {
        if (captainText.text != null)
            captainText.text = $"Nome: {name}";
    }

    private void UpdatePlayersDropdown()
    {
        playersListDropdown.ClearOptions();
        List<string> options = new List<string> { "Aleatório" };
        options.AddRange(playerNames);
        playersListDropdown.AddOptions(options);
    }

    public void SelectCaptain()
    {
        if (playersListDropdown.value == 0)
        {
            SelectRandomCaptain();
        }
        else
        {
            string selectedCaptain = playerNames[playersListDropdown.value - 1];
            SetCaptain(selectedCaptain);
        }
    }

    private void SelectRandomCaptain()
    {
        // Verifica se a lista de jogadores restantes está vazia
        if (remainingPlayers.Count == 0)
        {
            Debug.Log("Todos os jogadores foram escolhidos. Reiniciando...");

            // Recarrega a lista de jogadores para reiniciar o processo
            remainingPlayers = new List<string>(playerNames);
        }

        // Exibe todos os jogadores restantes antes da seleção
        Debug.Log("Jogadores disponíveis: " + string.Join(", ", remainingPlayers));

        // Escolhe um índice aleatório da lista
        int randomIndex = random.Next(remainingPlayers.Count);

        // Pega o nome do jogador no índice escolhido
        string chosenPlayer = remainingPlayers[randomIndex];

        // Exibe o nome do jogador escolhido
        Debug.Log("Jogador escolhido: " + chosenPlayer);

        // Remove o jogador escolhido da lista
        remainingPlayers.RemoveAt(randomIndex);

        // Verifica a lista de jogadores restantes após a remoção
        Debug.Log("Jogadores restantes após remoção: " + string.Join(", ", remainingPlayers));

        // Chama a função para definir o capitão
        SetCaptain(chosenPlayer);
    }



    private void SetCaptain(string captainName)
    {
        photonView.RPC("DisplayCaptain", RpcTarget.All, captainName);
        Debug.Log("Capitão da rodada: " + captainName);

        waitingForStart.SetActive(false);
    }

    [PunRPC]
    private void DisplayCaptain(string captainName)
    {
        if (captainText != null)
            captainText.text = $"Capitão da rodada: {captainName}";
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Garante que o jogo esteja no estado correto antes de iniciar
            ResetGameStates();

            // Seleciona o capitão para a nova rodada
            SelectRandomCaptain();

            // Garante que o estado de visibilidade é sincronizado corretamente
            photonView.RPC("SetWaitingForStartVisibility", RpcTarget.All, false);

            Debug.Log("Jogo iniciado pelo Master Client.");
        }
    }

    // Método auxiliar para resetar os estados do jogo
    private void ResetGameStates()
    {
        captainText.text = ""; // Reseta o texto do capitão
        WaitingForStart.SetActive(true); // Mostra o elemento de espera no início

    }


    [PunRPC]
    private void ResetGameForAll()
    {
        // Reseta os elementos necessários para uma nova partida
        captainText.text = "";
        WaitingForStart.SetActive(true);
        remainingPlayers.Clear();
        remainingPlayers.AddRange(playerNames);
    }

    private bool IsHost()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0];
    }
}
