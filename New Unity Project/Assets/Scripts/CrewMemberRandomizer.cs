using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrewMemberRandomizer : MonoBehaviour
{
    [SerializeField]
    GameObject CrewMemberPrefab;

    [SerializeField]
    List<Transform> CrewMemberPositions;

    [SerializeField]
    List<Sprite> CrewMemberSprites;

    [SerializeField]
    CrewQuartersKey crewQuartersKey;

    int crewDiscovered;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> crewTextList;

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

        crewQuartersKey.enabled = true;
    }

    public void ToggleNextText()
    {
        if(gameObject.GetComponentInChildren<TextMeshProUGUI>(true))
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>(true).enabled = true;
            Debug.Log(crewTextList[crewDiscovered]);
            gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = crewTextList[crewDiscovered];
            crewDiscovered++;
            gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
            Destroy(gameObject.GetComponentInChildren<TextMeshProUGUI>().gameObject, 3);

            if(crewDiscovered >= 4)
            {
                Instantiate(GameManager.Instance.KeycardPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            }
        }        
    }
}
