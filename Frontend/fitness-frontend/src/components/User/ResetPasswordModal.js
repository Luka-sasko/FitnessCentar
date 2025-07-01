import React, { useState } from "react";
import "../../styles/auth.css";

const ResetPasswordModal = ({ onClose, onSubmit }) => {
    const [form, setForm] = useState({
        passwordOld: "",
        passwordNew: "",
        confirmPassword: ""
    });

    const [error, setError] = useState("");
    const [message, setMessage] = useState("");

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        setMessage("");

        if (form.passwordNew !== form.confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        try {
            await onSubmit(form);
            setMessage("✔️ Password updated successfully.");
            setTimeout(onClose, 1000);
        } catch (err) {
            setError("❌ Failed to reset password.");
        }
    };

    return (
        <div className="modal-overlay">
            <div className="auth-form">
                <h2>Reset Password</h2>
                {error && <div className="error">{error}</div>}
                {message && <div className="success">{message}</div>}

                <form onSubmit={handleSubmit} className="auth-form-inner">
                    <label>
                        Old Password:
                        <input type="password" name="passwordOld" value={form.oldPassword} onChange={handleChange} required />
                    </label>
                    <label>
                        New Password:
                        <input type="password" name="passwordNew" value={form.newPassword} onChange={handleChange} required />
                    </label>
                    <label>
                        Confirm Password:
                        <input type="password" name="confirmPassword" value={form.confirmPassword} onChange={handleChange} required />
                    </label>

                    <div className="auth-buttons">
                        <button type="submit">Save</button>
                        <button type="button" onClick={onClose}>Cancel</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default ResetPasswordModal;
