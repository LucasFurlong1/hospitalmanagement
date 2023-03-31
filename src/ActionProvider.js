import axios from "axios";

var usertoken = ""

class ActionProvider {
  constructor(
    createChatBotMessage,
    setStateFunc,
    createClientMessage,
    stateRef,
    createCustomMessage,
    ...rest
  ) {
    this.createChatBotMessage = createChatBotMessage;
    this.setState = setStateFunc;
    this.createClientMessage = createClientMessage;
    this.stateRef = stateRef;
    this.createCustomMessage = createCustomMessage;
  }


  errorHandler= () => {
    const message = this.createChatBotMessage("You have already logged in!")
    this.setChatbotMessage(message)
  }

  loginHandler = (props) => {
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        'content-type': 'application/json',
        Authorization: 'token QTXDVGCtl1wehAZMgtr37hd8g9PkGKjb.dJlfIWZiyV8cNCW0hvJbOH7ujTzB1r6B',
        'x-api-key': 'gWdQEFh7265IsEBsHWtYP3dwUBtHZ9017ngbQm4m'
      },
      body: JSON.stringify({id: 'testtest', name: props, email: 'string', email_verified: true})
    };
    
    fetch('https://portal.your.md/v4/login', options)
      .then(response => response.json())
      .then((response) => {
        usertoken = response.access_token
        console.log(usertoken)
      })
      .catch(err => console.error(err));

    const message = this.createChatBotMessage("Logged In! Hi " + props + " message 'start' to start")
    this.setChatbotMessage(message)
  }

  chatHandler = () => {
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        'content-type': 'application/json',
        Authorization: usertoken,
        'x-api-key': 'gWdQEFh7265IsEBsHWtYP3dwUBtHZ9017ngbQm4m'
      },
      body: JSON.stringify({conversation_id: 'a', message: 'a'})
    };
    
    fetch('https://portal.your.md/v3/chat', options)
      .then(response => response.json())
      .then(response => console.log(response))
      .catch(err => console.error(err));
  }

  logoutHandler = () => {
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        authorization: 'Bearer ' + usertoken
      }
    };
    console.log(options.headers.authorization)

    fetch('https://portal.your.md/v4/logout', options)
      .then(response => response.json())
      .then(response => console.log(response))
      .catch(err => console.error(err));

    const message = this.createChatBotMessage("Successfully logged out!")
    this.setChatbotMessage(message)
  }


  setChatbotMessage = (message) => {
    this.setState(state => ({ ...state, messages: [...state.messages, message] }))
  }
}

export default ActionProvider;