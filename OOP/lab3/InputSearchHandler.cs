using System;
using System.Windows.Forms;

public class InputSearchHandler
{
    private string _state;
    private string _previousState;
    private string _nextState;

    public InputSearchHandler(string state)
    {
        _state = state;
        _previousState = string.Empty;
        _nextState = string.Empty;
    }
    public InputSearchHandler()
    {
        _state = "Ввведите поисковый запрос";
        _previousState = string.Empty;
        _nextState = string.Empty;
    }

    public void ClearState()
    {
        _previousState = _state;
        _state = string.Empty;
    }

    public void RestorePreviousState()
    {
        _nextState = _state;
        _state = _previousState;
    }

    public void RestoreNextState()
    {
        _previousState = _state;
        _state = _nextState;
    }

    public void RemoveLastCharacter()
    {
        if (!string.IsNullOrEmpty(_state))
        {
            _previousState = _state;
            _state = _state.Substring(0, _state.Length - 1);
        }
    }

    public void HandleTextChanged(object sender, EventArgs e)
    {
        if (sender is TextBox searchTextBox)
        {
            _previousState = _state;
            _state = searchTextBox.Text;
        }
    }

    public string CurrentState => _state;
    public string PreviousState => _previousState;
    public string NextState => _nextState;
}