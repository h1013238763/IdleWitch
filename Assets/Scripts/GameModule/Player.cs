using UnityEngine;

public class Player : MgrBaseMono<Player> {
    
    public bool has_core = false;
    public bool has_placed = false;

    public bool is_initial = true;

    public IInteractable interactable_object = null;

    public GameObject player_object;
    public GameObject core_object;

    public bool is_moving = false;
    public bool interactable = true;

    public Vector3 target_position;
    public float move_speed = 4.5f;
    private Rigidbody2D rb;

    public AudioSource audio_click = null;
    public AudioSource audio_walk = null;

    void Start() {
        SetPlayer();
    }

    public void Update() { 
        if (Input.GetMouseButtonDown(0)) {
            SetMove();
        }
        core_object.SetActive(has_core);
        Move();
    }

    public void SetMove() {
        MgrAudio.Mgr().PlayAudio(AudioClass.Sound, "click", false, (audio) => {
            audio_click = audio;
        });

        if( !interactable ) return;
        
        MgrAudio.Mgr().StopAudio(AudioClass.Sound, audio_walk);
        MgrAudio.Mgr().PlayAudio(AudioClass.Sound, "walk", true, (audio) => {
            audio_walk = audio;
        });
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target_position = new Vector3(mousePosition.x, mousePosition.y, 0);
        is_moving = true;
    }

    public void Move() {
        if (is_moving) {
            Vector2 direction = (target_position - player_object.transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * move_speed, rb.linearVelocity.y);
        }

        if (Mathf.Abs(player_object.transform.position.x - target_position.x) < 0.1f)
        {
            MgrAudio.Mgr().StopAudio(AudioClass.Sound, audio_walk);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            is_moving = false;
        }
    }

    public void Interact() {
        if (interactable_object != null) {
            interactable_object.Interact();
        }
    }

    public void SetPlayer(float x = -999f) {
        if (is_initial)
            MgrAudio.Mgr().PlayAudio(AudioClass.Music, "bgm", true);
        is_initial = false;
        player_object = GameObject.Find("Player");
        core_object = player_object.transform.Find("Circle").gameObject;
        rb = player_object.transform.GetComponent<Rigidbody2D>();
        target_position.x = player_object.transform.position.x;
        rb.linearVelocity = new Vector2(0, 0);

        if (x != -999f) {
            target_position.x = x;
            player_object.transform.position = new Vector3(x, -1.8f, 0);
        }
    }
}