import React, { useEffect, useState } from 'react'
import { useLocation } from 'react-router-dom';
import "../patinfo.css"

export const PatientInfo = () => {

    const [prescription, setPrescription] = useState([])
    const [diagnoses, setDiagnoses] = useState([])
    const [patientName, setPatientName] = useState('')
    const location = useLocation()

    useEffect(() => {
        fetch(`https://localhost:44304/api/Prescription/GetPrescriptionsByPatient?patientUsername=${location.state.prop}`).then(response => response.json()).then((response) => {
            setPrescription(response)
        })
        fetch(`https://localhost:44304/api/Diagnosis/GetDiagnosesByPatient?patientUsername=${location.state.prop}`).then(response => response.json()).then((response) => {
            setDiagnoses(response)
        })
        fetch(`https://localhost:44304/api/Patient/GetPatientInfo?patientUsername=${location.state.prop}`).then(response => response.json()).then((response) => {
            setPatientName(response.Name)
        })
    }, [])

    console.log(prescription)

    return (
        <div className='patient-info-page-container'>
            <h2>Patient Name - {patientName}</h2>
            <h3>Diagnoses:</h3>
            {diagnoses.map((data) => {
                return (
                    <div className='diagnoses-block'>
                        <div className='diag-info-1'>
                            <div className='diag-info-top-1'>
                                <label className='info-title'>Diagnosis Name: </label>
                                <p>{data.Diagnosis_Name}</p>
                            </div>
                            <div className='diag-info-top-2'>
                                <label className='info-title'>Diagnosis Date: </label>
                                <p>{data.Diagnosis_Date}</p>
                            </div>
                        </div>
                        <div className='diag-info-2'>
                            <label className='info-title'>Treatment: </label>
                            <p>{data.Diagnosis_Treatment}</p>
                        </div>
                        <div className='diag-info-3'>
                            <label className='info-title'>Diagnosis Description: </label>
                            <p>{data.Diagnosis_Description}</p>
                        </div>
                        <div className='diag-info-4'>
                            <div className='diag-info-bottom-1'>
                                <label className='info-title'>Resolved?: </label>
                                <input type='checkbox' checked={data.Is_Resolved} aria-disabled />
                            </div>
                            <div className='diag-info-bottom-2'>
                                <label className='info-title'>Admitted?: </label>
                                <input type='checkbox' checked={data.Was_Admitted} aria-disabled />
                            </div>
                        </div>
                    </div>
                )
            })}
            <h3>Prescriptions:</h3>
            {prescription.map((data) => {
                return (
                    <div className='prescriptions-block'>
                        <div className='pres-info-1'>
                            <label className='info-title'>Medication Name: </label>
                            <p>{data.Medication_Name}</p>
                        </div>
                        <div className='pres-info-2'>
                            <div className='pres-info-middle-1'>
                                <label className='info-title'>Dosage: </label>
                                <p>{data.Dosage}</p>
                            </div>
                            <div className='pres-info-middle-2'>
                                <label className='info-title'>Date: </label>
                                <p>{data.Prescribed_Date}</p>
                            </div>
                        </div>
                        <div className='pres-info-3'>
                            <label className='info-title'>Instructions: </label>
                            <p>{data.Instructions}</p>
                        </div>
                        <div className='pres-info-4'>
                            <label className='info-title'>Is Filled: </label>
                            <input type="checkbox" checked={data.Is_Filled} aria-disabled />
                        </div>
                    </div>
                )
            })}
        </div>
    );
}

export default PatientInfo;