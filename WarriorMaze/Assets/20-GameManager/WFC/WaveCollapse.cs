using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollapse : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private WcTileModel[] models;

    [SerializeField] private GameObject error;

    [SerializeField] private GameObject blank;

    private WcTileManager tileMngr;

    private WcAlgorithm algor;

    // Start is called before the first frame update
    void Start()
    {
        tileMngr = new WcTileManager(width, height, models, error, blank);

        algor = new WcAlgorithm(tileMngr);
    }

    public void StartWaveFunctionsCollapse()
    {
        algor.RunAlgorithm(100);
    }
}
