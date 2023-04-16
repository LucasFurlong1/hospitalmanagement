var usertoken = ""
var convotoken = ""
var prevMessage = []
var loggedIn = false
var apiKey = ""
var authToken = ""

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


  loginErrorHandler = () => {
    const message = this.createChatBotMessage("You have already logged in!")
    this.setChatbotMessage(message)
  }

  userMessageErrorHandler = () => {
    const message = this.createChatBotMessage("I'm sorry... I don't understand. Let me reiterate!")
    this.setChatbotMessage(message)
    if (loggedIn === false) {
      const message = this.createChatBotMessage('Hello, enter your name like this "my name is alex"')
      this.setChatbotMessage(message)
    }
    else {
      console.log(prevMessage)
      for (const element of prevMessage) {
        const message = this.createChatBotMessage(element.message)
        this.setChatbotMessage(message)
      }
    }
  }

  BMIHandler = () => {
    const message = this.createChatbotMessage("We're sorry! This doesn't seem to work for the API. You can try it anyways")
    this.setChatbotMessage(message)
  }

  tooYoungHandler = () => {
    const message = this.createChatBotMessage("If you would like to restart, log out and log back in!")
    this.setChatbotMessage(message)
  }

  loginHandler = async (props) => {

    await fetch(`https://localhost:44304/api/Security/GetChatBotData`).then(response => response.json()).then((response) => {
      apiKey = response[0].Key
      authToken = response[0].Token
    })

    prevMessage = []
    this.state = { loginID: Math.random() }
    const options = {
      method: 'POST',
      headers: {
        accept: '*/*',
        'content-type': 'application/json',
        Authorization: `token ${authToken}`,
        'x-api-key': apiKey
      },
      body: JSON.stringify({ id: this.state.loginID.toString(), name: props, email: 'string', email_verified: true })
    };


    console.log(options.body)

    fetch('https://portal.your.md/v4/login', options)
      .then(response => response.json())
      .then((response) => {
        usertoken = response.access_token
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
        'x-api-key': apiKey
      },
      body: JSON.stringify({ conversation_id: 'a', message: props })
    };

    fetch('https://portal.your.md/v3/chat', options)
      .then(response => response.json())
      .then((response) => {
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
        'x-api-key': apiKey
      },
      body: JSON.stringify({ conversation_id: convotoken, message: props })
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
        let checker = JSON.stringify(response.conversation_model.report)
        if(checker !== "{}") {
          console.log("hello")
          response.conversation_model.report.possible_causes.forEach(element => {
            let message = this.createChatBotMessage(element.name)
            this.setChatbotMessage(message)
            prevMessage.push(message)
            message = this.createChatBotMessage(element.triage.triage_diagnostic)
            this.setChatbotMessage(message)
            prevMessage.push(message)
            message = this.createChatBotMessage(element.triage.triage_message)
            this.setChatbotMessage(message)
            prevMessage.push(message)
            message = this.createChatBotMessage(element.triage.triage_treatment)
            this.setChatbotMessage(message)
            prevMessage.push(message)
            message = this.createChatBotMessage(element.triage.triage_worries)
            this.setChatbotMessage(message)
            prevMessage.push(message)
          })
        }
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