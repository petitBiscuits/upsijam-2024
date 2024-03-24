using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScreenManager : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    private VisualElement _root;
    
    private Label _scoreLabel;
    private Label _multi;
    
    private VisualElement _progression;
    private VisualElement _distance;

    private VisualElement _life;
    
    private VisualElement _pause;
    private VisualElement _resumeButton;
    private VisualElement _restartButton;
    private VisualElement _rageQuit;
    
    // Start is called before the first frame update
    void Start()
    {
        _root = _uiDocument.rootVisualElement;
        
        InitWidgets();
        
        InitCallBack();
    }

    private void InitWidgets()
    {
        _distance = _root.Q<VisualElement>("distance");
        _progression = _root.Q<VisualElement>("progression");
        
        _scoreLabel = _root.Q<Label>("score");
        
        _pause = _root.Q<VisualElement>("pause");
        _resumeButton = _pause.Q<VisualElement>("resumeButton");
        _restartButton = _pause.Q<VisualElement>("restartButton");
        _rageQuit = _pause.Q<VisualElement>("rageQuit");
        
        _resumeButton.RegisterCallback<ClickEvent>(ev => GameManager.Instance.GameState = GameState.LevelStage);
        _restartButton.RegisterCallback<ClickEvent>(ev => SceneManager.LoadScene("MainScene"));
        _rageQuit.RegisterCallback<ClickEvent>(ev => Application.Quit());
    }

    private void InitCallBack()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnDistanceChange += OnDistanceChange;
        GameManager.Instance.OnScoreChange += OnScoreChange;
        GameManager.Instance.OnPause += OnPause;
    }

    private void OnPause(bool arg)
    {
        _pause.style.display = arg ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void OnDistanceChange(float distance)
    {
        if (distance > SettingsManager.Instance.DISTANCE_MAX)
        {
            distance = SettingsManager.Instance.DISTANCE_MAX;
        }
        _distance.style.width = Length.Percent(distance / SettingsManager.Instance.DISTANCE_MAX * 100);
        _progression.style.left = Length.Percent((distance / SettingsManager.Instance.DISTANCE_MAX * 100)-2.5f);
    }

    private void OnScoreChange(bool arg2, int arg3)
    {
        if (arg2)
        {
            _scoreLabel.AddToClassList("biggerScore");
        }else
        {
            _scoreLabel.RemoveFromClassList("biggerScore");
        }
        _scoreLabel.text = arg3.ToString();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
