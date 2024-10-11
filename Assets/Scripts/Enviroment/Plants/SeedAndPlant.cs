using UnityEngine;

[CreateAssetMenu(fileName = "SeedData", menuName = "ScriptableObjects/Seed")]
public class Seed : ScriptableObject
{
    public string seedName;
    public Sprite seedSprite;
    public Plant plant;
}

[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/Plant")]
public class Plant : ScriptableObject
{
    public string plantName;
    public GameObject plantPrefab;
}