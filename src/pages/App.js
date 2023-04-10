import React, { useState } from "react";
import '../CSS/App.css';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import { Login } from "./login";
import { Register } from "./register";
import { Doctor } from "./doctor";
import { Patient } from "./patient";
import { DocDiag } from "./docdiag"
import { DocPres } from "./docpres"
import { PatientInfo } from "./patientInfo"
import { useNavigate } from "react-router-dom";





const App = (props) => {
  let navigate = useNavigate()

  const handleLogout = () => {
    navigate("/")
  }

  return (
    <div className="App">
      <button className="logout-button" onClick={() => {handleLogout()}}>Log-out</button>
      <Routes>
        <Route path="/" element={<Login />} />
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
