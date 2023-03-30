import { createChatBotMessage } from "react-chatbot-kit";
import BotAvatar from "./BotAvatar";

const config = {
  initialMessages: [createChatBotMessage(`Hello! I am your personal health assistant! Please message me your name to start.`)],
  botName: "HealthBot",
  customComponents: {
    botAvatar: (props) => <BotAvatar {...props} />
  },
  customStyles: {
    botMessageBox: {
      backgroundColor: "#808080"
    },
    chatButton: {
      backgroundColor: "#808080"
    }
  }
}

export default config;