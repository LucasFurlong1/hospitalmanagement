import React, { useEffect, useState } from 'react'
import { useLocation } from 'react-router-dom'
import "../diagform.css"

let id = ""
let patientName = ""

export const DocDiag = () => {

    let location = useLocation()
    const [patients, setPatients] = useState([])
    const [diagnoses, setDiagnoses] = useState([])
    const [diagName, setDiagName] = useState("")
    const [treatment, setTreatment] = useState("")
    const [description, setDescription] = useState("")
    const [date, setDate] = useState("1900-01-01")
    const [resolved, setResolved] = useState(false)
    const [admitted, setAdmitted] = useState(false)

    useEffect(() => {
        fetch(`https://localhost:44304/api/Patient/GetPatientsByDoctor?doctorUsername=${location.state.username}`).then(response => response.json()).then((response) => {
            setPatients(response)
            console.log(response[0].Username)
            patientName = response[0].Username
            getDiagnoses(patientName)
        })
    }, [])

    const getDiagnoses = (e) => {
        fetch(`https://localhost:44304/api/Diagnosis/GetDiagnosesByPatient?patientUsername=${e}`).then(response => response.json()).then((response) => {
            console.log(response)
            setDiagnoses(response)
        })
        patientName = e
        id = ""
        setDiagName("")
        setTreatment("")
        setDescription("")
        setDate("1900-01-01")
        setResolved(false)
        setAdmitted(false)
    }

    const populateData = (e) => {
        if (typeof (e) === 'string') {
            var result = e.includes("ID")
            if (e === "New") {
                id = ""
                setDiagName("")
                setTreatment("")
                setDescription("")
                setDate("1900-01-01")
                setResolved(false)
                setAdmitted(false)
            }
            else if (result === true) {
                id = e.slice(5)
                console.log(id)
                fetch(`https://localhost:44304/api/Diagnosis/GetDiagnosisByID?diagnosisId=${id}`).then(response => response.json()).then((response) => {
                    setDiagName(response[0].Diagnosis_Name)
                    setTreatment(response[0].Diagnosis_Treatment)
                    setDescription(response[0].Diagnosis_Description)
                    setDate(response[0].Diagnosis_Date)
                    setResolved(response[0].Is_Resolved)
                    setAdmitted(response[0].Was_Admitted)
                })
            }
        }
        else {
            id = ""
            setDiagName("")
            setTreatment("")
            setDescription("")
            setDate("1900-01-01")
            setResolved(false)
            setAdmitted(false)
        }
    }

    const handleResolved = (e) => {
        if (!e.target.checked) {
            setResolved(false)
        }
        else {
            setResolved(true)
        }
    }

    const handleAdmitted = (e) => {
        if (!e.target.checked) {
            setAdmitted(false)
        }
        else {
            setAdmitted(true)
        }
    }

    const handleUpdate = () => {
        console.log(id)
        if(id !== "") {
            fetch(`https://localhost:44304/api/Diagnosis/UpdateDiagnosis`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    "diagnosis_ID": id,
                    "patient_Username": patientName,
                    "doctor_Username": location.state.username,
                    "diagnosis_Name": diagName,
                    "diagnosis_Date": date,
                    "diagnosis_Description": description,
                    "diagnosis_Treatment": treatment,
                    "was_Admitted": admitted,
                    "is_Resolved": resolved
                },
                )
            }).then(response => response.json()).then((response) => {
                console.log(response)
                if(response===true){
                    alert("Update success!")
                }
                else{
                    alert("Update failure!")
                }
            })
        }
        else{
            console.log(patientName)
            fetch(`https://localhost:44304/api/Diagnosis/CreateDiagnosis`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    "diagnosis_ID": "",
                    "patient_Username": patientName,
                    "doctor_Username": location.state.username,
                    "diagnosis_Name": diagName,
                    "diagnosis_Date": date,
                    "diagnosis_Description": description,
                    "diagnosis_Treatment": treatment,
                    "was_Admitted": admitted,
                    "is_Resolved": resolved
                },
                )
            }).then(() => {
                alert("Success!")
                getDiagnoses(patientName)
                setDiagName("")
                setTreatment("")
                setDescription("")
                setDate("1900-01-01")
                setResolved(false)
                setAdmitted(false)
            }).catch((error) => {
                console.log(error)
                alert(error)
            })
        }
    }

    const handleDelete = () => {
        if(id !== "") {
            fetch(`https://localhost:44304/api/Diagnosis/DeleteDiagnosis?diagnosis_ID=${id}`, {
                method: 'DELETE'
            }).then((response) => {
            console.log(response.ok)    
            if(response.ok===true){
                    alert("Delete pass!")
                    let element = document.getElementById('diag')
                    element.value = "New"
                    getDiagnoses(patientName)
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
        <div className="diag-container">
            <form className='diag-form'>
                <div className='diag-1'>
                    <label className='select-label'>Select the patient: </label>
                    <select onChange={(event) => { getDiagnoses(event.target.value) }}>
                        {patients.map((data) => {
                            return (
                                <option value={data.Username}>{data.Name}</option>
                            )
                        })}
                    </select>
                    <label className='select-label'>Select the diagnosis: </label>
                    <select id="diag" onChange={(e) => { populateData(e.target.value) }}>
                        <option value="New">Create New</option>
                        {diagnoses.map((data) => {
                            return (
                                <option value={`ID - ${data.Diagnosis_ID}`}>{data.Diagnosis_Name}</option>
                            )
                        })}
                    </select>
                </div>
                <div className='diag-2'>
                    <label>Diagnosis Name: </label>
                    <input type="username" className='diag-2-input' value={diagName} onChange={(e) => { setDiagName(e.target.value) }} />

                    <label>Date: </label>
                    <input type="date" className='diag-2-input' value={date} onChange={(e) => { setDate(e.target.value) }} />
                </div>
                <div className='diag-3'>
                    <div className='diag-bottom-1'>
                        <div className='diag-bottom-top-1'>
                            <label>Description: </label>
                            <textarea className="diag-long-1" value={description} onChange={(e) => { setDescription(e.target.value) }} />
                        </div>
                        <div className='diag-bottom-top-2'>
                            <label>Treatment: </label>
                            <textarea type="username" className='diag-long-2' value={treatment} onChange={(e) => { setTreatment(e.target.value) }} />
                        </div>
                    </div>
                    <div className='diag-bottom-2'>
                        <div className='diag-bottom-bottom-1'>
                            <label>Resolved?</label>
                            <input type="checkbox" checked={resolved} onChange={(e) => { handleResolved(e) }} />
                        </div>
                        <div className='diag-bottom-bottom-2'>
                            <label>Admitted?</label>
                            <input type="checkbox" checked={admitted} onChange={(e) => { handleAdmitted(e) }} />
                        </div>
                    </div>
                </div>
                <button type="button" className='diag-update' onClick={() => {handleUpdate()}}>Update/Create</button>
                <button type="button" className='diag-delete' onClick={() => {handleDelete()}}>Delete</button>
            </form>
        </div>
    )
}

export default DocDiag;
