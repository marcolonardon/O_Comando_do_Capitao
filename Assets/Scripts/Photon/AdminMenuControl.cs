using UnityEngine;
using Photon.Pun;
using TMPro;

public class AdminMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject adminMenu; // Refer�ncia ao painel do menu de administra��o
    public GameObject targetObject; // O GameObject que ser� mostrado/ocultado apenas para os demais jogadores
    public TMP_Text roleText; // Texto que exibe o papel do jogador (Host, Capit�o, Marujo, etc.)

    private bool isObjectVisible = false; // Estado inicial do objeto

    void Start()
    {
        ShowAdminMenuForHost();
        AssignRole();

        if (photonView == null)
        {
            Debug.LogError("photonView n�o est� atribu�do!");
        }

        // Verifica se as refer�ncias est�o atribu�das
        if (adminMenu == null)
        {
            Debug.LogError("adminMenu n�o est� atribu�do!");
        }
        if (targetObject == null)
        {
            Debug.LogError("targetObject n�o est� atribu�do!");
        }
        if (roleText == null)
        {
            Debug.LogError("roleText n�o est� atribu�do!");
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
            roleText.text = "Capit�o";
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
        // Verifica se o jogador � o host
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
