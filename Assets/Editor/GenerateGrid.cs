using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GameBoard))]
public class GenerateGrid : Editor
{   /*
    Editor file for generate grid
     */
    string _prefabAssetPath = "Assets/Prefabs/BoardGrid.prefab";

    GameObject _gridPrefab;
    private GameBoard gameBoard;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Grids"))
        {
            _generateGrid();
        }
    }

    private void _generateGrid()
    {
        gameBoard = target as GameBoard;
        _gridPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(_prefabAssetPath);

        //Destroy oringal ones
        foreach (Transform childTransform in gameBoard.gameObject.transform)
        {
            UnityEditor.EditorApplication.delayCall += () => { GameObject.DestroyImmediate(childTransform.gameObject); };
        }


        float boardLeft = GFractalArt.boardCenter.x - GFractalArt.boardWidth / 2;
        float boardRight = GFractalArt.boardCenter.x + GFractalArt.boardWidth / 2;

        float boardTop = GFractalArt.boardCenter.y + GFractalArt.boardHeight / 2;
        float boardBottom = GFractalArt.boardCenter.y - GFractalArt.boardHeight / 2;

        float step = GFractalArt.boardWidth / GFractalArt.gridCountPerLine;
        float z = gameBoard.transform.position.z;

        Debug.Log("Editor");
        for (float x = boardLeft + step / 2; x <= boardRight - step / 2; x += step)
        {
            for (float y = boardBottom + step / 2; y <= boardTop - step / 2; y += step)
            {

                GameObject gridObject = Instantiate(_gridPrefab, gameBoard.transform);

                Debug.Log("Editor" + gridObject.transform.position);
                gridObject.transform.position = new Vector3(x, y, 0.1f);
            }
        }
    }
}
