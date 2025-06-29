import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { userStore } from "../../stores/UserStore";
import '../../styles/auth.css';

const RegisterPage = observer(() => {
  const [form, setForm] = useState({
    firstname: '',
    lastname: '',
    contact: null,
    height: 0.0,
    weight: 0.0,
    birthday: '01/01/1900',
    email: "",
    password: "",
    confirmPassword: ""
  });

  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleChange = e => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();

    if (form.password !== form.confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    try {
      await userStore.register(form);
      await userStore.login(form.email, form.password);
      navigate("/home");
    } catch (err) {
      setError(userStore.error || "Registration failed.");
    }
  };

  return (
    <div className="register-page">

      <div className="auth-form">
        <h2>Register</h2>
        <form onSubmit={handleSubmit} className="auth-form-inner">
          <label>
            Firstname:
            <input
              name="firstname"
              type="text"
              value={form.firstname}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Lastname:
            <input
              name="lastname"
              type="text"
              value={form.lastname}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Email:
            <input
              name="email"
              type="email"
              value={form.email}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Contact:
            <input
              name="contact"
              type="phone"
              value={form.contact}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Birthday:
            <input
              name="birthday"
              type="date"
              value={form.birthday}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Weight:
            <input
              name="weight"
              type="number"
              value={form.weight}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Height:
            <input
              name="height"
              type="number"
              value={form.height}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Password:
            <input
              name="password"
              type="password"
              value={form.password}
              onChange={handleChange}
              required
            />
          </label>
          <label>
            Confirm Password:
            <input
              name="confirmPassword"
              type="password"
              value={form.confirmPassword}
              onChange={handleChange}
              required
            />
          </label>
          {error && <div className="error">{error}</div>}
          <button type="submit">Register</button>
        </form>
      </div>
    </div>

  );
});

export default RegisterPage;
