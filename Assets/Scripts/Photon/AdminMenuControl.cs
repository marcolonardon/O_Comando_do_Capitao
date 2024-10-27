using UnityEngine;
using Photon.Pun;
using TMPro;

public class AdminMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject adminMenu; // Referência ao painel do menu de administração
    public GameObject targetObject; // O GameObject que será mostrado/ocultado apenas para os demais jogadores
    public TMP_Text roleText; // Texto que exibe o papel do jogador (Host, Capitão, Marujo, etc.)

    private bool isObjectVisible = false; // Estado inicial do objeto

    void Start()
    {
        ShowAdminMenuForHost();
        AssignRole();

        if (photonView == null)
        {
            Debug.LogError("photonView não está atribuído!");
        }

        // Verifica se as referências estão atribuídas
        if (adminMenu == null)
        {
            Debug.LogError("adminMenu não está atribuído!");
        }
        if (targetObject == null)
        {
            Debug.LogError("targetObject não está atribuído!");
        }
        if (roleText == null)
        {
            Debug.LogError("roleText não está atribuído!");
        }
    }

    private void ShowAdminMenuForHost()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            adminMenu.SetActive(true); // Exibe o menu para o Host
        }
        else
        {
            adminMenu.SetActive(false); // Oculta o menu para outros jogadores
        }
    }

    private void AssignRole()
    {
        int playerIndex = PhotonNetwork.PlayerList.Length;

        if (playerIndex == 1)
        {
            roleText.text = "Host";
        }
        else if (playerIndex == 2)
        {
            roleText.text = "Capitão";
        }
        else if (playerIndex == 3)
        {
            roleText.text = "Marujo";
        }
        else
        {
            roleText.text = "Espectador";
        }
    }

    public void ToggleObjectVisibility()
    {
        // Verifica se o jogador é o host
        if (PhotonNetwork.IsMasterClient)
        {
            // Alterna o estado de visibilidade e chama o RPC para os outros jogadores
            isObjectVisible = !isObjectVisible;

            // Define a visibilidade para todos os clientes exceto o host
            photonView.RPC("SetObjectVisibility", RpcTarget.Others, isObjectVisible);
        }
    }

    [PunRPC]
    private void SetObjectVisibility(bool visibility)
    {
        // Configura o objeto como ativo/inativo de acordo com o valor recebido, mas ignora no host
        targetObject.SetActive(visibility);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        ShowAdminMenuForHost(); // Atualiza o menu caso o host mude
    }
}
