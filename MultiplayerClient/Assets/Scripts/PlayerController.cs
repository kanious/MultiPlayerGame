using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public bool isLocalPlayer = false;

    Vector3 oldPosition;
    Vector3 currentPosition;
    Quaternion oldRotation;
    Quaternion currentRotation;

	void Start () {
        oldPosition = transform.position;
        currentPosition = oldPosition;
        oldRotation = transform.rotation;
        currentRotation = oldRotation;
	}

    void OnApplicationQuit()
    {
        if (isLocalPlayer)
        {
            NetworkManager._instance.GetComponent<NetworkManager>().CommandQuit();
        }
    }

    void Update () {

        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3f;

        transform.Rotate(0f, x, 0f);
        transform.Translate(0f, 0f, z);

        currentPosition = transform.position;
        currentRotation = transform.rotation;

        if (currentPosition != oldPosition)
        {
            NetworkManager._instance.GetComponent<NetworkManager>().CommandMove(transform.position);
            oldPosition = currentPosition;
        }

        if(currentRotation != oldRotation)
        {
            NetworkManager._instance.GetComponent<NetworkManager>().CommandRotate(transform.rotation);
            oldRotation = currentRotation;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            NetworkManager._instance.GetComponent<NetworkManager>().CommandShoot();
        }
	}

    public void CmdFire()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        Bullet b = bullet.GetComponent<Bullet>();
        b.playerFrom = this.gameObject;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.up * 6f;

        Destroy(bullet, 2f);
    }

}
