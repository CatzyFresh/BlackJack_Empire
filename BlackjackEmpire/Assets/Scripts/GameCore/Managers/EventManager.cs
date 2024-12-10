using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action OnPlayerWin;
    public static Action OnPlayerLose;

    public static void PlayerWin() => OnPlayerWin?.Invoke();
    public static void PlayerLose() => OnPlayerLose?.Invoke();
}
