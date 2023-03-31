import { createChatBotMessage } from "react-chatbot-kit";
import BotAvatar from "./BotAvatar";
import ChatResponse from "./chatResponse";

const config = {
  initialMessages: [createChatBotMessage('Hello, enter your name like this "my name is alex" REMINDER: The BMI Calculator does not work! This is on the Healthily (the company who let us use their API)')],
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
  },
  state: {
    access: []
  },
  widgets: [

  ]
}

export default config;