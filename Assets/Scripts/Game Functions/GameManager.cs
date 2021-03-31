using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Initialization

    //Booleans
    private bool _player1Start;
    private bool _player2Start;

    private bool _gameStarted;

    public bool countdownInProgress;

    private bool _timeRunning;
    private bool _timeRanOut;

    public bool allowTiebreaker;
    private bool _tiebreaker;

    private bool _gameEnded;
    public string gamemode;

    //Integers
    public int player1Score;
    public int player2Score;

    public int gameClockMinutes;
    public int gameClockSeconds;

    //Transforms
    public Transform ballServePoint;

    //UI
    public Text countdownText;

    private Text _gameClockText;
    private Text _roundCounterText;
    public Text topRoundCounterText;

    public TMP_Text gamemodeDisplayText;

    public Text player1ScoreText;
    public Text player2ScoreText;

    public GameObject pauseMenu;

    public GameObject settingsMenu;

    public GameObject endgameScreen;

    public GameObject gameClock;
    public GameObject roundCounter;

    public GameObject reServeText;

    public TMP_Text gameStatusText;

    public TMP_Text pauseMenuRestartButtonText;

    public GameObject[] pressAnyKeyUI;

    public TMP_Text playerWinText;
    public TMP_Text scoreText;

    public Text[] player1ControlUI;
    public Image[] player2ControlUI;

    public Color buttonPressedColor;

    //Hotkeys
    public KeyCode[] pauseKeys;

    //Paddles
    public GameObject player1Paddle;
    public GameObject player2Paddle;

    //Rounds Gamemode
    [SerializeField] private int roundAmount;
    [SerializeField] private int roundNumber;

    //Timed Gamemode
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;

    //Scripts
    private UIAnimationManager _UIAnimationManagerScript;
    private Settings _settings;

    //Ball Trail
    public TrailRenderer ballTrail;
    public bool trailEnabled;

    private void Awake()
    {
        _UIAnimationManagerScript = FindObjectOfType<UIAnimationManager>();
        _settings = FindObjectOfType<Settings>();

        countdownInProgress = true;

        _timeRunning = false;
        _timeRanOut = false;

        _tiebreaker = false;

        _player1Start = false;
        _player2Start = false;

        //Initialize all game variables
        gamemode = GameSetup.gamemode;

        gameClock.SetActive(false);
        roundCounter.SetActive(false);

        for (int i = 0; i < pressAnyKeyUI.Length; i++)
        {
            pressAnyKeyUI[i].SetActive(true);

            if (!_settings.chargingEnabled && i == 9)
                pressAnyKeyUI[i].SetActive(false);
        }

        switch (gamemode)
        {
            case "Timed":
                minutes = GameSetup.timeLimitInfo[0];
                seconds = GameSetup.timeLimitInfo[1];

                gameClockMinutes = minutes;
                gameClockSeconds = seconds;

                _gameClockText = gameClock.GetComponent<Text>();

                gamemodeDisplayText.text = "Gamemode: Timed";
                break;
            case "Rounds":
                roundNumber = 1;
                roundAmount = GameSetup.roundAmount;

                gamemodeDisplayText.text = "Gamemode: Rounds";
                break;
            default:
                Debug.LogWarning("Invalid Gamemode: " + gamemode);
                break;
        }

        _roundCounterText = roundCounter.GetComponent<Text>();
    }

    #endregion

    private void Update()
    {
        //Update Settings
        trailEnabled = _settings.ballTail;

        player1ScoreText.text = "" + player1Score;
        player2ScoreText.text = "" + player2Score;

        if (!_gameStarted)
        {
            //If any control keys from both sides are pressed, then start the game!
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D))
            {
                _player1Start = true;
                _UIAnimationManagerScript.TransitionKeyUIColor(true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                _player2Start = true;
                _UIAnimationManagerScript.TransitionKeyUIColor(false);
            }

            if (_player1Start)
            {
                for (int i = 0; i < player1ControlUI.Length; i++)
                {
                    player1ControlUI[i].color = buttonPressedColor;
                }
            }

            if (_player2Start)
            {
                for (int i = 0; i < player2ControlUI.Length; i++)
                {
                    player2ControlUI[i].color = buttonPressedColor;
                }
            }

            if (_player1Start && _player2Start)
            {
                InitiateCountdown();
                _UIAnimationManagerScript.HideUIKeys();
                _gameStarted = true;
            }
        }

        if (Input.GetKeyDown(pauseKeys[0]) || Input.GetKeyDown(pauseKeys[1]))
            PauseGame();

        switch (gamemode)
        {
            //Manage Game Clock UI
            case "Timed" when !_tiebreaker:
                {
                    string formattedMinutes = gameClockMinutes.ToString();
                    string formattedSeconds = gameClockSeconds.ToString();

                    if (formattedMinutes.Length == 1)
                        formattedMinutes = "0" + formattedMinutes;

                    if (formattedSeconds.Length == 1)
                        formattedSeconds = "0" + formattedSeconds;

                    _gameClockText.text = formattedMinutes + ":" + formattedSeconds;
                    gameStatusText.text = formattedMinutes + ":" + formattedSeconds + " left";
                    break;
                }
            case "Timed":
                _gameClockText.text = "Tiebreaker";
                _roundCounterText.text = "Tiebreaker Round";

                gameStatusText.text = "Tiebreaker Round";
                break;
            //Manage Round Counter UI
            //UI needs to: Keep track of rounds.
            case "Rounds" when !_tiebreaker:
                topRoundCounterText.text = "Round " + roundNumber;
                _roundCounterText.text = "Round " + roundNumber + "/" + roundAmount;

                gameStatusText.text = "Round " + roundNumber + "/" + roundAmount;
                break;
            case "Rounds":
                topRoundCounterText.text = "Tiebreaker";
                _roundCounterText.text = "Tiebreaker Round";

                gameStatusText.text = "Tiebreaker Round";
                break;
        }

        if (countdownInProgress)
            pauseMenuRestartButtonText.text = "Restart (after countdown)";
        else
            pauseMenuRestartButtonText.text = "Restart";

        if (pressAnyKeyUI[0].activeInHierarchy)
            countdownInProgress = true;
    }

    public void Score(bool isPlayer1, int scoreAmount)
    {
        if (_gameEnded)
            return;

        if (isPlayer1)
            player1Score += scoreAmount;
        else
            player2Score += scoreAmount;

        if (_tiebreaker)
            EndGame();

        if (gamemode == "Rounds" && roundNumber + 1 > roundAmount)
        {
            EndGame();
            return;
        }

        roundNumber++;

        if (gamemode == "Timed" && _timeRanOut)
            EndGame();

        InitiateCountdown();
    }

    private void PauseGame()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");

        if (!pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;

            pauseMenu.SetActive(true);
        }
        else if (settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
        }
        else
            ResumeGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        _UIAnimationManagerScript.PauseMenuFadeOut();
    }

    public void OpenInGameSettingsMenu(GameObject settingsMenu)
    {
        if (settingsMenu.activeInHierarchy)
        {
            FindObjectOfType<Settings>().SaveSettings();

            settingsMenu.SetActive(false);
        }
        else
            settingsMenu.SetActive(true);
    }

    public void RestartGame(bool fromPauseMenu = false)
    {
        _tiebreaker = false;
        _gameEnded = false;

        player1Score = 0;
        player2Score = 0;

        _gameStarted = false;

        _player1Start = false;
        _player2Start = false;

        for (int i = 0; i < pressAnyKeyUI.Length; i++)
        {
            pressAnyKeyUI[i].SetActive(false);
            pressAnyKeyUI[i].SetActive(true);
        }

        player1Paddle.transform.position = new Vector2(player1Paddle.transform.position.x, 0f);
        player2Paddle.transform.position = new Vector2(player2Paddle.transform.position.x, 0f);

        player1Paddle.transform.eulerAngles = Vector3.zero;
        player2Paddle.transform.eulerAngles = Vector3.zero;

        if (gamemode == "Timed")
        {
            gameClock.SetActive(false);
            gameClockMinutes = minutes;
            gameClockSeconds = seconds;

            _timeRanOut = false;
        }
        else
        {
            topRoundCounterText.gameObject.SetActive(false);
            roundNumber = 1;
        }

        Time.timeScale = 1;

        if (fromPauseMenu)
        {
            GetComponent<BallServe>().RepositionBall(ballServePoint);
            pauseMenu.SetActive(false);
        }
        else
        {
            endgameScreen.SetActive(false);
        }
    }

    private void EndGame()
    {
        //First, check if player scores are the same to see whether or not a _tiebreaker round should be held.
        if (player1Score == player2Score && allowTiebreaker)
        {
            //Conduct _tiebreaker round
            //If timed, conduct a timeless round that never ends until somebody wins. Give option to declare a tie before conducting every _tiebreaker.
            /*Later on, maybe try to have some sort of _tiebreaker text to signal to the players that it will be an epic tie-breaking round.*/
            _tiebreaker = true;
            InitiateCountdown();
            return;
        }

        Time.timeScale = 0;

        if (player1Score > player2Score)
        {
            playerWinText.text = "Player 1 Wins!";
            scoreText.text = player1Score + " - " + player2Score;
        }
        else if (player2Score > player1Score)
        {
            playerWinText.text = "Player 2 Wins!";
            scoreText.text = player2Score + " - " + player1Score;
        }
        else
        {
            playerWinText.text = "Tie!";
            scoreText.text = player1Score + " - " + player2Score;
        }

        endgameScreen.SetActive(true);
        _gameEnded = true;
    }

    #region Countdown

    public void InitiateCountdown(bool reServe = false)
    {
        StartCoroutine(Countdown(3));

        ballTrail.Clear();
        ballTrail.emitting = false;

        if (reServe)
            reServeText.SetActive(true);

        GetComponent<BallServe>().RepositionBall(ballServePoint);

        GetComponent<ColorSchemeChange>().ChangeColorScheme(0);
    }

    IEnumerator Countdown(int numberOfSeconds)
    {
        countdownInProgress = true;

        GetComponent<BallServe>().PreServeBall();

        countdownText.text = "" + numberOfSeconds;

        gameClock.SetActive(false);
        countdownText.gameObject.SetActive(true);

        if (gamemode == "Rounds" || gamemode == "Timed" && _tiebreaker)
        {
            topRoundCounterText.gameObject.SetActive(false);
            roundCounter.SetActive(true);
        }

        for (int i = 1; i <= numberOfSeconds; i++)
        {
            yield return new WaitForSeconds(1);

            countdownText.text = "" + (numberOfSeconds - i);
        }

        countdownText.gameObject.SetActive(false);

        countdownInProgress = false;

        if (gamemode == "Timed")
        {
            if (_tiebreaker)
                roundCounter.GetComponent<ActivateWithFade>().FadeOut();
            else
            {
                gameClock.SetActive(true);

                if (!_timeRunning)
                    StartCoroutine(GameClock());
            }
        }

        if (gamemode == "Rounds")
            roundCounter.GetComponent<ActivateWithFade>().FadeOut();

        if (reServeText.activeInHierarchy)
            reServeText.GetComponent<ActivateWithFade>().FadeOut();

        ballTrail.emitting = trailEnabled;

        GetComponent<BallServe>().ServeBall();
    }

    #endregion

    private IEnumerator GameClock()
    {
        _timeRunning = true;

        int timeInSeconds = ((minutes * 60) + seconds);

        for (int timeLeft = 0; timeLeft < timeInSeconds; timeLeft++)
        {
            if (countdownInProgress)
                yield return new WaitUntil(() => !countdownInProgress);

            yield return new WaitForSeconds(1);

            if (gameClockSeconds != 0)
                gameClockSeconds--;
            else
            {
                if (gameClockMinutes == 0)
                    yield break;

                gameClockMinutes--;
                gameClockSeconds = 59;
            }
        }

        _timeRunning = false;
        _timeRanOut = true;

        EndGame();
    }
}