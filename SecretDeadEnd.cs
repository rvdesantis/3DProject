using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDeadEnd : DunCube
{
    public SecretWall secretWall;

    private void Start()
    {
        dunBuilder = FindObjectOfType<DunBuilder>();
        secretWall.areaController = FindObjectOfType<AreaController>();
    }
}
