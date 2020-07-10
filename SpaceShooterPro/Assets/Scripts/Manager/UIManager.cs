using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum AmountLife
{
    Zero = 0,
    One,
    Two,
    Three
}

public class UIManager : MonoBehaviour
{ 
    private GameManager _gameManager;

    private string _defaultScoreText = "Score : ";

    [Header("UI elements")]
    [SerializeField] private TextMeshProUGUI _scoreTMPro;
    [SerializeField] private Image _livesImage;
    [SerializeField] private TextMeshProUGUI _gameoverText;
    [SerializeField] private TextMeshProUGUI _restartText;

    [Space(2)]
    [Header("Image Source")]
    [SerializeField] private List<Sprite> _livesSpriteList = new List<Sprite>();

    public void SetScoreText()
    {
        int score = _gameManager.CurrScore;
        _scoreTMPro.text = _defaultScoreText + score.ToString();
    }

    public void UpdateLivesImage()
    {
        int lives = (int)_gameManager.CurrPlayer.Life;
        
        switch (lives)
        {
            case 3:
                _livesImage.sprite = _livesSpriteList[(int)AmountLife.Three];
                break;
            case 2:
                _livesImage.sprite = _livesSpriteList[(int)AmountLife.Two];
                break;
            case 1:
                _livesImage.sprite = _livesSpriteList[(int)AmountLife.One];
                break;
            case 0:
                _livesImage.sprite = _livesSpriteList[(int)AmountLife.Zero];
                break;
            default:
                Debug.LogError("Wrong amount of player lives.");
                break;
        }
    }

    public void SetActiveGameOverText(bool option)
    {
        _gameoverText.gameObject.SetActive(option);
        _restartText.gameObject.SetActive(option);
        if(option)
        {
            StartCoroutine(GameOverFlicker());
        }
        else
        {
            StopCoroutine(GameOverFlicker());
        }
    }

    private void Start() 
    {
        _gameManager = gameObject.GetComponent<GameManager>();       
        if(_gameManager == null)
        {
            Debug.LogError("The GameManager is NULL");
            return;
        }

        SetActiveGameOverText(false);
        SetScoreText();
    }


    private IEnumerator GameOverFlicker()
    {
        while(true)
        {
            _gameoverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameoverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
