import React, { useState } from "react";
import baseApi from "../../api/BaseApi";

const AddExerciseModal = ({ open, onClose, onAdded }) => {
    const [form, setForm] = useState({ Name: "", Desc: "", Reps: 0, Sets: 0, RestPeriod: 0 });
    const [loading, setLoading] = useState(false);

    const handleChange = e => setForm({ ...form, [e.target.name]: e.target.value });

    const handleSubmit = async e => {
        e.preventDefault();
        setLoading(true);
        await baseApi.post("/exercise", form);
        setLoading(false);
        onAdded?.();
        onClose();
    };

    if (!open) return null;

    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <button className="modal-close" onClick={onClose}>&times;</button>
                <h3 className="modal-title">ADD EXERCISE</h3>
                <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: 10 , color: "black"}} className="modal-form">
                    <label>
                        NAME:
                        <input
                            name="Name"
                            type="text"
                            value={form.Name}
                            onChange={handleChange}
                            required
                        />
                    </label>
                    <label>
                        DESCRIPTION:
                        <input
                            name="Desc"
                            type="text"
                            value={form.Desc}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        REPS:
                        <input
                            name="Reps"
                            type="number"
                            value={form.Reps}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        SETS:
                        <input
                            name="Sets"
                            type="number"
                            value={form.Sets}
                            onChange={handleChange}
                        />
                    </label>
                    <label>
                        REST PERIOD:
                        <input
                            name="RestPeriod"
                            type="number"
                            value={form.RestPeriod}
                            onChange={handleChange}
                        />
                    </label>
                    <button type="submit" disabled={loading}>
                        {loading ? "ADDING..." : "ADD"}
                    </button>
                </form>

            </div>
        </div>
    );
};

export default AddExerciseModal;
