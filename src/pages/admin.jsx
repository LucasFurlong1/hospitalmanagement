import React, {useState, useEffect} from 'react'
import { useNavigate } from 'react-router-dom'
import "../CSS/registerform.css"



const Admin = () => {

    const navigate = useNavigate()

    const [first, setFirst] = useState('')
    const [last, setLast] = useState('')
    const [date, setDate] = useState('1900-01-01')
    const [pass, setPass] = useState('')
    const [gender, setGender] = useState('')
    const [department, setDepartment] = useState('')
    const [degree, setDegree] = useState('')
    const [number, setNumber] = useState('')
    const [staff, setStaff] = useState(false)
    const [email, setEmail] = useState('')
    const [address, setAddress] = useState('')
    const [emergencyName, setEmergencyName] = useState('')
    const [emergencyNumber, setEmergencyNumber] = useState('')

    useEffect(() => {
        setDate('1900-01-01')
    }, [])

    const handleSubmit = () => {
        var name = `${first} ${last}`
        var todayDateConv = new Date().toISOString().slice(0, 10);
        var todayDate = new Date()
        var formDate = new Date(date)
        if(first === ""){
            alert('First and Middle name cannot be empty!')
        }
        else if(last === ""){
            alert('Last name cannot be empty!')
        }
        else if(formDate>todayDate){
            alert('DOB cannot be in the future!')
        }
        else if(pass === ""){
            alert('Password cannot be empty!')
        }
        else if(number === ""){
            alert('Number cannot be empty!')
        }
        else if(isNaN(number) || number.length<10) {
            alert('Emergency number must be a number or at least 10 digits!')
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
            fetch(`https://localhost:44304/api/Doctor/CreateDoctor`, {
                method: 'PUT',
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "username": "",
                    "account_Type": "D",
                    "name": name,
                    "birth_Date": date,
                    "gender": gender,
                    "address": address,
                    "phone_Number": number,
                    "email_Address": email,
                    "emergency_Contact_Name": emergencyName,
                    "emergency_Contact_Number": emergencyNumber,
                    "date_Created": todayDateConv,
                    "doctor_Department": department,
                    "is_On_Staff": staff,
                    "doctorate_Degree": degree,
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

    const handleCheck = (e) => {
        if (!e.target.checked) {
            setStaff(false)
        }
        else {
            setStaff(true)
        }
    }

    return (
        <div className="reg-form-container">
            <h1 className="reg-title">Create doctor account: </h1>
            <form className='register-form'>
                <div className='doc-reg-1'>
                    <div className='doc-reg-1-1'>
                        <label className='doc-reg-label'>First and Middle Name</label>
                        <input className='doc-input' onChange={(e) => {setFirst(e.target.value)}}/>
                    </div>
                    <div className='doc-reg-1-2'>
                        <label className='doc-reg-label'>Last Name</label>
                        <input className='doc-input' onChange={(e) => {setLast(e.target.value)}}/>
                    </div>
                    <div className='doc-reg-1-3'>
                        <label className='doc-reg-label'>DOB</label>
                        <input type='date' className='doc-date' value={date} onChange={(e) => {setDate(e.target.value)}}/>
                    </div>
                </div>
                <div className='doc-reg-2'>
                    <div className='doc-reg-2-1'>
                        <label className='doc-reg-label'>Password</label>
                        <input className='doc-input' onChange={(e) => {setPass(e.target.value)}}/>
                    </div>
                    <div className='doc-reg-2-2'>
                        <label className='doc-reg-label'>Gender</label>
                        <select className='doc-select-2' onChange={(e) => {setGender(e.target.value)}}>
                            <option value="M">Male</option>
                            <option value="F">Female</option>
                            <option value="T">Transgender</option>
                            <option value="N">Non-binary</option>
                            <option value="O">Other - more specific</option>
                            <option value="P">Prefer not to say</option>
                        </select>
                    </div>
                </div>
                <div className='doc-reg-3'>
                    <div className='doc-reg-3-1'>
                        <label className='doc-reg-label'>Department</label>
                        <select className='doc-select' onChange={(e) => {setDepartment(e.target.value)}}>
                            <option value="Emergency">Emergency</option>
                            <option value="Labor/Delivery">Labor + Delivery</option>
                            <option value="ICU">ICU</option>
                            <option value="NICU">NICU</option>
                            <option value="General Surgery">General Surgery</option>
                            <option value="Cardiology">Cardiology</option>
                            <option value="Gastroenterology">Gastroenterology</option>
                            <option value="Neurology">Neurology</option>
                            <option value="Pediatrics">Pediatrics</option>
                            <option value="Orthopaedic">Orthopaedic</option>
                            <option value="Oncology">Oncology</option>
                            <option value="Geriatric">Geriatric</option>
                            <option value="ENT">ENT</option>
                            <option value="Anesthesiology">Anesthesiology</option>
                        </select>
                    </div>
                    <div className='doc-reg-3-2'>
                        <label className='doc-reg-label'>Type of Degree</label>
                        <select className='doc-select-2' onChange={(e) => {setDegree(e.target.value)}}>
                            <option value="MD">MD</option>
                            <option value="DO">DO</option>
                            <option value="PhD">PhD</option>
                            <option value="PsyD">PsyD</option>
                        </select>
                    </div>
                </div>
                <div className='doc-reg-4'>
                    <div className='doc-reg-4-1'>
                        <label className='doc-reg-label'>Phone Number</label>
                        <input className='doc-input' type="tel" onChange={(e) => {setNumber(e.target.value)}}/>
                    </div>
                    <div className='doc-reg-4-2'>
                        <label className='doc-reg-label'>On Staff?</label>
                        <input type='checkbox' className='check' checked={staff} onChange={(e) => {handleCheck(e)}}/>
                    </div>
                </div>
                <div className='doc-reg-5'>
                    <div className='doc-reg-5-1'>
                        <label className='doc-reg-label'>Email</label>
                        <input className='doc-email' onChange={(e) => {setEmail(e.target.value)}}/>
                    </div>
                    <div className='doc-reg-5-2'>
                        <label className='doc-reg-label'>Address</label>
                        <textarea className='doc-long' onChange={(e) => {setAddress(e.target.value)}}/>
                    </div>
                </div>
                <div className='doc-reg-6'>
                    <div className='doc-reg-6-1'>
                        <label className='doc-reg-label'>Emergency Contact Name</label>
                        <input className='doc-e-name' onChange={(e) => {setEmergencyName(e.target.value)}}/>
                    </div>
                    <div className='doc-reg-6-2'>
                        <label className='doc-reg-label'>Emergency Contact Number</label>
                        <input className='doc-e-number' type="tel" onChange={(e) => {setEmergencyNumber(e.target.value)}}/>
                    </div>
                </div>
                <button type="button" className="register-button" onClick={() => { handleSubmit() }}>Create</button>
            </form>
        </div>
    );
}

export default Admin;