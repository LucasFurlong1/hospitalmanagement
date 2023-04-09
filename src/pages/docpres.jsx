import React from 'react'
import "../presform.css";

export const DocPres = () => {

    

    return (
        <div className='pres-container'>
            <form className='pres-form'>
                <div className='pres-1'>
                    <label className='select-label'>Select patient: </label>
                    <select>
                        <option>hello</option>
                    </select>
                    <label>Select prescription: </label>
                    <select>
                        <option>hello</option>
                    </select>
                </div>
                <div className='pres-2'>
                    <label>Medication Name: </label>
                    <input></input>
                    <label>Dosage: </label>
                    <input></input>
                </div>
                <div className='pres-3'>
                    <label>Instructions: </label>
                    <textarea className='pres-long'></textarea>
                    <label>Filled?: </label>
                    <input type="checkbox"></input>
                </div>
                <button className='pres-update'>Update/Create</button>
                <button className='pres-delete'>Delete</button>
            </form>
        </div>
    )
}

export default DocPres;