using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class MultiplayerGameManager : NetworkBehaviour
{
    #region Variables
    //Booleans
    public bool gameStarted;

    [Header("Game Settings/Data")]
    public bool countdownInProgress;

    public bool player1Ready;
    public bool player2Ready;

    private bool _timeRunning;
    private bool _timeRanOut;

    public bool allowTiebreaker;
    private bool _tiebreaker;

    private bool _gameEnded;
    [SyncVar] public string gamemode;

    //Integers
    public int playerID;

    [SyncVar(hook = nameof(UpdatePlayer1Score))]
    public int player1Score;

    [SyncVar(hook = nameof(UpdatePlayer2Score))]
    public int player2Score;

    [SyncVar] public int gameClockMinutes;
    [SyncVar] public int gameClockSeconds;

    //UI
    [Header("UI")]
    public Text countdownText;

    [SerializeField] private Text gameClockText;
    [SerializeField] private Text roundCounterText;
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

    public GameObject pressAnyKeyText;

    public TMP_Text gameStatusText;

    public TMP_Text pauseMenuRestartButtonText;

    public GameObject[] pressAnyKeyUI;

    public TMP_Text playerWinText;
    public TMP_Text scoreText;

    public Text[] playerControlUI;

    public Color buttonPressedColor;

    //Keybinds
    public KeyCode[] pauseKeys;

    //Paddles
    public GameObject player1Paddle;
    public GameObject player2Paddle;

    //Rounds Gamemode
    [SyncVar] [SerializeField] private int roundAmount;
    [SyncVar] [SerializeField] private int roundNumber;

    //Timed Gamemode
    [SyncVar] [SerializeField] private int minutes;
    [SyncVar] [SerializeField] private int seconds;

    //Scripts
    [Header("Components")]
    [SerializeField] UIAnimationManager _UIAnimationManagerScript;

    [SerializeField] ColorSchemeChange colorSchemeManager;

    [SerializeField] Settings _settings;

    //Ball Tail
    [Header("Ball Tail")]
    public bool tailEnabled;

    private TrailRenderer ballTail;
    #endregion

    private void UpdatePlayer1Score(int oldScore, int newScore)
    {
        Debug.Log("Player 1 Score Updated. Changed from " + oldScore + " to " + newScore);

        player1ScoreText.text = newScore.ToString();
    }

    private void UpdatePlayer2Score(int oldScore, int newScore)
    {
        Debug.Log("Player 2 Score Updated. Changed from " + oldScore + " to " + newScore);

        player2ScoreText.text = newScore.ToString();
    }

    void OnEnable()
    {
        gamemode = GameSetup.gamemode == null ? "Undefined" : GameSetup.gamemode;

        //Load in game data
        switch (gamemode)
        {
            case "Timed":
                minutes = GameSetup.timeLimitInfo[0];
                seconds = GameSetup.timeLimitInfo[1];

                gameClockMinutes = minutes;
                gameClockSeconds = seconds;

                gamemodeDisplayText.text = "Gamemode: Timed";
                break;
            case "Rounds":
                roundNumber = 1;
                roundAmount = GameSetup.roundAmount;

                gamemodeDisplayText.text = "Gamemode: Rounds";
                break;
            default:
                if (MultiplayerPaddleSetup.paddleID == 1)
                    Debug.LogWarning("Invalid Gamemode: " + gamemode);
                break;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        //Initialize all game variables
        countdownInProgress = true;

        _timeRunning = false;
        _timeRanOut = false;

        _tiebreaker = false;

        if (MultiplayerPaddleSetup.paddleID == 1)
            gamemode = GameSetup.gamemode;

        gameClock.SetActive(false);
        roundCounter.SetActive(false);

        for (int i = 0; i < pressAnyKeyUI.Length; i++)
        {
            pressAnyKeyUI[i].SetActive(true);
        }

        MultiplayerNetworkManager.OnPlayerChange += ShowPressKeyReminder;
        PaddleControlsMultiplayer.OnPlayerReady += SetPlayerStatus;
    }

    private void ShowPressKeyReminder(bool roomFull)
    {
        if (pressAnyKeyText != null) pressAnyKeyText.SetActive(roomFull);
    }

    private void SetPlayerStatus(int playerID) => SetPlayerStatus(playerID, true);
    private void SetPlayerStatus(int playerID, bool ready)
    {
        if (playerID == 1)
            player1Ready = true;
        else
            player2Ready = true;

        if (player1Ready && player2Ready && !gameStarted)
            BothPlayersReady();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        for (int i = 0; i < pressAnyKeyUI.Length; i++)
        {
            pressAnyKeyUI[i].SetActive(false);
        }

        pressAnyKeyText.SetActive(false);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server Started.");

        if (NetworkServer.connections.Count == 2)
            AssignBallTailInstance(true);
        else
            MultiplayerNetworkManager.OnPlayerChange += AssignBallTailInstance;

        //Stop game if somebody leaves.
        MultiplayerNetworkManager.OnPlayerChange += StopIfRoomNotFull;
    }

    private void StopIfRoomNotFull(bool roomFull)
    {
        if (!roomFull && !_gameEnded)
            StopGame();
    }

    private void AssignBallTailInstance(bool roomFull)
    {
        if (roomFull)
            ballTail = GameObject.FindGameObjectWithTag("Ball")?.transform.GetChild(0)?.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        //Update Settings
        tailEnabled = _settings.ballTail;

        player1ScoreText.text = "" + player1Score;
        player2ScoreText.text = "" + player2Score;

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

                    gameClockText.text = formattedMinutes + ":" + formattedSeconds;
                    gameStatusText.text = formattedMinutes + ":" + formattedSeconds + " left";
                    break;
                }
            case "Timed":
                gameClockText.text = "Tiebreaker";
                roundCounterText.text = "Tiebreaker Round";

                gameStatusText.text = "Tiebreaker Round";
                break;
            //Manage Round Counter UI
            //UI needs to: Keep track of rounds.
            case "Rounds" when !_tiebreaker:
                topRoundCounterText.text = "Round " + roundNumber;
                roundCounterText.text = "Round " + roundNumber + "/" + roundAmount;

                gameStatusText.text = "Round " + roundNumber + "/" + roundAmount;
                break;
            case "Rounds":
                topRoundCounterText.text = "Tiebreaker";
                roundCounterText.text = "Tiebreaker Round";

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

    public void BothPlayersReady()
    {
        AssignBallTailInstance(true);
        GetComponent<BallServe>().ball = ballTail?.transform.parent;

        if (playerID == 1)
            InitiateCountdown(false);



        _UIAnimationManagerScript.HideUIKeys();
        gameStarted = true;
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

        if (playerID == 1)
            InitiateCountdown(false);
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

        gameStarted = false;

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

            if (playerID == 1)
                roundNumber = 1;
        }

        Time.timeScale = 1;

        if (fromPauseMenu)
        {
            GetComponent<BallServe>().RepositionBall();
            pauseMenu.SetActive(false);
        }
        else
        {
            endgameScreen.SetActive(false);
        }
    }

    private void StopGame()
    {
        _tiebreaker = false;
        _gameEnded = false;

        player1Score = 0;
        player2Score = 0;

        gameStarted = false;

        ResetUI();
    }

    private void EndGame()
    {
        //First, check if player scores are the same to see whether or not a tiebreaker round should be held.
        if (player1Score == player2Score && allowTiebreaker)
        {
            //Conduct tiebreaker round
            //If timed, conduct a timeless round that never ends until somebody wins. Give option to declare a tie before conducting every _tiebreaker.
            /*Later on, maybe try to have some sort of _tiebreaker text to signal to the players that it will be an epic tie-breaking round.*/
            _tiebreaker = true;

            if (playerID == 1)
                InitiateCountdown(false);

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

        ResetUI();
    }

    #region Countdown

    private void ResetUI()
    {
        switch (gamemode)
        {
            case "Timed":
                if (gameClock != null)
                    gameClock.SetActive(false);
                break;
            case "Rounds":
                if (roundCounter != null)
                    roundCounter.SetActive(false);
                break;
        }

        if (player1ScoreText != null)
        {
            player1ScoreText.gameObject.SetActive(false);
            player2ScoreText.gameObject.SetActive(false);
        }
    }

    public void InitiateReServeCountdown() => InitiateCountdown(true);

    [ClientRpc]
    public void InitiateCountdown(bool reServe)
    {
        if (MultiplayerPaddleSetup.paddleID == 2)
            BothPlayersReady();

        StartCoroutine(Countdown(3));

        ballTail?.Clear();

        if (ballTail != null)
            ballTail.emitting = false;

        if (reServe)
            reServeText.SetActive(true);

        if (playerID == 1)
            GetComponent<BallServe>().RepositionBall();

        colorSchemeManager.ChangeColorScheme(0);
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

        player1ScoreText.gameObject.SetActive(true);
        player2ScoreText.gameObject.SetActive(true);

        if (gamemode == "Timed")
        {
            if (_tiebreaker)
                roundCounter.GetComponent<ActivateWithFade>().FadeOut();
            else
            {
                gameClock.SetActive(true);

                if (!_timeRunning && playerID == 1)
                    StartCoroutine(GameClock());
            }
        }

        if (gamemode == "Rounds")
            roundCounter.GetComponent<ActivateWithFade>().FadeOut();

        if (reServeText.activeInHierarchy)
            reServeText.GetComponent<ActivateWithFade>().FadeOut();

        if (ballTail != null)
            ballTail.emitting = tailEnabled;

        if (playerID == 1)
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