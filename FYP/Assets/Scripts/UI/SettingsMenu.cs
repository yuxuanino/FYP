using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//All PlayerPref
// mVolumePref          (Master Volume)         float
// sfxVolumePref        (SFX Volume)            float   
// mouseSensPref        (Mouse sensitivity)     float
// resolutionPref       (Resolution I)          int
// resolutionWidthPref  (Resolution width)      int
// resolutionHeightPref (Resolution height)     int
// fullscreenPref       (Fullscreen)            int     1 = True

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    Resolution[] resolutions;

    int resolutionI;
    Resolution resolution;

    public GameObject[] resolutionSelection;
    public GameObject[] fullScreenNoYesNextBack;

    private int isFullScreen = 1;

    //SetSliders to Player's pref
    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider mouseSensitivitySlider;
    
    bool resolutionChanged;
    bool fullScreenChanged;
    bool masterChanged;
    bool sfxChanged;
    bool mouseChanged;

    private void Awake()
    {
        resolutionI = PlayerPrefs.GetInt("resolutionPref", 7);

        resolution.width = PlayerPrefs.GetInt("resolutionWidthPref", 1920);
        resolution.height = PlayerPrefs.GetInt("resolutionHeightPref", 1080);
        isFullScreen = PlayerPrefs.GetInt("fullscreenPref", 1);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        fullScreenNoYesNextBack[isFullScreen].SetActive(true);
        if (isFullScreen == 1)
        {
            Screen.fullScreen = true;
        }
        else if (isFullScreen == 0)
        {
            Screen.fullScreen = false;
        }

        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensPref", .5f);
        //masterVolumeSlider.value = PlayerPrefs.GetFloat("mVolumePref", 1);
        audioMixer.SetFloat("mVolume", PlayerPrefs.GetFloat("mVolumePref", 1));
        //sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolumePref", 1);
        audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("mVolumePref", 1));

    }

    private void Start()
    {
        //Get current slider value from existing PlayerPref.
        masterVolumeSlider.value = PlayerPrefs.GetFloat("mVolumePref",1);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolumePref",1);
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat("mouseSensPref",.5f);

        resolutionI = PlayerPrefs.GetInt("resolutionPref", 7);
        resolutionSelection[resolutionI].SetActive(true);
        resolution.width = PlayerPrefs.GetInt("resolutionWidthPref", 1920);
        resolution.height = PlayerPrefs.GetInt("resolutionHeightPref", 1080);
        Debug.Log("Width = " + resolution.width + " Height = " + resolution.height);
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);


        isFullScreen = PlayerPrefs.GetInt("fullscreenPref", 1);
        fullScreenNoYesNextBack[isFullScreen].SetActive(true);
        if(isFullScreen == 1)
        {
            Screen.fullScreen = true;
            fullScreenNoYesNextBack[2].SetActive(false);
            fullScreenNoYesNextBack[3].SetActive(true);
        }else if(isFullScreen == 0)
        {
            Screen.fullScreen = false;
            fullScreenNoYesNextBack[2].SetActive(true);
            fullScreenNoYesNextBack[3].SetActive(false);
        }
    }

    public void MouseSensitivity(float mouseSensitivity) 
    {
        mouseChanged = true;
        Debug.Log("sensitivity = " + mouseSensitivity);
    }


    public void SetMVolume (float mVolume)
    {
        masterChanged = true;
        Debug.Log("mVolume = " + mVolume);
        audioMixer.SetFloat("mVolume", Mathf.Log10(mVolume) * 20);
       
    }

    public void SetSFXVolume(float sfxVolume)
    {
        sfxChanged = true;
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
        resolutionChanged = true;
        if (resolutionI <= 7) {
            resolutionI++;
        }
        Debug.Log("RI = " + resolutionI);
        Resolutions();
    }
    public void DecreaseResolution()
    {
        resolutionChanged = true;
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
        if (fullScreenChanged)
        {
            //Apply fullscreen

            if(isFullScreen == 1)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;

            }
            PlayerPrefs.SetInt("fullscreenPref", isFullScreen);
            fullScreenChanged = false;
        }
        //Apply settings to PlayerPref.
        if (masterChanged)
        {
            PlayerPrefs.SetFloat("mVolumePref", masterVolumeSlider.value);
            masterChanged = false;
        }
        if (sfxChanged)
        {
            PlayerPrefs.SetFloat("sfxVolumePref", sfxVolumeSlider.value);
            sfxChanged = false;
        }
        if (mouseChanged)
        {
            PlayerPrefs.SetFloat("mouseSensPref", mouseSensitivitySlider.value);
            mouseChanged = false;
        }
        if (resolutionChanged)
        {
            PlayerPrefs.SetInt("resolutionPref", resolutionI);
            PlayerPrefs.SetInt("resolutionWidthPref", resolution.width);
            PlayerPrefs.SetInt("resolutionHeightPref", resolution.height);
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            Debug.Log("Width = " + resolution.width + ", Height = " + resolution.height);
            resolutionChanged = false;
        }
    }

    public void ResetToDefault()
    {
        PlayerPrefs.SetFloat("mVolumePref", 1);
        PlayerPrefs.SetFloat("sfxVolumePref", 1);
        PlayerPrefs.SetFloat("mouseSensPref", 0.5f);

        PlayerPrefs.SetInt("resolutionPref", 7);
        resolution.width = 1920;
        resolution.height = 1080;

        isFullScreen = 1;

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
        isFullScreen = 0;
        fullScreenNoYesNextBack[1].SetActive(false);
        fullScreenNoYesNextBack[0].SetActive(true);

        fullScreenNoYesNextBack[2].SetActive(true);
        fullScreenNoYesNextBack[3].SetActive(false);
        fullScreenChanged = true;
    }

    public void OnFullscreen()
    {
        isFullScreen = 1;
        fullScreenNoYesNextBack[1].SetActive(true);
        fullScreenNoYesNextBack[0].SetActive(false);

        fullScreenNoYesNextBack[2].SetActive(false);
        fullScreenNoYesNextBack[3].SetActive(true);
        fullScreenChanged = true;
    }

    public void DeletePref()
    {
        PlayerPrefs.DeleteAll();
    }
}
