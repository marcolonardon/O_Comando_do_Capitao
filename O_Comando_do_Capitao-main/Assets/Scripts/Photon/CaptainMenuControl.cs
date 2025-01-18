using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CaptainMenuControl : MonoBehaviourPunCallbacks
{
    public GameObject Background;
    public GameObject ShowCaptain;
    public GameObject CaptainCharacter;
    public GameObject customizationMenu; // Referência ao GameObject do menu de personalização
    public GameObject CaptainsCommands;
    public GameObject roleText;
    public GameObject PlayButton;
    public Button toggleCustomizationButton; // Referência ao botão para ativar/desativar o menu de personalização
    public Button printCustomizationButton; // Referência ao botão para imprimir opções de personalização
    public GameObject InitialButton;
    private bool isCustomizationMenuActive = false; // Estado inicial do menu

    public GameObject sailor; // GameObject do Sailor

    // Variável para armazenar o ActorNumber do capitão
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
        // Verifique se há ao menos dois jogadores conectados para definir o capitão como o segundo jogador
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            // Define o ActorNumber do capitão como o segundo jogador na lista
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
            Debug.LogError("O GameObject customizationMenu não está atribuído!");
        }
        else
        {
            customizationMenu.SetActive(false);
        }

        if (toggleCustomizationButton == null)
        {
            Debug.LogError("O botão toggleCustomizationButton não está atribuído!");
        }
        else
        {
            toggleCustomizationButton.onClick.AddListener(ToggleCustomizationMenu);
        }

        if (printCustomizationButton == null)
        {
            Debug.LogError("O botão printCustomizationButton não está atribuído!");
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
            customizationMenu.SetActive(isCustomizationMenuActive); // Exibe o menu se for o capitão
            ShowCaptain.SetActive(!isCustomizationMenuActive);
        }
        else
        {
            customizationMenu.SetActive(false); // Esconde o menu para todos os outros jogadores
            CaptainCharacter.SetActive(false);
            ShowCaptain.SetActive(false);
        }
    }

    // Método para ativar/desativar o menu de personalização
    public void ToggleCustomizationMenu()
    {
        if (IsCaptain())
        {
            isCustomizationMenuActive = !isCustomizationMenuActive;
            customizationMenu.SetActive(isCustomizationMenuActive);
            CaptainCharacter.SetActive(true);
            PlayButton.SetActive(!isCustomizationMenuActive);
            Debug.Log("Menu de personalização " + (isCustomizationMenuActive ? "ativado" : "desativado"));
        }
        else
        {
            Debug.LogWarning("Apenas o capitão pode ver o menu de personalização.");
        }

        TurnUIOff();
    }

    // Método para imprimir as opções de personalização salvas no PlayerPrefs
    public void PrintCustomizationOptions()
    {
       // Debug.Log("Opções de personalização salvas nos PlayerPrefs:");
       // Debug.Log("Hair: " + PlayerPrefs.GetInt("Hair", -1));
       // Debug.Log("Hat: " + PlayerPrefs.GetInt("Hat", -1));
       // Debug.Log("Glasses: " + PlayerPrefs.GetInt("Glasses", -1));
       // Debug.Log("SkinColor: " + PlayerPrefs.GetInt("SkinColor", -1));
       // Debug.Log("Eyebrows: " + PlayerPrefs.GetInt("Eyebrows", -1));
       // Debug.Log("Face: " + PlayerPrefs.GetInt("Face", -1));
       // Debug.Log("Beard: " + PlayerPrefs.GetInt("Beard", -1));
       // Debug.Log("Cheek: " + (PlayerPrefs.GetInt("Cheek", 0) == 1 ? "Ativado" : "Desativado"));
       // Debug.Log("Dress: " + PlayerPrefs.GetInt("Dress", -1));
        Debug.Log("GÊNERO:::: " + PlayerPrefs.GetString("Gender"));
    }

    // Método que verifica se o jogador atual é o capitão
    private bool IsCaptain()
    {
        bool isCaptain = PhotonNetwork.LocalPlayer.ActorNumber == captainActorNumber;
        Debug.LogWarning("Player ID: " + PhotonNetwork.LocalPlayer.ActorNumber + " | Capitão ID: " + captainActorNumber + " | É Capitão: " + isCaptain);
        return isCaptain;
    }

    private void TurnUIOff()
    {
        roleText.SetActive(false);
        InitialButton.SetActive(false);
    }
}
