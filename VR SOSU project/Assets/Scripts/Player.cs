using UnityEngine;
using LLMUnity;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public DictationScript dictationScript;

    //public LLM llm;
    public LLMCharacter llm;
    public Rigidbody m_Rigidbody;

    [Header ("ChatBox")]
    public string username;
    public GameObject chatPanel, textObject, chatBox;
    public TMP_InputField chatField;
    public Color playerMessage, tutorMessage, info;
    private bool chatIsActive = false;
    private int maxMessages = 25;

    private bool isLLMProcessing = false;
    private float dotAnimationTimer = 0f;
    private string typingIndicatorId = "typing_indicator";
    private bool hasLLMResponded = false;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void HandleReply(string reply)
    {
        if (!hasLLMResponded)
        {
            RemoveTypingIndicator();
            Debug.Log(reply);
            SendMessageToChat("Tutor: " + reply, Message.MessageType.tutorMessage);
            hasLLMResponded = true;
        }
    }

    private void HandleDictationResult(string text)
    {
        SendMessageToChat(username + ": " + text, Message.MessageType.playerMessage);
        hasLLMResponded = false;
        isLLMProcessing = true;
        ShowTypingIndicator();
        _ = llm.Chat(text, HandleReply, ReplyCompleted);
    }

    void OnDestroy()
    {
        if (dictationScript != null)
        {
            dictationScript.OnTextRecognized -= HandleDictationResult;
        }
    }

    void ReplyCompleted()
    {
        isLLMProcessing = false;
        RemoveTypingIndicator();
        Debug.Log("LLM response completed");
    }

    private void Start()
    {
        if (dictationScript != null)
        {
            dictationScript.OnTextRecognized += HandleDictationResult;
        }
    }

    private void Update()
    {
        if (chatField.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                string userMessage = chatField.text;
                SendMessageToChat(username + ": " + userMessage, Message.MessageType.playerMessage);
                hasLLMResponded = false;  // Reset the flag for new message
                isLLMProcessing = true;
                ShowTypingIndicator();
                _ = llm.Chat(userMessage, HandleReply, ReplyCompleted);
                chatField.text = "";
            }
        }
        else if (!chatField.isActiveAndEnabled && Input.GetKeyDown(KeyCode.Return))
            chatField.ActivateInputField();

        // Animate typing indicator
        if (isLLMProcessing)
        {
            dotAnimationTimer += Time.deltaTime;
            if (dotAnimationTimer >= 0.5f)
            {
                dotAnimationTimer = 0f;
                UpdateTypingIndicator();
            }
        }

        if (!chatField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.M) && chatIsActive == false)
            {
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                chatIsActive = true;
                chatBox.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.M) && chatIsActive == true)
            {
                m_Rigidbody.constraints = RigidbodyConstraints.None;
                chatIsActive = false;
                chatBox.SetActive(false);
            }
        }
    }

    private void ShowTypingIndicator()
    {
        SendMessageToChat("Tutor is typing...", Message.MessageType.info, typingIndicatorId);
    }

    private void UpdateTypingIndicator()
    {
        foreach (Message msg in messageList)
        {
            if (msg.id == typingIndicatorId)
            {
                string currentText = msg.textObject.text;
                if (currentText.EndsWith("...")) msg.textObject.text = "Tutor is typing.";
                else if (currentText.EndsWith("..")) msg.textObject.text = "Tutor is typing...";
                else if (currentText.EndsWith(".")) msg.textObject.text = "Tutor is typing..";
                else msg.textObject.text = "Tutor is typing.";
                break;
            }
        }
    }

    private void RemoveTypingIndicator()
    {
        messageList.RemoveAll(msg => {
            if (msg.id == typingIndicatorId)
            {
                Destroy(msg.textObject.gameObject);
                return true;
            }
            return false;
        });
    }

    public void SendMessageToChat(string text, Message.MessageType messageType, string id = "")
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;
        newMessage.id = id;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TMP_Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch(messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
            case Message.MessageType.tutorMessage:
                color = tutorMessage;
                break;
            case Message.MessageType.info:
                color = info;
                break;
            default:
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string id;
    public string text;
    public TMP_Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        tutorMessage,
        playerMessage, 
        info
    }
}