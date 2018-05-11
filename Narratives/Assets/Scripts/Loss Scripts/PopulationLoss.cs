﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopulationLoss : MonoBehaviour
{

    private string gameOverTitle = "GAME OVER",
                    buttonOne = "NEW GAME",
                    buttonTwo = "QUIT TO MAIN MENU",
                    buttonThree = "QUIT TO DESKTOP";

    private string gameOverDescription = "You stand in the door to your small manor, watching the sun set on an empty street. A stray dog howls in the night, as rats feast on the corpses on the villagers. All your friends are dead, all of your family... A tear falls from your face as your hands grasp the hilt of your knife. Goodbye.";

    public GUISkin skin;

    Rect titleRect,
         descRect,
         buttonOneRect,
         buttonTwoRect,
         buttonThreeRect;

    private float titleAndDescWidth,
                    titleHeight,
                    descHeight,
                    buttonWidth,
                    buttonHeight;

    private float titleAndDescStartX,
                    titleStartY,
                    descStartY,
                    buttonsX,
                    buttonOneY,
                    buttonTwoY,
                    buttonThreeY;

    private void Start()
    {
        titleAndDescWidth = Screen.width / 2;
        titleHeight = Screen.height / 12;
        descHeight = Screen.height / 4;
        buttonWidth = Screen.width / 3;
        buttonHeight = Screen.height / 12;

        titleAndDescStartX = Screen.width / 4;
        titleStartY = Screen.height / 4;
        descStartY = titleStartY + titleHeight;
        buttonsX = Screen.width / 3;
        buttonOneY = descStartY + descHeight + Screen.height / 24;
        buttonTwoY = buttonOneY + buttonHeight;
        buttonThreeY = buttonTwoY + buttonHeight;

        titleRect = new Rect(titleAndDescStartX, titleStartY, titleAndDescWidth, titleHeight);
        descRect = new Rect(titleAndDescStartX, descStartY, titleAndDescWidth, descHeight);
        buttonOneRect = new Rect(buttonsX, buttonOneY, buttonWidth, buttonHeight);
        buttonTwoRect = new Rect(buttonsX, buttonTwoY, buttonWidth, buttonHeight);
        buttonThreeRect = new Rect(buttonsX, buttonThreeY, buttonWidth, buttonHeight);

    }

    private void OnGUI()
    {
        GUI.skin = skin;
        skin.GetStyle("eventWindowDescription").wordWrap = true;
        Event e = Event.current;

        GUI.Box(new Rect(titleRect), gameOverTitle, skin.GetStyle("EventWindowName"));
        GUI.Box(new Rect(descRect), gameOverDescription, skin.GetStyle("EventWindowDescription"));
        GUI.Box(new Rect(buttonOneRect), buttonOne, skin.GetStyle("EventWindowOption"));
        GUI.Box(new Rect(buttonTwoRect), buttonTwo, skin.GetStyle("EventWindowOption"));
        GUI.Box(new Rect(buttonThreeRect), buttonThree, skin.GetStyle("EventWindowOption"));

        if (e.button == 0 && e.type == EventType.MouseUp)
        {
            if (buttonOneRect.Contains(e.mousePosition)) NewGame();
            else if (buttonTwoRect.Contains(e.mousePosition)) QuitToMenu();
            else if (buttonThreeRect.Contains(e.mousePosition)) QuitToDesktop();
        }
    }

    // Start a new game
    private void NewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    // Quit to main menu
    private void QuitToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    // Quit the game
    private void QuitToDesktop()
    {
        Application.Quit();
    }
}