using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public float volume;

    public Image volumeStatus;

    public Sprite[] volumeStatusSprites;

    private void Update()
    {
        volume = GetComponent<Slider>().value;

        if (volume > 0.5f)
            volumeStatus.sprite = volumeStatusSprites[0];
        else if (volume < 0.5 && volume > 0f)
            volumeStatus.sprite = volumeStatusSprites[1];
        else
            volumeStatus.sprite = volumeStatusSprites[2];
    }

    public void LoadExistingVolume(float _volume)
    {
        volume = _volume;

        GetComponent<Slider>().value = _volume;
    }
}