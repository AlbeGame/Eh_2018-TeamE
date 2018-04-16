﻿using UnityEngine;

[RequireComponent(typeof(SelectableBehaviour), typeof(PuzzleGraphic))]
public class PuzzleLockCode : MonoBehaviour, IPuzzle, ISelectable
{
    public Renderer MonitorRend;
    public SelectableSwitch Key1st;
    public SelectableSwitch Key2nd;
    public SelectableSwitch Key3rd;

    SelectableBehaviour selectable;
    PuzzleGraphic graphicCtrl;

    PuzzleLockCodeData data;
    PuzzleLockCodeData.PossibleSetup chosenSetup;

    int solutionProgression;

    PuzzleState _solutionState;
    public PuzzleState SolutionState
    {
        get
        {
            return _solutionState;
        }

        set
        {
            _solutionState = value;
        }
    }

    public bool CheckIfSolved()
    {
        if(solutionProgression >= 3)
        {
            DoWin();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DoLoose()
    {
        SolutionState = PuzzleState.Broken;
        selectable.GetRoot().GetComponent<LevelManager>().NotifyPuzzleBreakdown(this);

        graphicCtrl.Paint(SolutionState);
    }

    public void DoWin()
    {
        SolutionState = PuzzleState.Solved;
        selectable.GetRoot().GetComponent<LevelManager>().NotifyPuzzleSolved(this);

        graphicCtrl.Paint(SolutionState);
    }

    public void Setup(IPuzzleData _data)
    {
        selectable = GetComponent<SelectableBehaviour>();
        graphicCtrl = GetComponent<PuzzleGraphic>();

        //Choosing a Setup between the possibilities
        data = _data as PuzzleLockCodeData;
        int _setupIndex = Random.Range(0, data.Setups.Count);
        chosenSetup = data.Setups[_setupIndex];


        Material chosenMat = new Material(MonitorRend.material);
        chosenMat.mainTexture = chosenSetup.ScreenImage;
        MonitorRend.material = chosenMat;

        Key1st.Init(this, new PuzzleLockCode_KeyData() { keyID = KeyOrder.Key1st });
        Key2nd.Init(this, new PuzzleLockCode_KeyData() { keyID = KeyOrder.Key2nd });
        Key3rd.Init(this, new PuzzleLockCode_KeyData() { keyID = KeyOrder.Key3rd });
    }

    public void Init()
    {
        SolutionState = PuzzleState.Unsolved;
        solutionProgression = 0;
    }

    public void OnButtonSelect(SelectableButton _button)
    {
        throw new System.NotImplementedException();
    }

    public void OnMonitorSelect(SelectableMonitor _monitor)
    {
        throw new System.NotImplementedException();
    }

    public void OnSelection()
    {
        Debugger.DebugLogger.Clean();
        Debugger.DebugLogger.LogText(chosenSetup.Seq_1st + " - " + chosenSetup.Seq_2nd + " - " + chosenSetup.Seq_3rd);
    }

    public void OnStateChange(SelectionState _state)
    {
    }

    public void OnSwitchSelect(SelectableSwitch _switch)
    {
        switch (solutionProgression)
        {
            case 0:
                {
                    if ((_switch.InputData as PuzzleLockCode_KeyData).keyID == chosenSetup.Seq_1st)
                    {
                        solutionProgression++;
                    }
                    else
                        DoLoose();
                }
                break;
            case 1:
                {
                    if ((_switch.InputData as PuzzleLockCode_KeyData).keyID == chosenSetup.Seq_2nd)
                    {
                        solutionProgression++;
                    }
                    else
                        DoLoose();
                }
                break;
            case 2:
                {
                    if ((_switch.InputData as PuzzleLockCode_KeyData).keyID == chosenSetup.Seq_3rd)
                    {
                        solutionProgression++;
                        CheckIfSolved();
                    }
                    else
                        DoLoose();
                }
                break;
            default:
                CheckIfSolved();
                break;
        }

        if (SolutionState != PuzzleState.Solved)
            selectable.Select();
    }

    public void OnUpdateSelectable(IPuzzleInput _input)
    {
        throw new System.NotImplementedException();
    }

    public enum KeyOrder
    {
        Key1st,
        Key2nd,
        Key3rd
    }

    public class PuzzleLockCode_KeyData: IPuzzleInputData
    {
        public KeyOrder keyID;
    }
}
