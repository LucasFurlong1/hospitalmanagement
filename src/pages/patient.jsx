import React, { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import SideNavBar from "./sidenavpat";
import 'react-chatbot-kit/build/main.css'
import "../App.css"
import "../patform.css"
import Chatbot from "react-chatbot-kit";
import ActionProvider from '../ActionProvider';
import MessageParser from "../MessageParser";
import config from "../chatConfig";

export const Patient = () => {
    let result = []
    const location = useLocation()
    const [doctor, setDoctor] = useState([])

    useEffect(() => {
        fetch(`https://localhost:44304/api/Patient/GetPatientInfo?patientUsername=${location.state.username}`).then(response => response.json()).then((response) => {
            return fetch(`https://localhost:44304/api/Doctor/GetDoctorInfo?doctorUsername=${response[0].Doctor_Username}`)
        }).then(response => response.json()).then((response) => {
            console.log(response)
            setDoctor(response)
        })
    }, [])




    return (
        <div className="pat-container">
            <div className="doctor-info-container">
                <h2>Your Doctor:</h2>
                {doctor.map((data) => {
                    console.log(doctor)
                    return (
                        <div className="doctor-info-main-container">
                            <div className="doc-info-1">
                                <label className="doc-info-title">Name: </label>
                                <p>{data.Name} {data.Doctorate_Degree}</p>
                            </div>
                            <div className="doc-info-2">
                                <div className="doc-info-bottom-1">
                                    <label className="doc-info-title">Email: </label>
                                    <p>{data.Email_Address}</p>
                                </div>
                                <div className="doc-info-bottom-2">
                                    <label className="doc-info-title">Phone Number: </label>
                                    <p>{data.Phone_Number}</p>
                                </div>
                            </div>
                        </div>
                    )
                })}
            </div>
            <Chatbot config={config} actionProvider={ActionProvider} messageParser={MessageParser} />
            <SideNavBar props={location.state.username}/>
        </div>
    );
}

export default Patient;