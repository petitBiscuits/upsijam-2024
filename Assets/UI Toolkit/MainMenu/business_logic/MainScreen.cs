using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public UIDocument UIDocument;
    
    private VisualElement _root;
    
    private Button playButton;
    private Button quitButton;
    
    // Start is called before the first frame update
    void Start()
    {
        _root = UIDocument.rootVisualElement;
        
        InitWidgets();
    }
    
    private void InitWidgets()
    {
        var playButton = _root.Q<Button>("play");
        playButton.clicked += OnPlayButtonClicked;
        
        var quitButton = _root.Q<Button>("quit");
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
}
