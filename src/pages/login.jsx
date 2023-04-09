import React, { useState } from "react"
import { Link, useNavigate } from 'react-router-dom'
export const Login = (props) => {
    const [username, setUsername] = useState('');
    const [pass, setPass] = useState('');
    const navigate = useNavigate()

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(username)
        fetch("https://localhost:44304/api/Security/LoginRequest", {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                "username": username,
                "password": pass
            })
        }).then(response => response.json()).then((response) => {
            if (response === true) {
                fetch(`https://localhost:44304/api/User/GetUser?username=${username}`).then(res => res.json()).then((res) => {
                    if(res[0].Account_Type === "D"){
                        navigate("/doctor", {state: {username}})
                    }
                    else if(res[0].Account_Type === "P"){
                        navigate("/patient", {state: {username}})
                    }
                })
            }
            else {
                alert("Invalid username or password!")
            }
        })
    }



    return (
        <div className="full-container">
            <div className="auth-form-container">
                <h2 className="auth-title">Welcome to ABC Hospital</h2>
                <form className="login-form" onSubmit={handleSubmit}>
                    <label className="login-label" htmlFor="username">Username: </label>
                    <input className="login-input" value={username} onChange={(e) => { setUsername(e.target.value) }} type="username" placeholder="username" id="username" name="username" />
                    <label className="login-label" htmlFor="password">Password: </label>
                    <input className="login-input" value={pass} onChange={(e) => { setPass(e.target.value) }} type="password" placeholder="**********" id="password" name="password" />

                    {<button type="submit" className="submit-button">Log In</button>}
                </form>
                <Link to="/register">
                    <button className="link-btn" >Need to register with the hospital? Press here</button>
                </Link>
            </div>
        </div>
    )
}