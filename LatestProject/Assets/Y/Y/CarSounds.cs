using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    //THis code is all about sounds only.
    [SerializeField] private Rigidbody _player = null;
    
    
    public AudioSource carSound;
    public AudioClip carStartSound;
    public AudioClip accelarateSound;
    public AudioClip decelarateSound;
    public AudioClip highSound;
    public AudioClip IdleSound;
    public AudioClip breakSound;
    public AudioClip maxRpm;
    

    //audioclips for android
    public AudioClip androidAcceSound;
    public AudioClip androidDeceSound;


    bool stickDownLast = false;
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
       EngineStartOff();
        Accelarate();
        BreakSystemSound();
    }
    private void BreakSystemSound()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //|| CarControlling.Instance.On == true)
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
            if(isEngineOn)
                PlaySound(IdleSound);

        }
    }

    private void PlaySound(AudioClip a, bool oneShot = false)
    {
        //This function play the AudioClip on AudioSource.
        
        if(oneShot == true)
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
        if (CarControlling.isAndroid)
        {

            //This function starts the engine and turn it off
            if (SimpleInput.GetButton("shadedDark45")) //SimpleInput/Sprites/ShadedDark/Buttons (Location of this button)
            {
                if (carSound.isPlaying)
                {
                    ifIdle(IdleSound);

                    carSound.Stop();
                    isEngineOn = false;
                }
                else
                {
                    PlaySound(carStartSound, true);
                    isEngineOn = true;
                    PlaySound(IdleSound);
                    ifIdle(IdleSound);
                }

            }

        }
        else
        {
            //This function starts the engine and turn it off
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (carSound.isPlaying)
                {
                    ifIdle(IdleSound);

                    carSound.Stop();
                    isEngineOn = false;
                }
                else
                {
                    PlaySound(carStartSound, true);
                    isEngineOn = true;
                    PlaySound(IdleSound);
                    ifIdle(IdleSound);
                }

            }

        }

           
            
    }
    private void ifIdle( AudioClip sound)
    {
        //checks whether to put loop sound or not.
        if(carSound.isPlaying == sound)
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
        ifIdle(IdleSound);
        if (CarControlling.isAndroid)
        {
            //If Android

            if (SimpleInput.GetAxis("Vertical") > 0)
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
            else if (Input.GetAxis("Vertical") > 0) //Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
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
        else
        {
            //put HEHECODE here. (it is in WindowsCodeForControlling method).
            //and remove other code or comment which is in this ELSE brackets.

            //if Windows
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (stickDownLast == false)
                {
                    // Call your event function here.
                    if (carSound.isPlaying)
                    {
                        carSound.Stop();
                        PlaySound(accelarateSound, true);

                    }
                    //Stops coroutine if key isn't pressed (last coroutine)
                    if (_coroutineMethod != null)
                        StopCoroutine(_coroutineMethod);
                    _coroutineMethod = StartCoroutine(NextSound(accelarateSound.length, accelarateSound, maxRpm));

                    stickDownLast = true;
                }
            }
            else
            {
                stickDownLast = false;
            }
        }
        
        
    }

    void WindowsCodeForControlling()
    {

        //trick to convert getaxis to getkeydown ( not working as expected )
        if (Input.GetAxis("Vertical") < 0)
        {
            if (!stickDownLast)
            {
                if (carSound.isPlaying)
                {
                    carSound.Stop();
                    PlaySound(androidAcceSound, true);

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
            stickDownLast = false;
        }
            

        //HEHECODE
        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))  //  //|| )SimpleInput.GetAxis("Vertical") == 1

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
        if(carSound.isPlaying == checkClip) // checking which clip is playing and work according
            PlaySound(playClip);
    }
    private void DelaySound(AudioClip audioClip)
    {
        ifIdle(IdleSound);
       
            carSound.clip = audioClip;
            carSound.PlayDelayed(3f);
        
    }
            
}
