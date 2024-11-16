using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class AdminMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject adminMenu; // Refer�ncia ao painel do menu de administra��o
    public GameObject targetObject; // O GameObject que ser� mostrado/ocultado apenas para os demais jogadores
    public GameObject WaitingForStart;
    public TMP_Text CaptainNameText;
    public TMP_InputField playerNameInput; // Campo para inserir o nome do jogador
    public TMP_Dropdown playerGenderDropdown; // Dropdown para selecionar o sexo do jogador
    public TMP_Dropdown playersListDropdown; // Dropdown para exibir todos os jogadores adicionados manualmente
    public TMP_Text captainText; // Texto que exibe o capit�o atual
    public GameObject playButton; // Bot�o de "Play"
    public GameObject waitingForStart; // Refer�ncia ao GameObject WaitingForStart

    private bool isObjectVisible = false; // Estado inicial do objeto
    private bool isWaitingForStartVisible = false;

    private List<string> playerNames = new List<string>(); // Lista para armazenar nomes dos jogadores
    private List<string> remainingPlayers; // Lista para armazenar os jogadores que ainda n�o foram sorteados nesta rodada

    void Start()
    {
        playerGenderDropdown.value = 0; // Define "Masculino" como valor padr�o
        ShowAdminMenuForHost();
        AssignRole();

        // Verifica se as refer�ncias est�o atribu�das
        if (photonView == null) Debug.LogError("photonView n�o est� atribu�do!");
        if (adminMenu == null) Debug.LogError("adminMenu n�o est� atribu�do!");
        if (targetObject == null) Debug.LogError("targetObject n�o est� atribu�do!");
        //if (roleText == null) Debug.LogError("roleText n�o est� atribu�do!");
        if (playerNameInput == null) Debug.LogError("playerNameInput n�o est� atribu�do!");
        if (playerGenderDropdown == null) Debug.LogError("playerGenderDropdown n�o est� atribu�do!");
        if (playersListDropdown == null) Debug.LogError("playersListDropdown n�o est� atribu�do!");
        if (captainText == null) Debug.LogError("captainText n�o est� atribu�do!");
        if (playButton == null) Debug.LogError("playButton n�o est� atribu�do!");
        if (waitingForStart == null) Debug.LogError("waitingForStart n�o est� atribu�do!");

        remainingPlayers = new List<string>(); // Inicializa a lista de jogadores restantes
    }

    private void ShowAdminMenuForHost()
    {
        adminMenu.SetActive(PhotonNetwork.IsMasterClient); // Exibe o menu para o Host
    }

    private void AssignRole()
    {
     //  int playerIndex = PhotonNetwork.PlayerList.Length;
     //
     //  if (playerIndex == 1) roleText.text = "Host";
     //  else if (playerIndex == 2) roleText.text = "Capit�o";
     //  else if (playerIndex == 3) roleText.text = "Marujo";
     //  else roleText.text = "Espectador";
     //
     //  // Mostra o WaitingForStart apenas para Capit�o e Marujo
     //  if (roleText.text == "Capit�o" || roleText.text == "Marujo")
     //  {
     //      waitingForStart.SetActive(true);
     //  }
     //  else
     //  {
     //      waitingForStart.SetActive(false);
     //  }
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
        ShowAdminMenuForHost(); // Atualiza o menu caso o host mude
    }

    public void SetPlayerInfo()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string playerName = playerNameInput.text;

            if (!string.IsNullOrEmpty(playerName) && !playerNames.Contains(playerName))
            {
                playerNames.Add(playerName); // Adiciona o nome � lista
                remainingPlayers.Add(playerName); // Adiciona tamb�m aos jogadores restantes
                UpdatePlayersDropdown(); // Atualiza o dropdown
                photonView.RPC("UpdatePlayerInfo", RpcTarget.All, playerName);

                Debug.Log("Player adicionado: " + playerName);
            }
            else
            {
                Debug.LogWarning("Nome de jogador vazio ou j� adicionado.");
            }
        }
    }

    [PunRPC]
    private void UpdatePlayerInfo(string name)
    {
        //roleText.text = $"Nome: {name}";
    }

    private void UpdatePlayersDropdown()
    {
        playersListDropdown.ClearOptions();
        List<string> options = new List<string> { "Aleat�rio" }; // Primeira op��o padr�o
        options.AddRange(playerNames);
        playersListDropdown.AddOptions(options);
    }

    // Fun��o para sortear o capit�o
    public void SelectCaptain()
    {
        if (playersListDropdown.value == 0)
        {
            // Se a op��o "Aleat�rio" estiver selecionada
            SelectRandomCaptain();
        }
        else
        {
            // Caso contr�rio, define o jogador selecionado como capit�o
            string selectedCaptain = playerNames[playersListDropdown.value - 1]; // Ajuste para ignorar a op��o "Aleat�rio"
            SetCaptain(selectedCaptain);
        }
    }

    private void SelectRandomCaptain()
    {
        if (remainingPlayers.Count == 0)
        {
            // Reabastece a lista de jogadores restantes quando todos j� foram sorteados
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
        // Exibe o nome do capit�o no campo de texto e para todos os clientes
        photonView.RPC("DisplayCaptain", RpcTarget.All, captainName);
        Debug.Log("Capit�o da rodada: " + captainName);

        waitingForStart.SetActive(false);
    }

    [PunRPC]
    private void DisplayCaptain(string captainName)
    {
        captainText.text = $"Capit�o da rodada: {captainName}";
    }

    public void StartGame()
    {
        SelectRandomCaptain();
        ToggleWaitingForStartVisibility();
    }
}
