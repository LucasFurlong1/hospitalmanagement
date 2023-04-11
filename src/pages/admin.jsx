import React from 'react'
import "../CSS/registerform.css"

const handleSubmit = () => {

}

const Admin = () => {
    return (
        <div className="reg-form-container">
            <h1 className="reg-title">Create doctor account: </h1>
            <form className='register-form'>
                <div className='doc-reg-1'>
                    <div className='doc-reg-1-1'>
                        <label className='doc-reg-label'>First and Middle Name</label>
                        <input className='doc-input' />
                    </div>
                    <div className='doc-reg-1-2'>
                        <label className='doc-reg-label'>Last Name</label>
                        <input className='doc-input' />
                    </div>
                    <div className='doc-reg-1-3'>
                        <label className='doc-reg-label'>DOB</label>
                        <input type='date' />
                    </div>
                </div>
                <div className='doc-reg-2'>
                    <div className='doc-reg-2-1'>
                        <label className='doc-reg-label'>Password</label>
                        <input className='doc-input' />
                    </div>
                    <div className='doc-reg-2-2'>
                        <label className='doc-reg-label'>Gender</label>
                        <select className='doc-select'>
                            <option></option>
                        </select>
                    </div>
                </div>
                <div className='doc-reg-3'>
                    <div className='doc-reg-3-1'>
                        <label className='doc-reg-label'>Department</label>
                        <select className='doc-select'>
                            <option></option>
                        </select>
                    </div>
                    <div className='doc-reg-3-2'>
                        <label className='doc-reg-label'>Type of Degree</label>
                        <select className='doc-select'>
                            <option></option>
                        </select>
                    </div>
                </div>
                <div className='doc-reg-4'>
                    <div className='doc-reg-4-1'>
                        <label className='doc-reg-label'>Phone Number</label>
                        <input />
                    </div>
                    <div className='doc-reg-4-2'>
                        <label className='doc-reg-label'>On Staff?</label>
                        <input type='checkbox' />
                    </div>
                </div>
                <div className='doc-reg-5'>
                    <div className='doc-reg-5-1'>
                        <label className='doc-reg-label'>Email</label>
                        <input />
                    </div>
                    <div className='doc-reg-5-2'>
                        <label className='doc-reg-label'>Address</label>
                        <textarea />
                    </div>
                </div>
                <div className='doc-reg-6'>
                    <div className='doc-reg-6-1'>
                        <label className='doc-reg-label'>Emergency Contact Name</label>
                        <input />
                    </div>
                    <div className='doc-reg-6-2'>
                        <label className='doc-reg-label'>Emergency Contact Number</label>
                        <input />
                    </div>
                </div>
                <button type="button" className="register-button" onClick={() => { handleSubmit() }}>Register</button>
            </form>
        </div>
    );
}

export default Admin;