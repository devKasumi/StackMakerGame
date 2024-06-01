using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private bool isBridgeBrick;

    private MeshRenderer brickMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        brickMeshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (isBridgeBrick)
            {
                Bridge bridge = GameObject.FindGameObjectWithTag("BridgeBrick").GetComponent<Bridge>();
                if (!brickMeshRenderer.enabled)
                {
                    brickMeshRenderer.enabled = true;
                    player.RemoveBrick();
                    bridge.AddBrick(this.gameObject.GetComponent<Brick>());
                    if (bridge.NotEnoughBrick() && player.GetListBrickCount() == 0)
                    {
                        player.StopMoving(this.gameObject.GetComponent<Brick>());
                        UIManager.GetInstance.ShowLoseUI();
                    }
                }
            }
            else
            {
                player.AddBrick(this.gameObject.GetComponent<Brick>());
            }
        }
    }
}
