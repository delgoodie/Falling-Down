using UnityEngine;

public class ObstacleManagerScript : MonoBehaviour
{
    public GameObject player;
    public GameObject borderPrefab;
    public GameObject[] obstacleTypes = new GameObject[9];
    public float obstacleSeperation = 35;
    public int obstacleMultiplier = 3;
    public bool gameOn = false;


    private GameObject[] borders = new GameObject[4];
    private GameObject[] obstacles;
    private int obsCount = 0;
    private int borderCount = 0;
    private float targetY = 0;
    private float borderY = -10;

    void Start()
    {
        gameOn = false;
        obstacles = new GameObject[obstacleTypes.Length * obstacleMultiplier];
    }
    void Update()
    {
        if (gameOn)
            gameLoop();
    }

    public void startGame()
    {
        for (int i = 0; i < obstacles.Length; i++)
            Destroy(obstacles[i]);
        for (int i = 0; i < borders.Length; i++)
            Destroy(borders[i]);

        obstacleSeperation = 35;
        obsCount = 0;
        borderCount = 0;
        borderY = -55f;
        targetY = 0f;

        for (int i = 0; i < 4; i++)
            borders[i] = Instantiate(borderPrefab, new Vector3(0, i * -50 + borderY, 0), Quaternion.identity);

        for (int i = 0; i < Mathf.Floor(obstacleTypes.Length * obstacleMultiplier); i++)
        {
            Vector3 pos = new Vector3(0, obsHeight(targetY - i * obstacleSeperation, player.GetComponent<Rigidbody>().velocity.y), 0);
            int randIndex = Random.Range(0, obstacleTypes.Length - 1);
            obstacles[i] = Instantiate(obstacleTypes[randIndex], pos, Quaternion.Euler(0, Random.Range(0, 4) * 90, 0));
        }
    }

    private void gameLoop()
    {
        if (player.transform.position.y + obstacleSeperation < targetY)
        {
            targetY = obsHeight(player.transform.position.y, player.GetComponent<Rigidbody>().velocity.y);
            Vector3 randomPosition = new Vector3(0, obsHeight(targetY - (obstacles.Length - 1) * obstacleSeperation, player.GetComponent<Rigidbody>().velocity.y), 0);
            obstacles[obsCount % obstacles.Length].transform.position = randomPosition;
            obstacles[obsCount % obstacles.Length].transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
            obsCount++;
            obstacleSeperation -= .25f;
        }
        if (player.transform.position.y < borderY)
        {
            borderY -= 50;
            borders[borderCount % 4].transform.position = new Vector3(0, borderY - 50 * 2, 0);
            borderCount++;
        }
    }

    float obsHeight(float x, float v)
    {
        return (x - obstacleSeperation); // + .02f * v;
    }

}
