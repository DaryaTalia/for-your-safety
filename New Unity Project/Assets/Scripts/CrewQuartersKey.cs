using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewQuartersKey : MonoBehaviour
{
    [SerializeField]
    GameObject KeycardPrefab;

    [SerializeField]
    List<Transform> CrewMemberPositions;

    int randomCrew;

    // Start is called before the first frame update
    void Start()
    {
        randomCrew = Random.Range(0, CrewMemberPositions.Count);
        Instantiate(KeycardPrefab, CrewMemberPositions[randomCrew].transform);        
    }
}
