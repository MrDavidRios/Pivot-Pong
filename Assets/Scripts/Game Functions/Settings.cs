using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private VolumeSlider volumeSlider;

    //Booleans (Settings)
    public bool chargingEnabled;
    public bool cameraShakeEnabled;
    public bool autoReServe;
    public bool ballTail;

    //Floats (Settings)
    public float volume;

    /*
     * Indexes
     * 
     * Charging - 0
     * Camera Shake - 1
     * Automatic Re-serve - 2
     * Ball Trail - 3
     * 
     */

    public bool[] settings;
    public string[] loadKeys;

    //GetComponent<PaddleControls>().chargingEnabled = chargingEnabled;

    public void SaveSettings()
    {
        //Settings to save: Paddle Charging, Camera Shake, Automatic Re-Serve, Ball Tail

        //If the variable is true, the int will be 1. If the variable is not true, the int will be 0.
        PlayerPrefs.SetInt("Charging", chargingEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Camera Shake", cameraShakeEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Automatic Re-serve", autoReServe ? 1 : 0);
        PlayerPrefs.SetInt("Ball Tail", ballTail ? 1 : 0);

        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void LoadSettings()
    {
        //First check if the needed keys exist. If not, then there's nothing to load!
        //This for loop checks if there are any missing keys. If so, variables are set to default, and the function is exited from.
        for (int i = 0; i < loadKeys.Length; i++)
        {
            if (!PlayerPrefs.HasKey(loadKeys[i]))
            {
                ClearSettings();
                return;
            }
        }

        //If the variable is true, the int will be 1. If the variable is not true, the int will be 0.
        chargingEnabled = PlayerPrefs.GetInt("Charging") == 1;
        cameraShakeEnabled = PlayerPrefs.GetInt("Camera Shake") == 1;
        autoReServe = PlayerPrefs.GetInt("Automatic Re-serve") == 1;
        ballTail = PlayerPrefs.GetInt("Ball Tail") == 1;

        volume = PlayerPrefs.GetFloat("Volume");
        FindObjectOfType<VolumeSlider>().LoadExistingVolume(volume);

        UpdateSettingsArray();
    }

    public void ClearSettings()
    {
        chargingEnabled = true;
        cameraShakeEnabled = true;
        autoReServe = true;
        ballTail = true;

        PlayerPrefs.SetInt("Charging", chargingEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Camera Shake", cameraShakeEnabled ? 1 : 0);
        PlayerPrefs.SetInt("Automatic Re-serve", autoReServe ? 1 : 0);
        PlayerPrefs.SetInt("Ball Tail", ballTail ? 1 : 0);

        volume = 1.0f;

        PlayerPrefs.SetFloat("Volume", 1.0f);
        FindObjectOfType<VolumeSlider>().LoadExistingVolume(volume);

        UpdateSettingsArray();
    }

    //Settings to enable and disable
    public void EnableCharging(bool enable)
    {
        //Enable variable in script
        chargingEnabled = enable;

        //Have newly set variable change desired game function
        //GetComponent<PaddleControls>().chargingEnabled = chargingEnabled;

        UpdateSettingsArray();
    }

    public void EnableCameraShake(bool enable)
    {
        cameraShakeEnabled = enable;

        UpdateSettingsArray();
    }

    public void EnableAutoReServe(bool enable)
    {
        autoReServe = enable;

        UpdateSettingsArray();
    }

    public void EnableBallTail(bool enable)
    {
        ballTail = enable;

        UpdateSettingsArray();
    }

    public void UpdateVolumeValue()
    {
        if (volumeSlider == null)
            return;

        volume = volumeSlider.volume;

        UpdateSettingsArray();
    }

    private void UpdateSettingsArray()
    {
        settings = new bool[] { chargingEnabled, cameraShakeEnabled, autoReServe, ballTail };
    }

    private void Awake()
    {
        volumeSlider = FindObjectOfType<VolumeSlider>();
    }

    private void Update()
    {
        UpdateVolumeValue();
    }
}
