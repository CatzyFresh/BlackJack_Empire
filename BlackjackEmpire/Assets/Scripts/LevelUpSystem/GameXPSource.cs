using UnityEngine;

public class GameXPSource : MonoBehaviour, IXPSource
{
    public enum GameResult
    {
        Win,
        Loss,
        Tie,
        BlackJack
    }

    private GameResult _result;

    public void SetGameResult(GameResult result)
    {
        _result = result;
    }

    public int GetXP()
    {
        switch (_result)
        {
            case GameResult.BlackJack:
                return 100;
            case GameResult.Win:
                return 50;
            case GameResult.Loss:
                return 10;
            case GameResult.Tie:
                return 25;
            default:
                return 0;
        }
    }

    public string GetSourceName()
    {
        return "Game";
    }
}
