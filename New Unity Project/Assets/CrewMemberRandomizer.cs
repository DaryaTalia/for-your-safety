using System.Collections.Generic;
using UnityEngine;

public class CrewMemberRandomizer : MonoBehaviour
{
    [SerializeField]
    GameObject CrewMemberPrefab;

    [SerializeField]
    List<Transform> CrewMemberPositions;

    [SerializeField]
    List<Sprite> CrewMemberSprites;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeSprites();
    }

    void RandomizeSprites()
    {
        foreach(Transform go in CrewMemberPositions)
        {
            GameObject crew = Instantiate(CrewMemberPrefab, go, true);
            crew.transform.localPosition = new Vector3(0,0,0);
            crew.transform.localRotation = Quaternion.Euler(0, 0, 0);
            crew.GetComponentInChildren<SpriteRenderer>().sprite = CrewMemberSprites[Random.Range(0, CrewMemberSprites.Count)];
        }
    }
}
