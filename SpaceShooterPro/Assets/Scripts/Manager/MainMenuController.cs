using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _newGameText;

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeColorNewGameText(bool enterPoint)
    {
        if(enterPoint)
        {
            _newGameText.color = Color.red;
        }
        else
        {
            _newGameText.color = Color.white;
        }
    }
}
