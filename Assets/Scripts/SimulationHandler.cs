﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationHandler : MonoBehaviour {
    private int score;
    public GameObject point, panel, exampleBall, examplePlayer, indexFinger, exampleYellowCard0, exampleYellowCard1, gameoverPanel;
    public Text scoreLabel, gameOverText, instructionSet0, instructionSet1;
    public Image hit_icon0, hit_icon1; //images passed through inspector
    private bool gameOver;
    private float timer;

    // Use this for initialization
    void Start () {
        score = 0;
        gameOver = false;
        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (panel.transform.localScale.x < 0.65)
        {
            panel.transform.localScale = new Vector3((float)(panel.transform.localScale.x + 0.005), (float)(panel.transform.localScale.y + 0.005));
        }
        else
        {
            Simulation();
        }
    }

    private void Simulation()
    {
        SpriteRenderer sr, sr0, sr1;

        if (gameOver)
        {
            if(gameoverPanel.GetComponent<Image>().color.a < 0.5)
            {
                fadeIn(gameoverPanel.GetComponent<Image>());
            }
            fadeIn(gameOverText);
        }

        //instruction set 0 start
        if (exampleBall != null)
        {
            sr = exampleBall.GetComponent<SpriteRenderer>();
            sr0 = examplePlayer.GetComponent<SpriteRenderer>();
            sr1 = indexFinger.GetComponent<SpriteRenderer>();
            if (sr.color.a < 1 || sr0.color.a < 1 || sr1.color.a < 1)
            {
                fadeIn(sr);
                fadeIn(sr0);
                fadeIn(sr1);
                fadeIn(scoreLabel);
                fadeIn(instructionSet0);
            }

            if (examplePlayer.transform.position.y < exampleBall.transform.position.y)
            {
                examplePlayer.transform.position = new Vector3(examplePlayer.transform.position.x, examplePlayer.transform.position.y + 0.02f);
            }

            exampleBall.transform.position = new Vector3(exampleBall.transform.position.x - 0.035f, exampleBall.transform.position.y);
        }

        //instruction set 1 start
        if (exampleBall == null)
        {
            if(exampleYellowCard1 != null)
            {
                timer += Time.deltaTime;

                if (timer % 60 >= 1.5) //mod 60 to get seconds
                {
                    fadeOut(instructionSet0);
                    fadeIn(instructionSet1);
                    GameObject obj = instructionSet1.transform.GetChild(0).gameObject;
                    Image img = obj.GetComponent<Image>();
                    fadeIn(img);

                    if (exampleYellowCard0 != null)
                    {
                        sr = exampleYellowCard0.GetComponent<SpriteRenderer>();
                        fadeIn(sr);
                        exampleYellowCard0.transform.position = new Vector3(exampleYellowCard0.transform.position.x - 0.035f, exampleYellowCard0.transform.position.y);
                    }

                    if (exampleYellowCard1 != null && exampleYellowCard0 == null)
                    {
                        sr = exampleYellowCard1.GetComponent<SpriteRenderer>();
                        fadeIn(sr);

                        exampleYellowCard1.transform.position = new Vector3(exampleYellowCard1.transform.position.x - 0.035f, exampleYellowCard1.transform.position.y);
                        if (examplePlayer.transform.position.y > exampleYellowCard1.transform.position.y)
                        {
                            examplePlayer.transform.position = new Vector3(examplePlayer.transform.position.x, examplePlayer.transform.position.y - 0.025f);
                        }
                    }
                }
            }       
        }
        
        //insturction set 2 start
        if(exampleYellowCard1 == null)
        {
            timer += Time.deltaTime;

            if(timer%60 >= 1.5)
            {
                fadeIn(indexFinger.GetComponent<SpriteRenderer>());
                fadeOut(instructionSet1);
                fadeOut(gameoverPanel.GetComponent<Image>());
                fadeOut(gameOverText);
                GameObject obj = instructionSet1.transform.GetChild(0).gameObject;
                Image img = obj.GetComponent<Image>();
                fadeOut(img);
                fadeOut(hit_icon0);
                fadeOut(hit_icon1);

                examplePlayer.GetComponent<Rigidbody2D>().gravityScale = 0;
                if (examplePlayer.transform.position.y < point.transform.position.y)
                {
                    examplePlayer.transform.position = new Vector3(examplePlayer.transform.position.x, examplePlayer.transform.position.y + 0.05f);
                }
            }
        }
    }

    private void fadeIn(SpriteRenderer sr)
    {
        Color c = sr.color;
        c.a += 0.025f;
        sr.color = c;
    }

    private void fadeIn(Text text)
    {
        Color c = text.color;
        c.a += 0.025f;
        text.color = c;
    }

    private void fadeIn(Image image)
    {
        Color c = image.color;
        c.a += 0.025f;
        image.color = c;
    }

    private void fadeOut(SpriteRenderer sr)
    {
        Color c = sr.color;
        c.a -= 0.05f;
        sr.color = c;
    }

    private void fadeOut(Image image)
    {
        Color c = image.color;
        c.a -= 0.05f;
        image.color = c;
    }

    private void fadeOut(Text text)
    {
        Color c = text.color;
        c.a -= 0.05f;
        text.color = c;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        GameObject obj = coll.gameObject;
        if (obj.tag.Equals("Ball"))
        {
            score++;
            scoreLabel.text = "SCORE: " + score.ToString();
            Destroy(obj);
        }
        if (obj.tag.Equals("Yellow Card"))
        {
            Color c = hit_icon0.color;
            c.a = 1;
            if (hit_icon0.color.a == 1)
            {
                hit_icon1.color = c;
                gameOver = true;
                examplePlayer.GetComponent<Rigidbody2D>().gravityScale = 1;
                c = indexFinger.GetComponent<SpriteRenderer>().color;
                c.a = 0;
                indexFinger.GetComponent<SpriteRenderer>().color = c;
                timer = 0;
            }
            else
            {
                hit_icon0.color = c;
            }
            Destroy(obj);
        }
        if (obj.tag.Equals("Red Card"))
        {
            gameOver = true;
            Destroy(obj);
        }   
    }
}