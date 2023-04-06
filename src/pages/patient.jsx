import React, { useState, useEffect } from "react";
import SideNavBar from "./sidenav";
import 'react-chatbot-kit/build/main.css'
import "../App.css"
import "../patform.css"
import Chatbot from "react-chatbot-kit";
import ActionProvider from '../ActionProvider';
import MessageParser from "../MessageParser";
import config from "../chatConfig";

export const Patient = () => {


    useEffect(() => {
        fetch("https://localhost:44304/api/User/GetUsers"
          ).then(response => response.json()).then((response) => {
            console.log(response)
        })
    },[])


    return (
        <div className="pat-container">
            <div className="doctor-info-container">
                <h2>Your Doctor:</h2>
                <div className="doctor-info-main-container">
                    <label className="pat-lbl">Name: </label>
                    <p className="pat-p">Doctor, Doctor M.D.</p>
                    <label className="pat-lbl">Name: </label>
                    <p className="pat-p">Doctor, Doctor M.D.</p>
                    <label className="pat-lbl">Name: </label>
                    <p className="pat-p">Doctor, Doctor M.D.</p>
                </div>
            </div>
            <Chatbot config={config} actionProvider={ActionProvider} messageParser={MessageParser}/>
            <SideNavBar />
        </div>
    );
}

export default Patient;