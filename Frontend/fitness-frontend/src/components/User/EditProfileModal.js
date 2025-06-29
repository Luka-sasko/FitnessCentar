import React, { useState } from "react";
import { userStore } from "../../stores/UserStore";
import "../../styles/auth.css";

const EditProfileModal = ({ user, onClose, onUpdate }) => {
  const [form, setForm] = useState({
    firstname: user.Firstname || "",
    lastname: user.Lastname || "",
    contact: user.Contact || "",
    birthdate: user.Birthdate?.split("T")[0] || "",
    weight: user.Weight || 0,
    height: user.Height || 0
  });

  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage("");
    setError("");

    try {
      await userStore.update(form);
      setMessage("✔️ Profile updated successfully.");
      await onUpdate();
      setTimeout(() => onClose(), 1000);
    } catch {
      setError("❌ Failed to update profile.");
    }
  };

  return (
    <div className="modal-overlay">
      <div className="auth-form">
        <h2>Edit Profile</h2>
        {message && <div className="success">{message}</div>}
        {error && <div className="error">{error}</div>}

        <form onSubmit={handleSubmit} className="auth-form-inner">
          <label>Firstname:
            <input name="firstname" value={form.firstname} onChange={handleChange} required />
          </label>
          <label>Lastname:
            <input name="lastname" value={form.lastname} onChange={handleChange} required />
          </label>
          <label>Contact:
            <input name="contact" value={form.contact} onChange={handleChange} />
          </label>
          <label>Birthday:
            <input name="birthdate" type="date" value={form.birthdate} onChange={handleChange} />
          </label>
          <label>Weight (kg):
            <input name="weight" type="number" step="0.1" value={form.weight} onChange={handleChange} />
          </label>
          <label>Height (cm):
            <input name="height" type="number" value={form.height} onChange={handleChange} />
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

export default EditProfileModal;
