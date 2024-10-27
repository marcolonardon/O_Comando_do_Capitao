using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerRoleDisplay : MonoBehaviourPunCallbacks
{
    public TMP_Text roleText;

    void Start()
    {
        UpdateRoleText();
    }

    // Atualiza o texto de acordo com o status do jogador (host ou user)
    private void UpdateRoleText()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            roleText.text = "HOST";
        }
        else
        {
            roleText.text = "USER";
        }
    }

    // Callback que é chamado quando o Master Client muda (por exemplo, se o host sair e outro jogador se tornar o novo host)
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        UpdateRoleText();
    }
}
