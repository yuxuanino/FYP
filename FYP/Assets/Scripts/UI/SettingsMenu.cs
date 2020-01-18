using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    int resolutionI;
    Resolution resolution;

    public GameObject[] resolutionSelection;
    public GameObject[] fullScreenYesNoNextBack;

    private bool isFullScreen;

    //SetSliders to Player's pref
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider mouseSensitivitySlider;
    

    private void Start()
    {
        //Get current slider value from existing PlayerPref.
        masterVolumeSlider.value = PlayerPrefs.GetFloat("mVolumePref",1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolumePref",1);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensPref",.5f);
        resolutionI = PlayerPrefs.GetInt("resolutionPref", 7);

    }

    public void MouseSensitivity(float mouseSensitivity) 
    {
        Debug.Log("sensitivity = " + mouseSensitivity);
    }


    public void SetMVolume (float mVolume)
    {
        Debug.Log("mVolume = " + mVolume);
        audioMixer.SetFloat("mVolume", Mathf.Log10(mVolume) * 20);
    }

    public void SetSFXVolume(float sfxVolume)
    {
        Debug.Log("sfxVolume = " + sfxVolume);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxVolume) * 20);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Setting screen resolution
    public void IncreaseResolution()
    {
        if (resolutionI <= 7) {
            resolutionI++;
        }
        Debug.Log("RI = " + resolutionI);
        Resolutions();
    }
    public void DecreaseResolution()
    {
        if (resolutionI >= 1)
        {
            resolutionI--;
        }
        Debug.Log("RI = " + resolutionI);
        Resolutions();
    }

    public void Apply()
    {
        
        //Apply resolution
        //resolution = resolutions[resolutionI];        
        Screen.SetResolution(resolution.width, resolution.height , Screen.fullScreen );
        Debug.Log("Width = " + resolution.width + ", Height = " + resolution.height);

        //Apply fullscreen
        Screen.fullScreen = isFullScreen;

        //Apply settings to PlayerPref.
        PlayerPrefs.SetFloat("mVolumePref", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("sfxVolumePref", sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("mouseSensPref", mouseSensitivitySlider.value);

        PlayerPrefs.SetInt("resolutionPref", resolutionI);
    }

    public void ResetToDefault()
    {
        PlayerPrefs.SetFloat("mVolumePref", 1);
        PlayerPrefs.SetFloat("sfxVolumePref", 1);
        PlayerPrefs.SetFloat("mouseSensPref", 0.5f);

        PlayerPrefs.SetInt("resolutionPref", 7);

    }
    void Resolutions()
    {
        //Set image of resolutions off to all, except the chosen resolution.
        for(int i = 0; i < resolutionSelection.Length; i++)
        {
            if(i != resolutionI )
            {
                 resolutionSelection[i].SetActive(false);
            }
            if ( i == resolutionI)
            {
                resolutionSelection[i].SetActive(true);
            }
        }

        switch (resolutionI)
        {
            case 0:
                resolution.width = 640; resolution.height = 480;
                break;

            case 1:
                resolution.width = 800; resolution.height = 600;
                break;

            case 2:
                resolution.width = 960; resolution.height = 720;
                break;

            case 3:
                resolution.width = 1024; resolution.height = 768;
                break;

            case 4:
                resolution.width = 1280; resolution.height = 960;
                break;

            case 5:
                resolution.width = 1440; resolution.height = 1080;
                break;

            case 6:
                resolution.width = 1600; resolution.height = 1200;
                break;

            case 7:
                resolution.width = 1920; resolution.height = 1080;
                break;

            case 8:
                resolution.width = 1920; resolution.height = 1440;
                break;
        }
    }

    public void OffFullscreen()
    {
        isFullScreen = false;
        fullScreenYesNoNextBack[0].SetActive(false);
        fullScreenYesNoNextBack[1].SetActive(true);

        fullScreenYesNoNextBack[2].SetActive(true);
        fullScreenYesNoNextBack[3].SetActive(false);
    }

    public void OnFullscreen()
    {
        isFullScreen = true;
        fullScreenYesNoNextBack[0].SetActive(true);
        fullScreenYesNoNextBack[1].SetActive(false);

        fullScreenYesNoNextBack[2].SetActive(false);
        fullScreenYesNoNextBack[3].SetActive(true);
    }
}
