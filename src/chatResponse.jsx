import React, { useEffect } from 'react'
import axios from 'axios';


const ChatResponse = (props) => {

    const auth = props.headline

    const options = {
        method: 'POST',
        url: 'https://portal.your.md/v4/chat',
        headers: {
          accept: 'application/json',
          'content-type': 'application/json',
          authorization: auth,
          'x-api-key': 'gWdQEFh7265IsEBsHWtYP3dwUBtHZ9017ngbQm4m'
        },
        data: {conversation_id: '1'}
    };

    useEffect(() => {
        axios.request(options)
    }, [])

    console.log(props)
    return (
        <div></div>
    );
}

export default ChatResponse;