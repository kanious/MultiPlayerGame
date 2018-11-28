using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public const int maxHealth = 100;
    public bool destroyOnDeath = false;

    public int currentHealth = maxHealth;
    public bool isEnemy = false;

    public RectTransform healthBar;

    private bool isLocalPlayer;

	void Start () {
        PlayerController pc = GetComponent<PlayerController>();
        isLocalPlayer = pc.isLocalPlayer;
	}

    public void TakeDamage(GameObject playerFrom, int amount)
    {
        currentHealth -= amount;
        OnChangeHealth();

        NetworkManager._instance.GetComponent<NetworkManager>().CommandHealthChange(
            playerFrom, this.gameObject, amount, isEnemy);
    }

    public void OnChangeHealth()
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
                Destroy(gameObject);
            else
            {
                currentHealth = maxHealth;
                healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
                Respawn();
            }
        }
    }

    private void Respawn()
    {
        if(isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;
            Quaternion spawnRotation = Quaternion.Euler(0f, 180f, 0f);
            transform.position = spawnPoint;
            transform.rotation = spawnRotation;
        }
    }
}
