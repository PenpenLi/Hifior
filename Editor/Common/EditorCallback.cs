//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.Experimental.SceneManagement;
//using UnityEngine;

//[InitializeOnLoad]
//public class EditorCallback
//{
//    // Start is called before the first frame update
//    static EditorCallback()
//    {
//        PrefabStage.prefabStageOpened += OnPrefabStageOpened;
//        PrefabStage.prefabSaved += OnPrefabStageSaved;
//        UnityEditor.PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdate;
//    }

//    private static void OnPrefabInstanceUpdate(GameObject obj)
//    {
//        SLGChapter chapter = obj.GetComponent<SLGChapter>();
//        if (chapter != null)
//        {
//            chapter.CheckSequenceName();
//            foreach (var v in chapter.EventInfo.BattleTalkEvent) { v.SequenceName = v.Sequence.SequenceName; }
//            foreach (var v in chapter.EventInfo.EnemiesLessEvent) { v.SequenceName = v.Sequence.SequenceName; }
//            foreach (var v in chapter.EventInfo.EnemyDieEvent) { v.SequenceName = v.Sequence.SequenceName; }
//            foreach (var v in chapter.EventInfo.LocationEvent) { v.SequenceName = v.Sequence.SequenceName; }
//            foreach (var v in chapter.EventInfo.RangeEvent) { v.SequenceName = v.Sequence.SequenceName; }
//            foreach (var v in chapter.EventInfo.TurnEvent) { v.SequenceName = v.Sequence.SequenceName; }
//            Debug.Log("Change SequenceName" + obj.name);
//        }
//    }

//    private static void OnPrefabStageSaved(GameObject obj)
//    {

//    }

//    static void OnPrefabStageOpened(PrefabStage prefabStage)
//    {
//        //Debug.Log("OnPrefabStageOpened " + prefabStage.prefabAssetPath);
//    }
//}
