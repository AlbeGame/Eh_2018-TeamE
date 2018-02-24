﻿using UnityEngine;

public class PuzzleGPS : SelectableItem, IPuzzle
{
    public PuzzleGPSData Data;
    public GPS_IO Interactables;

    Vector2Int solutionCoordinates;
    float solutionOrientation;

    SelectableMonitor currentSelectedMonitor;

    #region IPuzzle
    PuzzleState _solutionState = PuzzleState.Unsolved;
    public PuzzleState SolutionState
    {
        get
        {
            return _solutionState;
        }

        set
        {
            _solutionState = value;
            OnSolutionStateChange(_solutionState);
        }
    }

    public void Setup(IPuzzleData _data)
    {
        Data = _data as PuzzleGPSData;
    }

    public void OnButtonSelect(SelectableButton _button)
    {
        PuzzleGPSNumericData data = _button.InputData as PuzzleGPSNumericData;
        PuzzleGPSMonitorData coordinateData = currentSelectedMonitor.InputData as PuzzleGPSMonitorData;

        if(data.ActualValue < 10)
        {
            coordinateData.coordinateValue *= 10;
            coordinateData.coordinateValue += data.ActualValue;
            currentSelectedMonitor.TypeOn(coordinateData.coordinateValue.ToString());
        }
        else if(data.ActualValue == 10)
        {
            coordinateData.coordinateValue /= 10;
            currentSelectedMonitor.TypeOn(coordinateData.coordinateValue.ToString());
        }
        else if(data.ActualValue == 11)
        {
            CheckSolution();
        }
    }
    public void OnSwitchSelect(SelectableSwitch _switch) { }
    public void OnMonitorSelect(SelectableMonitor _monitor) {
        if(_monitor == Interactables.Latitude)
        {
            currentSelectedMonitor = Interactables.Latitude;
        }
        else if (_monitor == Interactables.Longitude)
        {
            currentSelectedMonitor = Interactables.Longitude;
        }

        this.Select(true);
    }

    public void OnUpdateSelectable(SelectableAbstract _selectable)
    {
        //CurrentSelectedMonitor deve lampeggiare.
    }
    #endregion

    protected override void OnInitEnd(SelectableAbstract _parent)
    {
        Init();
    }

    protected override void OnStateChange(SelectionState _state)
    {
        if(SolutionState == PuzzleState.Unsolved)
            base.OnStateChange(_state);
    }

    void OnSolutionStateChange(PuzzleState _solutionState)
    {
        graphicCtrl.Paint(_solutionState);
    }

    public void Init()
    {
        GenerateRandomCombination();
        InitOutputMonitor();
        InitNumerics();
        InitSelectableMonitors();
    }

    void InitNumerics()
    {
        for (int i = 0; i < Interactables.NumericalButtons.Length; i++)
        {
            Interactables.NumericalButtons[i].Init(this);
            Interactables.NumericalButtons[i].DataInjection(new PuzzleGPSNumericData() { ActualValue = i });
        }
    }

    void InitSelectableMonitors()
    {
        Interactables.Latitude.Init(this);
        Interactables.Latitude.DataInjection(new PuzzleGPSMonitorData());
        Interactables.Longitude.Init(this);
        Interactables.Longitude.DataInjection(new PuzzleGPSMonitorData());

        ////DEBUG POURPOSE ONLY
        //Interactables.Latitude.TypeOn(solutionCoordinates.x.ToString());
        //Interactables.Longitude.TypeOn(solutionCoordinates.y.ToString());
        ////---------

        currentSelectedMonitor = Interactables.Latitude;
    }

    void InitOutputMonitor()
    {
        Interactables.OutputMonitor.Init(Data.Grid);
        Interactables.OutputMonitor.DisplayAndRotate(solutionCoordinates, solutionOrientation);
    }

    void GenerateRandomCombination()
    {
        int randCoordIndex = Random.Range(0, Data.PossibleCoordinates.Count);
        solutionCoordinates = Data.PossibleCoordinates[randCoordIndex];

        int randOrient = Random.Range(0, 4);
        solutionOrientation = randOrient * 90;
    }

    void CheckSolution()
    {
        int latitude = (Interactables.Latitude.InputData as PuzzleGPSMonitorData).coordinateValue;
        int longitude = (Interactables.Longitude.InputData as PuzzleGPSMonitorData).coordinateValue;

        if (solutionCoordinates.x == latitude && solutionCoordinates.y == longitude)
            DoWinningThings();
        else
            DoBreakingThings();
    }

    void DoWinningThings()
    {
        Parent.Select(true);
        SolutionState = PuzzleState.Solved;
        graphicCtrl.Paint(_solutionState);
        State = SelectionState.Unselectable;
    }

    void DoBreakingThings()
    {
        SolutionState = PuzzleState.Broken;
        graphicCtrl.Paint(_solutionState);
    }

    [System.Serializable]
    public class GPS_IO
    {
        public SelectableButton[] NumericalButtons = new SelectableButton[12];
        public SelectableMonitor Latitude;
        public SelectableMonitor Longitude;
        public PuzzleGPSOutputMonitor OutputMonitor;
    }
}
