using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundWINDOW: MonoBehaviour
{
    //FOR WINDOWS
    [SerializeField] private Rigidbody _player = null;


    public AudioSource carSound;
    public AudioClip carStartSound;
    public AudioClip accelarateSound;
    public AudioClip decelarateSound;
    public AudioClip highSound;
    public AudioClip IdleSound;
    public AudioClip breakSound;
    public AudioClip maxRpm;


   


    bool stickDownLast = false;
    bool stickDownLast1 = false;
    bool flag = false;

    public static bool isEngineOn = false;
    private Coroutine _coroutineMethod = null;
    public float volumeForIdle = 0.120f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start method");
    }

    // Update is called once per frame
    void Update()
    {
        EngineStartOff();
        Accelarate();
        BreakSystemSound();
    }

    private void BreakSystemSound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carSound.Stop();
            if (isEngineOn)
            {
                if (_player.velocity.magnitude > 0)
                {
                    PlaySound(breakSound, true);
                }
            }


        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            carSound.Stop();
            if (isEngineOn)
                PlaySound(IdleSound);

        }
    }
    private void PlaySound(AudioClip a, bool oneShot = false)
    {
        if (oneShot == true)
        {
            carSound.PlayOneShot(a);
            if (a != IdleSound)
            {
                carSound.volume = 1f;
            }

            if (a == IdleSound)
            {
                carSound.volume = volumeForIdle;
            }
        }
        else
        {
            if (a != IdleSound)
            {
                carSound.volume = 1f;
            }

            if (a == IdleSound)
            {
                carSound.volume = volumeForIdle;
            }



            carSound.clip = a;
            carSound.Play();
        }

    }
    private void EngineStartOff()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("HEllo");
            if (carSound.isPlaying)
            {
                ifIdle(IdleSound);

                carSound.Stop();
                isEngineOn = false;
            }
            else
            {
                Debug.Log("hello2");
                PlaySound(carStartSound, true);
                isEngineOn = true;
                PlaySound(IdleSound);
                ifIdle(IdleSound);
            }

        }
    }

    private void ifIdle(AudioClip sound)
    {
        //checks whether to put loop sound or not.
        if (carSound.isPlaying == sound)
        {
            carSound.loop = true;
        }
        else
        {
            carSound.loop = false;
        }
    }
    private void Accelarate()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            if (carSound.isPlaying)
            {
                carSound.Stop();
                PlaySound(accelarateSound, true);

            }
            //Stops coroutine if key isn't pressed (last coroutine)
            if (_coroutineMethod != null)
                StopCoroutine(_coroutineMethod);
            _coroutineMethod = StartCoroutine(NextSound(accelarateSound.length, accelarateSound, maxRpm));
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {


            if (carSound.isPlaying)
            {
                carSound.Stop();
                PlaySound(decelarateSound, true);
                //Stops coroutine if key isn't pressed (last coroutine)
                if (_coroutineMethod != null)
                    StopCoroutine(_coroutineMethod);
                _coroutineMethod = StartCoroutine(NextSound(decelarateSound.length, decelarateSound, IdleSound));

                //decelarate mode making it to Idle in 3 secs. Have to cancel Start coroutine
            }
        }
    }
    IEnumerator NextSound(float len, AudioClip checkClip, AudioClip playClip)
    {
        //Next sound after delay of length as per first sound length

        yield return new WaitForSeconds(len);
        //Debug.Log("Got return");
        if (carSound.isPlaying == checkClip) // checking which clip is playing and work according
            PlaySound(playClip);
    }
}
