const chatMessages = document.getElementById("chatMessages");
const userInput = document.getElementById("userInput");

let collectedEmailData = {
    emailType: "",
    subject: "",
    recipientName: "",
    senderName: "",
    tone: "",
    purpose: "",
    additionalInfo: ""
};

function appendMessage(text, sender) {
    const msgDiv = document.createElement("div");
    msgDiv.classList.add("message", sender);
    msgDiv.textContent = text;
    chatMessages.appendChild(msgDiv);
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

async function sendMessage(event) {
    event.preventDefault();
    const message = userInput.value.trim();
    if (!message) return;

    appendMessage(message, "user");
    userInput.value = "";
    userInput.disabled = true;

    const loadingMsg = document.createElement("div");
    loadingMsg.classList.add("message", "ai");
    loadingMsg.textContent = "Typing...";
    chatMessages.appendChild(loadingMsg);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    try {
        const response = await fetch("/CreateEmailDTO/Chat", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: new URLSearchParams({ userMessage: message })
        });

        const data = await response.json();
        chatMessages.removeChild(loadingMsg);

        if (data.reply) {
            appendMessage(data.reply, "ai");

            const lowerReply = data.reply.toLowerCase();
            if (lowerReply.includes("email type")) collectedEmailData.emailType = message;
            else if (lowerReply.includes("subject")) collectedEmailData.subject = message;
            else if (lowerReply.includes("recipient")) collectedEmailData.recipientName = message;
            else if (lowerReply.includes("sender")) collectedEmailData.senderName = message;
            else if (lowerReply.includes("tone")) collectedEmailData.tone = message;
            else if (lowerReply.includes("purpose")) collectedEmailData.purpose = message;
            else if (lowerReply.includes("additional")) collectedEmailData.additionalInfo = message;

        } else {
            appendMessage("⚠️ Unexpected server response.", "ai");
        }
    } catch (error) {
        chatMessages.removeChild(loadingMsg);
        appendMessage("⚠️ Error contacting server. Please try again.", "ai");
    }

    userInput.disabled = false;
    userInput.focus();
    return false;
}
