import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { userStore } from "../../stores/UserStore";
import '../../styles/auth.css';


const LoginPage = observer(() => {
  const [form, setForm] = useState({ username: "", password: "" });
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleChange = e => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      await userStore.login(form.username, form.password);
      navigate("/home");
    } catch (err) {
      setError(userStore.error || "Login failed.");
    }
  };

  return (
    <div className="login-page">
      <div className="auth-form">
        <h2>LOGIN</h2>
        <form onSubmit={handleSubmit} className="auth-form-inner">
          <label>
            Email:
            <input
              name="username"
              type="email"
              value={form.username}
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
          {error && <div className="error">{error}</div>}
          <button type="submit">Login</button>
        </form>
      </div>
    </div>
  );
});

export default LoginPage;
