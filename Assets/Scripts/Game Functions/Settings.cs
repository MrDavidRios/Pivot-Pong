using UnityEngine;

public class Settings : MonoBehaviour
{
    private VolumeSlider volumeSlider;

    //Booleans (Settings)
    public bool cameraShakeEnabled;
    public bool autoReServe;
    public bool ballTail;

    public bool adaptiveColor;

    //Floats (Settings)
    public float volume;

    /*
     * Indexes
     * 
     * Adaptive Color - 0
     * Camera Shake - 1
     * Automatic Re-serve - 2
     * Ball Trail - 3
     * 
     */

    public bool[] settings;
    public string[] loadKeys;

    public void SaveSettings()
    {
        //Settings to save: Adaptive Color, Camera Shake, Automatic Re-Serve, Ball Tail

        //If the variable is true, the int will be 1. If the variable is not true, the int will be 0.
        PlayerPrefs.SetInt("Adaptive Color", adaptiveColor ? 1 : 0);
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
        adaptiveColor = PlayerPrefs.GetInt("Adaptive Color") == 1;
        cameraShakeEnabled = PlayerPrefs.GetInt("Camera Shake") == 1;
        autoReServe = PlayerPrefs.GetInt("Automatic Re-serve") == 1;
        ballTail = PlayerPrefs.GetInt("Ball Tail") == 1;

        volume = PlayerPrefs.GetFloat("Volume");
        FindObjectOfType<VolumeSlider>().LoadExistingVolume(volume);

        UpdateSettingsArray();
    }

    public void ClearSettings()
    {
        adaptiveColor = true;
        cameraShakeEnabled = true;
        autoReServe = true;
        ballTail = true;

        volume = 1.0f;

        SaveSettings();

        FindObjectOfType<VolumeSlider>().LoadExistingVolume(volume);

        UpdateSettingsArray();
    }

    //Settings to enable and disable
    public void EnableAdaptiveColor(bool enable)
    {
        adaptiveColor = enable;

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
        settings = new bool[] { adaptiveColor, cameraShakeEnabled, autoReServe, ballTail };
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
