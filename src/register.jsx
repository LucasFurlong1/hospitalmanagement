import React, {useState} from "react";

export const Register = (props) => {
    const [email, setEmail] = useState('');
    const [pass, setPass] = useState('');
    const [name, setName] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email);
    }

    return (
        <div className="auth-form-container">
            <h2 className="auth-title">Register here!</h2>
            <form className="register-form" onSubmit={handleSubmit}>
                <label htmlFor="name">Name: </label>
                <input value={name} onChange={(e) => setName(e.target.value)} name="name" id="name" placeholder="Full Name"/>
                <label htmlFor="email">Email: </label>
                <input value={email} onChange={(e) => setEmail(e.target.value)} type="email" placeholder="youremail@email.com" id="email" name="email" />
                <label htmlFor="password">Password: </label>
                <input value={pass} onChange={(e) => setPass(e.target.value)} type="password" placeholder="**********" id="password" name="password" />
                <button  type="submit">Register</button>
            </form>
            <button className="link-btn" onClick={() => props.onFormSwitch('login')}>Already have an account with the hospital? Log in here</button>
        </div>
    )
}