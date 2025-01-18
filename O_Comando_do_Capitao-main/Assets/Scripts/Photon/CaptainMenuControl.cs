using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CaptainMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject Background;
    public GameObject ShowCaptain;
    public GameObject CaptainCharacter;
    public GameObject customizationMenu; // Refer�ncia ao GameObject do menu de personaliza��o
    public GameObject CaptainsCommands;
    public GameObject roleText;
    public GameObject PlayButton;
    public Button toggleCustomizationButton; // Refer�ncia ao bot�o para ativar/desativar o menu de personaliza��o
    public Button printCustomizationButton; // Refer�ncia ao bot�o para imprimir op��es de personaliza��o
    public GameObject InitialButton;
    private bool isCustomizationMenuActive = false; // Estado inicial do menu

    public GameObject sailor; // GameObject do Sailor

    // Vari�vel para armazenar o ActorNumber do capit�o
    private int captainActorNumber;

    void Start()
    {
        SetCaptain();
        SetUI();
    }

   //private void Update()
   //{
   //    PrintCustomizationOptions();
   //}

    private void SetCaptain()
    {
        // Verifique se h� ao menos dois jogadores conectados para definir o capit�o como o segundo jogador
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            // Define o ActorNumber do capit�o como o segundo jogador na lista
            captainActorNumber = PhotonNetwork.PlayerList[1].ActorNumber;
        }
    }

    private void SetUI()
    {
        CaptainsCommands.SetActive(false);
        sailor.SetActive(false);
        CaptainCharacter.SetActive(false);
        roleText.SetActive(true);
        Background.SetActive(true);

        if (customizationMenu == null)
        {
            Debug.LogError("O GameObject customizationMenu n�o est� atribu�do!");
        }
        else
        {
            customizationMenu.SetActive(false);
        }

        if (toggleCustomizationButton == null)
        {
            Debug.LogError("O bot�o toggleCustomizationButton n�o est� atribu�do!");
        }
        else
        {
            toggleCustomizationButton.onClick.AddListener(ToggleCustomizationMenu);
        }

        if (printCustomizationButton == null)
        {
            Debug.LogError("O bot�o printCustomizationButton n�o est� atribu�do!");
        }
        else
        {
            printCustomizationButton.onClick.AddListener(PrintCustomizationOptions);
        }

        if (PlayButton != null)
        {
            PlayButton.SetActive(false);
        }

        if (IsCaptain())
        {
            customizationMenu.SetActive(isCustomizationMenuActive); // Exibe o menu se for o capit�o
            ShowCaptain.SetActive(!isCustomizationMenuActive);
        }
        else
        {
            customizationMenu.SetActive(false); // Esconde o menu para todos os outros jogadores
            CaptainCharacter.SetActive(false);
            ShowCaptain.SetActive(false);
        }
    }

    // M�todo para ativar/desativar o menu de personaliza��o
    public void ToggleCustomizationMenu()
    {
        if (IsCaptain())
        {
            isCustomizationMenuActive = !isCustomizationMenuActive;
            customizationMenu.SetActive(isCustomizationMenuActive);
            CaptainCharacter.SetActive(true);
            PlayButton.SetActive(!isCustomizationMenuActive);
            Debug.Log("Menu de personaliza��o " + (isCustomizationMenuActive ? "ativado" : "desativado"));
        }
        else
        {
            Debug.LogWarning("Apenas o capit�o pode ver o menu de personaliza��o.");
        }

        TurnUIOff();
    }

    // M�todo para imprimir as op��es de personaliza��o salvas no PlayerPrefs
    public void PrintCustomizationOptions()
    {
       // Debug.Log("Op��es de personaliza��o salvas nos PlayerPrefs:");
       // Debug.Log("Hair: " + PlayerPrefs.GetInt("Hair", -1));
       // Debug.Log("Hat: " + PlayerPrefs.GetInt("Hat", -1));
       // Debug.Log("Glasses: " + PlayerPrefs.GetInt("Glasses", -1));
       // Debug.Log("SkinColor: " + PlayerPrefs.GetInt("SkinColor", -1));
       // Debug.Log("Eyebrows: " + PlayerPrefs.GetInt("Eyebrows", -1));
       // Debug.Log("Face: " + PlayerPrefs.GetInt("Face", -1));
       // Debug.Log("Beard: " + PlayerPrefs.GetInt("Beard", -1));
       // Debug.Log("Cheek: " + (PlayerPrefs.GetInt("Cheek", 0) == 1 ? "Ativado" : "Desativado"));
       // Debug.Log("Dress: " + PlayerPrefs.GetInt("Dress", -1));
        Debug.Log("G�NERO:::: " + PlayerPrefs.GetString("Gender"));
    }

    // M�todo que verifica se o jogador atual � o capit�o
    private bool IsCaptain()
    {
        bool isCaptain = PhotonNetwork.LocalPlayer.ActorNumber == captainActorNumber;
        Debug.LogWarning("Player ID: " + PhotonNetwork.LocalPlayer.ActorNumber + " | Capit�o ID: " + captainActorNumber + " | � Capit�o: " + isCaptain);
        return isCaptain;
    }

    private void TurnUIOff()
    {
        roleText.SetActive(false);
        InitialButton.SetActive(false);
    }
}
