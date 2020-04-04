using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEventListener : MonoBehaviour 
{
    #region Inspector Variables
    //[Header("References")]
    //[SerializeField]
    protected GameplayController gameplay;
    #endregion

    #region MonoBehaviour Functions
    protected virtual void Awake()
    {
        if (this.gameplay == null)
        {
            GameObject gameplayGameObject = GameObject.Find("Gameplay");

            if (gameplayGameObject != null)
            {
                this.gameplay = gameplayGameObject.GetComponent<GameplayController>();
            }
        }
    }

    protected virtual void Start() 
    {
        if (this.gameplay != null)
        {
            this.gameplay.OnGameStart += OnGameStart;
            this.gameplay.OnGamePause += OnGamePause;
            this.gameplay.OnGameResume += OnGameResume;
            this.gameplay.OnGameWon += OnGameWon;
            this.gameplay.OnGameLose += OnGameLose;
            this.gameplay.OnGameEnd += OnGameEnd;
            this.gameplay.OnGameRestart += OnGameRestart;
            this.gameplay.OnGameQuit += OnGameQuit;
        }

        this.enabled = false;
    }

    protected virtual void OnDestroy()
    {
        if (this.gameplay != null)
        {
            this.gameplay.OnGameStart -= OnGameStart;
            this.gameplay.OnGamePause -= OnGamePause;
            this.gameplay.OnGameResume -= OnGameResume;
            this.gameplay.OnGameWon -= OnGameWon;
            this.gameplay.OnGameLose -= OnGameLose;
            this.gameplay.OnGameEnd -= OnGameEnd;
            this.gameplay.OnGameRestart -= OnGameRestart;
            this.gameplay.OnGameQuit -= OnGameQuit;
        }
    }
    #endregion

    #region Listeners
    protected virtual void OnGameStart()
    {
        this.enabled = true;
    }

    protected virtual void OnGamePause()
    {
        this.enabled = false;
    }

    protected virtual void OnGameResume()
    {
        this.enabled = true;
    }

    protected virtual void OnGameWon()
    {
    }

    protected virtual void OnGameLose()
    {
    }

    protected virtual void OnGameEnd()
    {
        this.enabled = false;
    }

    protected virtual void OnGameRestart()
    {
    }

    protected virtual void OnGameQuit()
    {
        this.enabled = false;
    }
    #endregion
}
