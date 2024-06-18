using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathAssignment : MonoBehaviour
{
    private int assignedPath;

    public void setAssignedPath(int pathNumber)
    {
        assignedPath = pathNumber;
    }
    public int GetAssignedPath() => assignedPath; 

}
