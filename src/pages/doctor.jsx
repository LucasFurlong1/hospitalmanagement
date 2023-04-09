import React, { useState, useEffect } from "react"
import SideNavBar from "./sidenavdoc"
import { Link, useLocation, useNavigate } from 'react-router-dom'
import "../docform.css"

export const Doctor = () => {
    const [patient, setPatient] = useState([]);
    const location = useLocation()
    const navigate = useNavigate()

    useEffect(() => {
        patients();
    }, [])

    const patients = async () => {
        fetch(`https://localhost:44304/api/Patient/GetPatientsByDoctor?doctorUsername=${location.state.username}`).then(response => response.json()).then((response) => {
            console.log(response)
            setPatient(response)
        })
    }

    const patientCardClick = (prop) => {
        navigate("/patient-info", { state: { prop } })
    }

    return (
        <div className="doctor-page-container">
            <h1>Patients:</h1>
            <div className="doctor-data-container">
                {patient.map((data) => {
                    return (
                        <div className="patient-block">
                            <div className="block" onClick={() => { patientCardClick(data.Username) }}>
                                <div className="pat-block-1">
                                    <div className="pat-block-top-1">
                                        <label className="label-title">Name: </label>
                                        <p>{data.Name}</p>
                                    </div>
                                    <div className="pat-block-top-2">
                                        <label className="label-title">DOB: </label>
                                        <p>{data.Birth_Date}</p>
                                    </div>
                                </div>
                                <div className="pat-block-2">
                                    <div className="pat-block-middle-top-1">
                                        <label className="label-title">Gender: </label>
                                        <p>{data.Gender}</p>
                                    </div>
                                    <div className="pat-block-middle-top-2">
                                        <label className="label-title">Email: </label>
                                        <p>{data.Email_Address}</p>
                                    </div>
                                </div>
                                <div className="pat-block-3">
                                    <div className="pat-block-middle-bottom-1">
                                        <label className="label-title">Address: </label>
                                        <p>{data.Address}</p>
                                    </div>
                                    <div className="pat-block-middle-bottom-2">
                                        <label className="label-title">Phone Number: </label>
                                        <p>{data.Phone_Number}</p>
                                    </div>
                                </div>
                                <div className="pat-block-4">
                                    <h3>Emergency Contact:</h3>
                                    <div className="pat-block-bottom-1">
                                        <label className="label-title">Contact Name: </label>
                                        <p>{data.Emergency_Contact_Name}</p>
                                    </div>
                                    <div className="pat-block-bottom-2">
                                        <label className="label-title">Contact Number: </label>
                                        <p>{data.Emergency_Contact_Number}</p>
                                    </div>
                                </div>

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