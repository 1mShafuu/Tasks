using UnityEngine.Events;

public class EndScreen : Screen
{
    public event UnityAction RestartButtonClicked;

    protected override void OnRestartButtonClicked()
    {
        RestartButtonClicked?.Invoke();
    }

    public override void Open()
    {
        CanvasGroup.alpha = 1;
        RestartButton.interactable = true;
    }

    public override void Close()
    {
        CanvasGroup.alpha = 0;
        RestartButton.interactable = false;
    }
}
