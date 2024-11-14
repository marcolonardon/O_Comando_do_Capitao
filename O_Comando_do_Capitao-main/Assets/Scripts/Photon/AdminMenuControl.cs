using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class AdminMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject adminMenu; // Referência ao painel do menu de administração
    public GameObject targetObject; // O GameObject que será mostrado/ocultado apenas para os demais jogadores
    public TMP_Text roleText; // Texto que exibe o papel do jogador (Host, Capitão, Marujo, etc.)
    public TMP_InputField playerNameInput; // Campo para inserir o nome do jogador
    public TMP_Dropdown playerGenderDropdown; // Dropdown para selecionar o sexo do jogador
    public TMP_Dropdown playersListDropdown; // Dropdown para exibir todos os jogadores adicionados manualmente
    public TMP_Text captainText; // Texto que exibe o capitão atual
    public GameObject playButton; // Botão de "Play"

    private bool isObjectVisible = false; // Estado inicial do objeto

    private List<string> playerNames = new List<string>(); // Lista para armazenar nomes dos jogadores
    private List<string> remainingPlayers; // Lista para armazenar os jogadores que ainda não foram sorteados nesta rodada

    void Start()
    {
        playerGenderDropdown.value = 0; // Define "Masculino" como valor padrão
        ShowAdminMenuForHost();
        AssignRole();

        // Verifica se as referências estão atribuídas
        if (photonView == null) Debug.LogError("photonView não está atribuído!");
        if (adminMenu == null) Debug.LogError("adminMenu não está atribuído!");
        if (targetObject == null) Debug.LogError("targetObject não está atribuído!");
        if (roleText == null) Debug.LogError("roleText não está atribuído!");
        if (playerNameInput == null) Debug.LogError("playerNameInput não está atribuído!");
        if (playerGenderDropdown == null) Debug.LogError("playerGenderDropdown não está atribuído!");
        if (playersListDropdown == null) Debug.LogError("playersListDropdown não está atribuído!");
        if (captainText == null) Debug.LogError("captainText não está atribuído!");
        if (playButton == null) Debug.LogError("playButton não está atribuído!");

        remainingPlayers = new List<string>(); // Inicializa a lista de jogadores restantes
    }

    private void ShowAdminMenuForHost()
    {
        adminMenu.SetActive(PhotonNetwork.IsMasterClient); // Exibe o menu para o Host
    }

    private void AssignRole()
    {
        int playerIndex = PhotonNetwork.PlayerList.Length;

        if (playerIndex == 1) roleText.text = "Host";
        else if (playerIndex == 2) roleText.text = "Capitão";
        else if (playerIndex == 3) roleText.text = "Marujo";
        else roleText.text = "Espectador";
    }

    public void ToggleObjectVisibility()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isObjectVisible = !isObjectVisible;
            photonView.RPC("SetObjectVisibility", RpcTarget.Others, isObjectVisible);
        }
    }

    [PunRPC]
    private void SetObjectVisibility(bool visibility)
    {
        targetObject.SetActive(visibility);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        ShowAdminMenuForHost(); // Atualiza o menu caso o host mude
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
        roleText.text = $"Nome: {name}";
    }

    private void UpdatePlayersDropdown()
    {
        playersListDropdown.ClearOptions();
        playersListDropdown.AddOptions(playerNames);
    }

    // Função para sortear o capitão
    public void SelectRandomCaptain()
    {
        if (remainingPlayers.Count == 0)
        {
            // Reabastece a lista de jogadores restantes quando todos já foram sorteados
            remainingPlayers.AddRange(playerNames);
        }

        if (remainingPlayers.Count > 0)
        {
            // Escolhe um jogador aleatório da lista de jogadores restantes
            int randomIndex = Random.Range(0, remainingPlayers.Count);
            string selectedCaptain = remainingPlayers[randomIndex];

            // Remove o jogador sorteado da lista de jogadores restantes
            remainingPlayers.RemoveAt(randomIndex);

            // Exibe o nome do capitão no campo de texto e para todos os clientes
            photonView.RPC("DisplayCaptain", RpcTarget.All, selectedCaptain);

            Debug.Log("Capitão da rodada: " + selectedCaptain);
        }
    }

    [PunRPC]
    private void DisplayCaptain(string captainName)
    {
        captainText.text = $"Capitão da rodada: {captainName}";
    }
}
