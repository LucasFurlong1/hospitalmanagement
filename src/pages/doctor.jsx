import React, { useState, useEffect } from "react"
import axios from "axios"
import SideNavBar from "./sidenav"
import { Link } from 'react-router-dom'
import "../docform.css"

export const Doctor = () => {
    const [patient, setPatient] = useState([]);


    useEffect(() => {
        patients();
    }, [])

    const patients = async () => {
        const { data } = await axios.get("https://jsonplaceholder.typicode.com/users")
            .then((data) => {
                console.log(data);
                setPatient(data.data);
            });
    }

    return (
        <div className="doctor-page-container">
            <div className="doctor-data-container">
                <p className="title">Patients:</p>
                {patient.map((data) => {
                    return (
                        <div className="patient-block">
                            <div className="block-1">
                                <label className="name-label">Name: </label>
                                <p className="patient-name" key={data.id}>{data.name}</p>
                                <label>Email: </label>
                                <p className="patient-email" key={data.id}>{data.email}</p>
                                <label>Phone: </label>
                                <p className="patient-phone" key={data.id}>{data.phone}</p>
                            </div>

                        </div>
                    )
                })}
            </div>
            <SideNavBar />
        </div>
    );
}

export default Doctor;