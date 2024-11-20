using UnityEngine;
using Photon.Pun;

public class PlayerDataSync : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        ResetCharacterData();
    }

    // M�todo para resetar os dados (caso necess�rio)
    private void ResetCharacterData()
    {
        PlayerPrefs.SetInt("Hair", 0);
        PlayerPrefs.SetInt("Hat", 0);
        PlayerPrefs.SetInt("Glasses", 0);
        PlayerPrefs.SetInt("SkinColor", 0);
        PlayerPrefs.SetInt("Eyebrows", 0);
        PlayerPrefs.SetInt("Eyes", 0);
        PlayerPrefs.SetInt("Beard", 0);
        PlayerPrefs.SetInt("Cheek", 0);
        PlayerPrefs.SetInt("Dress", 0);
    }

    // Chama a sincroniza��o de dados dependendo do papel do jogador
    public void SyncCharacterData()
    {
        if (IsCaptain())
        {
            SendCharacterData();
        }
        else if (IsSailor())
        {
            photonView.RPC("ReceiveCharacterDataRPC", RpcTarget.AllBuffered); // ou RpcTarget.Others se n�o precisar enviar para o pr�prio jogador
        }
    }


    // Envia os dados do PlayerPrefs (do Captain) para os Sailors
    private void SendCharacterData()
    {
        // Coleta os dados de PlayerPrefs
        int hair = PlayerPrefs.GetInt("Hair");
        int hat = PlayerPrefs.GetInt("Hat");
        int glasses = PlayerPrefs.GetInt("Glasses");
        int skinColor = PlayerPrefs.GetInt("SkinColor");
        int eyebrows = PlayerPrefs.GetInt("Eyebrows");
        int eyes = PlayerPrefs.GetInt("Eyes");
        int beard = PlayerPrefs.GetInt("Beard");
        int cheek = PlayerPrefs.GetInt("Cheek");
        int dress = PlayerPrefs.GetInt("Dress");

        // Envia todos os dados para os outros jogadores (Sailors)
        photonView.RPC("ReceiveCharacterDataRPC", RpcTarget.Others, hair, hat, glasses, skinColor, eyebrows, eyes, beard, cheek, dress);
    }

    // Recebe os dados do Captain e salva no PlayerPrefs do Sailor
    [PunRPC]
    private void ReceiveCharacterDataRPC(int hair, int hat, int glasses, int skinColor, int eyebrows, int eyes, int beard, int cheek, int dress)
    {
        // Salva os dados no PlayerPrefs do Sailor
        PlayerPrefs.SetInt("Hair", hair);
        PlayerPrefs.SetInt("Hat", hat);
        PlayerPrefs.SetInt("Glasses", glasses);
        PlayerPrefs.SetInt("SkinColor", skinColor);
        PlayerPrefs.SetInt("Eyebrows", eyebrows);
        PlayerPrefs.SetInt("Eyes", eyes);
        PlayerPrefs.SetInt("Beard", beard);
        PlayerPrefs.SetInt("Cheek", cheek);
        PlayerPrefs.SetInt("Dress", dress);

        // Log para depura��o (opcional)
        Debug.Log("Dados recebidos do Captain e salvos no Sailor.");
    }

    // M�todo para identificar se o jogador atual � o Host (primeiro jogador)
    private bool IsHost()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[0];
    }

    // M�todo para identificar se o jogador atual � o Captain (segundo jogador)
    private bool IsCaptain()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[1];
    }

    // M�todo para identificar se o jogador atual � o Sailor (terceiro jogador)
    private bool IsSailor()
    {
        return PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[2];
    }
}
