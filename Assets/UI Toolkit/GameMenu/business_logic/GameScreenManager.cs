using UnityEngine;
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
    }

    private void InitCallBack()
    {
        GameManager.Instance.OnDistanceChange += OnDistanceChange;
        GameManager.Instance.OnScoreChange += OnScoreChange;
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

    private void OnScoreChange(GameManager arg1, bool arg2, int arg3)
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
