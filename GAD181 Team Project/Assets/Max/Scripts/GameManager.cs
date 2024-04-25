using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //Declare variables
    //Game data
    public int gameState;          //The current game state. 0 = Pre-Game, 1 = In-Game, 2 = Post-Game
    public int gamePlayersAlive;   //The current amount of players alive in the game.
    public string gameWinnerName;  //The playerName of the winner.
    public Color gameWinnerColor;  //The color of the winner.

    //Game settings
    public int settingPlayerCount;  //The amount of players playing the game.
    public int settingMap;          //The selected map.
    public int settingItemScale;  //The scale modifier for items.
    public int settingItemBounce; //The bounce modifier for items.
    public int settingPowerupMulti; //The powerup multiplier
    public int settingJunkMulti;    //The junk multiplier

    //Queue related vars
    public List<GameObject> gameQueue; //The list of gameobjects currently in the queue.
    [SerializeField] private List<GameObject> referenceItems;          //A list of all the items that can appear in the queue. Goes [Orb1 -> Orb7, JunkSquare, JunkTriangle, PowerupEraser, PowerupPaint]
    [SerializeField] private List<UnityEngine.UI.Image> referenceQueueImages;         //A reference to the 7 queue image slots.
    [SerializeField] private List<TextMeshProUGUI> referenceQueueText; //A reference to the 7 queue text slots.

    //Pregame/Postgame UI related vars
    [SerializeField] private Canvas referenceCanvasPregame;             //A reference to the pregame canvas.
    [SerializeField] private TextMeshProUGUI referencePlayerCountText;  //A reference to the pregame player count text.
    [SerializeField] private TextMeshProUGUI referenceMapText;          //A reference to the pregame map text.
    [SerializeField] private TextMeshProUGUI referenceItemScaleText;    //A reference to the pregame item scale text.
    [SerializeField] private TextMeshProUGUI referenceItemBounceText;   //A reference to the pregame item bounce text.
    [SerializeField] private TextMeshProUGUI referencePowerupMultiText; //A reference to the pregame powerup multi text.
    [SerializeField] private TextMeshProUGUI referenceJunkMultiText;    //A reference to the pregame junk multi text.

    [SerializeField] private GameObject referenceWarningPopup; //A reference to the player count warning popup.
    [SerializeField] private GameObject referenceTutorialPopup; //A reference to the tutorial popup.
    [SerializeField] private GameObject referenceControlsPopup; //A reference to the controls popup.

    [SerializeField] private Canvas referenceCanvasPostgame; //A reference to the postgame canvas.
    [SerializeField] private TextMeshProUGUI referenceWinnerText; //A reference to the postgame winner text.

    [SerializeField] private Canvas referenceCanvasIngame; //A reference to the ingame canvas.
    [SerializeField] private GameObject referenceEvolutionParticle; //A reference to the evolution particle effects.
    [SerializeField] private GameObject referenceQueueParticle; //A reference to the evolution particle effects.

    //Map related vars
    [SerializeField] private List<GameObject> referenceMapObjects; //A list of parent objects for each map.
    [SerializeField] private List<string> referenceMapNames; //A list of map names.

    //Player related vars
    [SerializeField] private List<string> playerNames; //A list of the player names.
    [SerializeField] private List<Color> playerColors; //A list of the player colours.
    [SerializeField] private List<Sprite> playerStyles; //A list of the player styles.

    [SerializeField] private List<KeyCode> playerControlsLeft;  //A list of the player left buttons
    [SerializeField] private List<KeyCode> playerControlsRight; //A list of the player right buttons.
    [SerializeField] private List<KeyCode> playerControlsDrop;  //A list of the player drop buttons.

    [SerializeField] private GameObject referenceMarker; //A reference to the player marker gameobject.
    private List<GameObject> referencePlayers; //A list of all the current player control scripts. Used to determine game end conditions and winners.

    //Particle related vars
    public ParticleSystem referenceMergeParticle; //A reference to the merge particles, which is grabbed by the orbs when required.
    public ParticleSystem referenceEraseParticle; //A reference to the erase particles, which is grabbed by the powerups when required.
    public ParticleSystem referencePaintParticle; //A reference to the paint particles, which is grabbed by the powerups when required.

    public GameObject referenceFloatingText; //A reference to the temporary floating text.

    //Temp Audio related vars
    public GameObject referenceMergeAudio; //A reference to the merge audio, which is grabbed by orbs when required.
    public GameObject referenceEraseAudio; //A reference to the erase audio, which is grabbed by powerups when required.
    public GameObject referencePaintAudio; //A reference to the paint audio, which is grabbed by powerups when required.
    public GameObject referenceOutAudio;   //A reference to the out audio, which is grabbed by orbs when the player becomes out.

    //Perm Audio related vars
    [SerializeField] AudioSource referenceWinAudio; //Areference to the  win audio, which is called when a player wins.
    [SerializeField] AudioSource referenceClickAudio; //A reference to the UI click audio, which is called when a UI button is pressed.
    [SerializeField] AudioSource referenceBackgroundAudio; //A reference to the background audio.

    //Misc vars
    float launchCooldown = 0; //Used for title screen launching.

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Only update if game in pre-game.
        if (gameState == 0)
        {
            //Show specific canvases
            referenceCanvasIngame.enabled = false;
            referenceCanvasPregame.enabled = true;
            referenceCanvasPostgame.enabled = false;

            referenceEvolutionParticle.SetActive(false);
            referenceQueueParticle.SetActive(false);

            //Update settings text
            referencePlayerCountText.text = "Player Count: " + settingPlayerCount;
            referenceMapText.text = "Map Type: " + referenceMapNames[settingMap];
            referenceItemScaleText.text = "Item Size: x" + (float)settingItemScale/10;
            referenceItemBounceText.text = "Item Bounce: x" + (float)settingItemBounce /10;
            referencePowerupMultiText.text = "Powerup Multi: x" + settingPowerupMulti;
            referenceJunkMultiText.text = "Junk Multi: x" + settingJunkMulti;

            //If the player count is less than 4, enable the warning popup, otherwise hide it.
            if (settingPlayerCount >= 4)
            {
                referenceWarningPopup.SetActive(false);
            } else
            {
                referenceWarningPopup.SetActive(true);
            }

            //Launch orbs around!
            launchCooldown += 1;
            if(launchCooldown >= 0.5*120)
            {
                launchCooldown = 0;
                foreach (GameObject i in GameObject.FindGameObjectsWithTag("Orb"))
                {
                    //If it has low motion, launch it.
                    //i.GetComponent<Rigidbody2D>().mass = 3;
                    if(i.GetComponent<Rigidbody2D>().velocity.x < 1 && i.GetComponent<Rigidbody2D>().velocity.y < 1)
                    {
                        i.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(1000, -1001) * i.GetComponent<Rigidbody2D>().mass, Random.Range(1000, -1001) * i.GetComponent<Rigidbody2D>().mass));
                    }
                }
            }
        }

        //Only update if the game is in progress.
        if(gameState == 1)
        {
            //Show specific canvases
            referenceCanvasIngame.enabled = true;
            referenceCanvasPregame.enabled = false;
            referenceCanvasPostgame.enabled = false;

            referenceEvolutionParticle.SetActive(true);
            referenceQueueParticle.SetActive(true);

            //If there are only 8 values in the list left, add another bag to the end.
            if (gameQueue.Count <= 8)
            {
                SetupQueue(20, 10, 4, 1, 1, 1, 1, 1);
            }

            //Update visuals for the shown queue.
            for (int i = 0; i < 7; i++)
            {
                //Set the queue image.
                referenceQueueImages[i].sprite = gameQueue[i].GetComponent<SpriteRenderer>().sprite;

                //If the queue item is a orb, then update the text to show its orbValue, otherwise hide the text.
                if (gameQueue[i].gameObject.tag == "Orb")
                {
                    referenceQueueImages[i].transform.localScale = new Vector3(0.3f + ((float)gameQueue[i].GetComponent<ItemController>().orbValue * 0.1f), 0.3f + ((float)gameQueue[i].GetComponent<ItemController>().orbValue * 0.1f), 1);
                    referenceQueueText[i].fontSize = 36f + ((float)gameQueue[i].GetComponent<ItemController>().orbValue * 1f);
                    referenceQueueText[i].enabled = true;
                    ItemController referenceItemController = gameQueue[i].GetComponent<ItemController>();
                    referenceQueueText[i].text = referenceItemController.orbValue.ToString();
                }
                else
                {
                    referenceQueueText[i].enabled = false;
                }

                //Set scales of powerups/junk items.
                if (gameQueue[i].gameObject.tag == "Powerup")
                {
                    referenceQueueImages[i].transform.localScale = new Vector3(1, 1, 1);
                }
                if (gameQueue[i].gameObject.tag == "Junk")
                {
                    referenceQueueImages[i].transform.localScale = new Vector3(0.8f, 0.8f, 1);
                }
            }

            //Loop through each script reference to see if they are still alive, and set the gamePlayersAlive value accordingly.
            gamePlayersAlive = 0;
            for (int i = 0; i < referencePlayers.Count; i++)
            {
                if(referencePlayers[i].GetComponent<PlayerController>().playerIsAlive == true)
                {
                    gamePlayersAlive += 1;
                }
            }

            //If there are 1 or no players left, end the game. OR if they are playing singleplayer, end if there are 0 players.
            if ((gamePlayersAlive <= 1 && settingPlayerCount != 1) || (gamePlayersAlive == 0 && settingPlayerCount == 1))
            {
                List<int> playerScores = new List<int>();
                //Find the player with the highest score by creating a list of scores, sorting them and taking the first index. Then compare this index back to the scripts, stopping when the first index matchs the script's score value.
                for (int i = 0; i < referencePlayers.Count; i++)
                {
                    playerScores.Add(referencePlayers[i].GetComponent<PlayerController>().playerScore);
                }
                playerScores.Sort();
                for (int i = 0; i < referencePlayers.Count; i++)
                {
                    if (referencePlayers[i].GetComponent<PlayerController>().playerScore == playerScores[referencePlayers.Count-1])
                    {
                        gameWinnerName = referencePlayers[i].GetComponent<PlayerController>().playerName;
                        gameWinnerColor = referencePlayers[i].GetComponent<PlayerController>().playerColor;
                        break;
                    }
                }

                //End the game and set winner text.
                gameState = 2;
                referenceWinnerText.text = gameWinnerName + " WON!";
                referenceWinnerText.color = gameWinnerColor;

                //Play the winner sound effect.
                referenceWinAudio.Play();

                //Disable player markers and held items, but not player score texts.
                for (int i = 0; i < referencePlayers.Count; i++)
                {
                    referencePlayers[i].GetComponent<PlayerController>().referenceMarker.GetComponent<SpriteRenderer>().enabled = false;
                    referencePlayers[i].GetComponent<PlayerController>().referenceCooldownBar.GetComponent<SpriteRenderer>().enabled = false;
                    referencePlayers[i].GetComponent<PlayerController>().referenceDropLine.GetComponent<SpriteRenderer>().enabled = false;
                    referencePlayers[i].GetComponent<PlayerController>().referenceOutOfGameEffect.GetComponent<SpriteRenderer>().enabled = false;
                    referencePlayers[i].GetComponent<PlayerController>().referenceHeldItem.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }

        //Only update if the game is post-game.
        if(gameState == 2)
        {
            //Show specific canvases
            referenceCanvasIngame.enabled = false;
            referenceCanvasPregame.enabled = false;
            referenceCanvasPostgame.enabled = true;

            referenceEvolutionParticle.SetActive(false);
            referenceQueueParticle.SetActive(false);
        }

    }

    //Function called when the game starts.
    public void SetupGame()
    {
        referenceClickAudio.Play();

        //Clear the game board/queue.
        ClearGame();
        referenceTutorialPopup.SetActive(false);
        referenceTutorialPopup.SetActive(false);

        //Create a new queue.
        SetupQueue(20, 10, 4, 1, 1, 1, 1, 1);
        referencePlayers = new List<GameObject>();

        gamePlayersAlive = settingPlayerCount;

        //Calculate passing/spawn positions so players are split evenly.
        float spawnPos = 0;
        float divide = 14 / (float)settingPlayerCount;
        float padding = 14 / ((float)settingPlayerCount * 2);
        spawnPos += padding;

        //Setup players from the list using the selected player count.
        for (int i = 0; i < settingPlayerCount; i++)
        {
            //Create player marker
            GameObject referencePlayer = Instantiate(referenceMarker, new Vector3(spawnPos - 7, 3.5f, 0), Quaternion.identity) as GameObject;
            //Debug.Log("Divide: " + divide + ", Padding: " + padding + ", SpawnPos: " + spawnPos);
            spawnPos += divide;
            //Old Position Formula: Mathf.Clamp((float)i / settingPlayerCount * 14 - 7, -6, 6)
            //Get the new player's script and set relevent variables.
            PlayerController referencePlayerControl = referencePlayer.GetComponent<PlayerController>();
            referencePlayerControl.playerID = i + 1;
            referencePlayerControl.playerColor = playerColors[i];
            referencePlayerControl.playerStyle = playerStyles[i];
            referencePlayerControl.playerControlLeft = playerControlsLeft[i];
            referencePlayerControl.playerControlRight = playerControlsRight[i];
            referencePlayerControl.playerControlDrop = playerControlsDrop[i];
            referencePlayerControl.referenceGameManager = this.gameObject;
            referencePlayerControl.playerName = playerNames[i];

            //Add the player control script to the list.
            referencePlayers.Add(referencePlayer);
        }

        //Declare that the game has started.
        gameState = 1;
    }

    //Ammends a new, full queue of prefabs based on the list settings.
    void SetupQueue(int orb1, int orb2, int orb3, int orb4, int junkSquare, int junkTriangle, int powerupEraser, int powerupPaint)
    {
        //Make a new temp queue list.
        List<GameObject> tempQueue = new List<GameObject>();

        //Apply multiplier settings.
        junkSquare = junkSquare * settingJunkMulti;
        junkTriangle = junkTriangle * settingJunkMulti;
        powerupEraser = powerupEraser * settingPowerupMulti;
        powerupPaint = powerupPaint * settingPowerupMulti;

        //Dont spawn paint buckets in singleplayer!
        if(settingPlayerCount == 1)
        {
            powerupPaint = 0;
        }

        //Add items to the queue.
        for (int i = 0; i < orb1; i++) //Lv 1 Orbs
        {
            tempQueue.Add(referenceItems[0]);
        }
        for (int i = 0; i < orb2; i++) //Lv 2 Orbs
        {
            tempQueue.Add(referenceItems[1]);
        }
        for (int i = 0; i < orb3; i++) //Lv 3 Orbs
        {
            tempQueue.Add(referenceItems[2]);
        }
        for (int i = 0; i < orb4; i++) //Lv 4 Orbs
        {
            tempQueue.Add(referenceItems[3]);
        }
        for (int i = 0; i < junkSquare; i++) //Square Junk. Multiply by the JunkMulti Stat.
        {
            tempQueue.Add(referenceItems[4]);
        }
        for (int i = 0; i < junkTriangle; i++) //Triangular Junk. Multiply by the JunkMulti Stat.
        {
            tempQueue.Add(referenceItems[5]);
        }
        for (int i = 0; i < powerupEraser; i++) //Eraser Powerup. Multiply by the PowerupMulti Stat.
        {
            tempQueue.Add(referenceItems[6]);
        }
        for (int i = 0; i < powerupPaint; i++) //Paint Powerup. Multiply by the PowerupMulti Stat.
        {
            tempQueue.Add(referenceItems[7]);
        }

        //Randomize the list order
        ShuffleList(tempQueue);

        //Add this temp list to the end of the gameQueue.
        gameQueue.AddRange(tempQueue);

    }

    //Clears the game board/players/queue.
    public void ClearGame()
    {
        //Delete all currently existing player markers and items.
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(i);
        }
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Orb"))
        {
            Destroy(i);
        }
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Junk"))
        {
            Destroy(i);
        }
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Powerup"))
        {
            Destroy(i);
        }
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerUI"))
        {
            Destroy(i);
        }

        //Clear the previous queue.
        gameQueue.Clear();
    }

    //List Shuffle Function
    public List<GameObject> ShuffleList(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

    //Below is click events for UI:

    //OnClick event for + playercount
    public void ClickPlayerCountPlus()
    {
        referenceClickAudio.Play();
        settingPlayerCount += 1;
        settingPlayerCount = Mathf.Clamp(settingPlayerCount,1,4);
    }

    //OnClick event for - playercount
    public void ClickPlayerCountMinus()
    {
        referenceClickAudio.Play();
        settingPlayerCount -= 1;
        settingPlayerCount = Mathf.Clamp(settingPlayerCount, 1, 4);
    }

    //OnClick event for + map
    public void ClickMapPlus()
    {
        referenceClickAudio.Play();
        settingMap += 1;
        settingMap = Mathf.Clamp(settingMap, 0, referenceMapObjects.Count-1);
        UpdateMap();
    }

    //OnClick event for - map
    public void ClickMapMinus()
    {
        referenceClickAudio.Play();
        settingMap -= 1;
        settingMap = Mathf.Clamp(settingMap, 0, referenceMapObjects.Count-1);
        UpdateMap();
    }

    //Update map based on selected map.
    public void UpdateMap()
    {
        //Hide all maps
        for (int i = 0; i < referenceMapObjects.Count; i++)
        {
            referenceMapObjects[i].SetActive(false);
        }

        //Show specific map
        referenceMapObjects[settingMap].SetActive(true);
    }

    //OnClick event for + itemScale
    public void ClickItemScalePlus()
    {
        referenceClickAudio.Play();
        settingItemScale += 1;
        settingItemScale = Mathf.Clamp(settingItemScale, 5, 30);
    }

    //OnClick event for - itemScale
    public void ClickItemScaleMinus()
    {
        referenceClickAudio.Play();
        settingItemScale -= 1;
        settingItemScale = Mathf.Clamp(settingItemScale, 5, 30);
    }

    //OnClick event for + itemBounce
    public void ClickItemBouncePlus()
    {
        referenceClickAudio.Play();
        settingItemBounce += 1;
        settingItemBounce = Mathf.Clamp(settingItemBounce, 0, 10);
    }

    //OnClick event for - itemBounce
    public void ClickItemBounceMinus()
    {
        referenceClickAudio.Play();
        settingItemBounce -= 1;
        settingItemBounce = Mathf.Clamp(settingItemBounce, 0, 10);
    }

    //OnClick event for + powerupMulti
    public void ClickPowerupMultiPlus()
    {
        referenceClickAudio.Play();
        settingPowerupMulti += 1;
        settingPowerupMulti = Mathf.Clamp(settingPowerupMulti, 0, 3);
    }

    //OnClick event for - powerupMulti
    public void ClickPowerupMultiMinus()
    {
        referenceClickAudio.Play();
        settingPowerupMulti -= 1;
        settingPowerupMulti = Mathf.Clamp(settingPowerupMulti, 0, 3);
    }

    //OnClick event for + junkMulti
    public void ClickJunkMultiPlus()
    {
        referenceClickAudio.Play();
        settingJunkMulti += 1;
        settingJunkMulti = Mathf.Clamp(settingJunkMulti, 0, 3);
    }

    //OnClick event for - junkMulti
    public void ClickJunkMultiMinus()
    {
        referenceClickAudio.Play();
        settingJunkMulti -= 1;
        settingJunkMulti = Mathf.Clamp(settingJunkMulti, 0, 3);
    }

    //OnClick event for the reset settings button.
    public void ClickResetSettings()
    {
        referenceClickAudio.Play();

        //Reset settings
        settingPlayerCount = 4;
        settingMap = 0;
        settingItemScale = 10;
        settingItemBounce = 1;
        settingPowerupMulti = 1;
        settingJunkMulti = 1;

        UpdateMap();
}


    //OnClick event for the exit button. If in-game, go back to the title screen, if in the pre-game, then send them back to the gems select screen.
    public void ClickExit()
    {
        referenceClickAudio.Play();
        if (gameState == 0)
        {
            Application.Quit();
        }

        if (gameState == 1 || gameState == 2)
        {
            gameState = 0;

            //Destroy player markers
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("Player"))
            {
                Destroy(i);
            }
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("PlayerUI"))
            {
                Destroy(i);
            }
        }
    }

    //OnClick event for the fullscreen button. Toggles between windows and borderless window.
    public void ClickFullscreen()
    {
        referenceClickAudio.Play();
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            return;
        }
        
        if(Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            return;
        }
    }


    //OnClick event for the tutorial button. Toggles based on visibility.
    public void ClickTutorial()
    {
        referenceClickAudio.Play();
        referenceControlsPopup.SetActive(false);
        if(referenceTutorialPopup.activeSelf == true)
        {
            referenceTutorialPopup.SetActive(false);
        } else
        {
            referenceTutorialPopup.SetActive(true);
        }
    }

    //OnClick event for the tutorial button. Toggles based on visibility.
    public void ClickControls()
    {
        referenceClickAudio.Play();
        referenceTutorialPopup.SetActive(false);
        if (referenceControlsPopup.activeSelf == true)
        {
            referenceControlsPopup.SetActive(false);
        }
        else
        {
            referenceControlsPopup.SetActive(true);
        }
    }

    public void ClickMusic()
    {
        referenceClickAudio.Play();
        if (referenceBackgroundAudio.mute == true)
        {
            referenceBackgroundAudio.mute = false;
        }
        else
        {
            referenceBackgroundAudio.mute = true;
        }

    }
}
