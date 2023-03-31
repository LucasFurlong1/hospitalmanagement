var login = false
var initsymptoms = false

class MessageParser {
    constructor(actionProvider, state) {
      this.actionProvider = actionProvider;
      this.state = state;
    }

    parse(message) {

      console.log(this.state)

      console.log(message)

      const lower = message.toLowerCase()


      var reversedMessages = this.state.messages.reverse()
      var lastBotMessages = []
      for(const element of reversedMessages) {
        if(element.type==='bot'){
          lastBotMessages.push(element)
        }
        if(element.type==='user'){
          break
        }
      }



      if(login===true && lower.includes("start")) {
        this.actionProvider.initChatHandler(lower)
        return
      }


      if(lower.includes("logout")){
        this.actionProvider.logoutHandler()
        login = false
        return
      }

      if(lower.includes("my name is") && login === true){
        this.actionProvider.loginErrorHandler()
        return
      }

      if(lower.includes("my name is") && login === false){
        let name = message.substring(10)
        this.actionProvider.loginHandler(name)
        login = true
        return
      }

      //if you read this next area... im sorry
      //I could definitely do a switch here but I am too lazy to implement it 
      login = true
      console.log(lastBotMessages)
      for(let i = 0; i<lastBotMessages.length; i++) {
        if(lastBotMessages[i].message.includes("is an indicator of whether you are")) {
          this.actionProvider.BMIHandler()
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Before I calculate your BMI")) {
          this.actionProvider.BMIHandler()
          this.actionProvider.chatHandler(lower)
          break
        }
        if(login===true && lastBotMessages[i].message.includes(lower)){
          console.log(i)
          this.actionProvider.chatHandler(lower)
          break
        } 
        if(login===true && lastBotMessages[i].message.includes("please tell me what symptoms") && initsymptoms===false) {
          this.actionProvider.chatHandler(lower)
          initsymptoms = true
          break
        }
        if(lastBotMessages[i].message.includes("male or female")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("What year were you")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("What year was")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("please enter your height")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Now please enter your weight")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("the name of the person")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Please choose all the ones you have")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("How long have you had")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Have you been diagnosed with")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Do you think you are overweight")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Do you smoke")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("you ever had a stroke")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Do you have any of these symptoms")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("I'm sorry, that selection isn't")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("would you like to know")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("would help you most")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("here's some information that might be helpful")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Was this information useful")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("first choose a category")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("so short sentences work best")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Please note, this is not a diagnosis. Always visit a doctor if you are in doubt")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("Click below for your")) {
          this.actionProvider.chatHandler(lower)
          break
        }
        if(lastBotMessages[i].message.includes("I can only provide information for people over")) {
          this.actionProvider.tooYoungHandler()
          break
        }
        if(i === lastBotMessages.length - 1){
          this.actionProvider.userMessageErrorHandler()
          break
        }
      }


    }
  }
  
  export default MessageParser;