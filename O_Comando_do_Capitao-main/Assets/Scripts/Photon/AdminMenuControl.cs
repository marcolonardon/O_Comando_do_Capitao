using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class AdminMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject adminMenu; // Referência ao painel do menu de administração
    public GameObject targetObject; // O GameObject que será mostrado/ocultado apenas para os demais jogadores
    public GameObject WaitingForStart;
    public TMP_Text CaptainNameText;
    public TMP_InputField playerNameInput; // Campo para inserir o nome do jogador
    public TMP_Dropdown playerGenderDropdown; // Dropdown para selecionar o sexo do jogador
    public TMP_Dropdown playersListDropdown; // Dropdown para exibir todos os jogadores adicionados manualmente
    public TMP_Text captainText; // Texto que exibe o capitão atual
    public GameObject playButton; // Botão de "Play"
    public GameObject waitingForStart; // Referência ao GameObject WaitingForStart

    private bool isObjectVisible = false; // Estado inicial do objeto
    private bool isWaitingForStartVisible = false;

    private List<string> playerNames = new List<string>(); // Lista para armazenar nomes dos jogadores
    private List<string> remainingPlayers; // Lista para armazenar os jogadores que ainda não foram sorteados nesta rodada

    void Start()
    {
        SetUI();

        playerNameInput.onSelect.AddListener((text) =>
        {
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        });


        playerGenderDropdown.value = 0; // Define "Masculino" como valor padrão
        ShowAdminMenuForHost();

        remainingPlayers = new List<string>(); // Inicializa a lista de jogadores restantes
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
        adminMenu.SetActive(PhotonNetwork.IsMasterClient); // Exibe o menu para o Host
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
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //ShowAdminMenuForHost(); // Atualiza o menu caso o host mude
    }

    public void SetPlayerInfo()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string playerName = playerNameInput.text;

            if (!string.IsNullOrEmpty(playerName) && !playerNames.Contains(playerName))
            {
                playerNames.Add(playerName); // Adiciona o nome à lista
                remainingPlayers.Add(playerName); // Adiciona também aos jogadores restantes
                UpdatePlayersDropdown(); // Atualiza o dropdown
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
        if(captainText.text != null) 
            captainText.text = $"Nome: {name}";
    }

    private void UpdatePlayersDropdown()
    {
        playersListDropdown.ClearOptions();
        List<string> options = new List<string> { "Aleatório" }; // Primeira opção padrão
        options.AddRange(playerNames);
        playersListDropdown.AddOptions(options);
    }

    // Função para sortear o capitão
    public void SelectCaptain()
    {
        if (playersListDropdown.value == 0)
        {
            // Se a opção "Aleatório" estiver selecionada
            SelectRandomCaptain();
        }
        else
        {
            // Caso contrário, define o jogador selecionado como capitão
            string selectedCaptain = playerNames[playersListDropdown.value - 1]; // Ajuste para ignorar a opção "Aleatório"
            SetCaptain(selectedCaptain);
        }
    }

    private void SelectRandomCaptain()
    {
        if (remainingPlayers.Count == 0)
        {
            // Reabastece a lista de jogadores restantes quando todos já foram sorteados
            remainingPlayers.AddRange(playerNames);
        }

        if (remainingPlayers.Count > 0)
        {
            int randomIndex = Random.Range(0, remainingPlayers.Count);
            string selectedCaptain = remainingPlayers[randomIndex];

            remainingPlayers.RemoveAt(randomIndex); // Remove o jogador sorteado da lista de jogadores restantes
            SetCaptain(selectedCaptain);
        }
    }

    private void SetCaptain(string captainName)
    {
        // Exibe o nome do capitão no campo de texto e para todos os clientes
        photonView.RPC("DisplayCaptain", RpcTarget.All, captainName);
        Debug.Log("Capitão da rodada: " + captainName);

        waitingForStart.SetActive(false);
    }

    [PunRPC]
    private void DisplayCaptain(string captainName)
    {
        if(captainText != null)
            captainText.text = $"Capitão da rodada: {captainName}";
    }

    public void StartGame()
    {
        SelectRandomCaptain();
        ToggleWaitingForStartVisibility();
    }

    private bool IsHost()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0];
    }
}
