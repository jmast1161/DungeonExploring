using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    WaitingGameplayInput,
    MenuActive,
    GameOver,
    Win
}

public class GameManager : MonoBehaviour
{
    private GameState gameState;
    [SerializeField]
    private Player player;

    [SerializeField]
    private AudioSource backgroundMusic;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private AudioSource deathAudioSource;

    [SerializeField]
    private AudioSource winAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundMusic.Play();
        playButton.onClick.AddListener(OnPlayButtonClicked);
        player.SpikeHit += OnSpikeHit;
        player.DoorHit += OnDoorHit;
        InitializeGameState();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingGameplayInput:
                player.MovePlayer();
                break;
            case GameState.GameOver:
            case GameState.Win:
                InitializeGameState();
                break;
            case GameState.MenuActive:
                break;
        }
    }

    private void InitializeGameState()
    {
        gameState = GameState.MenuActive;
        player.Initialize();
        playButton.gameObject.SetActive(true);
    }

    private void OnPlayButtonClicked()
    {
        gameState = GameState.WaitingGameplayInput;
        playButton.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
    }

    private void OnSpikeHit(Player player)
    {
        deathAudioSource.Play();
        gameState = GameState.GameOver;
    }

     private void OnDoorHit(Player player)
    {
        winAudioSource.Play();
        gameState = GameState.Win;
    }
}
