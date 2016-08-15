using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
namespace RPG.UI
{
    public class SayDialog : IPanel
    {
        // Currently active Say Dialog used to display Say text
        public static SayDialog activeSayDialog;

        // Most recent speaking character
        public static CharacterDef speakingCharacter;

        public float fadeDuration = 0.25f;

        public Button continueButton;
        public Text nameText;
        public Text storyText;
        public Image characterImage;

        [Tooltip("Adjust width of story text when Character Image is displayed (to avoid overlapping)")]
        public bool fitTextWithImage = true;

        protected float startStoryTextWidth;
        protected float startStoryTextInset;

        protected WriterAudio writerAudio;
        protected Writer writer;
        protected CanvasGroup canvasGroup;

        protected bool fadeWhenDone = true;
        protected float targetAlpha = 0f;
        protected float fadeCoolDownTimer = 0f;
        public override void Show()
        {
            base.Show();
            continueButton.Select();
        }
        public static SayDialog GetSayDialog()
        {
            return UIController.Instance.GetUI<SayDialog>();
        }
        protected Writer GetWriter()
        {
            if (writer != null)
            {
                return writer;
            }

            writer = GetComponent<Writer>();
            if (writer == null)
            {
                writer = gameObject.AddComponent<Writer>();
            }

            return writer;
        }

        protected CanvasGroup GetCanvasGroup()
        {
            if (canvasGroup != null)
            {
                return canvasGroup;
            }

            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            return canvasGroup;
        }

        protected WriterAudio GetWriterAudio()
        {
            if (writerAudio != null)
            {
                return writerAudio;
            }

            writerAudio = GetComponent<WriterAudio>();
            if (writerAudio == null)
            {
                writerAudio = gameObject.AddComponent<WriterAudio>();
            }

            return writerAudio;
        }

        protected void Start()
        {
            // Dialog always starts invisible, will be faded in when writing starts
            GetCanvasGroup().alpha = 0f;

            // Add a raycaster if none already exists so we can handle dialog input
            GraphicRaycaster raycaster = GetComponent<GraphicRaycaster>();
            if (raycaster == null)
            {
                gameObject.AddComponent<GraphicRaycaster>();
            }
        }

        public virtual void Say(string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, AudioClip voiceOverClip, Action onComplete)
        {
            Show();
            StartCoroutine(SayInternal(text, clearPrevious, waitForInput, fadeWhenDone, voiceOverClip, onComplete));
        }

        protected virtual IEnumerator SayInternal(string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, AudioClip voiceOverClip, Action onComplete)
        {
            Writer writer = GetWriter();

            // Stop any existing Say Command and write this one instead
            // This will probably take a frame or two to complete
            while (writer.isWriting || writer.isWaitingForInput)
            {
                writer.Stop();
                yield return null;
            }

            this.fadeWhenDone = fadeWhenDone;

            // Voice over clip takes precedence over a character sound effect if provided

            AudioClip soundEffectClip = null;
            if (voiceOverClip != null)
            {
                WriterAudio writerAudio = GetWriterAudio();
                writerAudio.PlayVoiceover(voiceOverClip);
            }
            else if (speakingCharacter != null)
            {
                //soundEffectClip = speakingCharacter.soundEffect;
            }
            writer.Write(text, clearPrevious, waitForInput, soundEffectClip, onComplete);

        }

        protected virtual void LateUpdate()
        {
            UpdateAlpha();

            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(GetWriter().isWaitingForInput);
            }
        }

        /**
         * Tell dialog to fade out if it's finished writing.
         */
        public virtual void FadeOut()
        {
            fadeWhenDone = true;
        }

        /**
         * Stop a Say Dialog while its writing text.
         */
        public virtual void Stop()
        {
            fadeWhenDone = true;
            GetWriter().Stop();
        }

        protected virtual void UpdateAlpha()
        {
            if (GetWriter().isWriting)
            {
                targetAlpha = 1f;
                fadeCoolDownTimer = 0.1f;
            }
            else if (fadeWhenDone && fadeCoolDownTimer == 0f)
            {
                targetAlpha = 0f;
            }
            else
            {
                // Add a short delay before we start fading in case there's another Say command in the next frame or two.
                // This avoids a noticeable flicker between consecutive Say commands.
                fadeCoolDownTimer = Mathf.Max(0f, fadeCoolDownTimer - Time.deltaTime);
            }

            CanvasGroup canvasGroup = GetCanvasGroup();
            float fadeDuration = GetSayDialog().fadeDuration;
            if (fadeDuration <= 0f)
            {
                canvasGroup.alpha = targetAlpha;
            }
            else
            {
                float delta = (1f / fadeDuration) * Time.deltaTime;
                float alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, delta);
                canvasGroup.alpha = alpha;

                if (alpha <= 0f)
                {
                    // Deactivate dialog object once invisible
                    gameObject.SetActive(false);
                }
            }
        }

        public virtual void SetCharacter(int CharacterID,int FaceID=0)
        {
            CharacterDef def = ResourceManager.GetPlayerDef(CharacterID);
            if (def == null)
            {
                if (characterImage != null)
                {
                    characterImage.gameObject.SetActive(false);
                }
                if (nameText != null)
                {
                    nameText.text = "";
                }
                speakingCharacter = null;
            }
            else
            {
                speakingCharacter = def;
                string characterName = def.CommonProperty.Name;

                SetCharacterImage(def.TalkPortrait[FaceID]);
                SetCharacterName(characterName,Color.blue);
            }
        }

        public virtual void SetCharacterImage(Sprite image)
        {
            if (characterImage == null)
            {
                return;
            }

            if (image != null)
            {
                characterImage.sprite = image;
                characterImage.gameObject.SetActive(true);
            }
            else
            {
                characterImage.gameObject.SetActive(false);

                if (startStoryTextWidth != 0)
                {
                    storyText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                                                                          startStoryTextInset,
                                                                          startStoryTextWidth);
                }
            }

            // Adjust story text box to not overlap image rect
            if (fitTextWithImage &&
                storyText != null &&
                characterImage.gameObject.activeSelf)
            {
                if (startStoryTextWidth == 0)
                {
                    startStoryTextWidth = storyText.rectTransform.rect.width;
                    startStoryTextInset = storyText.rectTransform.offsetMin.x;
                }

                // Clamp story text to left or right depending on relative position of the character image
                if (storyText.rectTransform.position.x < characterImage.rectTransform.position.x)
                {
                    storyText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                                                                          startStoryTextInset,
                                                                          startStoryTextWidth - characterImage.rectTransform.rect.width);
                }
                else
                {
                    storyText.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                                                                          startStoryTextInset,
                                                                          startStoryTextWidth - characterImage.rectTransform.rect.width);
                }
            }
        }

        public virtual void SetCharacterName(string name, Color color)
        {
            if (nameText != null)
            {
                nameText.text = name;
                nameText.color = color;
            }
        }

        public virtual void Clear()
        {
            ClearStoryText();

            // Kill any active write coroutine
            StopAllCoroutines();
        }

        protected virtual void ClearStoryText()
        {
            if (storyText != null)
            {
                storyText.text = "";
            }
        }
        public override void OnSubmitKeyDown()
        {
            writer.OnNextLineEvent();
        }
    }
}