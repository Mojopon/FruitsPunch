using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ScorePanel))]
public class UpdateScorePanelOnInspectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var scorePanel = target as ScorePanel;
        scorePanel.UpdateScore();
    }
}
