using UI;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    public uint id;
    public uint clip;
    public uint maxClip;
    public uint speed;
    public uint damage;
    public GameObject bullet;
    public Transform shootingPoint;
    public Vector3 offset;
    public Vector3 scale;
    public float timeBeforeNextShot;
    private float _timer;
    private void Awake()
    {
        _timer = timeBeforeNextShot;
        var transform1 = transform;
        transform1.localScale = scale;
        transform1.position += offset;
        clip = maxClip;
    }

    private void Start()
    {
        UI_InGame.Instance.ammoText.enabled = true;
        GameEvents.Current.OnFire += Fire;
        RefreshHud();
    }

    private void RefreshHud()
    {
        UI_InGame.Instance.ammoText.text = $"{clip} / {maxClip} CLIPS";
    }

    public bool AddClip(uint pClip)
    {
        if (clip >= maxClip) return false;
        var sum = clip + pClip;

        if (sum < maxClip)
        {
            clip += pClip;
        }
        else
        {
            clip += maxClip - clip;
        }
        RefreshHud();
        return true;

    }
    private void Fire()
    {
        if (clip <= 0) return;
        if (_timer < timeBeforeNextShot) return;
        var bulletScript = Instantiate(bullet, shootingPoint.position, Quaternion.identity).GetComponent<Bullet>();
        clip--;
        bulletScript.damage = damage;
        bulletScript.speed = speed;
        bulletScript.direction = shootingPoint.right * transform.parent.localScale.x;
        _timer = 0.0f;
        RefreshHud();
    }
    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
    }
}
