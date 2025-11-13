using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour
{
    [Header("Conecciones")]
    [SerializeField] private AudioMixer audioMixer;

    public void FullScreenSize( bool fullScreenSize)
    {
        Screen.fullScreen = fullScreenSize;
    }


    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

}
