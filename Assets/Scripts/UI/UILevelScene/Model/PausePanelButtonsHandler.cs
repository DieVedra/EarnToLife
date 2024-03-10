using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PausePanelButtonsHandler
{
    private Button _buttonClose;
    private Button _buttonRestart;
    private Button _buttonGarage;
    private Button _buttonMusic;
    private Button _buttonSound;
    
    public Button ButtonClose => _buttonClose;
    public Button ButtonRestart => _buttonRestart;
    public Button ButtonGarage => _buttonGarage;
    public Button ButtonMusic => _buttonMusic;
    public Button ButtonSound => _buttonSound;
    public PausePanelButtonsHandler(PanelPause panelPause)
    {
        _buttonClose = panelPause.ButtonClose;
        _buttonRestart = panelPause.ButtonRestart;
        _buttonGarage = panelPause.ButtonGarage;
        _buttonMusic = panelPause.ButtonMusic;
        _buttonSound = panelPause.ButtonSound;

    }
    public void Unsubscribe()
    {
        _buttonClose.onClick.RemoveAllListeners();
        _buttonRestart.onClick.RemoveAllListeners();
        _buttonGarage.onClick.RemoveAllListeners();
        _buttonMusic.onClick.RemoveAllListeners();
        _buttonSound.onClick.RemoveAllListeners();
    }
}
