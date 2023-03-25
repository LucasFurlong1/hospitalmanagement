import React from 'react'
import "../diagform.css"

export const DocDiag = () => {
    return (
        <div className="diag-container">
            <form className='diag-form'>
                <div className='diag-1'>
                    <label className='select-label'>Select the patient: </label>
                    <select type="patient">
                        <option>Hello</option>
                    </select>
                    <label className='select-label'>Select the diagnosis: </label>
                    <select>
                        <option>Hello</option>
                    </select>
                </div>
                <div className='diag-2'>
                    <label>Diagnosis Name: </label>
                    <input type="username"></input>
                    <label>Date: </label>
                    <input type="date"></input>
                    <label>Treatment: </label>
                    <input type="username"></input>
                </div>
                <div className='diag-3'>
                    <label>Description: </label>
                    <textarea className="diag-long"></textarea>
                    <label>Resolved?</label>
                    <input type="checkbox"></input>
                </div>
                <button className='diag-update'>Update/Create</button>
                <button className='diag-delete'>Delete</button>
            </form>
        </div>
    )
}

export default DocDiag;
