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

    static int crewDiscovered;

    [SerializeField]
    [TextArea(2, 5)]
    List<string> crewTextList;

    bool textDisabled;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeSprites();
        textDisabled = false;
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

    public void ToggleNextText(Collider crew)
    {
        if(crew.gameObject.GetComponentInChildren<TextMeshProUGUI>(true) && !textDisabled)
        {
            crew.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).gameObject.SetActive(true);
            Debug.Log(crewTextList[crewDiscovered]);
            crew.gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text = crewTextList[crewDiscovered];
            crewDiscovered++;
            crew.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
            Destroy(crew.gameObject.GetComponentInChildren<TextMeshProUGUI>().gameObject, 7);

            if(crewDiscovered >= 4)
            {
                Instantiate(GameManager.Instance.KeycardPrefab, crew.gameObject.transform.position, Quaternion.identity, crew.gameObject.transform);
                GameManager.Instance.keyFound = true;
                textDisabled = true;
            }
        }        
    }
}
