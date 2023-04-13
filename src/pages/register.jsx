import React, { useState, useEffect } from "react";
import { Link } from 'react-router-dom'
import { useNavigate } from "react-router-dom";
import "../CSS/registerform.css"

export const Register = (props) => {
    let navigate = useNavigate()
    const [pass, setPass] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('')
    const [date, setDate] = useState('1900-01-01');
    const [doctorName, setDoctorName] = useState()
    const [gender, setGender] = useState('M')
    const [phone, setPhone] = useState('')
    const [email, setEmail] = useState('')
    const [address, setAddress] = useState('')
    const [emergencyName, setEmergencyName] = useState('')
    const [emergencyNumber, setEmergencyNumber] = useState('')
    const [doctors, setDoctors] = useState([])


    useEffect(() => {
        fetch(`https://localhost:44304/api/Doctor/GetDoctorsAcceptingPatients`).then(response => response.json()).then((response) => {
            setDoctors(response)
            setDoctorName(response[0].Username)
            setDate('1900-01-01')
        })
    }, [])

    const handleSubmit = (e) => {
        var name = `${firstName} ${lastName}`
        var todayDateConv = new Date().toISOString().slice(0, 10);
        var todayDate = new Date()
        var formDate = new Date(date)
        if(firstName === ""){
            console.log(doctorName)
            alert('First and Middle name cannot be empty!')
        }
        else if(lastName === ""){
            alert('Last name cannot be empty!')
        }
        else if(formDate>todayDate){
            alert('DOB cannot be in the future!')
        }
        else if(pass === ""){
            alert('Password cannot be empty!')
        }
        else if(phone === ""){
            alert('Number cannot be empty!')
        }
        else if(isNaN(phone) || phone.length<10) {
            alert('Phone number must be a number or at least 10 digits!')
        }
        else if(email === ""){
            alert('Email cannot be empty!')
        }
        else if(address === ""){
            alert('Address cannot be empty!')
        }
        else if(emergencyName === ""){
            alert('Contact name cannot be empty!')
        }
        else if(emergencyNumber === ""){
            alert('Contact number cannot be empty!')
        }
        else if(isNaN(emergencyNumber) || emergencyNumber.length<10){
            alert('Emergency number must be a number or at least 10 digits!')
        }
        else {
            fetch(`https://localhost:44304/api/Patient/CreatePatient`, {
                method: 'PUT',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "username": "",
                    "account_Type": "P",
                    "name": name,
                    "birth_Date": date,
                    "gender": gender,
                    "address": address,
                    "phone_Number": phone,
                    "email_Address": email,
                    "emergency_Contact_Name": emergencyName,
                    "emergency_Contact_Number": emergencyNumber,
                    "date_Created": todayDateConv,
                    "doctor_Username": doctorName,
                    "last_Interacted": todayDateConv,
                    "password": pass
                })
            }).then(response => response.json()).then((response) => {
                console.log(response)
                if(response[0].str !== ""){
                    alert(`Your username is ${response[0].str}`)
                    navigate("/")
                }
                else{
                    alert('Create user failed!')
                }

            })
        }
        
    }

    return (
        <div className="reg-form-container">
            <h1 className="reg-title">Register here!</h1>
            <form className="register-form">
                <div className="pat-reg-1">
                    <div className="pat-reg-1-1">
                        <label className="pat-reg-label">First and Middle Name</label>
                        <input className="pat-reg-input" onChange={(e) => { setFirstName(e.target.value) }} />
                    </div>
                    <div className="pat-reg-1-2">
                        <label className="pat-reg-label">Last Name</label>
                        <input className="pat-reg-input" onChange={(e) => { setLastName(e.target.value) }} />
                    </div>
                    <div className="pat-reg-1-3">
                        <label className="pat-reg-label">DOB</label>
                        <input className="pat-reg-input" type="date" value={date} onChange={(e) => { setDate(e.target.value) }} />
                    </div>
                </div>
                <div className="pat-reg-2">
                    <div className="pat-reg-2-1">
                        <label className="pat-reg-label">Password</label>
                        <input className="pat-reg-input" type="password" onChange={(e) => { setPass(e.target.value) }} />
                    </div>
                    <div className="pat-reg-2-2">
                        <label className="pat-reg-label">Doctor</label>
                        <select className="pat-reg-select" onChange={(e) => {setDoctorName(e.target.value)}}>
                            {doctors.map((data) => {
                                return (
                                    <option value={data.Username}>{data.Name} {data.Doctorate_Degree}</option>
                                )
                            })}
                        </select>
                    </div>
                </div>
                <div className="pat-reg-3">
                    <div className="pat-reg-3-1">
                        <label className="pat-reg-label">Gender</label>
                        <select className="pat-reg-select" onChange={(e) => { setGender(e.target.value) }}>
                            <option value="M">Male</option>
                            <option value="F">Female</option>
                            <option value="T">Transgender</option>
                            <option value="N">Non-binary</option>
                            <option value="O">Other - more specific</option>
                            <option value="P">Prefer not to say</option>
                        </select>
                    </div>
                    <div className="pat-reg-3-2">
                        <label className="pat-reg-label">Phone Number</label>
                        <input className="pat-reg-input" type="tel" onChange={(e) => { setPhone(e.target.value) }} />
                    </div>
                </div>
                <div className="pat-reg-4">
                    <div className="pat-reg-4-1">
                        <label className="pat-reg-label">Email</label>
                        <input className="pat-reg-input" type='email' onChange={(e) => { setEmail(e.target.value) }} />
                    </div>
                    <div className="pat-reg-4-2">
                        <label className="pat-reg-label">Address</label>
                        <textarea className="reg-address" onChange={(e) => { setAddress(e.target.value) }} />
                    </div>
                </div>
                <div className="pat-reg-5">
                    <div className="pat-reg-5-1">
                        <label className="pat-reg-label">Emergency Contact Name</label>
                        <input className="e-input" onChange={(e) => { setEmergencyName(e.target.value) }} />
                    </div>
                    <div className="pat-reg-5-2">
                        <label className="pat-reg-label">Emergency Contact Number</label>
                        <input className="e-input" type="tel" onChange={(e) => { setEmergencyNumber(e.target.value) }} />
                    </div>
                </div>
                <button type="button" className="register-button" onClick={() => {handleSubmit()}}>Register</button>
            </form>
            <Link to="/">
                <button className="link-btn" to="/">Already have an account with the hospital? Log in here</button>
            </Link>
        </div>
    )
}