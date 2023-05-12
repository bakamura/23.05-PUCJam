public class P_Audio : AudioHandler {

    private const string P_JUMP = "P_Jump";


    private void Start() {
        P_Movement.Instance.OnJump.AddListener(JumpSFX);
    }

    private void JumpSFX() {
        PlaySound(P_JUMP);
    }
}