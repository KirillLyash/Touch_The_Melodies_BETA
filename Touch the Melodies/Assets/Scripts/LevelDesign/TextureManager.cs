using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
    public static TextureManager instance;
    [SerializeField]
    private UnityEngine.UI.RawImage Background;

    private void Awake()
    {
        instance = this;
    }
    public void EnemyChangeTextures(int themeID)
    {
        Material Enemy = Resources.Load<Material>($"Materials/Enemy");
        Material General = Resources.Load<Material>($"Materials/General");
        Material Fragile = Resources.Load<Material>($"Materials/Fragile");
        Material FragileActive = Resources.Load<Material>($"Materials/FragileActive");
        Material Mover = Resources.Load<Material>($"Materials/Mover");
        Material MoverAuto = Resources.Load<Material>($"Materials/MoverAuto");
        Enemy.mainTexture = Resources.Load<Texture2D>($"Textures/Enemy{themeID}");
        General.mainTexture = Resources.Load<Texture2D>($"Textures/General{themeID}");
        Fragile.mainTexture = Resources.Load<Texture2D>($"Textures/Fragile{themeID}");
        FragileActive.mainTexture = Resources.Load<Texture2D>($"Textures/FragileActive{themeID}");
        Mover.mainTexture = Resources.Load<Texture2D>($"Textures/Mover{themeID}");
        MoverAuto.mainTexture = Resources.Load<Texture2D>($"Textures/MoverAuto{themeID}");
        Background.texture = Resources.Load<Texture2D>($"Textures/Background{themeID}");
    }
}
