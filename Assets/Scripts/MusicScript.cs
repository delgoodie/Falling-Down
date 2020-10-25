using UnityEngine;

public class MusicScript : MonoBehaviour
{
    AudioSource[] music = new AudioSource[9];
    public bool playing;
    private int currentIndex;
    void Start()
    {
        playing = false;
        currentIndex = Random.Range(0, 8);
        music = GetComponents<AudioSource>();
        if (PlayerPrefs.GetInt("sound", 1) == 1)
            play();
    }
    void Update()
    {
        if (!music[currentIndex].isPlaying && playing)
        {
            if (Random.Range(0, 3) == 2)
            {
                currentIndex = Random.Range(0, music.Length);
                music[currentIndex].Play();
            }
            else
            {
                currentIndex++;
                currentIndex %= 9;
                music[currentIndex].Play();
            }
        }
    }

    public void play()
    {
        music[currentIndex].UnPause();
        playing = true;
    }

    public void pause()
    {
        music[currentIndex].Pause();
        playing = false;
    }
}
