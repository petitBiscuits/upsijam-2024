public enum MultiOperation
{
    Increase,
    Decrease
}

public class Multi
{
    private int _multi = 1;
    
    public int Value
    {
        get => _multi;
    }
    
    public void UpdateMulti(MultiOperation mo)
    {
        switch (mo)
        {
            case MultiOperation.Increase:
                if (_multi*2 > SettingsManager.Instance.MAX_MULTI) return;
                _multi*=2;
                break;
            case MultiOperation.Decrease:
                if (_multi == 1) return;
                _multi/=2;
                break;
        }
    }
}