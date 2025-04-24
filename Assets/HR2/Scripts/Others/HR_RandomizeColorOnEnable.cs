//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Randomize the color of the GameObject whenever it is enabled
/// </summary>
public class HR_RandomizeColorOnEnable : MonoBehaviour {

    [System.Serializable]
    public class PaintableRenderer {

        public MeshRenderer meshRenderer;
        public int materialIndex = 0;
        public string shaderKeyword = "_BaseColor";

    }

    public PaintableRenderer[] paintableRenderers;

    /// <summary>
    /// Randomize the color of the GameObject whenever it is enabled
    /// </summary>
    private void OnEnable() {

        // Randomize the color of the GameObject whenever it is enabled
        for (int i = 0; i < paintableRenderers.Length; i++) {

            if (paintableRenderers[i].meshRenderer != null) {

                Color randomColor = GetRandomColor();
                paintableRenderers[i].meshRenderer.materials[paintableRenderers[i].materialIndex].SetColor(paintableRenderers[i].shaderKeyword, randomColor);

            }

        }

    }

    /// <summary>
    /// Generate a random color
    /// </summary>
    /// <returns></returns>
    private Color GetRandomColor() {

        Color randomColor;

        do {

            // Generate a random color
            randomColor = new Color(Random.value, Random.value, Random.value);

        }

        // Continue generating a new color if it is too close to pink/magenta
        while (IsColorCloseToMagenta(randomColor) || IsColorTooBright(randomColor));

        return randomColor;

    }

    /// <summary>
    /// Pink/Magenta colors are typically defined by having high red and blue values and low green values.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private bool IsColorCloseToMagenta(Color color) {

        // Pink/Magenta colors are typically defined by having high red and blue values and low green values.
        // We can exclude colors that fall within this range.
        return color.r > 0.5f && color.b > 0.5f && color.g < 0.3f;

    }

    /// <summary>
    /// Calculate brightness using a common formula (average of the max and min RGB components)
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private bool IsColorTooBright(Color color) {

        // Calculate brightness using a common formula (average of the max and min RGB components)
        float brightness = (color.r + color.g + color.b) / 3f;
        return brightness > .65f;

    }

    private void OnValidate() {

        if (paintableRenderers == null)
            return;

        for (int i = 0; i < paintableRenderers.Length; i++) {

            if (paintableRenderers[i] != null) {

                if (paintableRenderers[i].shaderKeyword == "")
                    paintableRenderers[i].shaderKeyword = "_BaseColor";

            }

        }

    }

}
