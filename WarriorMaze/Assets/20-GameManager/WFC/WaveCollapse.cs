using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollapse : MonoBehaviour
{
    [SerializeField] private WcTileModel[] models;

    private WcTileManager tileMngr;

    private WcAlgorithm algor;

    // Start is called before the first frame update
    void Start()
    {
        tileMngr = new WcTileManager(models);

        algor = new WcAlgorithm(tileMngr, 2, 2);

        StartWaveFunctionsCollapse();
    }

    private void StartWaveFunctionsCollapse()
    {
        algor.RunAlgorithm();
    }
}
