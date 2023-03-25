import React, { useState } from "react"
import { Link } from 'react-router-dom'
export const Login = (props) => {
    const [username, setUsername] = useState('');
    const [pass, setPass] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(username);
    }

    return (
        <div className="full-container">
            <div className="auth-form-container">
                <h2 className="auth-title">Welcome to ABC Hospital</h2>
                <form className="login-form" onSubmit={handleSubmit}>
                    <label className="login-label" htmlFor="username">Username: </label>
                    <input className="login-input" value={username} onChange={(e) => setUsername(e.target.value)} type="username" placeholder="username" id="username" name="username" />
                    <label className="login-label" htmlFor="password">Password: </label>
                    <input className="login-input" value={pass} onChange={(e) => setPass(e.target.value)} type="password" placeholder="**********" id="password" name="password" />
                    <Link to="/doctor"><button type="submit" className="submit-button">Log In</button></Link>

                </form>
                <Link to="/register">
                    <button className="link-btn" >Need to register with the hospital? Press here</button>
                </Link>
            </div>
        </div>
    )
}