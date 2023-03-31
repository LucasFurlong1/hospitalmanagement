import axios from "axios";

var usertoken = ""
var convotoken = ""
var prevMessage = []
var loggedIn = false

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


  loginErrorHandler= () => {
    const message = this.createChatBotMessage("You have already logged in!")
    this.setChatbotMessage(message)
  }

  userMessageErrorHandler = () => {
    const message = this.createChatBotMessage("I'm sorry... I don't understand. Let me reiterate!")
    this.setChatbotMessage(message)
    if(loggedIn===false){
      const message = this.createChatBotMessage('Hello, enter your name like this "my name is alex"')
      this.setChatbotMessage(message)
    }
    else{
      console.log(prevMessage)
      for(const element of prevMessage){
        const message = this.createChatBotMessage(element.message)
        this.setChatbotMessage(message)
      }
    }
  }


  tooYoungHandler = () => {
    const message = this.createChatBotMessage("If you would like to restart, log out and log back in!")
    this.setChatbotMessage(message)
  }

  loginHandler = (props) => {
    prevMessage = []
    this.state = {loginID: Math.random()}
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        'content-type': 'application/json',
        Authorization: 'token QTXDVGCtl1wehAZMgtr37hd8g9PkGKjb.dJlfIWZiyV8cNCW0hvJbOH7ujTzB1r6B',
        'x-api-key': 'gWdQEFh7265IsEBsHWtYP3dwUBtHZ9017ngbQm4m'
      },
      body: JSON.stringify({id: this.state.loginID.toString(), name: props, email: 'string', email_verified: true})
    };
    

    console.log(options.body)

    fetch('https://portal.your.md/v4/login', options)
      .then(response => response.json())
      .then((response) => {
        usertoken = response.access_token
        console.log(usertoken)
      })
      .catch(err => console.error(err));

    const message = this.createChatBotMessage("Logged In! Hi, " + props + " message 'start' to start")
    this.setChatbotMessage(message)
    prevMessage.push(message)
    loggedIn = true
  }

  initChatHandler = (props) => {
    prevMessage = []
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        'content-type': 'application/json',
        Authorization: usertoken,
        'x-api-key': 'gWdQEFh7265IsEBsHWtYP3dwUBtHZ9017ngbQm4m'
      },
      body: JSON.stringify({conversation_id: 'a', message: props})
    };
    
    fetch('https://portal.your.md/v3/chat', options)
      .then(response => response.json())
      .then((response) => {
        console.log(response)
        let message = this.createChatBotMessage(response.messages[1].value)
        this.setChatbotMessage(message)
        response.question.choices.forEach(element => {
          message = this.createChatBotMessage(element.id + " - " + element.label)
          this.setChatbotMessage(message)
          prevMessage.push(message)
        });
        convotoken = response.conversation_id
      })
      .catch(err => console.error(err));
  }

  chatHandler = (props) => {
    prevMessage = []
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        'content-type': 'application/json',
        Authorization: usertoken,
        'x-api-key': 'gWdQEFh7265IsEBsHWtYP3dwUBtHZ9017ngbQm4m'
      },
      body: JSON.stringify({conversation_id: convotoken, message: props})
    };

    fetch('https://portal.your.md/v3/chat', options)
    .then(response => response.json())
    .then((response) => {
      console.log(response)
      response.messages.forEach(element => {
        let message = this.createChatBotMessage(element.value)
        this.setChatbotMessage(message)
        prevMessage.push(message)
      })
      response.question.choices.forEach(element => {
        let message = this.createChatBotMessage(element.id + " - " + element.label)
        this.setChatbotMessage(message)
        prevMessage.push(message)
      })
    })
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
      .catch(err => console.error(err));

    let message = this.createChatBotMessage("Successfully logged out!")
    this.setChatbotMessage(message)
    message = this.createChatBotMessage("To log back in, please type 'my name is ' followed by your name")
    this.setChatbotMessage(message)
    loggedIn = false
  }


  setChatbotMessage = (message) => {
    this.setState(state => ({ ...state, messages: [...state.messages, message] }))
  }
}

export default ActionProvider;