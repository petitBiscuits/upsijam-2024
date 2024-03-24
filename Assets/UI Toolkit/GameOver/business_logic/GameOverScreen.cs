using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverScreen : MonoBehaviour
{
    public UIDocument _uiDocument;
    private VisualElement _root;
    
    private Label _scoreLabel;
    private Button _restartButton;
    private Button _quitButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _root = _uiDocument.rootVisualElement;
        
        InitWidgets();
    }
    
    private void InitWidgets()
    {
        _restartButton = _root.Q<Button>("restartButton");
        _quitButton = _root.Q<Button>("quitButton");

        _restartButton.RegisterCallback<ClickEvent>(ev => SceneManager.LoadScene("MainScene"));
        _quitButton.RegisterCallback<ClickEvent>(ev => Application.Quit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
