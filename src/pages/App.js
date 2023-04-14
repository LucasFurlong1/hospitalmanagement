import React, { useState, useEffect } from "react";
import '../CSS/App.css';
import { Routes, Route, BrowserRouter, useLocation } from 'react-router-dom';
import { Login } from "./login";
import { Register } from "./register";
import { Doctor } from "./doctor";
import { Patient } from "./patient";
import { DocDiag } from "./docdiag"
import { DocPres } from "./docpres"
import { PatientInfo } from "./patientInfo"
import { useNavigate } from "react-router-dom";
import Admin from "./admin";
import { useIdleTimer } from "react-idle-timer";
import moment from "moment/moment";

moment.fn.toJSON = function() { return this.format(); }

const App = (props) => {

  let userLoggedIn = localStorage.getItem("userLoggedIn")

  const onIdle = () => {
    if(window.confirm("Press okay to keep going!")){
      reset()
      start()
    }
    else{
      handleLogout()
    }
  }

  const onAction = () => {
    let date = moment().toJSON()
    let datePlusThirty = moment().add(30, "minutes").format()
    let username = localStorage.getItem("username")
    fetch(`https://localhost:44304/api/Security/UpdateSessionData`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        "username": username,
        "sessionStart": date,
        "sessionExpire": datePlusThirty
      })
    })
  }

  const { start, pause, reset } = useIdleTimer({
    onIdle,
    onAction,
    timeout: 1500_000,
    startManually: true
  })


  useEffect(() => {
    if (userLoggedIn === "true") {
      start()
    }
    else {
      pause()
    }
  }, [userLoggedIn])

  let navigate = useNavigate()

  const handleLogout = () => {
    localStorage.setItem("userLoggedIn", false)
    navigate("/")
  }

  return (
    <div className="App">
      <button className="logout-button" onClick={() => { handleLogout() }} style={{ fontFamily: "monospace", fontSize: "xlarge" }}>Log-out</button>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/admin" element={<Admin />} />
        <Route path="/doctor" element={<Doctor />} />
        <Route path="/register" element={<Register />} />
        <Route path="/patient" element={<Patient />} />
        <Route path="/patient-info" element={<PatientInfo />} />
        <Route path="/docdiag" element={<DocDiag />} />
        <Route path="/docpres" element={<DocPres />} />
      </Routes>
    </div>
  );
}

export default App;
