import React, { useState } from "react";
import '../App.css';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import { Login } from "./login";
import { Register } from "./register";
import { Doctor } from "./doctor";
import { Patient } from "./patient";
import { DocDiag } from "./docdiag"
import { DocPres } from "./docpres"
import { PatientInfo } from "./patientInfo"

const App = (props) => {



  return (
      <div className="App">
        <Routes>
          <Route path="/" element={<Login/>}/>
          <Route path="/doctor" element={<Doctor/>}/>
          <Route path="/register" element={<Register/>} />
          <Route path="/patient" element={<Patient/>} />
          <Route path="/patient-info" element={<PatientInfo/>}/>
          <Route path="/docdiag" element={<DocDiag/>} />
          <Route path="/docpres" element={<DocPres/>} />
        </Routes>
      </div>
  );
}

export default App;
