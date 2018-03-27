﻿using UnityEngine;

public class PuzzleTurbineButtonReset : ISelectableBehaviour {

    PuzzleTurbine puzzleCtrl;

    public PuzzleTurbineButtonReset(PuzzleTurbine _puzzleCtrl)
    {
        puzzleCtrl = _puzzleCtrl;
    }

    public void OnInit(SelectableAbstract _holder) { }

    void ISelectableBehaviour.OnSelect()
    {
        if (puzzleCtrl != null)
            puzzleCtrl.CheckIfSolved();

        puzzleCtrl.Select(true);
    }
}