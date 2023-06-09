import React, { useEffect, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom';
import "../CSS/presform.css";

let id = ""
let patientName = ""

export const DocPres = () => {

    let navigate = useNavigate()
    let location = useLocation()
    const [patients, setPatients] = useState([])
    const [prescriptions, setPrescriptions] = useState([])
    const [medName, setMedName] = useState("")
    const [dosage, setDosage] = useState("")
    const [instructions, setInstructions] = useState("")
    const [date, setDate] = useState("1900-01-01")
    const [filled, setFilled] = useState(false)

    useEffect(() => {
        try{
            fetch(`https://localhost:44304/api/Patient/GetPatientsByDoctor?doctorUsername=${location.state.username}`).then(response => response.json()).then((response) => {    
                setPatients(response)
                patientName = response[0].Username
                getPrescriptions(patientName)
            })
        } catch(error){
            alert(error)
            navigate("/")
        }
    }, [])

    const getPrescriptions = (e) => {
        fetch(`https://localhost:44304/api/Prescription/GetPrescriptionsByPatient?patientUsername=${e}`).then(response => response.json()).then((response) => {
            setPrescriptions(response)
        })
        patientName = e
        id = ""
        setMedName("")
        setDosage("")
        setInstructions("")
        setDate("1900-01-01")
        setFilled(false)
    }

    const populateData = (e) => {
        if(typeof(e) === 'string') {
            var result = e.includes("ID")
            if(e==="New"){
                id = ""
                setMedName("")
                setDosage("")
                setInstructions("")
                setDate("1900-01-01")
                setFilled(false)
            }
            else if(result===true) {
                id = e.slice(5)
                fetch(`https://localhost:44304/api/Prescription/GetPrescriptionByID?prescriptionId=${id}`).then(response => response.json()).then((response) => {
                    setMedName(response[0].Medication_Name)
                    setDosage(response[0].Dosage)
                    setInstructions(response[0].Instructions)
                    setDate(response[0].Prescribed_Date)
                    setFilled(response[0].Is_Filled)
                })
            }
        }
        else {
            id = ""
            setMedName("")
            setDosage("")
            setInstructions("")
            setDate("1900-01-01")
            setFilled(false)
        }
    }

    const handleCheck = (e) => {
        if(!e.target.checked){
            setFilled(false)
        }
        else {
            setFilled(true)
        }
    }

    const handleUpdate = () => {
        if(medName === "") {
            alert("Medication name cannot be empty!")
        }
        else if(dosage === "") {
            alert("Dosage cannot be empty!")
        }
        else if(instructions === "") {
            alert("You must include instructions!")
        }
        else if(id !== "") {
            fetch(`https://localhost:44304/api/Prescription/UpdatePrescription`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    "prescription_ID": id,
                    "patient_Username": patientName,
                    "doctor_Username": location.state.username,
                    "medication_Name": medName,
                    "prescribed_Date": date,
                    "dosage": dosage,
                    "instructions": instructions,
                    "is_Filled": filled
                },
                )
            }).then(response => response.json()).then((response) => {
                if(response[0].success===true){
                    alert("Update success!")
                }
                else{
                    alert("Update failure!")
                }
            })
        }
        else if(patientName === ""){
            alert("Impossible to create!")
        }
        else{
            fetch(`https://localhost:44304/api/Prescription/CreatePrescription`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    "prescription_ID": "",
                    "patient_Username": patientName,
                    "doctor_Username": location.state.username,
                    "medication_Name": medName,
                    "prescribed_Date": date,
                    "dosage": dosage,
                    "instructions": instructions,
                    "is_Filled": filled
                },
                )
            }).then(() => {
                alert("Success!")
                getPrescriptions(patientName)
                setMedName("")
                setDosage("")
                setInstructions("")
                setDate("1900-01-01")
                setFilled(false)
            }).catch((error) => {
                alert(error)
            })
        }
    }

    const handleDelete = () => {
        if(id !== "") {
            fetch(`https://localhost:44304/api/Prescription/DeletePrescription?prescription_ID=${id}`, {
                method: 'DELETE'
            }).then((response) => {
            if(response.ok===true){
                    alert("Delete pass!")
                    let element = document.getElementById('pres')
                    element.value = "New"
                    getPrescriptions(patientName)
                }
                else{
                    alert("Delete failure!")
                }
            })
        }
        else{
            alert("Impossible to delete!")
        }
    }

    return (
        <div className='pres-container'>
            <form className='pres-form'>
                <div className='pres-1'>
                    <label className='select-label'>Select patient: </label>
                    <select onChange={(event) => { getPrescriptions(event.target.value) }}>
                        {patients.map((data) => {
                            return (
                                <option value={data.Username}>{data.Name}</option>
                            )
                        })}
                    </select>
                    <label>Select prescription: </label>
                    <select id="pres" onChange={(e) => {populateData(e.target.value)}}>
                        <option value="New">Create New</option>
                        {prescriptions.map((data) => {
                            return (
                                <option value={`ID - ${data.Prescription_ID}`}>{data.Medication_Name}</option>
                            )
                        })}
                    </select>
                </div>
                <div className='pres-2'>
                        <label>Medication Name: </label>
                        <input className='pres-2-input' value={medName} onChange={(e) => {setMedName(e.target.value)}}/>
                        <label>Dosage: </label>
                        <input className='pres-2-input' value={dosage} onChange={(e) => {setDosage(e.target.value)}}/>
                        <label>Date: </label>
                        <input className='pres-2-input' type="date" value={date} onChange={(e) => {setDate(e.target.value)}}/>
                    </div>
                    <div className='pres-3'>
                        <label>Instructions: </label>
                        <textarea className='pres-long' value={instructions} onChange={(e) => {setInstructions(e.target.value)}}/>
                        <label>Filled?: </label>
                        <input type="checkbox" checked={filled} onChange={(e) => {handleCheck(e)}}/>
                </div>
                <button type="button" className='pres-update' onClick={() => {handleUpdate()}}>Update/Create</button>
                <button type="button" className='pres-delete' onClick={() => {handleDelete()}}>Delete</button>
            </form>
        </div>
    )
}

export default DocPres;