using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Text101 : MonoBehaviour {

    #region GUI elements
    public Text TextDescription;
    public Text TextActions;
    #endregion

    #region Data

    /// <summary>
    /// All available states in the game
    /// </summary>
    enum eStates
    {
        cell,
        sheets0,
        mirror,
        lock0,
        sheets1,
        cellMirror,
        lock1,
        corridor0,
        stairs0,
        floor,
        closetDoor,
        corridor1,
        stairs1,
        inCloset,
        corridor2,
        stairs2,
        corridor3,
        courtyard,        
        NA
    }

    /// <summary>
    /// State definition
    /// </summary>
    struct State
    {
        public eStates Name;
        public string Description;
        public List<StateLink> StateLinks;
        
        public State(eStates name,
                     string description,                        //Describes the situation
                     List<StateLink> stateLinks)                 //Lists the availalbe states from this state  
        {
            Name = name;
            Description = description;
            StateLinks = stateLinks;
        }
    }

    /// <summary>
    /// Holds all the information to link the current state to the next state
    /// </summary>
    struct StateLink
    {
        public string ActionDescription;
        public string ActionKeyboard;
        public eStates StateName;

        public StateLink(string actionDescription = "",
                         string actionKeyboard = "",
                         eStates stateName = eStates.NA)
        {
            ActionDescription = actionDescription;
            ActionKeyboard = actionKeyboard;
            StateName = stateName;
        }
    }

    //Container to hold all possible states in the game
    List<State> lstStates = new List<State>();

    State currentState; 
    #endregion

    #region Events
    // Use this for initialization
    void Start () {
        //Initializes the game
        initializeStateList();
        currentState = lstStates.Find(x=>x.Name == eStates.cell);
        presentState();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update () {
        if (Input.anyKey)
        {
            setCurrentState(Input.inputString);
            presentState();
        }
    }
    #endregion

    #region Functions
    /// <summary>
    /// Initialize the game's states
    /// </summary>
    void initializeStateList()
    {
        State stateCell = new State(eStates.cell,
                                    "You're in a jail cell",
                                    new List<StateLink>() { new StateLink("look at Sheets",
                                                                          "S",
                                                                          eStates.sheets0),
                                                            new StateLink("check out Mirror",
                                                                          "M",
                                                                          eStates.mirror),
                                                            new StateLink("inspect Lock",
                                                                          "L",
                                                                          eStates.lock0)});
        State stateSheets0 = new State(eStates.sheets0,
                                       "Dirty sheets, you find nothing of use",
                                       new List<StateLink>() { new StateLink("Return to look for other things at the cell",
                                                                             "R",
                                                                             eStates.cell)});
        State stateMirror = new State(eStates.mirror,
                                      "The mirror has a thin sturdy frame",
                                      new List<StateLink>() { new StateLink("Take the mirror with you",
                                                                             "T",
                                                                             eStates.cellMirror),
                                      new StateLink("Return to look for other things at the cell",
                                                                             "R",
                                                                             eStates.cell)});
        State stateLock0 = new State(eStates.lock0,
                                     "You stare at your cell's lock, it's locked",
                                     new List<StateLink>() { new StateLink("Return to look for other things at the cell",
                                                                           "R",
                                                                           eStates.cell)});
        State stateCellMirror = new State(eStates.cellMirror,
                                          "You look around in your cell",
                                          new List<StateLink>() { new StateLink("inspect the cell's Lock",
                                                                                "L",
                                                                                eStates.lock1),
                                                                  new StateLink("look at Sheets",
                                                                                "S",
                                                                                eStates.sheets1)});
        State stateSheets1 = new State(eStates.sheets1,
                                       "Dirty sheets, you find nothing of use",
                                       new List<StateLink>() { new StateLink("Return to look for other things at the cell",
                                                                              "R",
                                                                              eStates.cellMirror)});
        State stateLock1 = new State(eStates.lock1,
                                     "You stare at your cell's lock",
                                     new List<StateLink>() { new StateLink("jam the cell's lock Open with the mirror's frame",
                                                                           "O",
                                                                           eStates.corridor0),
                                                             new StateLink("Return to look for other things at the cell",
                                                                           "R",
                                                                           eStates.cellMirror)});
        State stateCorridor0 = new State(eStates.corridor0,
                                        "You are in the main corridor now",
                                        new List<StateLink>() {new StateLink("check the Staircase",
                                                                             "S",
                                                                             eStates.stairs0),
                                                              new StateLink("look at the Floor",
                                                                            "F",
                                                                            eStates.floor),
                                                              new StateLink("open the Closet",
                                                                            "C",
                                                                            eStates.closetDoor)});
        State stateStairs0 = new State(eStates.stairs0,
                                       "The door at the end of the stairs is observed by a guard..." + 
                                       Environment.NewLine + 
                                       "you won't be able to pass him dressed like this!",
                                       new List<StateLink>(){new StateLink("Return to the corridor",
                                                                           "R",
                                                                           eStates.corridor0)});
        State stateCloset0 = new State(eStates.closetDoor,
                                       "The closet door is locked!",
                                       new List<StateLink>(){new StateLink("Return to the corridor",
                                                                           "R",
                                                                           eStates.corridor0)});
        State stateFloor = new State(eStates.floor,
                               "You see a hair clip on the floor",
                               new List<StateLink>(){new StateLink("pick up the Hair clip",
                                                                    "H",
                                                                    eStates.corridor1),
                                                     new StateLink("Return to the corridor",
                                                                    "R",
                                                                    eStates.corridor0)});
        State stateCorridor1 = new State(eStates.corridor1,
                                         "You've picked the hair clip",
                                         new List<StateLink>(){new StateLink("check the Staircase",
                                                                            "S",
                                                                            eStates.stairs1),
                                                                new StateLink("Pick up the lock of the closet",
                                                                              "P",
                                                                              eStates.inCloset)});
        State stateStairs1 = new State(eStates.stairs1,
                                       "The door at the end of the stairs is observed by a guard..." +
                                       Environment.NewLine +
                                       "you won't be able to pass him dressed like this!",
                                       new List<StateLink>(){new StateLink("Return to the corridor",
                                                                           "R",
                                                                           eStates.corridor1)});
        State stateInCloset = new State(eStates.inCloset,
                                       "Inside the closet are cleaners uniform, and in your size too!",
                                       new List<StateLink>(){new StateLink("Return to the corridor",
                                                                           "R",
                                                                           eStates.corridor2),
                                                             new StateLink("Dress up as a cleaner",
                                                                           "D",
                                                                           eStates.corridor3)});
        State stateCorridor2 = new State(eStates.corridor2,
                                         "You're back in the corridor",
                                         new List<StateLink>(){new StateLink("check the Staircase",
                                                                            "S",
                                                                            eStates.stairs2),
                                         new StateLink("Retrurn to look in the closet",
                                                                              "R",
                                                                              eStates.inCloset)});
        State stateStairs2 = new State(eStates.stairs2,
                                       "The door at the end of the stairs is observed by a guard...",
                                       new List<StateLink>(){new StateLink("Return to the corridor",
                                                                           "R",
                                                                           eStates.corridor2)});
        State stateCorridor3 = new State(eStates.corridor3,
                                         "You're back in the corridor",
                                        new List<StateLink>(){new StateLink("Undress and put the cleaner uniform back in the closet",
                                                                           "U",
                                                                           eStates.inCloset),
                                                              new StateLink("check the Staircase",
                                                                            "S",
                                                                            eStates.courtyard)});
        State stateCourtyard = new State(eStates.courtyard,
                                       "You've found yourself in the courtyard, the gate is in sight, when suddenly you feel a hand on your shoulder." ,
                                       new List<StateLink>(){new StateLink("try to Escape",
                                                                           "E",
                                                                           eStates.cell)});
        //Add all states to the state list
        lstStates.Add(stateCell);
        lstStates.Add(stateSheets0);
        lstStates.Add(stateLock0);
        lstStates.Add(stateMirror);
        lstStates.Add(stateCellMirror);
        lstStates.Add(stateSheets1);
        lstStates.Add(stateLock1);
        lstStates.Add(stateCorridor0);
        lstStates.Add(stateStairs0);
        lstStates.Add(stateCloset0);
        lstStates.Add(stateFloor);
        lstStates.Add(stateCorridor1);
        lstStates.Add(stateStairs1);
        lstStates.Add(stateInCloset);
        lstStates.Add(stateCorridor2);
        lstStates.Add(stateStairs2);
        lstStates.Add(stateCorridor3);
        lstStates.Add(stateCourtyard);
    }

    /// <summary>
    /// Sets the new state 
    /// </summary>
    void setCurrentState(string input)
    {
        //Check whether the user input correspond with the current states available options
        List<StateLink> currentStateLinks = currentState.StateLinks;
        foreach (StateLink link in currentStateLinks)
        {
            //Link found, set the link into the new current state and exit
            if (link.ActionKeyboard.ToUpper() == input.ToUpper())
            {
                currentState = lstStates.Find(x => x.Name == link.StateName);
                break;
            }
        }
    }
    /// <summary>
    /// Outputs the state's information to the user
    /// </summary>
    void presentState()
    {
        //Show state's description
        TextDescription.text = currentState.Description;
        //Present available option for this state
        TextActions.text = string.Empty;
        foreach (var item in currentState.StateLinks)
            TextActions.text = TextActions.text + "\n Press [" + item.ActionKeyboard + "] to " + item.ActionDescription;
    }
    #endregion
}
