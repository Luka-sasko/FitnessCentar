import React, { useState } from "react";
import baseApi from "../../api/BaseApi";

const AddWorkoutPlanModal = ({ open, onClose, onAdded }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");
        try {
            await baseApi.post("/workoutplan", { Name: name, Description: description });
            setLoading(false);
            setName("");
            setDescription("");
            onAdded();
            onClose();
        } catch (err) {
            setError("Error creating workout plan.");
            setLoading(false);
        }
    };

    if (!open) return null;
    return (
        <div className="modal-overlay">
            <div className="modal-glass">
                <button className="modal-close" onClick={onClose}>&times;</button>
                <h3 className="modal-title">ADD WORKOUT PLAN </h3>
                <form onSubmit={handleSubmit} className="modal-form">
                    <label>
                        NAME:
                        <input
                            type="text"
                            required
                            value={name}
                            onChange={e => setName(e.target.value)}
                            placeholder="e.g. Push/Pull/Legs"
                        />
                    </label>
                    <label>
                        DESCRIPTION:
                        <input
                            type="text"
                            value={description}
                            onChange={e => setDescription(e.target.value)}
                            placeholder="e.g. My favorite split"
                        />
                    </label>
                    {error && <div className="modal-error">{error}</div>}
                    <div className="modal-actions">
                        <button type="submit" disabled={loading}>
                            {loading ? "SAVING..." : "SAVE"}
                        </button>
                        <button type="button" onClick={onClose}>
                            CANCEL
                        </button>
                    </div>
                </form>

            </div>
        </div>
    );
};

export default AddWorkoutPlanModal;
