using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
public class GameManagerScript : MonoBehaviour
{
    public bool sound = true;
    public Button soundButton;
    public Button playButton;
    public Text title;
    public Text scoreText;
    public Text highscoreText;
    public GameObject ObstacleManager;
    public GameObject Player;
    public GameObject music;
    public bool testMode = true;
    public int rounds;
    public Text name;


    private ObstacleManagerScript obstacleManagerScript;
    private PlayerScript playerScript;
    private float highscore;
    private string gameId = "3848776";
    void Start()
    {
        title.fontSize = (int)(Screen.width / 20);
        rounds = 0;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
        soundButton.GetComponentInChildren<Text>().color = (sound) ? Color.white : Color.gray;
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.CrossFadeColor(Color.clear, .05f, false, true);
        soundButton.onClick.AddListener(toggleSound);
        playButton.onClick.AddListener(startGame);
        obstacleManagerScript = ObstacleManager.GetComponent<ObstacleManagerScript>();
        playerScript = Player.GetComponent<PlayerScript>();

        Advertisement.Initialize(gameId, testMode);
    }

    // Update is called once per frame
    void Update()
    {
        if (!obstacleManagerScript.gameOn && !playerScript.gameOn && !Advertisement.isShowing)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
                startGame();
        }
        else
            scoreText.text = "" + Mathf.Round(Mathf.Abs(Player.transform.position.y));
    }

    void startGame()
    {
        soundButton.interactable = false;
        playButton.interactable = false;

        scoreText.CrossFadeColor(Color.white, 1f, false, true);

        highscoreText.CrossFadeColor(Color.clear, 1f, false, true);
        soundButton.GetComponentInChildren<Text>().CrossFadeColor(Color.clear, 1f, false, true);
        title.CrossFadeColor(Color.clear, .5f, false, true);
        name.CrossFadeColor(Color.clear, .5f, false, true);

        playerScript.gameOn = true;
        obstacleManagerScript.gameOn = true;

        obstacleManagerScript.startGame();
        playerScript.startGame();

    }
    public void endGame()
    {
        if (rounds % 5 == 4)
            Advertisement.Show();
        GameObject.Find("Camera").transform.position = new Vector3(10, 10, 10);

        playButton.interactable = true;
        soundButton.interactable = true;

        scoreText.CrossFadeColor(Color.clear, .5f, false, true);

        soundButton.GetComponentInChildren<Text>().CrossFadeColor((sound) ? Color.white : Color.gray, 1f, false, true);
        highscoreText.CrossFadeColor(Color.white, 1f, false, true);
        title.CrossFadeColor(Color.white, 1f, false, true);
        name.CrossFadeColor(Color.white, 1f, false, true);

        if (highscore < Mathf.Round(Mathf.Abs(Player.transform.position.y)))
        {
            highscore = (int)Mathf.Round(Mathf.Abs(Player.transform.position.y));
            PlayerPrefs.SetInt("highscore", (int)Mathf.Round(Mathf.Abs(Player.transform.position.y)));
            PlayerPrefs.Save();
        }
        obstacleManagerScript.gameOn = false;
        playerScript.gameOn = false;

        highscoreText.text = "Highscore: " + highscore;
        rounds++;
    }

    private void toggleSound()
    {
        sound = !sound;
        soundButton.GetComponentInChildren<Text>().color = (sound) ? Color.white : Color.gray;
        if (sound)
            music.GetComponent<MusicScript>().play();
        else
            music.GetComponent<MusicScript>().pause();
        PlayerPrefs.SetInt("sound", (sound) ? 1 : 0);
        PlayerPrefs.Save();
    }
}