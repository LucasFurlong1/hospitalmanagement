var login = false

class MessageParser {
    constructor(actionProvider, state) {
      this.actionProvider = actionProvider;
      this.state = state;
    }


    parse(message) {



      console.log(message)

      const lower = message.toLowerCase()

      if(lower.includes("logout")){
        this.actionProvider.logoutHandler()
        login = false
      }

      if(lower.includes("my name is") && login == true){
        this.actionProvider.errorHandler()
      }

      if(lower.includes("my name is") && login == false){
        console.log(login)
        let name = message.substring(10)
        this.actionProvider.loginHandler(name)
        login = true
      }

    }
  }
  
  export default MessageParser;