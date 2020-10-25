using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public bool gameOn = false;
    public float movementSpeed = 6f;
    public float rotationSpeed = 2f;
    public GameObject ObstacleManager;
    public Camera cam;
    public ParticleSystem particles;
    private bool controlsOn;
    private float rotationConstant = 0f;
    // Start is called before the first frame update

    void Start()
    {
        particles.Stop();
        gameOn = false;
        GetComponent<Rigidbody>().useGravity = false;
        controlsOn = false;
    }
    void FixedUpdate()
    {
        if (gameOn)
            gameLoop();
    }

    public void startGame()
    {
        Invoke("enableControls", 1.5f);
        cam.transform.SetParent(transform);
        cam.transform.localPosition = new Vector3(0, 0, 0);
        cam.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cam.fieldOfView = 87f;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(0, 0, -5);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        GetComponent<Rigidbody>().AddForce(new Vector3(0, 5f, 2f), ForceMode.Impulse);
        GetComponent<Rigidbody>().AddTorque(new Vector3(.15f, 0, 0), ForceMode.Impulse);

        rotationConstant = Random.Range(-10f, 11f) / 100f;
    }

    private void gameLoop()
    {
        input();
        Vector3 targetDir = new Vector3(transform.position.x, transform.position.y - 20, transform.position.z) - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.fixedDeltaTime * .001f), .5f);
        float error = -GetComponent<Rigidbody>().velocity.magnitude * .01f;
        transform.Rotate(Random.Range(-error, error), 0, Random.Range(-error, error), Space.Self);
        GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(-rotationSpeed, rotationSpeed + rotationConstant), 0), ForceMode.Force);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (gameOn)
        {
            controlsOn = false;
            transform.DetachChildren();
            cam.transform.position = new Vector3(30, transform.position.y - 20, 30);
            cam.transform.LookAt(transform, Vector3.up);
            cam.fieldOfView = 30f;
            particles.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            particles.transform.rotation = Quaternion.Euler(90, 0, 0);
            particles.Play();
            gameOn = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            Invoke("callEndGame", 2);
        }
    }

    private void callEndGame()
    {
        transform.parent.GetComponent<GameManagerScript>().endGame();
    }

    private void input()
    {
        if (controlsOn)
        {

            if (Input.GetAxis("Vertical") > 0)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.up.x, 0f, transform.up.z) * movementSpeed, ForceMode.Acceleration);
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.up.x, 0f, transform.up.z) * movementSpeed * .05f, ForceMode.VelocityChange);
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.up.x, 0f, transform.up.z) * -movementSpeed, ForceMode.Acceleration);
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.up.x, 0f, transform.up.z) * -movementSpeed * .05f, ForceMode.VelocityChange);
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.right.x, 0f, transform.right.z) * movementSpeed, ForceMode.Acceleration);
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.right.x, 0f, transform.right.z) * movementSpeed * .05f, ForceMode.VelocityChange);
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {

                GetComponent<Rigidbody>().AddForce(new Vector3(transform.right.x, 0f, transform.right.z) * -movementSpeed, ForceMode.Acceleration);
                GetComponent<Rigidbody>().AddForce(new Vector3(transform.right.x, 0f, transform.right.z) * -movementSpeed * .05f, ForceMode.VelocityChange);
            }
            Vector2 net = new Vector2(0, 0);
            for (int i = 0; i < Input.touchCount; i++)
            {
                net += Input.touches[i].position - new Vector2(Screen.width / 2, Screen.height / 2);
            }
            net.x /= Screen.width / 2;
            net.y /= Screen.height / 2;
            GetComponent<Rigidbody>().AddForce(new Vector3(net.x * transform.right.x, 0f, net.x * transform.right.z) * movementSpeed, ForceMode.Acceleration);
            GetComponent<Rigidbody>().AddForce(new Vector3(net.x * transform.right.x, 0f, net.x * transform.right.z) * movementSpeed * .05f, ForceMode.VelocityChange);
            GetComponent<Rigidbody>().AddForce(new Vector3(net.y * transform.up.x, 0f, net.y * transform.up.z) * movementSpeed, ForceMode.Acceleration);
            GetComponent<Rigidbody>().AddForce(new Vector3(net.y * transform.up.x, 0f, net.y * transform.up.z) * movementSpeed * .05f, ForceMode.VelocityChange);
        }
    }

    private void enableControls()
    {
        controlsOn = true;
    }
}
