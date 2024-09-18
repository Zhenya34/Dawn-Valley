using UnityEngine;

[CreateAssetMenu(fileName = "New Seed", menuName = "Seed")]
public class Seed : ScriptableObject
{
    public string seedName;
    public Sprite seedSprite;
    public Plant plant;
}

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class Plant : ScriptableObject
{
    public string plantName;
    public Sprite plantSprite;
    public GameObject plantPrefab;
}