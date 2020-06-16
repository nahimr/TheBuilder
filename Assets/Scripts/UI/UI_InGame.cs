using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_InGame : MonoBehaviour
    {
        [Header("HUD")] 
        public GameObject hud;
        public Slider healthBar;
        public Slider staminaBar;
       
        public Text scoreText;
        public Text timerText;
        public Text ammoText;
        public Button pauseButton;

        [Header("Pause Menu")] 
        public GameObject pauseMenu;
        public Button resumeButton;
        public Button retryButton;
        public Button exitGameButton;
    
        [Header("Game Finished")] 
        public GameObject gameFinished;
        public Text finalScoreText;
        public Text finalTimeText;
        public Text resultText;
        public GameObject ad;
        public Button finalRetryButton;
        public Button finalSettingsButton;
        public Button finalExitButton;

        [Header("Mount")] 
        public Slider jetpackBar;
        public Image jetpackBarImage;
        
        [Header("Other")] 
        public GameObject joysticks;
        public static UI_InGame Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }
    }
}
