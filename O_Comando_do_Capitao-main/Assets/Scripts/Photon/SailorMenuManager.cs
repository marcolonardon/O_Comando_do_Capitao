using UnityEngine;
using Photon.Pun;

public class SailorMenuManager : MonoBehaviourPunCallbacks
{
    public GameObject UnlockScreenButton;
    public GameObject BlockScreen;
    public GameObject CaptainCharacter;
    public GameObject CompleteFirstMissionButton;
    public GameObject CoinsAnimation;

    private void Start()
    {
        SetUI();
    }

    private void SetUI()
    {
        CoinsAnimation.SetActive(false);
        UnlockScreenButton.SetActive(false);
        CompleteFirstMissionButton.SetActive(false);

        if (IsSailor())
        {
            CaptainCharacter.SetActive(false);
            BlockScreen.SetActive(true);
        }
        else
        {  
            BlockScreen.SetActive(false);
        }
    }

    public void CompleteMission()
    {
        UnlockScreenButton.SetActive(false);
        CompleteFirstMissionButton.SetActive(false);
        CoinsAnimation.SetActive(true);
    }


    // Método chamado pelo Capitão para mostrar o botão na tela do Sailor
    public void ShowStartButton()
    {
        if (IsCaptain())
        {
            photonView.RPC("ShowStartButtonRPC", RpcTarget.Others); // Chama o RPC para mostrar o botão no Sailor
        }
    }

    // RPC para mostrar o botão na tela do Sailor
    [PunRPC]
    private void ShowStartButtonRPC()
    {
        if (IsSailor())
        {
            UnlockScreenButton.SetActive(true);
        }
    }

    public void StartMissionOne()
    {
        if (IsSailor())
        {
            //UnlockScreenButton.SetActive(false);
            BlockScreen.SetActive(false);
            CaptainCharacter.SetActive(true);
            CompleteFirstMissionButton.SetActive(true);
        }
    }

    private bool IsCaptain()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1];
    }

    // Método para identificar se o jogador atual é o Sailor (terceiro a conectar)
    private bool IsSailor()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2];
    }
}
