using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public InputField input_Create;
    public InputField Input_Join;
    public GameObject errorPopup;
    public TMP_Text errorText;

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(input_Create.text))
        {
            ShowErrorPopup("Por favor, insira o nome da sala para criá-la.");
            return;
        }

        PhotonNetwork.CreateRoom(input_Create.text, new RoomOptions() { MaxPlayers = 3, IsVisible = true, IsOpen = true }, TypedLobby.Default, null);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(Input_Join.text))
        {
            ShowErrorPopup("Por favor, insira o nome da sala para entrar.");
            return;
        }

        PhotonNetwork.JoinRoom(Input_Join.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        ShowErrorPopup($"Falha ao criar a sala: {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ShowErrorPopup("Sala não encontrada. Verifique o nome e tente novamente.");
    }

    private void ShowErrorPopup(string message)
    {
        errorText.text = message;
        errorPopup.SetActive(true);
    }

    public void CloseErrorPopup()
    {
        errorPopup.SetActive(false);
    }
}
