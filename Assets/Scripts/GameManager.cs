using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player P1;
    public Player P2;

    public GameState State = GameState.ChooseAttack;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;
    public AudioSource audioSource;
    public AudioClip startClip;
    public AudioClip deathClip;

    private Player damagedPlayer;
    private Player winner;
    public enum GameState
    {
        ChooseAttack, //memilih attack
        Attacks, //antar player attack
        Damages, //damage yang diberikan
        Draw, //apabila seri
        GameOver, //game selesai
    }

    private void Start()
    {
        audioSource.PlayOneShot(startClip);
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.ChooseAttack:
                if (P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.isClickable(false);
                    P2.isClickable(false);
                    State = GameState.Attacks;
                }
                break;


            case GameState.Attacks:
                if (P1.isAnimating() == false && P2.isAnimating() == false)
                {
                    damagedPlayer = GetDamagedPlayer();
                    if (damagedPlayer != null)
                    {
                        damagedPlayer.AnimateDamage();
                        State = GameState.Damages;
                    }
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        State = GameState.Draw;
                    }
                }
                break;


            case GameState.Damages:
                if (P1.isAnimating() == false && P2.isAnimating() == false)
                {
                    //kalkulasi health
                    if (damagedPlayer == P1)
                    {
                        P1.ChangeHealth(-25);
                        P2.ChangeHealth(10);
                    }
                    else
                    {
                        P1.ChangeHealth(10);
                        P2.ChangeHealth(-25);
                    }

                    var winner = GetWinner();

                    if (winner == null)
                    {
                        ResetPlayers();
                        P1.isClickable(true);
                        P2.isClickable(true);
                        State = GameState.ChooseAttack;

                    }
                    else
                    {
                        audioSource.PlayOneShot(deathClip);
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? "Player 1 Wins!" : "Player 2 Wins!";
                        ResetPlayers();
                        State = GameState.GameOver;
                    }
                }
                break;

            case GameState.Draw:
                if (P1.isAnimating() == false && P2.isAnimating() == false)
                {
                    ResetPlayers();
                    P1.isClickable(true);
                    P2.isClickable(true);
                    State = GameState.ChooseAttack;
                }
                break;
        }
    }

    private void ResetPlayers()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }

    private Player GetDamagedPlayer()
    {
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if (PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissor)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissor)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Rock)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Paper)
        {
            return P2;
        }

        return null;
    }

    private Player GetWinner()
    {
        if (P1.Health == 0)
        {
            return P2;
        }
        else if (P2.Health == 0)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }


}

