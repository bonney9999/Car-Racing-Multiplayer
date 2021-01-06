using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleInputNamespace;

public class CarSoundANDROID : MonoBehaviour
{

    //FOR ANDROID
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
    bool breaking = false;

    public static bool isEngineOn = false;
    private Coroutine _coroutineMethod = null;
    public float volumeForIdle = 0.120f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // EngineStartOff();
        Accelarate();
        //BreakSystemSound();
    }
    public void HandBreak() //when you hold or touching the UI button.            
    {                       //USE it for HANDBREAK BUTTON.
        carSound.Stop();
        if (isEngineOn)
        {
            if(breaking)
            {
                carSound.Stop();
                PlaySound(IdleSound);
                breaking = false;

            }
            else
            {
                carSound.Stop();
                PlaySound(breakSound, true);
                breaking = true;

            }
                
        }
    }


    private void PlaySound(AudioClip a, bool oneShot = false)
    {
        //This function play the AudioClip on AudioSource.

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
    public void EngineOnOff() //THIS FUNCTION USED FOR ENGINE ON OFF
    {
        if (isEngineOn)
        {
            if (carSound.isPlaying)
            {
                ifIdle(IdleSound);

                carSound.Stop();
                isEngineOn = false;
                print("Is off");
            }
        }
        else
        {
            PlaySound(carStartSound, true);
            isEngineOn = true;
            PlaySound(IdleSound);
            ifIdle(IdleSound);
            print("Is On");
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
        //if Android

        ifIdle(IdleSound);
        if (CarControlling.isAndroid)
        {

            //FOR using GetAxis as Getkeydown and getkeyup

            if (SimpleInput.GetAxis("Vertical") > 0 || SimpleInput.GetAxis("Vertical") < 0)
            {
                print(SimpleInput.GetAxis("Vertical"));
                if (!stickDownLast)
                {
                    //when KEYDOWN
                    // if accelerating
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
                stickDownLast = true;

            }
            else
            {
                stickDownLast = false;
            }

            if (SimpleInput.GetAxis("Vertical") == 0)
            {
                if (!flag)
                {
                    flag = true;
                }
                else
                {
                    if (!stickDownLast1)
                    {
                        //when KEYUP
                        //if decelerating
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

                stickDownLast1 = true;

            }
            else  //this is changed.
            {

                stickDownLast1 = false;
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
