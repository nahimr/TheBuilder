using UnityEngine;
public class SmartphoneJoysticks : MonoBehaviour
{
    public FixedJoystick moveStick;
    public JoystickButton jumpButton;
    public JoystickButton takeButton;
    public JoystickButton fireButton;
    public JoystickButton specialButton;
    public static SmartphoneJoysticks Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }
}
