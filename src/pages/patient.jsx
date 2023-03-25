import React, { useState } from "react";
import SideNavBar from "./sidenav";
import 'react-chatbot-kit/build/main.css'
import "../App.css"
import "../patform.css"
import Chatbot from "react-chatbot-kit";
import ActionProvider from '../ActionProvider';
import MessageParser from "../MessageParser";
import config from "../chatConfig";

export const Patient = () => {
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
            <header className="chatbot-header">
                <Chatbot config={config} actionProvider={ActionProvider} messageParser={MessageParser} />
            </header>
            <SideNavBar />
        </div>
    );
}

export default Patient;