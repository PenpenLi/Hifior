using UnityEngine;
using RPG.UI;

namespace Sequence
{
    [AddComponentMenu("Sequence/Say")]
    public class Say : SequenceEvent
    {
        // Removed this tooltip as users's reported it obscures the text box
        [TextArea(5, 10)]
        public string storyText = "";

        [Tooltip("Notes about this story text for other authors, localization, etc.")]
        public string description = "";

        [Tooltip("Character that is speaking")]
        public int characterID;

        [Tooltip("Portrait that represents speaking character")]
        public Sprite portrait;

        [Tooltip("Voiceover audio to play when writing the text")]
        public AudioClip voiceOverClip;

        [Tooltip("Always show this Say text when the command is executed multiple times")]
        public bool showAlways = true;

        [Tooltip("Number of times to show this Say text when the command is executed multiple times")]
        public int showCount = 1;

        [Tooltip("Type this text in the previous dialog box.")]
        public bool extendPrevious = false;

        [Tooltip("Fade out the dialog box when writing has finished and not waiting for input.")]
        public bool fadeWhenDone = true;

        [Tooltip("Wait for player to click before continuing.")]
        public bool waitForClick = true;

        [Tooltip("Sets the active Say dialog with a reference to a Say Dialog object in the scene. All story text will now display using this Say Dialog.")]
        public SayDialog setSayDialog;

        protected int executionCount;

        public override void OnEnter()
        {
            if (!showAlways && executionCount >= showCount)
            {
                Continue();
                return;
            }

            executionCount++;

            // Override the active say dialog if needed
            if (setSayDialog != null)
            {
                SayDialog.activeSayDialog = setSayDialog;
            }

            SayDialog sayDialog = SayDialog.GetSayDialog();

            if (sayDialog == null)
            {
                Continue();
                return;
            }

            sayDialog.SetCharacter(characterID);
            sayDialog.SetCharacterImage(portrait);

            sayDialog.Say(storyText, !extendPrevious, waitForClick, fadeWhenDone, voiceOverClip, delegate
            {
                sayDialog.Hide();
                Continue();
            });
        }

        public override void OnReset()
        {
            executionCount = 0;
        }

        public override bool OnStopExecuting()
        {
            SayDialog sayDialog = SayDialog.GetSayDialog();
            if (sayDialog != null)
            {
                sayDialog.Stop();
            }
            return true;
        }

        public string GetStandardText()
        {
            return storyText;
        }

        public void SetStandardText(string standardText)
        {
            storyText = standardText;
        }

        public virtual string GetDescription()
        {
            return description;
        }
    }
}