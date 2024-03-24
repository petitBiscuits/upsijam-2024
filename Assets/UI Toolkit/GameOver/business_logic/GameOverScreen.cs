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
        _restartButton = _root.Q<Button>("restart");
        _quitButton = _root.Q<Button>("quit");

        _restartButton.RegisterCallback<ClickEvent>(ev => Test());
        _quitButton.RegisterCallback<ClickEvent>(ev => Application.Quit());
    }

    void Test()
    {
        Destroy(GameManager.Instance);
        SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
