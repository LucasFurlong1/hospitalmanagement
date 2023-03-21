import React, { useState } from "react";
import '../App.css';
import { Routes, Route, BrowserRouter } from 'react-router-dom';
import { Login } from "./login";
import { Register } from "./register";
import { Doctor } from "./doctor";
import { Patient } from "./patient";

const App = () => {

  return (
      <div className="App">
        <Routes>
          <Route path="/" element={<Login/>}/>
          <Route path="/doctor" element={<Doctor/>} />
          <Route path="/register" element={<Register/>} />
          <Route path="/patient" element={<Patient />} />
        </Routes>
      </div>
  );
}

export default App;
