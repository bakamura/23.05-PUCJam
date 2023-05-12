public class P_Audio : AudioHandler {

    private const string P_JUMP = "P_Jump";
    private const string P_HEAL = "P_Heal";
    private const string P_DODGE = "P_Dodge";

    private void Start() {
        P_Movement.Instance.OnJump.AddListener(JumpSFX);
        P_Ability.Instance.OnHeal.AddListener(HealSFX);
        P_Ability.Instance.OnDodge.AddListener(JumpSFX);
    }

    private void JumpSFX() {
        PlaySound(P_JUMP);
    }

    private void HealSFX() {
        PlaySound(P_HEAL);
    }

    private void DodgeSFX() {
        PlaySound(P_DODGE);
    }
}