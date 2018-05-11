using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GUISkin skin;

    private string  startButtonString = "Start Game", 
                    exitButtonString = "Exit Game";

    private Rect startButtonRect, exitButtonRect;
    private float buttonY_start, buttonY_exit, buttonX, buttonWidth, buttonHeight;

    private void Start()
    {
        buttonWidth = Screen.width / 5;
        buttonHeight = Screen.height / 5;

        buttonX = Screen.width / 5 * 2;
        buttonY_start = Screen.height / 5;
        buttonY_exit = buttonY_start + 2 * buttonHeight;

        startButtonRect = new Rect(buttonX, buttonY_start, buttonWidth, buttonHeight);
        exitButtonRect = new Rect(buttonX, buttonY_exit, buttonWidth, buttonHeight);
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        skin.GetStyle("buttons").fontSize = Screen.height / 16;

        Event e = Event.current;

        GUI.Box(new Rect(startButtonRect), startButtonString, skin.GetStyle("buttons"));
        GUI.Box(new Rect(exitButtonRect), exitButtonString, skin.GetStyle("buttons"));

        if (e.button == 0 && e.type == EventType.MouseUp)
        {
            if (startButtonRect.Contains(e.mousePosition))
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            else if (exitButtonRect.Contains(e.mousePosition))
            {
                Application.Quit();
            }
        }
    }
}
