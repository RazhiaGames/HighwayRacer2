//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HR_ResetDecal : MonoBehaviour {

    public float lifeTime = 2f;
    private float timer = 0f;
    private Material mat;

    private void OnEnable() {

        mat = GetComponentInChildren<DecalProjector>().material;
        timer = 0f;

    }

    private void Update() {

        // Calculate the ping-pong value based on time
        float t = Mathf.PingPong(Time.time / .2f, 1f);

        // Interpolate between the two colors
        mat.SetColor("_BaseColor", Color.Lerp(Color.red, new Color(0f, 0f, 0f, 0f), t));

        timer += Time.deltaTime;

        if (timer >= lifeTime)
            Destroy(gameObject);

    }

}
