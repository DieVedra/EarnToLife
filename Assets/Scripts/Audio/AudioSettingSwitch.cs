using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioSettingSwitch
{
    private const string TEXT_ON = "On";
     private const string TEXT_OFF = "Off"; 
     private TextMeshProUGUI _textButtonMusic;
     private TextMeshProUGUI _textButtonSound;
     private IAudioSettingSwitch _globalSwitch;

     private bool _musicOn => _globalSwitch.MusicOn;
     private bool _soundOn => _globalSwitch.SoundOn;
     
     public AudioSettingSwitch(IAudioSettingSwitch globalSwitch, TextMeshProUGUI textButtonMusic, TextMeshProUGUI textButtonSound)
     {
         _textButtonMusic = textButtonMusic;
         _textButtonSound = textButtonSound;
         _globalSwitch = globalSwitch;
         Init();
     }
     private void Init()
     {
         if (_musicOn == true)
         {
             SetOn(_textButtonMusic);
         }
         else
         {
             SetOff(_textButtonMusic);
         }
         if (_soundOn == true)
         {
             SetOn(_textButtonSound);
         }
         else
         {
             SetOff(_textButtonSound);
         }
     }
     public void SoundSwitch()
     {
         if (_soundOn == true)
         {
             SetOff(_textButtonSound);
             _globalSwitch.SetSoundsOff();
         }
         else
         {
             SetOn(_textButtonSound);
             _globalSwitch.SetSoundsOn();
         }
     }
     public void MusicSwitch()
     {
         if (_musicOn == true)
         {
             SetOff(_textButtonMusic);
             _globalSwitch.SetMusicOff();
         }
         else
         {
             SetOn(_textButtonMusic);
             _globalSwitch.SetMusicOn();
         }
     }
     private void SetOff(TextMeshProUGUI text)
     {
         text.text = TEXT_OFF;
     }
     private void SetOn(TextMeshProUGUI text)
     {
         text.text = TEXT_ON;
     }
}
