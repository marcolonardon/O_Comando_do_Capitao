using UnityEngine;
using Photon.Pun;

public class Gameplay : MonoBehaviourPunCallbacks
{
    //public GameObject CaptainCharacter; // Refer�ncia ao GameObject que deve ser ativo apenas para jogadores que n�o s�o o host

    void Start()
    {
        //CaptainCharacter.SetActive(false);

        // // Verifica se o jogador atual � o host (master client)
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     CaptainCharacter.SetActive(false); // Desativa o objeto para o host
        // }
        // else
        // {
        //     CaptainCharacter.SetActive(true); // Ativa o objeto para outros jogadores
        // }
    }
}
