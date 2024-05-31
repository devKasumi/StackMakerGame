using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    abstract void OnEnter();
    abstract void OnExcute();
    abstract void OnExit(); 

}
